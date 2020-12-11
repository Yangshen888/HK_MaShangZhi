import axios from '@/libs/api.request'

//列表
export const SchoolLifeList = (data) => {
    return axios.request({
      url: 'NewsReport/SchoolLife/SchoolLifeList',
      method: 'post',
      data
    })
  }
   
  //添加
export const Create = (data) => {
    return axios.request({
      url: 'NewsReport/SchoolLife/Create',
      method: 'post',
      data
    })
  }

  //获取数据
export const SchoolLifeGet = (data) => {
    return axios.request({
      url: 'NewsReport/SchoolLife/SchoolLifeGet?guid=' + data,
      method: 'get',
    })
  }

 // 编辑
export const SchoolLifeEdit = (data) => {
    return axios.request({
      url: 'NewsReport/SchoolLife/SchoolLifeEdit',
      method: 'post',
      data
    })
  }



  // delete
export const Delete = (ids) => {
    return axios.request({
      url: 'NewsReport/SchoolLife/Delete/' + ids,
      method: 'get'
    })
  }



  // batch command
export const Batch = (data) => {
  return axios.request({
    url: 'NewsReport/SchoolLife/batch',
    method: 'get',
    params: data
  })
}
//附件上传
export const DeletetoFile = (data) => {
  return axios.request({
    url: 'NewsReport/SchoolLife/DeletetoFile',
    method: 'post',
    data
  })
}