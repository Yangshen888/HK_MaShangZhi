<template>
	<!-- <view>
		<u-navbar title="厨房视频"></u-navbar>
	</view> -->
	<view class="wrap">
		<view>
			<view style="overflow: hidden;border: 1rpx solid #dddddd;padding: 20rpx 0;margin: 0 20px;border-radius: 20rpx;">
				<view style="float: left;font-size: 32rpx;font-weight: bold;margin-left: 40rpx;">
					日期：
				</view>
				<view class="dateBox" @click="openDropdown" style="background-color: rgba(249, 250, 250, 1);">
					<view style="float: left; margin-right: 10px;" v-if="isdateshow">{{ timetitle }}</view>
					<u-icon v-if="selectShow == false" name="arrow-down-fill" size="20" top="-2px"></u-icon>
					<u-icon v-else name="arrow-up-fill" size="20" @click="closeDropdown" top="-2px"></u-icon>
				</view>
			</view>
			
			<u-calendar v-model="selectShow" :mode="'date'" @change="timechange"></u-calendar>
		</view>
		<view class="u-tabs-box">
			<u-tabs-swiper
				bar-width="130"
				height="100"
				font-size="32"
				active-color="rgba(0, 151, 255, 1)"
				ref="tabs"
				:list="list"
				:current="current"
				@change="change"
				:is-scroll="false"
				swiperWidth="750"
			></u-tabs-swiper>
		</view>
		<swiper class="swiper-box" :current="swiperCurrent" @transition="transition" @animationfinish="animationfinish">
			<swiper-item class="swiper-item">
				<scroll-view scroll-y style="height: 100%;width: 100%;" @scroll="doScroll">
					<view
						v-for="(item, index) in videolist1"
						:key="index"
						style="padding: 10px; margin: 40rpx;box-shadow: 0 0 2px rgba(0,0,0,0.2);border-radius: 5px; background-color: #FFFFFF;"
					>
						<view style="overflow: hidden; line-height: 30px;">
							<view style="float: left; font-size: 32rpx;">{{item.name}}</view>
							<view style="float: right;color: rgba(165, 165, 165, 1);">{{item.addTime}}</view>
						</view>
						<video
							:id="'kv' + index"
							:ref="'kv' + index"
							:src="item.accessory"
							@error="videoErrorCallback"
							enable-danmu
							enable-play-gesture
							auto-pause-if-navigate
							controls
							:show-play-btn="false"
							:show-center-play-btn="true"
							style="width: 100%; margin-top: 20rpx;border-radius: 5px; border: 1rpx solid rgba(233, 233, 233, 1);"
						></video>
					</view>
				</scroll-view>
			</swiper-item>
			<swiper-item class="swiper-item">
				<scroll-view scroll-y style="height: 100%;width: 100%;" @scroll="doScroll2">
					<view
						v-for="(item, index) in videolist2"
						:key="index"
						style="padding: 10px; margin: 40rpx;box-shadow: 0 0 2px rgba(0,0,0,0.2);border-radius: 5px; background-color: #FFFFFF;"
					>
						<view style="overflow: hidden; line-height: 30px;">
							<view style="float: left; font-size: 32rpx;">{{item.name}}</view>
							<view style="float: right;color: rgba(165, 165, 165, 1);">{{item.addTime}}</view>
						</view>
						<video
							:id="'dv' + index"
							:ref="'dv' + index"
							:src="item.accessory"
							@error="videoErrorCallback"
							enable-danmu
							enable-play-gesture
							auto-pause-if-navigate
							controls
							style="width: 100%; margin-top: 20rpx;border-radius: 5px; border: 1rpx solid rgba(233, 233, 233, 1);"
						></video>
					</view>
				</scroll-view>
			</swiper-item>
		</swiper>
		<button v-if="checkShow()" style="position: fixed;bottom: 30px;right: 30px;border-radius: 50px;border: 1px solid #2979ff;" @click="geturlShow">
			<u-icon name="plus" color="#2979ff" size="38"></u-icon>
		</button>
		<u-popup v-model="popupShow" mode="bottom" height="25%" border-radius="10">
			<view style="height: 100%; width: 100%;text-align: center; letter-spacing: 2rpx;background-color: rgba(249, 250, 250, 1);">
				<view style="height: 64%;">
					<view class="itemTop" style="border-bottom: 1rpx solid rgba(220, 220, 220, 1) ;">
						<span @click="geturl" style="display: table-cell;vertical-align: middle;">本地上传</span>
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
import http from '@/utils/http.js';
import { showVideo } from '@/api/diningRoom/kitchenVideo.js';
export default {
	data() {
		return {
			list: [
				{
					name: '厨房监控'
				},
				{
					name: '餐厅监控'
				}
			],
			swiperCurrent: 0,
			url: http.baseUrl,
			current: 0,
			dx: 0,
			videolist1: [],
			videolist2: [],
			checkselect: true,
			distance: 0,
			distance2: 0,
			isfirst: true,
			isOpenWifi: false,
			suuid: '',
			popupShow: false,
			// 日历 默认关闭
			selectShow: false,
			isdateshow: false,
			timetitle: '',
		};
	},
	onLoad(opt) {
		// console.log(opt);
		// return;
		let date = new Date();
		console.log(22222);
		console.log(date);
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
		
		console.log(this.timetitle)
		this.suuid = uni.getStorageSync('schoolguid');
		this.getOrderList('厨房',this.suuid);
		// this.checkWifi();
	},
	onReady: function(res) {
		let that = this;
		uni.getNetworkType({
			success: function(res) {
				console.log(res.networkType);
				if (res.networkType.toString() == 'wifi') {
					console.log(111);
					that.isOpenWifi = true;
					// check = true;
					// let videoContext = uni.createVideoContext('kv0');
					// videoContext.play();
				} else {
					// check = false;
					that.isOpenWifi = false;
				}
			}
		});
	},
	methods: {
		// 打开下拉选择框
		openDropdown() {
			this.selectShow = true;
		},
		// 关闭下拉选择框
		closeDropdown() {
			this.selectShow = false;
		},
		timechange(e) {
			console.log(1212121212);
			console.log(e);
			this.timetitle = e.result;
			let uuid = uni.getStorageSync('schoolguid');
			if(this.swiperCurrent==0){
				this.getOrderList('厨房',this.suuid);
			}
			else{
				this.getOrderList('餐厅',this.suuid);
			}
		},
		videoErrorCallback: function(e) {
			uni.showModal({
				content: e.target.errMsg,
				showCancel: false
			});
		},
		geturlShow() {
			console.log(123456);
			this.popupShow = true;
			console.log(123456);
		},
		geturlHide() {
			this.popupShow = false;
		},
		async getOrderList(type, uuid) {
			await showVideo({ type: type, uuid: uuid,time:this.timetitle }).then(res => {
				console.log(11111111);
				console.log(res);
				console.log(this.timetitle);

				// this.url = (this.url + res.data.data[0].accessory.replace('\\', '/')).toString();
				if (type == '厨房') {
					this.videolist1 = res.data.data;
					this.videolist1.forEach(x => (x.accessory = this.url + x.accessory.replace('\\', '/')));
				} else {
					this.videolist2 = res.data.data;
					this.videolist2.forEach(x => (x.accessory = this.url + x.accessory.replace('\\', '/')));
				}
				this.isdateshow = true;
				// this.videolist = res.data.data;
				// this.videolist.forEach(x => (x.accessory = this.url + x.accessory.replace('\\', '/')));
				// console.log(this.videolist);
			});
		},
		// tab栏切换
		async change(index) {
			this.checkWifi();
			this.swiperCurrent = index;
			console.log('==========');
			console.log(index);
			if (index == 0) {
				await this.getOrderList('厨房',this.suuid);
				this.checkselect = true;
				// if (this.isOpenWifi && this.isfirst) {
				// 	let videoContext = uni.createVideoContext('kv0');
				// 	videoContext.play();
				// }
			}
			if (index == 1) {
				await this.getOrderList('餐厅',this.suuid);
				this.checkselect = false;
				// if (this.isOpenWifi && this.isfirst) {
				// 	let videoContext = uni.createVideoContext('dv0');
				// 	videoContext.play();
				// }
				// this.isfirst = false;
			}
			//this.getOrderList();
		},
		transition({ detail: { dx } }) {
			this.$refs.tabs.setDx(dx);
		},
		animationfinish({ detail: { current } }) {
			this.$refs.tabs.setFinishCurrent(current);
			this.swiperCurrent = current;
			this.current = current;
		},
		async checkWifi() {
			let that=this;
			let check = false;
			await uni.getNetworkType({
				success: function(res) {
					// console.log(res.networkType);
					if (res.networkType.toString() == 'wifi') {
						// console.log(111);
						that.isOpenWifi = true;
						check = true;
					} else {
						check = false;
						that.isOpenWifi = false;
					}
				}
			});
			console.log(check);
			return check;
		},
		doScroll(e) {
			this.checkWifi();
			// console.log(e.detail.scrollTop);
			let distance = e.detail.scrollTop;
			//this.videoContext = uni.createVideoContext('myVideo');

			for (let i = 0; i < this.videolist1.length; i++) {
				let id = 'kv' + i;
				// console.log(id);
				// let videoContext = uni.createVideoContext(id);
				// console.log(videoContext);
				let vindex = parseInt(distance / 311);
				let out = distance % 311;
				if (out > 69) {
					vindex++;
					// console.log(vindex);
				}
				if (this.isOpenWifi && i == vindex) {
					// console.log(2222);
					//videoContext.play();
				} else {
					//videoContext.pause();
				}
			}
		},
		doScroll2(e) {
			this.checkWifi();
			console.log(e.detail.scrollTop);
			let distance2 = e.detail.scrollTop;
			for (let i = 0; i < this.videolist2.length; i++) {
				let id = 'dv' + i;
				console.log(id);
				// let videoContext = uni.createVideoContext(id);
				// console.log(videoContext);
				let vindex = parseInt(distance2 / 311);
				let out = distance2 % 311;
				if (out > 69) {
					vindex++;
					console.log(vindex);
				}
				if (this.isOpenWifi && i == vindex) {
					console.log(2222);
					// videoContext.play();
				} else {
					// videoContext.pause();
				}
			}
		},
		geturl() {
			uni.redirectTo({
				url: '/pages/diningRoom/UpKitchenVideo'
			});
		},
		wlfromLocal() {
			// uni.redirectTo({
			// 	url: '/pages/diningRoom/wlkitchenvideo'
			// });
		},
		checkShow() {
			console.log(this.$store.state.isUploadV);
			console.log(this.$store.state.schoolGuid);
			return this.$store.state.isUploadV && this.$store.state.schoolGuid != '' && this.$store.state.schoolGuid == uni.getStorageSync('schoolguid');
		}
	}
};
</script>
<style>
/* #ifndef H5 */
page {
	height: 100%;
	background-color: #fff;
}
	.dateBox {
		display: inline-block;
		font-size: 14px;
		background-color: rgba(255, 255, 255, 1);
		float: right;
		margin-right: 40rpx;
		font-weight: bold;
	}
/* #endif */
</style>

<style lang="scss" scoped>
.order {
	width: 710rpx;
	background-color: #ffffff;
	margin: 20rpx auto;
	border-radius: 20rpx;
	box-sizing: border-box;
	padding: 20rpx;
	font-size: 30rpx;
	.top {
		display: flex;
		justify-content: space-between;
		.left {
			display: flex;
			align-items: center;
			.store {
				margin: 0 10rpx;
				font-size: 32rpx;
				font-weight: bold;
			}
		}
		.right {
			color: $u-type-warning-dark;
		}
	}
	.item {
		display: flex;
		margin: 20rpx 0 0;
		.left {
			margin-right: 20rpx;
			image {
				width: 200rpx;
				height: 200rpx;
				border-radius: 10rpx;
			}
		}
		.content {
			width: 137px;
			.title {
				margin: 10rpx 0;
				font-size: 30rpx;
				line-height: 50rpx;
			}
			.type {
				margin: 14rpx 0;
				font-size: 30rpx;
				color: $u-tips-color;
			}
			.delivery-time {
				color: #e5d001;
				font-size: 24rpx;
			}
		}
		.right {
			margin-left: 10rpx;
			padding-top: 30rpx;
			text-align: right;
			.decimal {
				font-size: 26rpx;
				margin-top: 4rpx;
			}
			.number {
				color: $u-tips-color;
				font-size: 26rpx;
			}
		}
	}
	.total {
		text-align: right;
		font-size: 24rpx;
		.total-price {
			font-size: 32rpx;
		}
	}
	.bottom {
		display: flex;
		margin-top: 40rpx;
		padding: 0 10rpx;
		justify-content: space-between;
		align-items: center;
		.btn {
			line-height: 52rpx;
			width: 160rpx;
			border-radius: 26rpx;
			border: 2rpx solid $u-border-color;
			font-size: 26rpx;
			text-align: center;
			color: $u-type-info-dark;
		}
		.evaluate {
			color: $u-type-warning-dark;
			border-color: $u-type-warning-dark;
		}
	}
}
.wrap {
	display: flex;
	flex-direction: column;
	height: calc(100vh - var(--window-top));
	width: 100%;
}
.swiper-box {
	flex: 1;
	background-color: rgba(249, 250, 250, 1);
}
.swiper-item {
	height: 100%;
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
