import axios from '@/libs/api.request'

//列表
export const GetList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/List',
    method: 'post',
    data
  })
}

//添加
export const Create = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/Create',
    method: 'post',
    data
  })
}

//获取数据
export const Load = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/Load?id=' + data,
    method: 'get',
  })
}
//编辑
export const Edit = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/Edit',
    method: 'post',
    data
  })
}

// delete department
export const Delete = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/batch',
    method: 'get',
    params: data
  })
}


export const GetInfoList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/InfoList',
    method: 'post',
    data
  })
}


//获取部门列表
export const GetDepartments = () => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/DepList',
    method: 'get'
  })
} 



//添加
export const CreateInfo = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/CreateInfo',
    method: 'post',
    data
  })
}

//获取数据
export const LoadInfo = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/LoadInfo?id=' + data,
    method: 'get',
  })
}
//编辑
export const EditInfo = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/EditInfo',
    method: 'post',
    data
  })
}

// batch command
export const batchCommand2 = (data) => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/batch2',
    method: 'get',
    params: data
  })
}


export const TypeList = () => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/Typelist',
    method: 'get',
    
  })
}


export const UsersList = () => {
  return axios.request({
    url: 'PurchasesInfo/MorningCheck/UsersList',
    method: 'get',
    
  })
}