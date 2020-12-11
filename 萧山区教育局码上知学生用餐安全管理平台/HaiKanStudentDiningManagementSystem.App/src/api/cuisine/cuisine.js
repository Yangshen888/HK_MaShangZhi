import axios from '@/libs/api.request'

//列表
export const CuisineList = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/CuisineList',
    method: 'post',
    data
  })
}
 
//添加
export const CuisineCreate = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/CuisineCreate',
    method: 'post',
    data
  })
}

  //获取学校
  export const SchoolList = () => {
    return axios.request({
      url: 'Cuisine/Cuisine/SchoolList',
      method: 'get',
    })
  }

  //验证菜品
  export const getiscuisine = (data) => {
    return axios.request({
      url: 'Cuisine/Cuisine/iscuisine?name='+data,
      method: 'get',
    })
  }

//获取食材
export const IngredientGet = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/IngredientGet?guid='+data,
    method: 'get',
  })
}

//获取食材
export const IngredientGet2 = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/IngredientGet2?guid='+data,
    method: 'get',
  })
}

//获取数据
export const CuisineGet = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/CuisineGet?guid=' + data,
    method: 'get',
  })
}

//编辑
export const CuisineEdit = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/CuisineEdit',
    method: 'post',
    data
  })
}

// delete department
export const deleteDepartment = (ids) => {
  return axios.request({
    url: 'Cuisine/Cuisine/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/batch',
    method: 'get',
    params: data
  })
} 

//导入
export const CuisineImport = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/CuisineImport',
    method: 'post',
    data
  })
}

//导出
export const CuisineExport = (ids) => {
  return axios.request({
    url: 'Cuisine/Cuisine/ExportPass?ids=' + ids,
    method: 'get'
  })
}

//附件上传
// export const getRegistPicture = (data) => {
//   return axios.request({
//     url: 'picture/picture/registpicture',
//     method: 'post',
//     data
//   })
// }


//菜品下拉列表
export const cuisineSelectList = () => {
  return axios.request({
    url: 'Cuisine/Cuisine/list',
    method: 'get',
  })
}

export const deletetoFile = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/DeleteFile',
    method: 'post',
    data
  })
}

export const getRegistPicture = (data) => {
  return axios.request({
    url: 'Cuisine/Cuisine/UpLoad',
    method: 'post',
    data
  })
}