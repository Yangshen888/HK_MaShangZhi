import axios from '@/libs/api.request'

//列表
export const GetList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Pesticide/List',
    method: 'post',
    data
  })
}
 
//添加
export const GetCreate = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Pesticide/Create',
    method: 'post',
    data
  })
}

//果蔬下拉框
export const GetVList = () => {
  return axios.request({
    url: 'PurchasesInfo/Pesticide/VList',
    method: 'get'
  })
}

//获取数据
export const GetShow = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Pesticide/Show?id=' + data,
    method: 'get',
  })
}

//编辑
export const GetEdit = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Pesticide/Edit',
    method: 'post',
    data
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/Pesticide/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Pesticide/batch',
    method: 'get',
    params: data
  })
} 




