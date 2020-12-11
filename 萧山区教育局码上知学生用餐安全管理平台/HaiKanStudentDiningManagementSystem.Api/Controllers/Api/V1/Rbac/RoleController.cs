using System;
using Microsoft.Data.SqlClient;
using System.Linq;
using AutoMapper;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.Entities.Enums;
using HaiKanStudentDiningManagementSystem.Api.Extensions;
using HaiKanStudentDiningManagementSystem.Api.Extensions.AuthContext;
using HaiKanStudentDiningManagementSystem.Api.Extensions.CustomException;
using HaiKanStudentDiningManagementSystem.Api.Models.Response;
using HaiKanStudentDiningManagementSystem.Api.RequestPayload.Rbac.Role;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.SystemRole;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace HaiKanStudentDiningManagementSystem.Api.Controllers.Api.V1.Rbac
{
    /// <summary>
    /// 
    /// </summary>
    //[CustomAuthorize]
    [Route("api/v1/rbac/[controller]/[action]")]
    [ApiController]
    [CustomAuthorize]
    public class RoleController : ControllerBase
    {
        private readonly haikanSDMSContext _dbContext;
        private readonly IMapper _mapper;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="mapper"></param>
        public RoleController(haikanSDMSContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        /// <summary>
        /// 角色列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult List(RoleRequestPayload payload)
        {
            var response = ResponseModelFactory.CreateResultInstance;
            using (_dbContext)
            {
                var query = _dbContext.SystemRole.Select(x => new
                {
                    x.SystemRoleUuid,
                    x.RoleName,
                    x.AddTime,
                    x.AddPeople,
                    x.IsDeleted,
                    x.Id,
                    x.IsFixation,
                    x.SchoolUuid,
                    x.IsCreEdu,
                    _disabled = IsDisabled(x.IsCreEdu.Value, x.IsFixation.Value),
                });
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    query = query.Where(x => x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid);
                }
                if (!string.IsNullOrEmpty(payload.Kw))
                {
                    query = query.Where(x => x.RoleName.Contains(payload.Kw.Trim()));
                }
                if (payload.IsDeleted > CommonEnum.IsDeleted.All)
                {
                    query = query.Where(x => (CommonEnum.IsDeleted)x.IsDeleted == payload.IsDeleted);
                }

                var list = query.Paged(payload.CurrentPage, payload.PageSize).ToList();
                var totalCount = query.Count();
                //var data = list.Select(_mapper.Map<SystemRole, RoleJsonModel>);

                response.SetData(list, totalCount);
                return Ok(response);
            }
        }

        private static bool IsDisabled(int i, int f)
        {

            var check = false;
            if (AuthContextService.CurrentUser.SchoolGuid != null)
            {
                if (i == 0)
                {
                    check = false;
                }
                if (i == 1)
                {
                    check = true;
                }
            }
            else
            {


                if (f == 1)
                {
                    check = true;
                }
                else
                {
                    check = false;
                }
            }



            return check;

        }

        /// <summary>
        /// 创建角色
        /// </summary>
        /// <param name="model">角色视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Create(RoleCreateViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (model.RoleName.Trim().Length <= 0)
            {
                response.SetFailed("请输入角色名称");
                return Ok(response);
            }
            using (_dbContext)
            {
                if (AuthContextService.CurrentUser.SchoolGuid == null)
                {
                    if (_dbContext.SystemRole.Any(x => x.RoleName == model.RoleName))
                    {
                        response.SetFailed("角色已存在");
                        return Ok(response);
                    }
                }
                else
                {
                    if (_dbContext.SystemRole.Any(x => x.RoleName == model.RoleName && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid))
                    {
                        response.SetFailed("角色已存在");
                        return Ok(response);
                    }

                }

                var entity = new SystemRole();
                entity.SystemRoleUuid = Guid.NewGuid();
                entity.RoleName = model.RoleName;
                entity.SchoolUuid = model.SchoolUuid;
                entity.AddTime = DateTime.Now.ToString("yyyy-MM-dd");
                entity.AddPeople = AuthContextService.CurrentUser.DisplayName;
                entity.IsDeleted = 0;
                if (AuthContextService.CurrentUser.SchoolGuid != null)
                {
                    entity.IsCreEdu = 0;
                }
                else
                {
                    entity.IsCreEdu = 1;
                }
                _dbContext.SystemRole.Add(entity);
                _dbContext.SaveChanges();

                response.SetSuccess();
                return Ok(response);
            }
        }

        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="guid">角色惟一编码</param>
        /// <returns></returns>
        [HttpGet("{guid}")]
        [ProducesResponseType(200)]
        public IActionResult Edit(Guid guid)
        {
            using (_dbContext)
            {
                var entity = _dbContext.SystemRole.FirstOrDefault(x => x.SystemRoleUuid == guid);
                var response = ResponseModelFactory.CreateInstance;
                response.SetData(_mapper.Map<SystemRole, RoleCreateViewModel>(entity));
                return Ok(response);
            }
        }

        /// <summary>
        /// 保存编辑后的角色信息
        /// </summary>
        /// <param name="model">角色视图实体</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(200)]
        public IActionResult Edit(RoleCreateViewModel model)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            using (_dbContext)
            {
                var rEntity = _dbContext.SystemRole.FirstOrDefault(x => x.SystemRoleUuid == model.SystemRoleUuid);
                if (rEntity == null)
                {
                    response.SetFailed("角色不存在");
                    return Ok(response);
                }
                else
                {
                    if (AuthContextService.CurrentUser.SchoolGuid == null)
                    {
                        if (_dbContext.SystemRole.Any(x => x.RoleName == model.RoleName))
                        {
                            response.SetFailed("角色已存在");
                            return Ok(response);
                        }
                    }
                    else
                    {
                        if (_dbContext.SystemRole.Any(x => x.RoleName == model.RoleName && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid))
                        {
                            response.SetFailed("角色已存在");
                            return Ok(response);
                        }
                    }
                }



                var entity = _dbContext.SystemRole.FirstOrDefault(x => x.SystemRoleUuid == model.SystemRoleUuid);
                entity.RoleName = model.RoleName;
                entity.SchoolUuid = model.SchoolUuid;
                _dbContext.SaveChanges();
                return Ok(response);
            }
        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Delete(string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            if (ConfigurationManager.AppSettings.IsTrialVersion)
            {
                response.SetIsTrial();
                return Ok(response);
            }
            response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
            return Ok(response);
        }

        /// <summary>
        /// 恢复角色
        /// </summary>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet("{ids}")]
        [ProducesResponseType(200)]
        public IActionResult Recover(string ids)
        {
            var response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
            return Ok(response);
        }

        /// <summary>
        /// 批量操作
        /// </summary>
        /// <param name="command"></param>
        /// <param name="ids">角色ID,多个以逗号分隔</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(200)]
        public IActionResult Batch(string command, string ids)
        {
            var response = ResponseModelFactory.CreateInstance;
            switch (command)
            {
                case "delete":
                    if (ConfigurationManager.AppSettings.IsTrialVersion)
                    {
                        response.SetIsTrial();
                        return Ok(response);
                    }
                    response = UpdateIsDelete(CommonEnum.IsDeleted.Yes, ids);
                    break;
                case "recover":
                    response = UpdateIsDelete(CommonEnum.IsDeleted.No, ids);
                    break;
                //case "forbidden":
                //    if (ConfigurationManager.AppSettings.IsTrialVersion)
                //    {
                //        response.SetIsTrial();
                //        return Ok(response);
                //    }
                //    response = UpdateStatus(UserStatus.Forbidden, ids);
                //    break;
                //case "normal":
                //    response = UpdateStatus(UserStatus.Normal, ids);
                //    break;
                default:
                    break;
            }
            return Ok(response);
        }

        /// <summary>
        /// 为指定角色分配权限
        /// </summary>
        /// <param name="payload">角色分配权限的请求载体类</param>
        /// <returns></returns>
        [HttpPost("/api/v1/rbac/role/assign_permission")]
        public IActionResult AssignPermission(RoleAssignPermissionPayload payload)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                var role = _dbContext.SystemRole.FirstOrDefault(x => x.SystemRoleUuid.ToString() == payload.RoleCode);
                if (role == null)
                {
                    response.SetFailed("角色不存在");
                    return Ok(response);
                }
                // 如果是超级管理员，则不写入到角色-权限映射表(在读取时跳过角色权限映射，直接读取系统所有的权限)
                if (role.RoleName == "超级管理员")
                {
                    response.SetSuccess();
                    return Ok(response);
                }
                //先删除当前角色原来已分配的权限
                _dbContext.Database.ExecuteSqlRaw("DELETE FROM SystemRolePermissionMapping WHERE systemroleuuid={0}", payload.RoleCode);
                if (payload.Permissions != null || payload.Permissions.Count > 0)
                {
                    var permissions = payload.Permissions.Select(x => new SystemRolePermissionMapping
                    {
                        AddTime = DateTime.Now.ToString("yyyy-MM-dd"),
                        SystemPermissionUuid = new Guid(x.Trim()),
                        SystemRoleUuid = new Guid(payload.RoleCode.Trim())
                    });
                    _dbContext.SystemRolePermissionMapping.AddRange(permissions);
                    _dbContext.SaveChanges();
                }

            }
            return Ok(response);
        }

        /// <summary>
        /// 获取指定用户的角色列表
        /// </summary>
        /// <param name="guid">用户GUID</param>
        /// <returns></returns>
        [HttpGet("/api/v1/rbac/role/find_list_by_user_guid/{guid}")]
        public IActionResult FindListByUserGuid(Guid guid)
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                //有N+1次查询的性能问题
                //var query = _dbContext.DncUser
                //    .Include(r => r.UserRoles)
                //    .ThenInclude(x => x.DncRole)
                //    .Where(x => x.Guid == guid);
                //var roles = query.FirstOrDefault().UserRoles.Select(x => new
                //{
                //    x.DncRole.Code,
                //    x.DncRole.Name
                //});
                var sql = @"SELECT R.* FROM DncUserRoleMapping AS URM
INNER JOIN DncRole AS R ON R.RoleUUID=URM.RoleUUID
WHERE URM.UserUUID={0}";
                var query = _dbContext.SystemRole.FromSqlRaw(sql, guid).ToList();
                var assignedRoles = query.ToList().Select(x => x.SystemRoleUuid).ToList();
                var roles = _dbContext.SystemRole.Where(x => (CommonEnum.IsDeleted)x.IsDeleted == CommonEnum.IsDeleted.No).ToList().Select(x => new { label = x.RoleName, key = x.SystemRoleUuid });
                response.SetData(new { roles, assignedRoles });
                return Ok(response);
            }
        }

        /// <summary>
        /// 查询所有角色列表(只包含主要的字段信息:name,code)
        /// </summary>
        /// <returns></returns>
        [HttpGet("/api/v1/rbac/role/find_simple_list")]
        public IActionResult FindSimpleList()
        {
            var response = ResponseModelFactory.CreateInstance;
            using (_dbContext)
            {
                if (AuthContextService.CurrentUser.SchoolGuid == null)
                {
                    var roles = _dbContext.SystemRole.Where(x => ((CommonEnum.IsDeleted)x.IsDeleted == CommonEnum.IsDeleted.No)).Select(x => new { x.RoleName, x.SystemRoleUuid, Disabled = SetDisable(x.RoleName) }).ToList();

                    response.SetData(roles);
                }
                else
                {
                    var roles = _dbContext.SystemRole.Where(x => (CommonEnum.IsDeleted)x.IsDeleted == CommonEnum.IsDeleted.No && x.SchoolUuid == AuthContextService.CurrentUser.SchoolGuid && x.IsCreEdu == 0).Select(x => new { x.RoleName, x.SystemRoleUuid, Disabled = SetDisable(x.RoleName) }).ToList();

                    response.SetData(roles);
                }

            }
            return Ok(response);
        }

        #region 私有方法

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="isDeleted"></param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        private ResponseModel UpdateIsDelete(CommonEnum.IsDeleted isDeleted, string ids)
        {
            using (_dbContext)
            {
                var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
                var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
                var sql = string.Format("UPDATE SystemRole SET IsDeleted=@IsDeleted WHERE SystemRoleUUID IN ({0}) and IsFixation !=1", parameterNames);
                parameters.Add(new SqlParameter("@IsDeleted", (int)isDeleted));
                var num = _dbContext.Database.ExecuteSqlRaw(sql, parameters);

                var response = ResponseModelFactory.CreateInstance;
                if (num <= 0)
                {
                    response.SetFailed("删除失败");
                }
                return response;
            }
        }

        public static bool SetDisable(string name)
        {
            if (name == "微信用户")
            {
                return true;
            }
            else if (AuthContextService.CurrentUser.SchoolGuid != null)
            {
                if (name == "超级管理员")
                {
                    return true;
                }
                if (name == "教育局")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }


        }

        /// <summary>
        /// 删除角色
        /// </summary>
        /// <param name="status">角色状态</param>
        /// <param name="ids">角色ID字符串,多个以逗号隔开</param>
        /// <returns></returns>
        //private ResponseModel UpdateStatus(UserStatus status, string ids)
        //{
        //    using (_dbContext)
        //    {
        //        var parameters = ids.Split(",").Select((id, index) => new SqlParameter(string.Format("@p{0}", index), id)).ToList();
        //        var parameterNames = string.Join(", ", parameters.Select(p => p.ParameterName));
        //        var sql = string.Format("UPDATE DncRole SET Status=@Status WHERE Code IN ({0})", parameterNames);
        //        parameters.Add(new SqlParameter("@Status", (int)status));
        //        _dbContext.Database.ExecuteSqlCommand(sql, parameters);
        //        var response = ResponseModelFactory.CreateInstance;
        //        return response;
        //    }
        //}
        #endregion
    }
}