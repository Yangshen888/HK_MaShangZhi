import axios from '@/libs/api.request'

export const getDepartmentList = (data) => {
  return axios.request({
    url: 'rbac/department/list',
    method: 'post',
    data
  })
}
// createDepartment
export const createDepartment = (data) => {
  return axios.request({
    url: 'rbac/department/create',
    method: 'post',
    data
  })
}

//loadDepartment
export const loadDepartment = (data) => {
  return axios.request({
    url: 'rbac/department/edit/' + data.guid,
    method: 'get'
  })
}

// editDepartment
export const editDepartment = (data) => {
  return axios.request({
    url: 'rbac/department/edit',
    method: 'post',
    data
  })
}
//get departmentlist
export const loaddepartmentListDetail =() => {
  return axios.request({
    url: 'rbac/department/DepartmentList',
    method: 'get'
  })
}
// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'rbac/department/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'rbac/department/batch',
    method: 'get',
    params: data
  })
}
