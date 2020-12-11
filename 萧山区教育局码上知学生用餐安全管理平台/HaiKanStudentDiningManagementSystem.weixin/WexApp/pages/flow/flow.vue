<template>
	<view class="wrap" style="height: 100%;">
		<u-calendar v-model="selectShow" :mode="'date'" @change="change"></u-calendar>
		<view style="height: 10%; padding: 40rpx; font-size: 16px; font-weight: 600;" @click="openDropdown">
			<view style="float: left; margin-right: 10px;">{{ timetitle }}</view>
			<u-icon name="arrow-down-fill" size="20" top="-2px" v-if="selectShow != true"></u-icon>
			<u-icon name="arrow-up-fill" size="20" top="-2px" v-else></u-icon>
		</view>
		<view style="position: relative; height: 90%; padding: 0 40rpx;" v-if="showall">
			<view class="wrap-item">
				<view class="warpBox">
					<view class="triangle"></view>
					<view @click="rotate(1)" style="float: right; width: 100%; position: relative;">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/point.png" class="wrap_point"></image>
						<view style="width: 100%; float: right;">
							<view class="title" style="float: left; font-size: 16px; font-weight: 600;">验收</view>
							<u-icon name="arrow-up" style="float: right; margin-top: 10rpx;" v-if="arrowDir[0] != 0"></u-icon>
							<u-icon name="arrow-down" style="float: right; margin-top: 10rpx;" v-if="arrowDir[0] == 0"></u-icon>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[0] == 1 ? 'display: block' : 'display: none'" v-if="show2">
						<view v-for="(item, index) in prilistys[0]" style="margin-top: 20rpx;width: 100%;overflow: hidden;border-radius: 10rpx;font-size: 0;"
						 :key="index">
							<u-image :src="item" mode="widthFix" style="width: 100%;"></u-image>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[0] == 1 ? 'display: block' : 'display: none'" v-if="!show2">暂无数据</view>
				</view>

				<view class="warpBox">
					<view class="triangle"></view>
					<view @click="rotate(2)" style="float: right; width: 100%; position: relative;">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/point.png" class="wrap_point"></image>
						<view style="width: 100%; float: right;">
							<view class="title" style="float: left; font-size: 16px; font-weight: 600;">清洗</view>
							<u-icon name="arrow-up" style="float: right; margin-top: 10rpx;" v-if="arrowDir[1] != 0"></u-icon>
							<u-icon name="arrow-down" style="float: right; margin-top: 10rpx;" v-if="arrowDir[1] == 0"></u-icon>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[1] == 2 ? 'display: block' : 'display: none'" v-if="show3">
						<!-- <view v-for="(item, index) in prilistqx[0]" style="margin-top: 20rpx;width: 100%;overflow: hidden;border-radius: 10rpx;font-size: 0;"
						 :key="index">
							<u-image :src="item" mode="widthFix" style="width: 100%;"></u-image>
						</view> -->
						<view v-for="(item, index) in prilistqx[0]" style="margin-top: 20rpx;width: 100%;overflow: hidden;border-radius: 10rpx;font-size: 0;"
						 :key="index">
							<u-image :src="item" mode="widthFix" style="width: 100%;"></u-image>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[1] == 2 ? 'display: block' : 'display: none'" v-if="!show3">暂无数据</view>
				</view>
				<view class="warpBox">
					<view class="triangle"></view>
					<view @click="rotate(3)" style="float: right; width: 100%; position: relative;">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/point.png" class="wrap_point"></image>
						<view style="width: 100%; float: right;">
							<view class="title" style="float: left; font-size: 16px; font-weight: 600;">切配</view>
							<u-icon name="arrow-up" style="float: right; margin-top: 10rpx;" v-if="arrowDir[2] != 0"></u-icon>
							<u-icon name="arrow-down" style="float: right; margin-top: 10rpx;" v-if="arrowDir[2] == 0"></u-icon>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[2] == 3 ? 'display: block' : 'display: none'" v-if="show4">
						<view v-for="(item, index) in prilistqp[0]" style="margin-top: 20rpx;width: 100%;overflow: hidden;border-radius: 10rpx;font-size: 0;"
						 :key="index">
							<u-image :src="item" mode="widthFix" style="width: 100%;"></u-image>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[2] == 3 ? 'display: block' : 'display: none'" v-if="!show4">暂无数据</view>
				</view>
				<view class="warpBox">
					<view class="triangle"></view>
					<view @click="rotate(4)" style="float: right; width: 100%; position: relative;">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/point.png" class="wrap_point"></image>
						<view style="width: 100%; float: right;">
							<view class="title" style="float: left; font-size: 16px; font-weight: 600;">加工</view>
							<u-icon name="arrow-up" style="float: right; margin-top: 10rpx;" v-if="arrowDir[3] != 0"></u-icon>
							<u-icon name="arrow-down" style="float: right; margin-top: 10rpx;" v-if="arrowDir[3] == 0"></u-icon>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[3] == 4 ? 'display: block' : 'display: none'" v-if="show5">
						<view v-for="(item, index) in prilistjg[0]" style="margin-top: 20rpx;width: 100%;overflow: hidden;border-radius: 10rpx;font-size: 0;"
						 :key="index">
							<u-image :src="item" mode="widthFix" style="width: 100%;"></u-image>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[3] == 4 ? 'display: block' : 'display: none'" v-if="!show5">暂无数据</view>
				</view>
				<view class="warpBox">
					<view class="triangle"></view>
					<view @click="rotate(5)" style="float: right; width: 100%; position: relative;">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/point.png" class="wrap_point"></image>
						<view style="width: 100%; float: right;">
							<view class="title" style="float: left; font-size: 16px; font-weight: 600;">成菜</view>
							<u-icon name="arrow-up" style="float: right; margin-top: 10rpx;" v-if="arrowDir[4] != 0"></u-icon>
							<u-icon name="arrow-down" style="float: right; margin-top: 10rpx;" v-else></u-icon>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[4] == 5 ? 'display: block' : 'display: none'" v-if="show6">
						<view v-for="(item, index) in prilistcc[0]" style="margin-top: 20rpx;width: 100%;overflow: hidden;border-radius: 10rpx;font-size: 0;"
						 :key="index">
							<u-image :src="item" mode="widthFix" style="width: 100%;"></u-image>
						</view>
					</view>
					<view class="collapse-body" :style="arrowDir[4] == 5 ? 'display: block' : 'display: none'" v-if="!show6">暂无数据</view>
				</view>
			</view>
			<!-- 连接线 -->
		</view>
		<button v-if="checkShow4()" style="position: fixed;bottom: 30px;right: 30px;border-radius: 50px;border: 1px solid #2979ff;"
		 @click="geturlShow">
			<u-icon name="plus" color="#2979ff" size="38"></u-icon>
		</button>
		<u-popup v-model="popupShow" mode="bottom" height="25%" border-radius="10">
			<view style="height: 100%; width: 100%;text-align: center; letter-spacing: 2rpx;background-color: rgba(249, 250, 250, 1);">
				<view style="height: 64%;">
					<view class="itemTop" style="border-bottom: 1rpx solid rgba(220, 220, 220, 1) ;">
						<span @click="fromLocal" style="display: table-cell;vertical-align: middle;">本地上传</span>
					</view>
					<view class="itemTop"><span @click="wlfromLocal" style="display: table-cell;vertical-align: middle;">网络上传</span></view>
				</view>
				<view style="display: table;width: 100%;height: 30%;background-color: rgba(255, 255, 255, 1);font-size: 32rpx;font-weight: 600;margin-top: 20rpx;">
					<span style="display: table-cell;vertical-align: middle;" @click="geturlHide">取消</span>
				</view>
			</view>
		</u-popup>
	</view>
</template>

<script>
import { GetAppList } from '@/api/flow/flow.js';
import http from '../../utils/http.js';
export default {
	data() {
		return {
			timetitle: '',
			height: 50,
			show2: false,
			show3: false,
			show4: false,
			show5: false,
			show6: false,
			prilistys: [],
			prilistqx: [],
			prilistqp: [],
			prilistjg: [],
			prilistcc: [],
			// 日历 默认关闭
			selectShow: false,
			// 箭头方向 默认向下
			arrowDir: [],
			// 为箭头添加旋转样式
			rotateStyle: 'transform:rotate(180deg)',

			showView: false,
			showall: false,
			geturl: 0,
			popupShow: false
		};
	},
	methods: {
		// 箭头旋转
		rotate(index) {
			console.log(index);
			if (this.arrowDir[index - 1] == index) {
				this.arrowDir[index - 1] = 0;
			} else {
				this.arrowDir[index - 1] = index;
			}
			console.log(this.arrowDir[index - 1]);
			this.$forceUpdate();
		},
		// 打开或关闭下拉选择框
		openDropdown() {
			if (this.selectShow == false) {
				this.selectShow = true;
			} else {
				this.selectShow = false;
			}
		},
		change(e) {
			this.timetitle = e.result;
			let uuid = uni.getStorageSync('schoolguid');
			this.doGetAppList();
		},
		doGetAppList() {
			this.prilistys = [];
			this.prilistqx = [];
			this.prilistqp = [];
			this.prilistjg = [];
			this.prilistcc = [];
			this.show2 = false;
			this.show3 = false;
			this.show4 = false;
			this.show5 = false;
			this.show6 = false;
			this.showall=true;
			let data = {
				date: this.timetitle,
				uuid: uni.getStorageSync('schoolguid')
			};
			console.log(data);
			GetAppList(data).then(res => {
				console.log(res.data.data);
				if (res.data.code == 200) {
					if(res.data.data.times!=undefined){
						this.timetitle=res.data.data.times;
					}
					if (res.data.data.ys.length > 0) {
						let arr = [];
						for (let i = 0; i < res.data.data.ys[0].prctlist.length; i++) {
							arr[i] = http.baseUrl + 'UploadFiles/RegistPicture/' + res.data.data.ys[0].prctlist[i];
						}
						this.prilistys.push(arr);
						this.show2 = true;
					}
					if (res.data.data.qx.length > 0) {
						let arr = [];
						for (let i = 0; i < res.data.data.qx[0].prctlist.length; i++) {
							arr[i] = http.baseUrl + 'UploadFiles/RegistPicture/' + res.data.data.qx[0].prctlist[i];
						}
						this.prilistqx.push(arr);
						this.show3 = true;
					}
					if (res.data.data.qp.length > 0) {
						let arr = [];
						for (let i = 0; i < res.data.data.qp[0].prctlist.length; i++) {
							arr[i] = http.baseUrl + 'UploadFiles/RegistPicture/' + res.data.data.qp[0].prctlist[i];
						}
						this.prilistqp.push(arr);
						this.show4 = true;
					}
					if (res.data.data.jg.length > 0) {
						let arr = [];
						for (let i = 0; i < res.data.data.jg[0].prctlist.length; i++) {
							arr[i] = http.baseUrl + 'UploadFiles/RegistPicture/' + res.data.data.jg[0].prctlist[i];
						}
						this.prilistjg.push(arr);
						this.show5 = true;
					}
					if (res.data.data.cc.length > 0) {
						let arr = [];
						for (let i = 0; i < res.data.data.cc[0].prctlist.length; i++) {
							arr[i] = http.baseUrl + 'UploadFiles/RegistPicture/' + res.data.data.cc[0].prctlist[i];
						}
						this.prilistcc.push(arr);
						this.show6 = true;
					}
				}
			});
		},
		geturlShow(){
			console.log(123456)
			this.popupShow=true;
			console.log(123456)
		},
		geturlHide(){
			this.popupShow=false;
		},
		fromLocal() {
			uni.navigateTo({
				url: '/pages/flow/flowupload'
			});
		},
		wlfromLocal() {
			uni.navigateTo({
				url: '/pages/flow/wlflowupload'
			});
		},
		checkShow4() {
			return this.$store.state.isFlow && this.$store.state.schoolGuid != '' && this.$store.state.schoolGuid == uni.getStorageSync('schoolguid');
		}
	},
	onLoad() {
		this.arrowDir = [1, 2, 3, 4, 5];
		let date = new Date();
		let day = date.getDate().toString();
		let month = (date.getMonth() + 1).toString();
		let year = date.getFullYear().toString();
		if (day.length < 2) {
			day = '0' + day;
		}
		if (month.length < 2) {
			month = '0' + month;
		}
		this.timetitle = year + '-' + month + '-' + day;
		this.doGetAppList();
	},
	onShow() {
		this.popupShow = false;
		this.arrowDir = [1, 2, 3, 4, 5];
		let date = new Date();
		let day = date.getDate().toString();
		let month = (date.getMonth() + 1).toString();
		let year = date.getFullYear().toString();
		if (day.length < 2) {
			day = '0' + day;
		}
		if (month.length < 2) {
			month = '0' + month;
		}
		this.timetitle = year + '-' + month + '-' + day;
		this.doGetAppList();
	}
};
</script>

<style>
page {
		height: 100%;
	}
	
	.wrap-item {
		clear: both;
		float: right;
		width: 100%;
		min-height: 100%;
		padding-left: 60rpx;
		border-left: 1px solid rgba(236, 236, 236, 1);
		border-radius: 5px;
	}

	.warpBox {
		position: relative;
		float: right;
		width: 100%;
		padding: 40rpx;
		box-shadow: 0 0 2px rgba(0, 0, 0, 0.2);
		margin-bottom: 40rpx;
		border-radius: 10rpx;
	}
	.triangle{
		position: absolute;
		top: 45rpx;
		left: -16rpx;
		background-color: #fff; 
		width: 30rpx;
		height: 30rpx;
		border-left:1rpx solid rgba(236, 236, 236, 1);
		border-bottom: 1rpx solid rgba(236, 236, 236, 1);
		transform: rotate(45deg);
	}
	.wrap_point {
		position: absolute;
		top: 50%;
		left: -100rpx;
		transform: translate(-50%,-50%);
		width: 24rpx;
		height: 24rpx;
		border-radius: 50%;
		float: left;
	}

	.collapse-body {
		float: right;
		width: 100%;
		margin-top: 28rpx;
		font-size: 14px;
	}


	.itemTop {
		display: table;
		width: 100%;
		height: 50%;
		background-color: rgba(255, 255, 255, 1);
		font-size: 32rpx;
		font-weight: 600;
	}
</style>
