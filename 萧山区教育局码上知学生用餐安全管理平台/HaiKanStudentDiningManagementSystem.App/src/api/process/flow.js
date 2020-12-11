import axios from '@/libs/api.request'

export const Getlist = (data) => {
  return axios.request({
    url: 'foodprocess/flow/list',
    method: 'post',
    data
  })
}


export const GetCreate = (data) => {
  return axios.request({
    url: 'foodprocess/flow/Create',
    method: 'post',
    data
  })
}

//获取数据
export const GetShow = (data) => {
  return axios.request({
    url: 'foodprocess/flow/show?guid=' + data,
    method: 'get',
  })
}

//编辑
export const GetEdit = (data) => {
  return axios.request({
    url: 'foodprocess/flow/Edit',
    method: 'post',
    data
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'foodprocess/flow/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'foodprocess/flow/batch',
    method: 'get',
    params: data
  })
}

export const deletetoFile = (data) => {
    return axios.request({
      url: 'foodprocess/flow/DeleteFile',
      method: 'post',
      data
    })
  }
  
  export const getRegistPicture = (data) => {
    return axios.request({
      url: 'foodprocess/flow/UpLoad',
      method: 'post',
      data
    })
  }