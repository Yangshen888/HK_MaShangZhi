import axios from '@/libs/api.request'

//列表
export const time = () => {
  return axios.request({
    url: 'Menu/WeekMenu/time',
    method: 'post'
  })
}

//菜品下拉框
export const cuisineList = (data) => {
    return axios.request({
      url: 'Menu/WeekMenu/cuisineList?guid='+data,
      method: 'get'
    })
  }

  //添加
export const WeekMenuCreate = (data) => {
    return axios.request({
      url: 'Menu/WeekMenu/WeekMenuCreate',
      method: 'post',
      data
    })
  }

    //获取菜单数据
export const Getweekmenu = (data) => {
  return axios.request({
    url: 'Menu/WeekMenu/Getweekmenu?time='+data.time+"&guid="+data.guid,
    method: 'get',
    data
  })
}

  //编辑
  export const WeekMenuEdit = (data) => {
    return axios.request({
      url: 'Menu/WeekMenu/WeekMenuEdit',
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