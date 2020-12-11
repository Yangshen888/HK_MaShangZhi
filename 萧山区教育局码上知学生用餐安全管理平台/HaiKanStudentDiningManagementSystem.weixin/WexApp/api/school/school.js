import http from '@/utils/http.js'

export const getSchoolList = () => {
	return http.httpRequest({
		url: 'api/v1/rbac/school/List1',
		method: 'get'
	})
}

export const getSchoolLink = (data) => {
	return http.httpRequest({
		url: 'api/v1/rbac/school/getLink?uuid='+data,
		method: 'get'
	})
}

export const getSchoolInfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/rbac/school/GetSchoolInfo?name='+data,
		method: 'get'
	})
}

export const getSchoolInfo2 = (data) => {
	return http.httpRequest({
		url: 'api/v1/rbac/school/GetSchoolInfo2?guid='+data,
		method: 'get'
	})
}