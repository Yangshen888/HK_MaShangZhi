import axios from '@/libs/api.request'
//获取信息
export const getPripostjobsList = data => {
  return axios.request({
    url: 'workstudy/postjobs/list',
    method: "post",
    data
  });
};
// createPostjobs
export const createPostjobs = (data) => {
  return axios.request({
    url: 'workstudy/postjobs/create',
    method: 'post',
    data
  })
}
// loadeditPostjobs
export const loadPostjobs = (data) => {
  return axios.request({
    url: 'workstudy/postjobs/edit/'+data.guid,
    method: 'get',
  })
}
// edit
export const editPostjobs = (data) => {
  return axios.request({
    url: 'workstudy/postjobs/edit',
    method: 'post',
    data
  })
}
// delete
export const getPostjobsDel = (ids) => {
  return axios.request({
    url: 'workstudy/postjobs/delete/'+ids,
    method: 'get',
  })
}
// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'workstudy/postjobs/batch',
    method: 'get',
    params: data
  })
}
