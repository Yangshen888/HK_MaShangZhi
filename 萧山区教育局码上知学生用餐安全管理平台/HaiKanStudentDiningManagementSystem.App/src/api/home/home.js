import axios from '@/libs/api.request'

export const getprice = () => {
  return axios.request({
    url: 'home/home/price',
    method: 'get'
  })
}
