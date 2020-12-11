import axios from '@/libs/api.request'

export const getKitchenVideoList = (data) => {
  return axios.request({
    url: 'diningRoom/kitchenVideo/list',
    method: 'post',
    data
  })
}

// createKitchenVideo
export const createKitchenVideo = (data) => {
  return axios.request({
    url: 'diningRoom/kitchenVideo/create',
    method: 'post',
    data
  })
}

//loadKitchenVideo
export const loadKitchenVideo = (data) => {
  return axios.request({
    url: 'diningRoom/kitchenVideo/edit/' + data.guid,
    method: 'get'
  })
}

// editKitchenVideo
export const editKitchenVideo = (data) => {
  return axios.request({
    url: 'diningRoom/kitchenVideo/edit',
    method: 'post',
    data
  })
}

// delete diningRoom
export const deleteKitchenVideo = (ids) => {
  return axios.request({
    url: 'diningRoom/kitchenVideo/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'diningRoom/kitchenVideo/batch',
    method: 'get',
    params: data
  })
}

export const deletetoFile = (data) => {
    return axios.request({
      url: 'diningRoom/kitchenVideo/DeleteFile',
      method: 'post',
      data
    })
  }