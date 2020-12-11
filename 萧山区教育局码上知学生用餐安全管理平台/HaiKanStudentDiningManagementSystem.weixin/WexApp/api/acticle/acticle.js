import http from '@/utils/http.js'

export const GetAppList = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/Acticle/Acticle/GetAppList?guid='+data,
		method: 'get'
	})
}

export const GetAppShow = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/Acticle/Acticle/GetAppShow?guid='+data,
		method: 'get'
	})
}
