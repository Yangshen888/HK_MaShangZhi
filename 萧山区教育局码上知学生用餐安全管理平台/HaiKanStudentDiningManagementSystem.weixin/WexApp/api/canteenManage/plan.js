import http from '@/utils/http.js'

export const planlist = (data) => {
	return http.httpRequest({
		url: 'api/v1/canteenManagement/appPlan/PlanList',
		method: 'post'
	},data)
}
export const planshowList = (data) => {
	return http.httpRequest({
		url: 'api/v1/canteenManagement/appPlan/PlanshowList',
		method: 'post'
	},data)
}