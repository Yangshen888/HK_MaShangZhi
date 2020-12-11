import http from '@/utils/http.js'

export const getlist = (data) => {
	return http.httpRequest({
		url: 'api/v1/Quality/AppQuality/FliesList',
		method: 'post'
	},data)
}


//详情
export const getFileInfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/Quality/AppQuality/FlieInfo?uuid='+data,
		method: 'get'
	})
}
