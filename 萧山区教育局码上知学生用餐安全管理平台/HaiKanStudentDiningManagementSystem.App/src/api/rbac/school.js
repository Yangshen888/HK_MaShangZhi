import axios from '@/libs/api.request'

export const getSchoolList = (data) => {
  return axios.request({
    url: 'rbac/school/list',
    method: 'post',
    data
  })
}

export const getSchoolList2 = () => {
  return axios.request({
    url: 'rbac/school/list',
    method: 'get',
  })
}
export const getSchoolListtoken = () => {
  return axios.request({
    url: 'rbac/school/list2',
    method: 'get',
  })
}

export const createSchool = (data) => {
  return axios.request({
    url: 'rbac/school/create',
    method: 'post',
    data
  })
}

export const loadSchool = (data) => {
  return axios.request({
    url: 'rbac/school/edit/' + data.guid,
    method: 'get'
  })
}

export const editSchool = (data) => {
  return axios.request({
    url: 'rbac/school/edit',
    method: 'post',
    data
  })
}

export const deleteSchool = (ids) => {
  return axios.request({
    url: 'rbac/school/delete/' + ids,
    method: 'get'
  })
}


export const batchCommand = (data) => {
  return axios.request({
    url: 'rbac/school/batch',
    method: 'get',
    params: data
  })
}

export const deleteFile = (data) => {
  return axios.request({
    url: 'rbac/school/DeleteFile',
    method: 'post',
    data
  })
}

export const dataToSync = () => {
  return axios.request({
    url: 'rbac/school/ToSync',
    method: 'post',
  })
}


export const getWxMenu = () => {
  return axios.request({
    url: 'rbac/school/GetWXMenuList',
    method: 'get',
  })
}