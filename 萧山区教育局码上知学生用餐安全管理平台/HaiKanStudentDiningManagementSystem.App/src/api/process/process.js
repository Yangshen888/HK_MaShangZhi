import axios from '@/libs/api.request'

export const getProcessList = (data) => {
    return axios.request({
      url: 'foodprocess/process/list',
      method: 'post',
      data
    })
  }
  
  // createProcess
  export const createProcess = (data) => {
    return axios.request({
      url: 'foodprocess/process/create',
      method: 'post',
      data
    })
  }
  
  //loadProcess
  export const loadProcess = (data) => {
    return axios.request({
      url: 'foodprocess/process/edit/' + data.guid,
      method: 'get'
    })
  }
  
  // editProcess
  export const editProcess = (data) => {
    return axios.request({
      url: 'foodprocess/process/edit',
      method: 'post',
      data
    })
  }
  
  // delete process
  export const deleteProcess = (ids) => {
    return axios.request({
      url: 'foodprocess/process/delete/' + ids,
      method: 'get'
    })
  }
  
  // batch command
  export const batchCommand = (data) => {
    return axios.request({
      url: 'foodprocess/process/batch',
      method: 'get',
      params: data
    })
  }