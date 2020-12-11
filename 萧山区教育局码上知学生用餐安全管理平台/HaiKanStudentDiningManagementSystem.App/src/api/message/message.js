import axios from '@/libs/api.request'

//列表
export const MessageList = (data) => {
  return axios.request({
    url: 'canteenManagement/manageMessage/MessageList',
    method: 'post',
    data
  })
}

//获取数据
export const MessageGet = (data) => {
  return axios.request({
    url: 'canteenManagement/manageMessage/MessageGet?guid=' + data,
    method: 'get',
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'canteenManagement/manageMessage/Delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'canteenManagement/manageMessage/Batch',
    method: 'get',
    params: data
  })
}

//添加
export const CuisineCreate = (data) => {
  return axios.request({
    url: 'canteenManagement/manageMessage/Create',
    method: 'post',
    data
  })
}

//编辑
export const CuisineEdit = (data) => {
  return axios.request({
    url: 'canteenManagement/manageMessage/Edit',
    method: 'post',
    data
  })
}

//导入
export const Import = (data) => {
  return axios.request({
    url: 'canteenManagement/manageMessage/Import',
    method: 'post',
    data
  })
}
//导出
export const Export = (ids) => {
  return axios.request({
    url: 'canteenManagement/manageMessage/Export?ids=' + ids,
    method: 'get'
  })
}
 