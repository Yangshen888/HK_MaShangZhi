import axios from '@/libs/api.request'

//列表
export const GetList = (data) => {
  return axios.request({
    url: 'student/student/List',
    method: 'post',
    data
  })
}

//添加
export const GetCreate = (data) => {
  return axios.request({
    url: 'student/student/Create',
    method: 'post',
    data
  })
}

//获取数据
export const GetShow = (data) => {
  return axios.request({
    url: 'student/student/show?guid=' + data,
    method: 'get',
  })
}

//编辑
export const GetEdit = (data) => {
  return axios.request({
    url: 'student/student/Edit',
    method: 'post',
    data
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'student/student/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'student/student/batch',
    method: 'get',
    params: data
  })
}

//导入
export const GetImport = (data) => {
  return axios.request({
    url: 'student/student/Import',
    method: 'post',
    data
  })
}

//导出
export const GetExportPass = (ids) => {
  return axios.request({
    url: 'student/student/ExportPass?ids=' + ids,
    method: 'get'
  })
}
