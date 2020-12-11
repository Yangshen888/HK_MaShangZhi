import http from '@/utils/http.js'

export const moneyList = (data) => {
	return http.httpRequest({
		url: 'api/v1/canteenManagement/AppMoney/MoneyList',
		method: 'post'
	},data)
}
export const moneyshowList = (data) => {
	return http.httpRequest({
		url: 'api/v1/canteenManagement/AppMoney/MoneyshowList',
		method: 'post'
	},data)
}


export const LedgerList = (data) => {
	return http.httpRequest({
		url: 'api/v1/Ledger/AllLedger/LedgerList',
		method: 'post'
	},data)
}

export const LedgerInfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/Ledger/AllLedger/LedgerInfo?id='+data,
		method: 'get'
	})
}

export const GetinspecInfo = (data) => {
	return http.httpRequest({
		url: 'api/v1/Ledger/AllLedger/GetinspecInfo?id='+data,
		method: 'get'
	})
}

export const InspectionList = (data) => {
	return http.httpRequest({
		url: 'api/v1/Ledger/AllLedger/InspectionList',
		method: 'post'
	},data)
}

