import axios from '@/libs/api.request'

export const getIngredientList = (data) => {
  return axios.request({
    url: 'ingredient/ingredient/list',
    method: 'post',
    data
  })
}

// createIngredient
export const createIngredient = (data) => {
  return axios.request({
    url: 'ingredient/ingredient/create',
    method: 'post',
    data
  })
}

//loadIngredient
export const loadIngredient = (data) => {
  return axios.request({
    url: 'ingredient/ingredient/edit/' + data.guid,
    method: 'get'
  })
}

// editIngredient
export const editIngredient = (data) => {
  return axios.request({
    url: 'ingredient/ingredient/edit',
    method: 'post',
    data
  })
}

// delete ingredient
export const deleteIngredient = (ids) => {
  return axios.request({
    url: 'ingredient/ingredient/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'ingredient/ingredient/batch',
    method: 'get',
    params: data
  })
}

export const loadIngredient2 = (data) => {
  return axios.request({
    url: 'ingredient/ingredient/get/' + data.name,
    method: 'get'
  })
}
export const loadIngredient3 = (data) => {
  return axios.request({
    url: 'ingredient/ingredient/get2/' + data.name,
    method: 'get'
  })
}

export const deletetoFile = (data) => {
  return axios.request({
    url: 'ingredient/ingredient/DeleteFile',
    method: 'post',
    data
  })
}

export const getRegistPicture = (data) => {
  return axios.request({
    url: 'ingredient/ingredient/UpLoad',
    method: 'post',
    data
  })
}


//FoodTypeList
export const GetFoodTypeList = () => {
  return axios.request({
    url: 'ingredient/ingredient/FoodTypeList',
    method: 'get'
  })
}