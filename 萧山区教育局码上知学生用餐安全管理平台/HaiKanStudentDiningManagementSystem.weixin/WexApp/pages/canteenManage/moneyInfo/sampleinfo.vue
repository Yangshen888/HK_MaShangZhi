<template>
	<view style="padding: 20rpx;">
		<view style="padding: 30rpx;background-color: rgba(255, 255, 255, 1);">
			<view style="font-size: 30rpx;font-weight: 600;margin: 10rpx 0;">基础信息</view>
			<view class="infoBox">
				<view class="infoItem">
					<view style="float: left;">留样时间：</view>
					<view style="float: right;">{{info.sampledDate}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">留样人员：</view>
					<view style="float: right;">{{info.sampleName}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">留样餐次：</view>
					<view style="float: right;">{{info.mealTimeName}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">留样数量：</view>
					<view style="float: right;">{{info.weight}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">留样规定：</view>
					<view style="float: right;">{{info.hours}}</view>
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 10rpx 0;">
					<view style="float: left;">留样食品：</view>
					<view style="float: left;background-color: rgba(234, 234, 234, 1);width: 100%;border-radius: 10rpx;min-height: 100rpx;word-break: break-all;padding: 10rpx;">
						{{info.foodName}}
					</view>
					<!-- <view style="float: right;">{{info.foodName}}</view> -->
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 10rpx 0;" v-if="info.note!=null">
					<view style="float: left;margin-bottom: 16rpx;">备注：</view>
					<view style="float: left;background-color: rgba(234, 234, 234, 1);width: 100%;border-radius: 10rpx;min-height: 200rpx;word-break: break-all;padding: 10rpx;">
						{{info.note}}
					</view>
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 16rpx 0;">采购照片</view>
				<image :src="item" v-for="(item,index) in imgs" style="border-radius: 10rpx;"></image>
				<view class="infoItem">
					<view style="float: left;">到期消样时间：</view>
					<view style="float: right;">{{info.maturedDate}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">实际消样时间：</view>
					<view style="float: right;">{{info.eliminateDate}}</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		LedgerInfo
	} from '@/api/canteenManage/money.js';
	export default {
		data() {
			return {
				id: -1,
				info:{},
				imgs:[],
			}
		},
		onLoad(opt) {
			console.log(opt);
			this.id = opt.id;
			this.getSampleInfo();
		},
		methods: {
			getSampleInfo() {
				LedgerInfo(this.id).then(res => {
					console.log(res);
					this.info=res.data.data;
					this.info.hours="冷藏 "+this.info.hours+"小时";
					this.imgs=res.data.data.img.split(',');
						
					for(let i=0;i<this.imgs.length;i++){
						if(i>0){
							this.imgs[i]='http://218.108.205.10:8016'+this.imgs[i];
						}
					}
				});
			}
		}
	}
</script>

<style>
.infoItem{
	float: left;
	width: 100%;
	line-height: 52rpx;
	padding: 16rpx 0;
	border-bottom: 1rpx solid rgba(234, 234, 234, 1);
}
</style>
