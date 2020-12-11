import http from '@/utils/http.js'

export const GetUserList = (data) => {
	return http.httpRequest({
		url: 'api/v1/UserInfo/UserInfo/GetUserList',
		method: 'post'
	},data)
}

export const GetUserInfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/UserInfo/UserInfo/GetUserInfo?id='+data,
		method: 'get'
	})
}