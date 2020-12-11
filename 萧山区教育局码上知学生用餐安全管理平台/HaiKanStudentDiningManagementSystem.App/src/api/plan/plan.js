import axios from '@/libs/api.request'

//列表
export const ManagePlanList = (data) => {
  return axios.request({
    url: 'canteenManagement/managePlan/ManagePlanList',
    method: 'post',
    data
  })
}
  //添加
  export const Create = (data) => {
    return axios.request({
      url: 'canteenManagement/managePlan/Create',
      method: 'post',
      data
    })
  }
  
  // delete
export const Delete = (ids) => {
  return axios.request({
    url: 'canteenManagement/managePlan/Delete/' + ids,
    method: 'get'
  })
}



// batch command
export const Batch = (data) => {
return axios.request({
  url: 'canteenManagement/managePlan/batch',
  method: 'get',
  params: data
})
}
 // 编辑
 export const Edit = (data) => {
  return axios.request({
    url: 'canteenManagement/managePlan/Edit',
    method: 'post',
    data
  })
}

  //获取数据
  export const PlanGet = (data) => {
    return axios.request({
      url: 'canteenManagement/managePlan/PlanGet?guid=' + data,
      method: 'get',
    })
  }
