import axios from '@/libs/api.request'

//列表
export const qualityList = (data) => {
    return axios.request({
      url: 'Quality/Quality/QualityList',
      method: 'post',
      data
    })
  }
   
  //添加
export const Create = (data) => {
    return axios.request({
      url: 'Quality/Quality/Create',
      method: 'post',
      data
    })
  }

  //获取数据
export const QualityGet = (data) => {
    return axios.request({
      url: 'Quality/Quality/QualityGet?guid=' + data,
      method: 'get',
    })
  }

 // 编辑
export const QualityEdit = (data) => {
    return axios.request({
      url: 'Quality/Quality/QualityEdit',
      method: 'post',
      data
    })
  }



  // delete
export const Delete = (ids) => {
    return axios.request({
      url: 'Quality/Quality/Delete/' + ids,
      method: 'get'
    })
  }


  // batch command
export const Batch = (data) => {
  return axios.request({
    url: 'Quality/Quality/batch',
    method: 'get',
    params: data
  })
}

export const deletetoFile = (data) => {
  return axios.request({
    url: 'Quality/Quality/DeleteFile',
    method: 'post',
    data
  })
}