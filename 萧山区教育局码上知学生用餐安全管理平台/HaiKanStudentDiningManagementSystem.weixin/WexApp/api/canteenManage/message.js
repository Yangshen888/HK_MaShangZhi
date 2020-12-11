import http from '@/utils/http.js'

export const messageList = (data) => {
	return http.httpRequest({
		url: 'api/v1/canteenManagement/appMessage/MessageList',
		method: 'post'
	},data)
}

export const messageshowList = (data) => {
	return http.httpRequest({
		url: 'api/v1/canteenManagement/appMessage/MessageshowList',
		method: 'post'
	},data)
}