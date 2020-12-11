import axios from '@/libs/api.request'

//列表
export const GetList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/waste/List',
    method: 'post',
    data
  })
}
 
//添加
export const GetCreate = (data) => {
  return axios.request({
    url: 'PurchasesInfo/waste/Create',
    method: 'post',
    data
  })
}

//单位下拉框
export const GetPartList = () => {
  return axios.request({
    url: 'PurchasesInfo/waste/PartList',
    method: 'get'
  })
}

//获取数据
export const GetShow = (data) => {
  return axios.request({
    url: 'PurchasesInfo/waste/Show?id=' + data,
    method: 'get',
  })
}

//编辑
export const GetEdit = (data) => {
  return axios.request({
    url: 'PurchasesInfo/waste/Edit',
    method: 'post',
    data
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/waste/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'PurchasesInfo/waste/batch',
    method: 'get',
    params: data
  })
} 




