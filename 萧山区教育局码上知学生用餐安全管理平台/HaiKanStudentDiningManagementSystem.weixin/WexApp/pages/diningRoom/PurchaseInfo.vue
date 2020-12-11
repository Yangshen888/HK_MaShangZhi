<template>
	<view>
		<view style="text-align: center;">
			<image :src="foodInfo.picture" mode="widthFix" style="width: 70%;border-radius: 10rpx;"></image>
			<!-- <view class="title">{{foodInfo.foodName}}</view> -->
		</view>
		<view style="padding-left: 50rpx;">
			<view class="itemBox" style="text-align: center;font-size: 32rpx;font-weight: 600;">
				{{foodInfo.foodName}}
			</view>
			<view class="itemBox">
				<view class="item">中标供应商</view>
				<view class="content">{{foodInfo.supplier}}</view>
			</view>
			<view class="itemBox">
				<view class="item">本次采购价格</view>
				<view class="content">{{foodInfo.unitPrice}}</view>
			</view>
			<view class="itemBox">
				<view class="item">市场平均价格</view>
				<view class="content">{{foodInfo.unitPrice}}</view>
			</view>
			<view class="itemBox">
				<view class="item">各校平均价格</view>
				<view class="content">{{foodInfo.unitAvgPrice}}</view>
			</view>
			<view class="itemBox">
				<view class="item">本次采购数量</view>
				<view class="content">{{foodInfo.purchaseNum}}</view>
			</view>
		</view>
		<!-- <view class="item" style="border-bottom: 1px solid rgba(234, 234, 234, 1);">
			<view class="title">时间</view>
			<view class="content">{{subTitle}}</view>
		</view>
		<view class="item" style="border-bottom: 1px solid rgba(234, 234, 234, 1);">
			<view class="title">采购人员</view>
			<view class="content">{{ people }}</view>
		</view>
		<view class="item">
			<view class="title">采购标准</view>
			<view class="content">{{ ps }}</view>
		</view> -->
	</view>
</template>

<script>
	import {
		purchaseInfo
	} from '@/api/diningRoom/PurchaseRecord.js';
	export default {
		data() {
			return {
				foodInfo:{}
			};
		},
		methods: {
			chenckHealth(h) {
				if (h == 1) {
					return '有';
				} else {
					return '无';
				}
			},
			setJob(j) {
				if (j == null || j == "") {
					return "无";
				} else {
					return j;
				}
			}
		},
		onLoad(option) {
			let that = this;
			console.log(option);
			that.foodInfo=option;
			console.log('菜品信息')
			console.log(that.foodInfo)
			// this.subTitle = option.time;
			// purchaseInfo(option.uuid).then(res => {
			// 	console.log(1111);
			// 	console.log(res);
			// 	that.people = res.data.data.arr;
			// 	that.ps = res.data.data.schoolPS;
			// });
		}
	};
</script>

<style scoped lang="scss">
	.itemBox {
		letter-spacing: 2rpx;
		padding: 40rpx 50rpx 40rpx 0;
		border-bottom: 2rpx solid rgba(234, 234, 234, 1);
		overflow: hidden;
	}

	.item {
		float: left;
		font-size: 32rpx;
		color: rgba(51, 52, 53, 1);
	}

	.title {
		font-size: 32rpx;
		font-weight: 600;
		line-height: 3;
	}

	.content {
		float: right;
		font-size: 32rpx;
		color: rgba(165, 165, 165, 1);
	}
</style>
