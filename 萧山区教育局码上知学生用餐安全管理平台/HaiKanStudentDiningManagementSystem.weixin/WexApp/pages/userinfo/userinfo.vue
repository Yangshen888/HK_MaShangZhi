<template>
	<view>
			<view style="margin-top: 20rpx;padding: 0 40rpx;">
				<u-search placeholder="请输入员工名称" shape="round" :show-action="false" :clearabled="true"  @search="searchUser"></u-search>
			</view>
			<view style="margin: 40rpx 0;background-color: rgba(255, 255, 255, 1);overflow: hidden;padding: 20rpx 0;">
				<view style="width: 33.333%;text-align: center;float: left;">
					<view style="width: 130rpx;display: inline-block;background-color: #84b6ff;height: 130rpx;line-height: 130rpx;text-align: center;border-radius: 50%;color: #fffffc;font-size: 32rpx;">
						{{usernum}}
					</view>
					<view style="margin-top: 10px;color: #878787;">员工总数</view>
				</view>
				<view style="width: 33.333%;text-align: center;float: left;">
					<view style="width: 130rpx;display: inline-block;background-color: #77f182;height: 130rpx;line-height: 130rpx;text-align: center;border-radius: 50%;color: #fffffc;font-size: 32rpx;">
						{{noglynum}}
					</view>
					<view  style="margin-top: 10px;color: #878787;">需持证人数</view>
				</view>
				<view style="width: 33.333%;text-align: center;float: left;">
					<view style="width: 130rpx;display: inline-block;background-color: #ffc074;height: 130rpx;line-height: 130rpx;text-align: center;border-radius: 50%;color: #fffffc;font-size: 32rpx;">
						{{reality}}
					</view>
					<view  style="margin-top: 10px;color: #878787;">实际持证数</view>
				</view>
			</view>
			<view class="userinfoBox" style="margin-top: 16rpx;">
				<view class="topItem">需要上传健康证人员</view>
				<view v-for="(res,index) in userlist" :key="index" style="overflow: hidden;font-size: 32rpx;background-color: rgba(255, 255, 255, 1);padding: 20rpx 30rpx;margin-bottom: 1px solid #f5f5f5;">
					<view style="width:33.333%;float: left;text-align:center;height: 50rpx;line-height: 50rpx;">
						<view style="display:inline-block;margin-right: 30rpx;width: 50%;text-align-last: justify;color: #878787;">{{res.name}}</view>
						<u-icon name="man-delete" size="40" style="color: #ccc;"></u-icon>
					</view>
					<view style="width:28%;float: left;text-align:center;height: 50rpx;line-height: 50rpx;">
						<u-icon name="woman" v-if="res.sex==1" size="36" style="color: #ffaaff;"></u-icon>
						<u-icon name="man" v-if="res.sex==0" size="36" style="color: #d0e38a;"></u-icon>
						<view style="display:inline-block;margin-left: 20rpx;color: #878787;">{{res.dutyName}}</view>
					</view>
					<view style="width:28%;float: left;text-align:center;height: 50rpx;line-height: 50rpx;" v-if="res.mzt!=''">
						<u-icon name="grid" size="36" style="color: #00ff7f;"></u-icon>
						<view style="display:inline-block;margin-left: 20rpx;color: #878787;">{{res.mzt}}</view>
					</view>
					<view style="width:9.666%;float: right;text-align:center;height: 50rpx;line-height: 50rpx;">
						<u-icon name="calendar" size="42" style="color: #b4d7ef;" @click="touserinfo(res.id)"></u-icon>
					</view>
				</view>
			</view>
			<view class="userinfoBox">
				<view class="topItem">无需上传健康证人员</view>
				<view v-for="(res,index) in glylist" :key="index" style="overflow: hidden;font-size: 32rpx;background-color: rgba(255, 255, 255, 1);padding: 20rpx 30rpx;">
					<view style="width:33.333%;float: left;text-align:center;height: 50rpx;line-height: 50rpx;">
						<view style="display:inline-block;margin-right: 30rpx;width: 50%;text-align-last: justify;color: #878787;">{{res.name}}</view>
						<u-icon name="man-delete" size="40" style="color: #ccc;"></u-icon>
					</view>
					<view style="width:28%;float: left;text-align:center;height: 50rpx;line-height: 50rpx;">
						<u-icon name="woman" v-if="res.sex==1" size="36" style="color: #ffaaff;"></u-icon>
						<u-icon name="man" v-if="res.sex==0" size="36" style="color: #d0e38a;"></u-icon>
						<view style="display:inline-block;margin-left: 20rpx;color: #878787;">{{res.dutyName}}</view>
					</view>
					<view style="width:28%;float: left;text-align:center;height: 50rpx;line-height: 50rpx;" v-if="res.mzt!=''">
						<u-icon name="grid" size="36" style="color: #00ff7f;"></u-icon>
						<view style="display:inline-block;margin-left: 20rpx;color: #878787;">{{res.mzt}}</view>
					</view>
					<view style="width:9.666%;float: right;text-align:center;height: 50rpx;line-height: 50rpx;">
						<u-icon name="calendar" size="42" style="color: #b4d7ef;" @click="touserinfo(res.id)"></u-icon>
					</view>
				</view>
			</view>
		</view>
</template>

<script>
	import http from '@/utils/http.js'
	import { GetUserList } from '@/api/userinfo/userinfo.js';
	export default {
		data() {
			return {		
				query: {
					kw: '',
					kw1: ''
				},
				schoolName: '',
				userlist:[],
				glylist:[],
				usernum:0,
				noglynum:0,
				reality:0
			};
		},
		onLoad() {		
			this.schoolName = uni.getStorageSync('schoolName');
			this.query.kw = this.schoolName;
			this.getFliesList();
		},
		computed: {
			
		},
		methods: {
			searchUser(e){
				console.log(e)
				this.query.kw1=e;
				this.getFliesList();
			},
			touserinfo(e){
				uni.navigateTo({
					url: '/pages/userinfo/userinfolist?id='+e,
				});
			},
			// 页面数据
			getFliesList() {					
				GetUserList(this.query).then(res => {	
					console.log(res)
					this.userlist = res.data.data.userlist;
					this.glylist = res.data.data.glylist;
					this.usernum = res.data.data.usernum;
					this.noglynum = res.data.data.noglynum;
					this.reality = res.data.data.reality;
				});
			}
		}
	};
</script>

<style>
	page{
		min-height: 100%;
		background-color: rgba(247, 247, 247, 1.0);
	}
	.topItem{
		line-height: 3;
		padding-left: 20rpx;
		color: #878787;
		font-size: 32rpx;
		/* font-weight: bold; */
	}
	.userinfoBox{
		background-color: #FFFFFF;
		/* padding: 20rpx; */
	}
</style>
