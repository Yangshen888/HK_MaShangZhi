<template>
	<view style="padding: 20rpx;">
		<view style="padding: 30rpx;background-color: rgba(255, 255, 255, 1);">
			<view style="font-size: 30rpx;font-weight: 600;margin: 10rpx 0;">基础信息</view>
			<view class="infoBox">
				<view class="infoItem">
					<view style="float: left;">消毒时间：</view>
					<view style="float: right;">{{info.createddate}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">消毒人员：</view>
					<view style="float: right;">{{info.disinfectionUserName}}</view>
				</view>
				<view class="infoItem">
					<view style="float: left;">消毒餐次：</view>
					<view style="float: right;">{{info.typeName}}</view>
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 10rpx 0;">
					<view style="float: left;margin-bottom: 16rpx;">消毒详情：</view>
					<view style="float: left;background-color: rgba(234, 234, 234, 1);width: 100%;border-radius: 10rpx;min-height: 200rpx;word-break: break-all;padding: 10rpx;">
						<view v-for="(item,index) in disinlist">{{item}}</view>
					</view>
				</view>
				<view style="float: left;width: 100%;line-height: 52rpx;padding: 10rpx 0;">
					<view style="float: left;margin-bottom: 16rpx;">空气消毒：</view>
					<view style="float: left;background-color: rgba(234, 234, 234, 1);width: 100%;border-radius: 10rpx;min-height: 200rpx;word-break: break-all;padding: 10rpx;">
						<view>空气消毒专间: {{room}}</view>
						<view>消毒时长 (分钟): {{time}}</view>
						<view>紫外线灯: {{tool}}</view>
					</view>
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
				info: {},
				imgs: [],
				disinlist: [],
				room:'',
				time:'',
				tool:'',
			}
		},
		onLoad(opt) {
			console.log(opt);
			this.id = opt.id;
			this.getDisinfectionInfo();
		},
		methods: {
			getDisinfectionInfo() {
				LedgerInfo(this.id).then(res => {
					console.log(res);
					this.info = res.data.data.entity;

					this.imgs = this.info.imgUrls.split(',');
					let list=res.data.data.list;
					console.log(list);
					for(let i=0;i<list.length;i++){
						if(list[i].typeName!='空气消毒'){
							let s=list[i].typeName+" "+list[i].number+" 件 "+list[i].method;
							console.log(s);
							this.disinlist.push(s);
						}
						
						if(list[i].typeName=='空气消毒'){
							this.room=list[i].roomName;
							this.time=list[i].number;
							this.tool=list[i].toolName;
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
