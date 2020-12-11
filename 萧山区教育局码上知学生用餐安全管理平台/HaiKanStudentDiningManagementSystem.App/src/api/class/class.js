import axios from '@/libs/api.request'

//列表
export const ClassList = (data) => {
  return axios.request({
    url: 'Class/Class/List',
    method: 'post',
    data
  })
}
 
//添加
export const ClassCreate = (data) => {
  return axios.request({
    url: 'Class/Class/Create',
    method: 'post',
    data
  })
}


//load
export const loadClass = (data) => {
    return axios.request({
      url: 'Class/Class/Load/' + data.guid,
      method: 'get'
    })
  }

//编辑
export const ClassEdit = (data) => {
  return axios.request({
    url: 'Class/Class/Edit',
    method: 'post',
    data
  })
}

// delete department
export const deleteClass = (ids) => {
  return axios.request({
    url: 'Class/Class/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const ClassBatchCommand = (data) => {
  return axios.request({
    url: 'Class/Class/batch',
    method: 'get',
    params: data
  })
} 

//导入
// export const ClassImport = (data) => {
//   return axios.request({
//     url: 'Class/Class/ClassImport',
//     method: 'post',
//     data
//   })
// }




