<template>
	<view style="padding-top: 80rpx; letter-spacing: 2rpx;">
		<view class="trainItem" style="text-align: center; font-size: 32rpx; font-weight: 600;">{{ flieList[0].theme }}</view>
		<view style="padding-left: 40rpx; margin-bottom: 40rpx;">
			<view class="trainLabel" style="border-bottom: 2rpx solid rgba(234, 234, 234, 1); ">
				<view style="float: left;">对象</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].trainName == '' ? '无' : flieList[0].trainName }}</view>
			</view>
			<view class="trainLabel" style="border-bottom: 2rpx solid rgba(234, 234, 234, 1); ">
				<view style="float: left;">课时</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].trainLession == '' ? '无' : flieList[0].trainLession }}</view>
			</view>
			<view class="trainLabel" style="border-bottom: 2rpx solid rgba(234, 234, 234, 1);">
				<view style="float: left;">地点</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].address == '' ? '无' : flieList[0].address }}</view>
			</view>
			<view class="trainLabel" style="border-bottom: 2rpx solid rgba(234, 234, 234, 1);">
				<view style="float: left;">授课人</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].speaker == '' ? '无' : flieList[0].speaker }}</view>
			</view>
			<view class="trainLabel" style="border-bottom: 2rpx solid rgba(234, 234, 234, 1);">
				<view style="float: left;">时间</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].trainTime == '' ? '无' : flieList[0].trainTime }}</view>
			</view>
			<view class="trainLabel" style="border-bottom: 2rpx solid rgba(234, 234, 234, 1);">
				<view style="float: left;">上午培训人数</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].mnum == '' ? '无' : flieList[0].mnum }}人</view>
			</view>
			<view class="trainLabel" style="border-bottom: 2rpx solid rgba(234, 234, 234, 1);">
				<view style="float: left;">下午培训人数</view>
				<view style="float: right; color: rgba(165, 165, 165, 1);">{{ flieList[0].anum == '' ? '无' : flieList[0].anum }}人</view>
			</view>
			<view class="trainLabel">
				<view style="float: left;">内容</view>
				<view style="float: right; "><u-parse :html="flieList[0].content"></u-parse></view>
			</view>
		</view>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { messageshowList } from '@/api/canteenManage/message.js';
export default {
	data() {
		return {
			labelPosition: 'left',
			head: false,
			stores: {
				schoolUuid: '',
				trainUuid: ''
			},
			flieList: []
		};
	},
	// onLoad() {
	// 	this.getFliesList();
	// },

	computed: {},
	methods: {
		// 页面数据
		getFliesList() {
			this.stores.schoolUuid = uni.getStorageSync('schoolguid');
			messageshowList(this.stores).then(res => {
				this.flieList = res.data.data;
				console.log(this.flieList);
			});
		}
	},
	onLoad: function(e) {
		this.stores.trainUuid = e.guid;
		this.getFliesList();
	}
};
</script>

<style>
page {
	height: 100%;
	/* background-color: #f2f2f2; */
}
</style>

<style scoped lang="scss">
.trainLabel{
	padding:20rpx 40rpx 20rpx 0; 
	overflow: hidden; 
	
	font-size: 32rpx;
	line-height: 60rpx;
	word-break: break-all;
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

.u-body-item image {
	width: 120rpx;
	flex: 0 0 120rpx;
	height: 120rpx;
	border-radius: 8rpx;
	margin-left: 12rpx;
}
</style>
