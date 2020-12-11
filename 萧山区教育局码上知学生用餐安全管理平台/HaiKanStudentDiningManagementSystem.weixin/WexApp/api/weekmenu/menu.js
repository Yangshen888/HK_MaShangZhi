import http from '@/utils/http.js'

export const getweekmenuList = (data) => {
	return http.httpRequest({
		url: 'api/v1/menu/appweekmenu/List',
		method: 'post'
	},data)
}

export const getMenuDateList = (data) => {
	return http.httpRequest({
		url: 'api/v1/menu/appweekmenu/MenuDateList',
		method: 'post'
	},data)
}

export const weektime = (data) => {
	return http.httpRequest({
		url: 'api/v1/menu/appweekmenu/time?guid='+data,
		method: 'get'
	})
}

export const Getweekmenu = (data) => {
	return http.httpRequest({
		url: 'api/v1/menu/appweekmenu/Getweekmenu?guid='+data,
		method: 'get'
	})
}
