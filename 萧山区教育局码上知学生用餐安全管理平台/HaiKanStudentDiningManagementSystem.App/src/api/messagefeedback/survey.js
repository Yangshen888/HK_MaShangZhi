import axios from '@/libs/api.request'
//获取信息
export const getSurveyList = data => {
  return axios.request({
    url: 'messagefeedback/survey/list',
    method: "post",
    data
  });
};
export const createSurvey = data => {
  return axios.request({
    url: 'messagefeedback/survey/create',
    method: "post",
    data
  });
};
//OpenOrClose
export const OpenOrClose = (data) => {
  return axios.request({
    url: 'messagefeedback/survey/OpenOrClose/'+data.guid,
    method: 'get',
  })
}
// loadSurvey
export const loadSurvey = (data) => {
  return axios.request({
    url: 'messagefeedback/survey/edit/'+data.guid,
    method: 'get',
  })
}
// loadSurveyDetail
export const loadSurveyDetail = (data) => {
  return axios.request({
    url: 'messagefeedback/survey/surveydetail/'+data.guid,
    method: 'get',
  })
}
// edit
export const editSurvey = (data) => {
  return axios.request({
    url: 'messagefeedback/survey/edit',
    method: 'post',
    data
  })
}
// delete
export const getSurveyDel = (ids) => {
  return axios.request({
    url: 'messagefeedback/survey/delete/'+ids,
    method: 'get',
  })
}
// batch command
export const batchCommand = (data) => {
  return axios.request({
    url: 'messagefeedback/survey/batch',
    method: 'get',
    params: data
  })
}







//获取信息
export const getSurveyQuestionList = data => {
  return axios.request({
    url: 'messagefeedback/survey/listquestion/'+data.guid,
    method: "get",
  });
};
export const createSurveyQuestion = data => {
  return axios.request({
    url: 'messagefeedback/survey/createquestion',
    method: "post",
    data
  });
};
// loadSurveyQuestion
export const loadSurveyQuestion = (data) => {
  return axios.request({
    url: 'messagefeedback/survey/editquestion/'+data.guid,
    method: 'get',
  })
}
// edit
export const editSurveyQuestion = (data) => {
  return axios.request({
    url: 'messagefeedback/survey/editquestion',
    method: 'post',
    data
  })
}
// delete
export const getSurveyQuestionDel = (ids) => {
  return axios.request({
    url: 'messagefeedback/survey/deletequestion/'+ids,
    method: 'get',
  })
}





//获取信息
export const getSurveyQuestionItemList = data => {
  return axios.request({
    url: 'messagefeedback/survey/listquestionitem/'+data.guid,
    method: "get",
  });
};
export const createSurveyQuestionItem = data => {
  return axios.request({
    url: 'messagefeedback/survey/createquestionitem',
    method: "post",
    data
  });
};
// loadSurveyQuestion
export const loadSurveyQuestionItem = (data) => {
  return axios.request({
    url: 'messagefeedback/survey/editquestionitem/'+data.guid,
    method: 'get',
  })
}
// edit
export const editSurveyQuestionItem = (data) => {
  return axios.request({
    url: 'messagefeedback/survey/editquestionitem',
    method: 'post',
    data
  })
}
// delete
export const getSurveyQuestionItemDel = (ids) => {
  return axios.request({
    url: 'messagefeedback/survey/deletequestionitem/'+ids,
    method: 'get',
  })
}
