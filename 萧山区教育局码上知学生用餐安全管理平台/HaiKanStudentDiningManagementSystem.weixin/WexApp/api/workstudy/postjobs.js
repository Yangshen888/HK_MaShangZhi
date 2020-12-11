import http from '@/utils/http.js'

export const getPostjobsList = (data) => {
	return http.httpRequest({
		url: 'api/v1/workstudy/wxpostjobs/List',
		method: 'post'
	},data)
}
export const getPostjobsAppealList = (data) => {
	return http.httpRequest({
		url: 'api/v1/workstudy/wxpostjobs/AppealList',
		method: 'post'
	},data)
}
export const getPostjobsDetial = (data) => {
	return http.httpRequest({
		url: 'api/v1/workstudy/wxpostjobs/PostjobsDetial/'+data.guid,
		method: 'get'
	})
}
export const createPostjobsAppeal = (data) => {
	return http.httpRequest({
		url: 'api/v1/workstudy/wxpostjobs/create',
		method: 'post'
	},data)
}