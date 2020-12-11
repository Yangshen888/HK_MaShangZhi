import axios from '@/libs/api.request'

//列表
export const GetList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Samples/List',
    method: 'post',
    data
  })
}
 
//添加
export const GetCreate = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Samples/Create',
    method: 'post',
    data
  })
}

//餐别下拉框
export const GetTypeList = () => {
  return axios.request({
    url: 'PurchasesInfo/Samples/TypeList',
    method: 'get'
  })
}

//菜品下拉框
export const GetfoodList = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Samples/foodList?type='+data,
    method: 'get'
  })
}

//获取数据
export const GetShow = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Samples/Show?id=' + data,
    method: 'get',
  })
}

//编辑
export const GetEdit = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Samples/Edit',
    method: 'post',
    data
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'PurchasesInfo/Samples/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'PurchasesInfo/Samples/batch',
    method: 'get',
    params: data
  })
} 




