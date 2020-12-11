import http from '@/utils/http.js'

export const getSurveylist = (data) => {
	return http.httpRequest({
		url: 'api/v1/messagefeedback/wxsurvey/list',
		method: 'post'
	},data)
}
export const getSurveydetail = (data) => {
	return http.httpRequest({
		url: 'api/v1/messagefeedback/wxsurvey/surveydetail/'+data.guid,
		method: 'get'
	})
}
export const createSurveyAnswer = (data) => {
	return http.httpRequest({
		url: 'api/v1/messagefeedback/wxsurvey/create',
		method: 'post'
	},data)
}