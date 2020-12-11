<template>
	<view style="padding: 20rpx;">
		<view style="padding: 30rpx;background-color: rgba(255, 255, 255, 1);">
			<view style="font-size: 30rpx;font-weight: 600;margin: 10rpx 0;">基础信息</view>
			<view class="infoBox">
				<view class="infoItem">
					<view style="float: left;">采购时间：</view>
					<view style="float: right;">{{info.purchaseDate}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">登记人员：</view>
					<view style="float: right;">{{info.register}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">采购人员：</view>
					<view style="float: right;">{{info.purchaseUser}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">货品种类：</view>
					<view style="float: right;">{{info.type}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">采购查验：</view>
					<view style="float: right;">{{info.status}}</view>
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 10rpx 0;">
					<view style="float: left;margin-bottom: 16rpx;">备注：</view>
					<view style="float: left;background-color: rgba(234, 234, 234, 1);width: 100%;border-radius: 10rpx;min-height: 200rpx;word-break: break-all;padding: 10rpx;">
						{{info.note}}
					</view>
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 16rpx 0;">采购照片</view>
				<image :src="item" v-for="(item,index) in imgs" style="border-radius: 10rpx;"></image>
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
				info:{},
				id:-1,
				imgs:[],
			}
		},
		onLoad(opt) {
			console.log(opt);
			this.id = opt.id;
			this.getPurchaseInfo();
		},
		methods: {
			getPurchaseInfo() {
				LedgerInfo(this.id).then(res => {
					console.log(res);
					this.info=res.data.data;
					this.imgs=this.info.ticketImgs.split(',');
					for(let i=0;i<this.imgs.length;i++){
						if(i>0&&this.imgs[i]!=""&&this.imgs[i]!="http://218.108.205.10:8016/"){
							this.imgs[i]='http://218.108.205.10:8016'+this.imgs[i];
						}
					}
					console.log(this.imgs);
					if(this.info.status==0){
						this.info.status='合格';
					}else{
						this.info.status='不合格';
					}
					// this.info.hours="冷藏 "+this.info.hours+"小时";
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
