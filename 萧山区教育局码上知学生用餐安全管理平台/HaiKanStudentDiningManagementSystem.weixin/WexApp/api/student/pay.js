import http from '@/utils/http.js'

export const getBillInfo = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/student/StudentBill/GetBillInfo?idcard='+data.idcard+"&suid="+data.suid,
		method: 'get'
	})
}


export const toUnifiedorder = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/student/StudentBill/Unifiedorder',
		method: 'post'
	},data)
}

export const BillSuccess = (data) => {
	return http.httpTokenRequest({
		url: 'api/v1/student/StudentBill/BillSuccess',
		method: 'post'
	},data)
}