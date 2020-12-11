import axios from '@/libs/api.request'
//获取信息
export const getMessageBoardList = data => {
  return axios.request({
    url: 'messagefeedback/report/list',
    method: "post",
    data
  });
};
// loadmessageboard
export const loadMessageboard = (data) => {
  return axios.request({
    url: 'messagefeedback/report/edit/'+data.guid,
    method: 'get',
  })
}
// edit
export const editMessageboard = (data) => {
  return axios.request({
    url: 'messagefeedback/report/edit',
    method: 'post',
    data
  })
}
