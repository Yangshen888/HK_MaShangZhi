import http from '@/utils/http.js'

export const getcuisineList = (data) => {
	return http.httpRequest({
		url: 'api/v1/cuisine/appcuisine/List',
		method: 'post'
	},data)
}

export const getGivelike = (data) => {
	return http.httpRequest({
		url: 'api/v1/cuisine/appcuisine/Givelike?guid='+data,
		method: 'get'
	})
}

export const getcuisine = (data) => {
	return http.httpRequest({
		url: 'api/v1/cuisine/appcuisine/GetCuisine?guid='+data,
		method: 'get'
	})
}
