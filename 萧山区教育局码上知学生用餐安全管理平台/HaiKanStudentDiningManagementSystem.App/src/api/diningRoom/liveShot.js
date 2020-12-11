import axios from '@/libs/api.request'

export const getLiveShotList = (data) => {
  return axios.request({
    url: 'diningRoom/liveShot/list',
    method: 'post',
    data
  })
}

// createLiveShot
export const createLiveShot = (data) => {
  return axios.request({
    url: 'diningRoom/liveShot/create',
    method: 'post',
    data
  })
}

//loadLiveShot
export const loadLiveShot = (data) => {
  return axios.request({
    url: 'diningRoom/liveShot/edit/' + data.guid,
    method: 'get'
  })
}

// editLiveShot
export const editLiveShot = (data) => {
  return axios.request({
    url: 'diningRoom/liveShot/edit',
    method: 'post',
    data
  })
}

// delete diningRoom
export const deleteLiveShot = (ids) => {
  return axios.request({
    url: 'diningRoom/liveShot/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'diningRoom/liveShot/batch',
    method: 'get',
    params: data
  })
}

export const deletetoFile = (data) => {
  return axios.request({
    url: 'diningRoom/liveShot/DeleteFile',
    method: 'post',
    data
  })
}

export const getRegistPicture = (data) => {
  return axios.request({
    url: 'diningRoom/liveShot/UpLoad',
    method: 'post',
    data
  })
}

export const cuisineSelectList = (data) => {
  return axios.request({
    url: 'diningRoom/liveShot/CuisineList?datetime='+data.date+"&type="+data.type,
    method: 'get',
  })
}