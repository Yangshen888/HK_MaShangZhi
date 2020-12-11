<template>
	<view style=" font-size: 32rpx;">
		<view style="text-align: center; font-weight: 700; line-height: 140rpx;">{{ flieList[0].cuisineName }}</view>
		<view style="padding-left: 40rpx;">
			<!-- 循环体 -->
			<view class="detailItem">
				<view style="float: left;">规格</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].specification == '' ? '无' : flieList[0].specification }}</view>
			</view>
			<view class="detailItem">
				<view style="float: left;">数量</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].quantity == '' ? '无' : flieList[0].quantity }}</view>
			</view>
			<view class="detailItem">
				<view style="float: left;">供货商</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].supplier == '' ? '无' : flieList[0].supplier }}</view>
			</view>
			<view class="detailItem">
				<view style="float: left;">保质期</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].expirationTime == '' ? '无' : flieList[0].expirationTime }}</view>
			</view>
			<view class="detailItem">
				<view style="float: left;">保存方式</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].rt == '' ? '无' : flieList[0].rt }}</view>
			</view>
			<view class="detailItem">
				<view style="float: left;">生产日期</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].producedTime == '' ? '无' : flieList[0].producedTime }}</view>
			</view>
		</view>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { moneyshowList } from '@/api/canteenManage/money.js';
export default {
	data() {
		return {
			labelPosition: 'left',
			head: false,
			stores: {
				schoolUuid: '',
				electronicUuid: ''
			},
			flieList: []
		};
	},
	computed: {},
	methods: {
		// 页面数据
		getFliesList() {
			this.stores.schoolUuid = uni.getStorageSync('schoolguid');
			moneyshowList(this.stores).then(res => {
				this.flieList = res.data.data;
			});
		}
	},
	onLoad: function(e) {
		console.log(e);
		this.stores.electronicUuid = e.guid;
		this.getFliesList();
	}
};
</script>

<style>
/* #ifndef H5 */
page {
	height: 100%;
	/* background-color: #f2f2f2; */
}
.wrap {
	padding: 30rpx;
	word-break: break-all;
}
/* #endif */
</style>

<style lang="scss" scoped>
.detailItem {
	overflow: hidden;
	border-bottom: 2rpx solid rgba(234, 234, 234, 1);
	padding-right: 40rpx;
	line-height: 120rpx;
}
.u-card-wrap {
	background-color: $u-bg-color;
	padding: 1px;
}
.u-body-item {
	font-size: 32rpx;
	color: #333;
	padding: 20rpx 10rpx;
	margin-top: -45px;
}
</style>
