import axios from '@/libs/api.request'

//列表
export const GetList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/GetList',
    method: 'post',
    data
  })
}
 
//添加
export const GetCreate = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/GetCreate',
    method: 'post',
    data
  })
}

//获取数据
export const GetfoGet = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/GetfoGet?id=' + data,
    method: 'get',
  })
}

//编辑
export const GetEdit = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/GetEdit',
    method: 'post',
    data
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/batch',
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




//品种列表
export const GetTypesList = () => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/TypesList',
    method: 'get'
  })
}

//列表
export const aGetList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/aList',
    method: 'post',
    data
  })
}
 
//添加
export const aGetCreate = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/aCreate',
    method: 'post',
    data
  })
}

//单位列表
export const aGetPartList = () => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/PartList',
    method: 'get'
  })
}


//人员列表
export const aGetUsersList = () => {
    return axios.request({
      url: 'PurchasesInfo/PurchasesInfo/UsersList',
      method: 'get'
    })
}

//获取数据
export const aGetShow = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/aShow?id=' + data,
    method: 'get',
  })
}

//编辑
export const aGetEdit = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/aEdit',
    method: 'post',
    data
  })
}

// delete department
export const adeleteDepartment = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/adelete/' + ids,
    method: 'get'
  })
}

// batch command
export const abatchCommand = (data) => {
  return axios.request({
    url: 'PurchasesInfo/PurchasesInfo/abatch',
    method: 'get',
    params: data
  })
} 
