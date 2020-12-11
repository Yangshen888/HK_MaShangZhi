<template>
	<view>
		<view class="wrap">
			<u-card :head-border-bottom="head" :foot-border-top="head">
				<view class="" slot="body">
					<view class="u-body-item">
						<view style="font-size: 36rpx;font-weight: 600;margin-top: 20rpx;text-align: center;">{{ flieList[0].headline }}</view>
						<view style="margin-top: 60rpx;margin-left: 30rpx;">
							<!-- <view style="font-size: 28rpx;width: 140rpx;float: left;">内容:</view> -->
							<u-parse :html="flieList[0].content"></u-parse>
							<!-- <view style="font-size: 28rpx;">{{ flieList[0].content==""?"无":flieList[0].content }}</view> -->
						</view>
						<view style="margin-top: 40rpx;margin-left: 30rpx;">
							<view style="font-size: 28rpx;width: 140rpx;float: left;">添加时间:</view>
							<view style="font-size: 28rpx;">{{ flieList[0].addTime==""?"无":flieList[0].addTime }}</view>
						</view>
					</view>
				</view>
			</u-card>
		</view>
	</view>
</template>
<script>
import http from '@/utils/http.js';
import { planshowList } from '@/api/canteenManage/plan.js';
export default {
	data() {
		return {
			labelPosition: 'left',
			head: false,
			disabled: true,
			stores: {
				manageUuid: ''
			},
			flieList: []
		};
	},
	onLoad: function(e) {
		//console.log(e.guid);
		this.stores.manageUuid = e.guid;
		this.getFliesList();
	},
	computed: {},
	methods: {
		// 页面数据
		getFliesList() {
			planshowList(this.stores).then(res => {
				this.flieList = res.data.data;
				console.log(this.flieList);
			});
		}
	}
};
</script>

<style>
/* #ifndef H5 */
page {
	height: 100%;
	background-color: #f2f2f2;
}
.wrap {
	word-break: break-all;
}
/* #endif */
</style>

<style lang="scss" scoped>
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