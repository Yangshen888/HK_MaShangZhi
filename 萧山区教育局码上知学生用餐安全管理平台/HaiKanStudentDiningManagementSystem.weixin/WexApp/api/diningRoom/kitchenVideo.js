import http from '@/utils/http.js'

  //监控
export const showVideo = (data) => {
	return http.httpRequest({
		url: 'api/v1/diningRoom/kitchenVideo/showVideo?type='+data.type+'&uuid='+data.uuid+'&time='+data.time,
		method: 'get'
	})
}

export const createDinningvideo = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/diningRoom/kitchenVideo/create',
		method: 'post'
	},data)
}