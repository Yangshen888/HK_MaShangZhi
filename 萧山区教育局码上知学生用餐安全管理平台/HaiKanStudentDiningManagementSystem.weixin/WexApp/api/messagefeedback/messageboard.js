import http from '@/utils/http.js'
export const createMessageboard = (data) => {
	return http.httpRequest({
		url: 'api/v1/messagefeedback/wxmessageboard/create',
		method: 'post'
	},data)
}
export const getMessagebacklist = (data) => {
	return http.httpRequest({
		url: 'api/v1/messagefeedback/wxmessageboard/list',
		method: 'post'
	},data)
}
export const createReport = (data) => {
	return http.httpRequest({
		url: 'api/v1/messagefeedback/wxmessageboard/createreport',
		method: 'post'
	},data)
}