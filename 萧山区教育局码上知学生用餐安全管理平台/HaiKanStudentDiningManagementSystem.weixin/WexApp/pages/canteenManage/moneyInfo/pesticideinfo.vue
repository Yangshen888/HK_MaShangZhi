<template>
	<view style="padding: 20rpx;">
		<view style="padding: 30rpx;background-color: rgba(255, 255, 255, 1);">
			<view style="font-size: 30rpx;font-weight: 600;margin: 10rpx 0;">基础信息</view>
			<view class="infoBox">
				<view class="infoItem">
					<view style="float: left;">登记时间：</view>
					<view style="float: right;">{{info.created}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">登记时间：</view>
					<view style="float: right;">{{info.created}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">登记人员：</view>
					<view style="float: right;">{{info.userName}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">检测时间：</view>
					<view style="float: right;">{{info.checked}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">检测人员：</view>
					<view style="float: right;">{{info.inspector}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">果蔬品名：</view>
					<view style="float: right;">{{info.vegetablesName}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">检测顺序：</view>
					<view style="float: right;">{{inspectionOrder}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">检测结果：</view>
					<view style="float: right;">{{inspectionResult}}</view>
				</view>
				<view class="infoItem" style="">
					<view style="float: left;">处理结果：</view>
					<view style="float: right;">{{handleMeasure}}</view>
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 10rpx 0;">
					<view style="float: left;margin-bottom: 16rpx;">备注：</view>
					<view style="text-indent: 2em; float: left;background-color: rgba(234, 234, 234, 1);width: 100%;border-radius: 10rpx;min-height: 200rpx;word-break: break-all;padding: 10rpx;">{{info.note}}</view>
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 16rpx 0;">消毒记录图片(实时拍照上传)</view>
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
				id: -1,
				info:{},
				inspectionOrder:"",
				inspectionResult:"",
				handleMeasure:"",
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
					//this.info.hours="冷藏 "+this.info.hours+"小时";
					if(this.info.inspectionOrder==0){
						this.inspectionOrder="清洗前";
					}
					if(this.info.inspectionResult==0){
						this.inspectionResult="阴性";
					}
					if(this.info.handleMeasure==0){
						this.handleMeasure="正常使用";
					}
					this.imgs=this.info.testPaper.split(',');
					for(let i=0;i<this.imgs.length;i++){
						if(i>0&&imgs[i]!=""){
							this.imgs[i]+="http://218.108.205.10:8016";
						}
					}
					console.log(this.imgs);
				});
			}
		}
	}
</script>

<style>
page{
	background-color: rgba(249, 250, 250, 1);
}
.infoBox u-field{
	width: 100%;
	line-height: 30px;
	margin: 10rpx 0;
	float: left;
}
.infoItem{
	float: left;
	width: 100%;
	line-height: 52rpx;
	padding: 16rpx 0;
	border-bottom: 1rpx solid rgba(234, 234, 234, 1);
}
</style>
