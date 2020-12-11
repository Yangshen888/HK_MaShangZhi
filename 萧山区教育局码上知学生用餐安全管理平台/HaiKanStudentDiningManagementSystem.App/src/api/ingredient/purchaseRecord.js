import axios from '@/libs/api.request'

export const getPurchaseRecordList = (data) => {
  return axios.request({
    url: 'ingredient/purchaseRecord/list',
    method: 'post',
    data
  })
}

// createPurchaseRecord
export const createPurchaseRecord = (data) => {
  return axios.request({
    url: 'ingredient/purchaseRecord/create',
    method: 'post',
    data
  })
}

//loadPurchaseRecord
export const loadPurchaseRecord = (data) => {
  return axios.request({
    url: 'ingredient/purchaseRecord/edit/' + data.guid,
    method: 'get'
  })
}

// editPurchaseRecord
export const editPurchaseRecord = (data) => {
  return axios.request({
    url: 'ingredient/purchaseRecord/edit',
    method: 'post',
    data
  })
}

// delete ingredient
export const deletePurchaseRecord = (ids) => {
  return axios.request({
    url: 'ingredient/purchaseRecord/delete/' + ids,
    method: 'get'
  })
}

// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'ingredient/purchaseRecord/batch',
    method: 'get',
    params: data
  })
}

export const deletetoFile = (data) => {
  return axios.request({
    url: 'Ingredient/PurchaseRecord/DeleteFile',
    method: 'post',
    data
  })
}

export const loadIngredient = (data) => {
  return axios.request({
    url: 'ingredient/purchaseRecord/Load?name=' + data,
    method: 'get'
  })
}

//导入
export const GetImport = (data) => {
  return axios.request({
    url: 'ingredient/purchaseRecord/Import',
    method: 'post',
    data
  })
}

//导出
export const GetExportPass = (ids) => {
  return axios.request({
    url: 'ingredient/purchaseRecord/ExportPass' ,
    method: 'get'
  })
}


//loadPurchaseRecord
export const GetGetCanteen = () => {
  return axios.request({
    url: 'ingredient/purchaseRecord/GetCanteen',
    method: 'get'
  })
}