import axios from '@/libs/api.request'
 
//列表
export const SchoolJourList = (data) => {
  return axios.request({
    url: 'NewsReport/SchoolJour/SchoolJourList',
    method: 'post',
    data
  })
}
 
//添加
export const Create = (data) => {
  return axios.request({
    url: 'NewsReport/SchoolJour/Create',
    method: 'post',
    data
  })
}
 
 
//获取数据
export const SchoolJourGet = (data) => {
  return axios.request({
    url: 'NewsReport/SchoolJour/SchoolJourGet?guid=' + data,
    method: 'get',
  })
}
 
//编辑
export const SchoolJourEdit = (data) => {
  return axios.request({
    url: 'NewsReport/SchoolJour/SchoolJourEdit',
    method: 'post',
    data
  })
}
 
// 
export const Delete = (ids) => {
  return axios.request({
    url: 'NewsReport/SchoolJour/delete/' + ids,
    method: 'get'
  })
}
 
// batch command
export const Batch = (data) => {
  return axios.request({
    url: 'NewsReport/SchoolJour/batch',
    method: 'get',
    params: data
  })
}
 
//附件上传
export const DeletetoFile = (data) => {
  return axios.request({
    url: 'NewsReport/SchoolJour/DeletetoFile',
    method: 'post',
    data
  })
}