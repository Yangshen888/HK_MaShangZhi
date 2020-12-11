import axios from '@/libs/api.request'

//列表
export const GetList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/GetList',
    method: 'post',
    data
  })
}
 
//添加
export const GetCreate = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/GetCreate',
    method: 'post',
    data
  })
}

//获取数据
export const GetfoGet = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/GetfoGet?id=' + data,
    method: 'get',
  })
}

//编辑
export const GetEdit = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/GetEdit',
    method: 'post',
    data
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/batch',
    method: 'get',
    params: data
  })
} 

//导入
export const GetImport = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/GetImport',
    method: 'post',
    data
  })
}

//导出
export const GetExport = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/ExportPass?ids=' + ids,
    method: 'get'
  })
}

//消毒方法下拉框
export const GetmethodList = () => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/methodList',
    method: 'get'
  })
}
//消毒对象下拉框
export const GetdisinfectionList = () => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/disinfectionList',
    method: 'get'
  })
}
//消毒餐次下拉框
export const GetccList = () => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/ccList',
    method: 'get'
  })
}
//消毒间下拉框
export const GetroomList = () => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/roomList',
    method: 'get'
  })
}
//消毒工具下拉框
export const GettoolList = () => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/toolList',
    method: 'get'
  })
}
//该组织下人员列表
export const GetuserList = () => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/userList',
    method: 'get'
  })
}

//人员列表
export const GetUsersList = () => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/UsersList',
    method: 'get'
  })
}


//列表
export const aGetList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/aList',
    method: 'post',
    data
  })
}
 
//添加
export const aGetCreate = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/aCreate',
    method: 'post',
    data
  })
}

//获取数据
export const aGetShow = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/aShow?id=' + data,
    method: 'get',
  })
}

//编辑
export const aGetEdit = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/aEdit',
    method: 'post',
    data
  })
}

// delete department
export const adeleteDepartment = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/adelete/' + ids,
    method: 'get'
  })
}

// batch command
export const abatchCommand = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Disinfection/abatch',
    method: 'get',
    params: data
  })
} 


