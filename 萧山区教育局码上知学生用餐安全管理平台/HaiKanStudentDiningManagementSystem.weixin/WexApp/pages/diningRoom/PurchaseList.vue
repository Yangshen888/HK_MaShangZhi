<template>
	<view style="padding: 0 20px;">
		<view class="topBox">
			<view class="totalPriceBox">
				<view style="display: inline-block;font-size: 32rpx;font-weight: 600;">汇总金额：</view>
				<view style="display: inline-block;color: rgba(0, 151, 255, 1);">{{totalPrice}}</view>
				<view style="display: inline-block;">元</view>
			</view>
			<view class="dateBox" @click="openDropdown" style="background-color: rgba(249, 250, 250, 1);">
				<view style="float: left; margin-right: 10px;" v-if="isdateshow">{{ timetitle }}</view>
				<u-icon v-if="selectShow == false" name="arrow-down-fill" size="20" top="-2px"></u-icon>
				<u-icon v-else name="arrow-up-fill" size="20" @click="closeDropdown" top="-2px"></u-icon>
			</view>
			<u-calendar v-model="selectShow" :mode="'date'" @change="change"></u-calendar>
		</view>
		<view class="infoBox">
			<view class="infoCard" v-for="(res, index) in orderList" :key="index" style="margin-top: 30rpx; overflow: hidden;  padding:30rpx 40rpx;both;border-radius: 5px; box-shadow: 0 0 10px rgba(234, 234, 234, 0.5);position: relative;letter-spacing: 1rpx;background-color: rgba(255, 255, 255, 1);">
				<image src="https://msz-b.jiulong.yoruan.com/img/images/point.png" style="width: 8px; height: 8px; border-radius: 50%;position: absolute;top: 50%;left: 10rpx;transform: translateY(-50%);"></image>
				<view style="font-size: 30rpx;color: rgba(51, 52, 53, 1);font-weight: 600;">{{ res.foodName }}</view>
				<view class="lineCon" style="font-size: 24rpx;color: rgba(165, 165, 165, 1);">
					<view class="lineCon">价格：</view>
					<view class="lineCon" style="color: rgba(0, 151, 255, 1);">{{ res.price }}</view>
					<view class="lineCon">元/{{res.unit==null?'斤':res.unit}}</view>
				</view>
				<view class="lineCon" style="font-size: 24rpx;color: rgba(165, 165, 165, 1);margin-left: 30rpx;">
					<view class="lineCon">数量：</view>
					<view class="lineCon">{{ res.purchaseNum }}</view>
					<view class="lineCon">{{res.unit==null?'斤':res.unit}}</view>
				</view>
				<view @click="show(res.purchaseUuid)" style="width: 140rpx; position: relative; color: rgba(0, 151, 255, 1);position: absolute;top: 50%;right: 10rpx;transform: translateY(-50%);">
					<u-icon name="arrow-right" size="20" style="float: right;position: absolute; top: 50%; right: 0; transform: translateY(-50%); "></u-icon>
					<view style="float: right;letter-spacing: 0;font-size: 24rpx;margin-right: 24rpx;">详情</view>
				</view>
				<!-- <view class="top" style="overflow: hidden;position: relative;line-height: 40rpx;">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/point.png" style="position: absolute;top: 50%; transform: translateY(-50%);left: 0;width: 8px; height: 8px; border-radius: 50%;"></image>
					<view class="store" style="float: left;font-size: 32rpx; margin-left: 40rpx;font-weight: 600;">采购信息</view>
					<view @click="show(res.purchaseUuid)" style="width: 140rpx; float: right; position: relative; color: rgba(0, 151, 255, 1);">
						<u-icon name="arrow-right" size="20" style="float: right;position: absolute; top: 50%; right: 0; transform: translateY(-50%); "></u-icon>
						<view style="float: right;letter-spacing: 0;font-size: 24rpx;margin-right: 24rpx;">查看详情</view>
					</view>
				</view>
				<view class="item">
					<image :src="res.accessory" style="width: 100px; height: 100px; float: left;border-radius: 5px;margin-right: 20rpx;"></image>
					<view class="content" style="float: right;">
						<view class="title u-line-2">{{ res.foodName }}</view>
						<view class="type u-line-2">
							<view style="display: inline;">采购价格：</view>
							<view style="color: rgba(0, 151, 255, 1);display: inline;">{{ res.price }}</view>
							<view style="display: inline;">元/斤</view>
						</view>
						<view class="type u-line-2">市场价格：元/斤</view>
						<view class="type u-line-2">各校平均价格：{{ res.avgPrice.toFixed(2) }}元/斤</view>
					</view>
				</view> -->
			</view>
			<!-- <swiper class="swiper-box">
				<swiper-item class="swiper-item">
					<scroll-view scroll-y style="height: 100%;width: 100%; margin-top: 60rpx;">
						<view class="page-box">
							<view class="order" v-for="(res, index) in orderList" :key="index">
								<view class="top">
									<view class="left">
										<image src="../../static/images/point.png" style="width: 8px; height: 8px; margin-right:8px; border-radius: 50%;"></image>
										<view class="store">采购信息</view>
									</view>
									<view class="right" @click="show(res.purchaseUuid)">
										<u-icon top="3px" name="arrow-right" style="float: right;"></u-icon>
										<view style="float: right;margin-right: 10rpx;">查看详情</view>
									</view>
								</view>
								<view class="item">
									<view class="left"><image :src="res.accessory"></image></view>
									<view class="content">
										<view class="title u-line-2">{{ res.foodName }}</view>
										<view class="type u-line-2">
											<view style="display: inline;">采购价格：</view>
											<view style="color: rgba(0, 151, 255, 1);display: inline;">{{ res.price }}</view>
											<view style="display: inline;">元/斤</view>
										</view>
										<view class="type u-line-2">市场价格：元/斤</view>
										<view class="type u-line-2">各校平均价格：{{ res.avgPrice.toFixed(2) }}元/斤</view>
									</view>
								</view>
							</view>
						</view>
					</scroll-view>
				</swiper-item>
			</swiper> -->
		</view>
		<button v-if="checkShow2()" style="position: fixed;bottom: 30px;right: 30px;border-radius: 50px;border: 1px solid #2979ff;"
		 @click="geturl">
			<u-icon name="plus" color="#2979ff" size="38"></u-icon>
		</button>
	</view>
</template>

<script>
	import {
		purchaseRecordList
	} from '@/api/diningRoom/PurchaseRecord.js';
	import http from '../../utils/http.js';
	export default {
		data() {
			return {
				// selectShow: false,
				timetitle: '',
				orderList: [],
				height: 50,
				dateDropdownStatus: 0,
				// 日历 默认关闭
				selectShow: false,
				isdateshow: false,
				totalPrice: 0,
				current:{}
			};
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
			change(e) {
				console.log(e);
				this.timetitle = e.result.split('-').join('/');
				let uuid = uni.getStorageSync('schoolguid');
				purchaseRecordList({
					data: this.timetitle,
					uuid: uuid,
					change: 1
				}).then(res => {
					console.log(22222);
					console.log(res)
					if (res.data.data != null) {
						this.totalPrice = res.data.data.pricesum;
						this.orderList = res.data.data.list3;
						this.orderList.forEach(x => (x.accessory = this.setPath(x.accessory)));
						console.log('这里吗')
						console.log(this.orderList);
					}
				});
			},
			show(puuid) {
				console.log(puuid);
				this.current = this.orderList.find(item=> item.purchaseUuid == puuid);
				console.log(123123123)
				console.log(this.current)
				let unit=this.current.unit==null?'斤':this.current.unit;
				uni.navigateTo({
					url: '/pages/diningRoom/PurchaseInfo?uuid=' + puuid + '&foodName=' + this.current.foodName+'&supplier='+this.current.supplier+'&unitPrice='+this.current.unitPrice+'&unitAvgPrice='+this.current.unitAvgPrice+'&purchaseNum='+this.current.purchaseNum+unit+'&picture='+this.current.accessory
				});
			},
			setPath(file) {
				return http.baseUrl + 'UploadFiles/LiveShotPicture/' + file;
			},
			geturl() {
				uni.navigateTo({
					url: '/pages/diningRoom/purchaseRecordToUp'
				});
			},
			checkShow2() {
				return this.$store.state.isAddPR && this.$store.state.schoolGuid != '' && this.$store.state.schoolGuid == uni.getStorageSync(
					'schoolguid');
			}
		},
		onLoad() {
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
			console.log(1111);
			console.log(this.timetitle);
			let that = this;
			let uuid = uni.getStorageSync('schoolguid');
			purchaseRecordList({
				data: that.timetitle,
				uuid: uuid,
				change: 0
			}).then(res => {
				console.log(66666);
				console.log('原来你在这里')
				console.log(res);
				this.totalPrice = res.data.data.pricesum;
				if (res.data.data != null) {
					that.timetitle = res.data.data.time.split('-').join('/');
					if (res.data.data.list3) {
						that.orderList = res.data.data.list3;
						console.log(that.timetitle);
						that.orderList.forEach(x => (x.accessory = that.setPath(x.accessory)));
						this.isdateshow = true;
					}
				}

			// for(let i=0;i<that.orderList.length;i++){
			// 	that.orderList[i].accessory=http.baseUrl+'UploadFiles/LiveShotPicture/'+that.orderList[i].accessory;
			// 	console.log(that.orderList[i].accessory);
			// }
			console.log(that.orderList);
		});
	}
};
</script>

<style>
	/* #ifndef H5 */
	page {
		height: 100%;
		background-color: rgba(249, 250, 250, 1);
	}
	.lineCon{
		display: inline-block;
	}
	.topBox {
		height: 100rpx;
		line-height: 100rpx;
	}

	.totalPriceBox {
		display: inline-block;
	}

	.dateBox {
		display: inline-block;
		font-size: 14px;
		padding-left: 20rpx;
		background-color: rgba(255, 255, 255, 1);
		float: right;
	}

	.u-form {
		padding: 10px;
	}

	/* #endif */
</style>

<style lang="scss" scoped>
.order {
	width: 100%;
	background-color: #ffffff;
	box-sizing: border-box;
	font-size: 30rpx;
	margin-bottom: 70rpx;
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
			color: rgba(0, 151, 255, 1);
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
			.title {
				font-size: 32rpx;
				font-weight: 600;
				line-height: 54rpx;
			}
			.type {
				font-size: 24rpx;
				color: $u-tips-color;
				line-height:42rpx;
				overflow: hidden;
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
.centre {
	text-align: center;
	margin: 200rpx auto;
	font-size: 32rpx;
	image {
		width: 164rpx;
		height: 164rpx;
		border-radius: 50%;
		margin-bottom: 20rpx;
	}
	.tips {
		font-size: 24rpx;
		color: #999999;
		margin-top: 20rpx;
	}
	.btn {
		margin: 80rpx auto;
		width: 200rpx;
		border-radius: 32rpx;
		line-height: 64rpx;
		color: #ffffff;
		font-size: 26rpx;
		background: linear-gradient(270deg, rgba(249, 116, 90, 1) 0%, rgba(255, 158, 1, 1) 100%);
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
}
.swiper-item {
	height: 100%;
}
.like {
	float: right;
	display: flex;
	align-items: center;
	color: #9a9a9a;
	font-size: 26rpx;
	.num {
		margin-right: 4rpx;
		color: #9a9a9a;
	}
}
.highlight {
	color: #5677fc;
	.num {
		color: #5677fc;
	}
}
</style>
