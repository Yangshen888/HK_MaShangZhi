<template>
	<view>
		<view class="wrap content" >
			<u-swiper :list="list" :height="500"></u-swiper>
			<view class="item">
				<view class="left">
					<view class="title u-line-2" style="font-size: 36rpx;font-weight: 600;margin-left: 30rpx;">
						{{ orderList.cuisineName }}
						<u-tag :text="orderList.cuisineType"  size="mini" style="margin-left: 30rpx;margin-top: 8rpx;" />
					</view>
					<view class="title u-line-2" style="font-size: 40rpx;color: #FA3534;margin-top: 6rpx;margin-left: 30rpx;">
						<text style="font-size: 30rpx;">￥</text>
						{{ priceInt(orderList.price) }}
						<text class="decimal">.{{ priceDecimal(orderList.price) }}</text>
					</view>
				</view>
				<view class="right">
					<view class="like highlight" style="font-size: 26rpx;margin-top: 60rpx;margin-left: 20rpx;">
						<view class="num">{{ orderList.likeNum }}</view>
						<u-icon name="thumb-up-fill" :size="30"></u-icon>
					</view>
				</view>
			</view>
			<u-gap height="30" bg-color="#f2f2f2"></u-gap>
			<!-- <view class="u-tabs-box" style="width: 200px;">
				<u-tabs-swiper activeColor="#f29100" ref="tabs" :list="list1"  :current="current" @change="change" :is-scroll="false" swiperWidth="750"></u-tabs-swiper>
			</view>
			<swiper class="swiper-box" :current="swiperCurrent" @transition="transition" @animationfinish="animationfinish">
				<swiper-item class="swiper-item" scroll-y style="height: 100%;width: 100%;">
					<scroll-view scroll-y style="height: 100%;width: 100%;">
					<view class="page-box">
						<view style="font-size: 30rpx;padding-top: 20rpx;padding-left: 7%;width: 93%;">
							<view style="float: left;color: #909399;width: 100rpx;">主料：</view><view class="type">{{ orderList.ingredient }}</view>
							<view style="float: left;color: #909399;width: 100rpx;">辅料：</view><view class="type">{{ orderList.burdening }}</view>
							<view style="float: left;color: #909399;width: 100rpx;">荤素：</view><view class="type">{{ orderList.cuisineType }}</view>
							<view style="float: left;color: #909399;width: 100rpx;">简介：</view><text>{{ orderList.abstract }}</text>
						</view>
					</view>
					</scroll-view>
				</swiper-item>
				<swiper-item class="swiper-item">
					<view style="padding: 20rpx;">
						<u-table style="font-size: 14px;width: 95%;margin: 0 auto;" align="left">
							<u-tr>
								<u-td width="260rpx">热能(Kca1/100g)</u-td>
								<u-td>{{ orderList.heatEnergy }}</u-td>
							</u-tr>
							<u-tr>
								<u-td width="260rpx">蛋白质(g/100g)</u-td>
								<u-td>{{ orderList.protein }}</u-td>
							</u-tr>
							<u-tr>
								<u-td width="260rpx">脂肪(g/100g)</u-td>
								<u-td>{{ orderList.fat }}</u-td>
							</u-tr>
							<u-tr>
								<u-td width="260rpx">糖类(g/100g)</u-td>
								<u-td>{{ orderList.saccharides }}</u-td>
							</u-tr>
							<u-tr>
								<u-td width="260rpx">VA(ug/100g)</u-td>
								<u-td>{{ orderList.va }}</u-td>
							</u-tr>
						</u-table>
					</view>
				</swiper-item>
			</swiper> -->
			<view class="head">
				<ul>
					<li @click="Tab(1)" style="overflow: hidden;">
						<image src="https://msz-b.jiulong.yoruan.com/img/image/jx15.png" style="width: 8rpx;height: 26rpx;margin-right: 8rpx;float: left;margin-top: -8rpx;"></image>
						<view style="margin-top: -12rpx;font-weight: 600;font-size: 30rpx;">菜品详情</view>
						<view style="width: 50rpx;height: 4rpx;border-radius: 200rpx;background-color: #2B85E4;margin-left:36rpx;margin-top: 10rpx;" :style="style1"></view>
					</li>
					<li @click="Tab(2)" style="overflow: hidden;">
						<image src="https://msz-b.jiulong.yoruan.com/img/image/jx15.png" style="width: 8rpx;height: 26rpx;margin-right: 8rpx;float: left;margin-top: -8rpx;"></image>
						<view style="margin-top: -12rpx;font-weight: 600;font-size: 30rpx;">营养价值</view>
						<view style="width: 50rpx;height: 4rpx;border-radius: 200rpx;background-color: #2B85E4;margin-left:36rpx;margin-top: 10rpx;" :style="style2"></view>
					</li>
				</ul>
			</view>
			<view class="d1"  :style="style1">
				<view class="page-box">
					<view style="font-size: 30rpx;padding-top: 40rpx;padding-left: 7%;width: 93%;">
						<view style="float: left;color: #909399;width: 100rpx;">主料：</view><view class="type">{{ orderList.ingredient }}</view>
						<view style="float: left;color: #909399;width: 100rpx;">辅料：</view><view class="type">{{ orderList.burdening }}</view>
						<view style="float: left;color: #909399;width: 100rpx;">荤素：</view><view class="type">{{ orderList.cuisineType }}</view>
						<view style="float: left;color: #909399;width: 100rpx;">简介：</view><text>{{ orderList.abstract }}</text>
					</view>
				</view>
			</view>
			<view class="d2" :style="style2">
				<view style="padding: 40rpx;">
					<u-table style="font-size: 28rpx;width: 95%;margin: 0 auto;" align="left">
						<u-tr>
							<u-td width="260rpx">热能(Kca1/100g)</u-td>
							<u-td>{{ orderList.heatEnergy }}</u-td>
						</u-tr>
						<u-tr>
							<u-td width="260rpx">蛋白质(g/100g)</u-td>
							<u-td>{{ orderList.protein }}</u-td>
						</u-tr>
						<u-tr>
							<u-td width="260rpx">脂肪(g/100g)</u-td>
							<u-td>{{ orderList.fat }}</u-td>
						</u-tr>
						<u-tr>
							<u-td width="260rpx">糖类(g/100g)</u-td>
							<u-td>{{ orderList.saccharides }}</u-td>
						</u-tr>
						<u-tr>
							<u-td width="260rpx">VA(ug/100g)</u-td>
							<u-td>{{ orderList.va }}</u-td>
						</u-tr>
					</u-table>
				</view>
			</view>
		</view>

		
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { getcuisine } from '@/api/cuisine/cuisine.js';
export default {
	data() {
		return {
			list1: [
				{
					name: '菜品详情'
				},
				{
					name: '营养价值'
				}
			],
			current: 0,
			swiperCurrent: 0,
			dx: 0,
			list: [],
			orderList: {
				cuisineName: '',
				cuisineType: '',
				likeNum: '',
				ingredient: '',
				burdening: '',
				heatEnergy: '',
				fat: '',
				price: '',
				protein: '',
				saccharides: '',
				va: ''
			},
			style1: '',
			style2: 'display:none',
			isLike: [],
			color: this.$u.color['info'],
			url: http.baseUrl + 'UploadFiles/RegistPicture/'
		};
	},
	onLoad(opt) {
		this.getOrderList(opt);
	},
	methods: {
		Tab(num) {
			if (num == 1) {
				this.style1 = 'display:block';
				this.style2 = 'display:none';
			} else {
				this.style1 = 'display:none';
				this.style2 = 'display:block';
			}
		},
		// tab栏切换
		change(index) {
			this.swiperCurrent = index;
			//this.getOrderList(opt);
		},
		transition({ detail: { dx } }) {
			this.$refs.tabs.setDx(dx);
		},
		animationfinish({ detail: { current } }) {
			this.$refs.tabs.setFinishCurrent(current);
			this.swiperCurrent = current;
			this.current = current;
		},
		// 页面数据
		getOrderList(opt) {
			getcuisine(opt.guid).then(res => {
				console.log(res);
				if (res.data.code == 200) {
					this.orderList = res.data.data[0];
					if (res.data.data[0].accessory != '') {
						for (let k = 0; k < res.data.data[0].accessory.split(',').length; k++) {
							let images = {
								image: (this.url + res.data.data[0].accessory.split(',')[k]).toString()
							};
							this.list[k] = images;
						}
					} else {
						this.list = [];
					}
				}
			});
		}
	},
	computed: {
		// 价格小数
		priceDecimal() {
			return val => {
				if (val != parseInt(val)) return val.slice(-2);
				else return '00';
			};
		},
		// 价格整数
		priceInt() {
			return val => {
				if (val !== parseInt(val)) return val.split('.')[0];
				else return val;
			};
		}
	}
};
</script>

<style>
/* #ifndef H5 */
page {
	height: 100%;
	/* background-color: #f2f2f2; */
}
/* #endif */
</style>

<style lang="scss" scoped>
.decimal {
	font-size: 26rpx;
	margin-top: 4rpx;
}
.head {
	width: 100%;
	height: 60rpx;
	padding-top: 28rpx;
}
.head ul {
	padding: 0;
	margin: 0;
}
.head ul li {
	list-style: none;
	float: left;
	text-decoration: none;
	display: block;
	width: 160rpx;
	padding: 10rpx;
	margin: 0px 0px 0px 20rpx;
	position: relative;
}
.d1 {
	width: 100%;
	// position: absolute;
	z-index: 1;
	margin-bottom: 140rpx;
}
.d2 {
	display: none;
	width: 100%;
	// position: absolute;
	z-index: 1;
	margin-bottom: 140rpx;
}
.item {
	// background-color: #FFFFFF;
	display: flex;
	margin-top: 20rpx;
	.left {
		width: 78%;
		margin-left: 10rpx;
		padding: 20rpx;
	}
	.right {
		margin-left: 10rpx;
		text-align: right;
		padding: 10rpx;
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
.wrap {
	display: flex;
	flex-direction: column;
	// height: calc(100vh - var(--window-top));
	// height: 100%;
	width: 100%;
}
.swiper-box {
	flex: 1;
}
.swiper-item {
	height: 100%;
}
</style>
