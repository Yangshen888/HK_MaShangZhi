import http from '@/utils/http.js'

export const GetAppCreate = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/FoodProcess/Flow/AppCreate',
		method: 'post'
	},data)
}

export const GetAppList = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/FoodProcess/Flow/AppList',
		method: 'post'
	},data)
}


export const Getscreen = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/FoodProcess/Flow/Getscreen',
		method: 'post'
	},data)
}

export const Getwlcreate = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/FoodProcess/Flow/wlcreate',
		method: 'post'
	},data)
}

