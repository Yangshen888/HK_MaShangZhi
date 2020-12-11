import axios from '@/libs/api.request'
//获取信息
export const getPostjobsAppealList = data => {
  return axios.request({
    url: 'workstudy/postjobsappeal/list',
    method: "post",
    data
  });
};
//审核通过
export const doAppealPass = data => {
  return axios.request({
    url: 'workstudy/postjobsappeal/auditpass/'+data.guid,
    method: "get",
    data
  });
};
//审核不通过
export const doAppealNoPass = data => {
  return axios.request({
    url: 'workstudy/postjobsappeal/auditnopass/'+data.guid,
    method: "get",
    data
  });
};


