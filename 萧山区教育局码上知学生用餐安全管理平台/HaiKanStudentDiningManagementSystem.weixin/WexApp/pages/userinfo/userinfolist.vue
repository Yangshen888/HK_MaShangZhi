<template>
	<view style="padding: 20rpx;">
		<view style="padding: 30rpx;background-color: rgba(255, 255, 255, 1);">
			<view style="font-size: 30rpx;font-weight: 600;margin: 10rpx 0;">基础信息</view>
			<view class="infoBox" style="overflow: hidden;">
				<view class="infoItem">
					<view style="float: left;">姓名：</view>
					<view style="float: right;">{{info.name}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">性别：</view>
					<view style="float: right;">{{info.sex}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">文化程度：</view>
					<view style="float: right;">{{whcdKeyValue}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">联系方式：</view>
					<view style="float: right;">{{info.phone}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">身份证：</view>
					<view style="float: right;">{{info.idcard}}</view>
				</view>
			</view>
			<view style="font-size: 30rpx;font-weight: 600;margin: 10rpx 0;">在职信息</view>
			<view class="infoBox" style="overflow: hidden;">
				<view class="infoItem">
					<view style="float: left;">职位：</view>
					<view style="float: right;">{{info.dutyName}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">部门：</view>
					<view style="float: right;">{{info.dName}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">班组：</view>
					<view style="float: right;">{{info.teamGroupName}}</view>
				</view>
			</view>
			<view style="font-size: 30rpx;font-weight: 600;margin: 10rpx 0;">健康证信息</view>
			<view class="infoBox">
				<view class="infoItem">
					<view style="float: left;">是否必须上传健康证：</view>
					<view style="float: right;">{{info.ishealth}}</view>
				</view>
				<view class="infoItem" style="">
					<view style="float: left;">健康证编号：</view>
					<view style="float: right;">{{info.healthNum}}</view>
				</view>
				<view class="infoItem" style="">
					<view style="float: left;">健康证类别：</view>
					<view style="float: right;">{{jkzKeyValue}}</view>
				</view>
				<view class="infoItem" style="">
					<view style="float: left;">健康证有效期：</view>
					<view style="float: right;">{{info.healthdate}}</view>
				</view>
				<view class="infoItem" style="">
					<view style="float: left;">发证机构：</view>
					<view style="float: right;">{{info.healthFrom}}</view>
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 16rpx 0;">健康证照片</view>
				<image :src="item" v-for="(item,index) in imgs" style="border-radius: 10rpx;"></image>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		GetUserInfo
	} from '@/api/userinfo/userinfo.js';
	export default {
		data() {
			return {
				id: -1,
				info:{},
				inspectionOrder:"",
				inspectionResult:"",
				handleMeasure:"",
				imgs:[],
				whcdKeyValue:"",
				jkzKeyValue:""
			}
		},
		onLoad(opt) {
			console.log(opt);
			this.id = opt.id;
			this.getSampleInfo();
		},
		methods: {
			getSampleInfo() {
				GetUserInfo(this.id).then(res => {
					console.log(res);                   
					this.info=res.data.data.entity;
					this.whcdKeyValue=res.data.data.whcdKeyValue;
					this.jkzKeyValue=res.data.data.jkzKeyValue;
					
					this.imgs=this.info.healthImg.split(',');
					// console.log(this.imgs);
				});
			}
		}
	}
</script>

<style>
	page{
		background-color: rgba(239, 243, 246, 1.0);
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
