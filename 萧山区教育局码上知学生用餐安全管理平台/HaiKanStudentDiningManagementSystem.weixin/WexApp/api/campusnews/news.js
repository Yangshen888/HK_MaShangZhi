import http from '@/utils/http.js'

export const getlist = (data) => {
	return http.httpRequest({
		url: 'api/v1/campusnewsApp/campusnews/NewsList',
		method: 'post'
	},data)
}
export const getlifelist = (data) => {
	return http.httpRequest({
		url: 'api/v1/campusnewsApp/campusnews/LifeList',
		method: 'post'
	},data)
}

export const getschoolNews = (data) => {
	return http.httpRequest({
		url: 'api/v1/campusnewsApp/campusnews/SchoolNews',
		method: 'post'
	},data)
}

export const SchoolJourGet = (data) => {
	return http.httpRequest({
		url: 'api/v1/newsReport/schooljour/SchoolJourGet?guid='+data,
		method: 'get'
	})
}

export const getGetMYNew = (data) => {
	return http.httpRequest({
		url: 'api/v1/campusnewsApp/campusnews/GetMYNew?guid='+data,
		method: 'get'
	})
}