import http from '@/utils/http.js'

export const getTimeHorizon = () => {
	return http.httpRequest({
		url: 'api/v1/DiningRoom/LiveShot/GeTimeHorizon',
		method: 'get'
	})
}

export const getPictureList = (data) => {
	return http.httpRequest({
		url: 'api/v1/DiningRoom/LiveShot/GetPictureList?date='+data.date+"&uuid="+data.uuid+"&type="+data.type+"&change="+data.change,
		method: 'get'
	})
}

export const getLiveShotInfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/DiningRoom/LiveShot/GeLiveShotInfo?guid='+data,
		method: 'get'
	})
}

export const CuisineList = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/DiningRoom/LiveShot/CuisineList?datetime='+data.datetime+"&type="+data.type,
		method: 'get'
	})
}

export const createLiveShot = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/DiningRoom/LiveShot/create',
		method: 'post'
	},data)
}

// export const getGivelike = (data) => {
// 	return http.httpRequest({
// 		url: 'api/v1/cuisine/appcuisine/Givelike?guid='+data,
// 		method: 'get'
// 	})
// }

// export const getcuisine = (data) => {
// 	return http.httpRequest({
// 		url: 'api/v1/cuisine/appcuisine/GetCuisine?guid='+data,
// 		method: 'get'
// 	})
// }