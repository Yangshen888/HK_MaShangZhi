import axios from '@/libs/api.request'

//列表M
export const MoneyList = (data) => {
  return axios.request({
    url: 'canteenManagement/money/MoneyList',
    method: 'post',
    data
  })
}

  //添加
  export const Create = (data) => {
    return axios.request({
      url: 'canteenManagement/money/Create',
      method: 'post',
      data
    })
  }

   // 编辑
export const MoneyEdit = (data) => {
    return axios.request({
      url: 'canteenManagement/money/MoneyEdit',
      method: 'post',
      data
    })
  }

  
  // delete
export const Delete = (ids) => {
    return axios.request({
      url: 'canteenManagement/money/Delete/' + ids,
      method: 'get'
    })
  }

    // batch command
export const Batch = (data) => {
    return axios.request({
      url: 'canteenManagement/money/Batch',
      method: 'get',
      params: data
    })
  }

    //获取数据
export const MoneyGet = (data) => {
    return axios.request({
      url: 'canteenManagement/money/MoneyGet?guid=' + data,
      method: 'get',
    })
  }
  //导入
export const Import = (data) => {
  return axios.request({
    url: 'canteenManagement/money/Import',
    method: 'post',
    data
  })
}
//导出
export const Export = (ids) => {
  return axios.request({
    url: 'canteenManagement/money/Export?ids=' + ids,
    method: 'get'
  })
}
 