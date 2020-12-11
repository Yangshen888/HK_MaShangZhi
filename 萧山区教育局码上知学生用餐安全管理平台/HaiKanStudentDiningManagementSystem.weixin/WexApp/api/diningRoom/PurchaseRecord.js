import http from '@/utils/http.js'

export const purchaseRecordList = (data) => {
	return http.httpRequest({
		url: 'api/v1/Ingredient/PurchaseRecord/PurchaseRecordList?date='+data.data+'&uuid='+data.uuid+'&change='+data.change,
		method: 'get'
	})
}

export const purchaseInfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/Ingredient/PurchaseRecord/PurchaseInfo?uuid='+data,
		method: 'get'
	})
}

export const loadIngredient2 = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/Ingredient/ingredient/get/'+data.name,
		method: 'get'
	})
}

export const getStaffList = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/rbac/user/GetStaffList2',
		method: 'get'
	})
}

export const createPurchaseRecord = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/ingredient/purchaseRecord/create',
		method: 'post'
	},data)
}

export const getFoodTypeList = () => {
	return http.httpTokenRequest({
		url: 'api/v1/ingredient/ingredient/FoodTypeList',
		method: 'get'
	})
}

// export const getPictureList = (data) => {
// 	return http.httpRequest({
// 		url: 'api/v1/DiningRoom/LiveShot/GetPictureList?date='+data.date+"&uuid="+data.uuid+"&type="+data.type,
// 		method: 'get'
// 	})
// }

// export const getLiveShotInfo = (data) => {
// 	return http.httpRequest({
// 		url: 'api/v1/DiningRoom/LiveShot/GeLiveShotInfo?guid='+data,
// 		method: 'get'
// 	})
// }