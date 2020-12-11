using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.Department;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.SystemMenu;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.SystemPermission;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.SystemRole;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.Rbac.SystemUser;
using HaiKanStudentDiningManagementSystem.Api.Entities;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.WorkStudy;
using HaiKanStudentDiningManagementSystem.Api.ViewModels.MessageFeedBack;
//using HaiKanStudentDiningManagementSystem.Api.ViewModels.Person;
//using HaiKanStudentDiningManagementSystem.Api.ViewModels.Base;
//using HaiKanStudentDiningManagementSystem.Api.ViewModels.Score;

namespace HaiKanStudentDiningManagementSystem.Api.Configurations
{
    public class MappingProfile: Profile
    {
        public MappingProfile()
        {
            #region SystemUser
            CreateMap<SystemUser, UserJsonModel>();
            CreateMap<UserCreateViewModel, SystemUser>();
            CreateMap<UserEditViewModel, SystemUser>();
            CreateMap<SystemUser, UserEditViewModel>();
            #endregion
            #region SystemRole
            CreateMap<SystemRole, RoleJsonModel>();
            CreateMap<RoleCreateViewModel, SystemRole>();
            CreateMap<SystemRole, RoleCreateViewModel>();
            #endregion

            #region SystemMenu
            CreateMap<SystemMenu, MenuJsonModel>();
            CreateMap<MenuCreateViewModel, SystemMenu>();
            CreateMap<MenuEditViewModel, SystemMenu>();
            CreateMap<SystemMenu, MenuEditViewModel>();
            #endregion

            #region SystemPermission
            CreateMap<SystemPermission, PermissionJsonModel>()
                .ForMember(d => d.MenuName, s => s.MapFrom(x => x.SystemMenuUu.Name))
                .ForMember(d => d.PermissionTypeText, s => s.MapFrom(x => x.Type.ToString()));
            CreateMap<PermissionCreateViewModel, SystemPermission>();
            CreateMap<PermissionEditViewModel, SystemPermission>();
            CreateMap<SystemPermission, PermissionEditViewModel>();
            #endregion

           #region Postjobs
            CreateMap< Postjobs, PostjobsViewModel >();
            CreateMap<PostjobsViewModel, Postjobs>();
            #endregion
            
            #region MessageBoard
            CreateMap<MessageBoard, MessageBoardViewModel>();
            CreateMap<MessageBoardViewModel, MessageBoard>();
            #endregion
            #region PostJobsAppeal
            CreateMap<WxPostjobsAppealViewModel, PostJobsAppeal>();
            #endregion
            #region Report
            CreateMap<Report, ReportViewModel>();
            #endregion
            #region MessageBoard
            CreateMap<WxMessageBoardViewModel, MessageBoard>();
            #endregion
            #region Report
            CreateMap<WxReportViewModel, Report>();
            #endregion
            #region Survey
            CreateMap<SurveyViewModel, Survey>();
            CreateMap< Survey, SurveyViewModel > ();
            #endregion
            #region SurveyQuestions
            CreateMap<SurveyQuestionsViewModel, SurveyQuestions>();
            CreateMap<SurveyQuestions, SurveyQuestionsViewModel>();
            #endregion
            #region SurveyQuestions
            CreateMap<SurveyQuestionsItemsViewModel, SurveyQuestionsItems>();
            CreateMap<SurveyQuestionsItems, SurveyQuestionsItemsViewModel>();
            #endregion
            #region SurveyAnswer
            CreateMap<WxSurveyAnswerViewModel, SurveyAnswer>();
            #endregion
            


        }
    }
}
