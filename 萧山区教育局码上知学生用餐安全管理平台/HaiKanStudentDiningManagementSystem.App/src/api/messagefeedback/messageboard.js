import axios from '@/libs/api.request'
//获取信息
export const getMessageBoardList = data => {
  return axios.request({
    url: 'messagefeedback/messageboard/list',
    method: "post",
    data
  });
};
// loadmessageboard
export const loadMessageboard = (data) => {
  return axios.request({
    url: 'messagefeedback/messageboard/edit/'+data.guid,
    method: 'get',
  })
}
// edit
export const editMessageboard = (data) => {
  return axios.request({
    url: 'messagefeedback/messageboard/edit',
    method: 'post',
    data
  })
}
