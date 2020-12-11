import axios from '@/libs/api.request'

//列表
export const GetList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Synthesize/List',
    method: 'post',
    data
  })
}
 
//添加
export const GetCreate = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Synthesize/Create',
    method: 'post',
    data
  })
}

//主题列表
export const GetTypesList = () => {
  return axios.request({
    url: 'PurchasesInfo/Synthesize/TypesList',
    method: 'get'
  })
}

//部门列表
export const GetDepartList = () => {
    return axios.request({
      url: 'PurchasesInfo/Synthesize/DepartList',
      method: 'get'
    })
}

//人员列表
export const GetUsersList = () => {
    return axios.request({
      url: 'PurchasesInfo/Synthesize/UsersList',
      method: 'get'
    })
}

//获取数据
export const GetShow = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Synthesize/Show?id=' + data,
    method: 'get',
  })
}

//编辑
export const GetEdit = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Synthesize/Edit',
    method: 'post',
    data
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/Synthesize/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Synthesize/batch',
    method: 'get',
    params: data
  })
} 