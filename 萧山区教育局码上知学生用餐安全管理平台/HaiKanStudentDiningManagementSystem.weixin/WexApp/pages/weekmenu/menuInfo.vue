<template>
	<view class="menuInfoBody" style="height: 100%;">
		<!-- 详情上部区域 -->
		<view class="infoTop" style="padding: 10px 20px 0 20px;background-color: rgba(255, 255, 255, 1);">
			<u-swiper :list="list" :height="480"></u-swiper>
			<view class="infoTitle" style="height: 50px; line-height: 50px;">
				<view style="float: left; display: inline; margin-right: 20rpx; font-size: 18px; font-weight: 600;">{{ orderList.cuisineName }}</view>
				<u-tag :text="orderList.cuisineType" type="primary" mode="light" size="mini"></u-tag>
			</view>
			<view class="priceInfo" style="overflow: hidden;">
				<view style="display: inline; float: left;color: #FD5D37;font-weight: 600;" v-if="isCuiPrices">
					<view style="font-size: 26rpx;display: inline;">￥</view>
					<view style="display: inline;font-size: 34rpx;">{{ priceInt(orderList.price) }}</view>
				</view>
				<view style="display: inline; float: right; font-size: 16px; color: #0097FF;">
					<view style="float: right;margin-left: 5px;" :style="numColor">{{ orderList.likeNum }}</view>
					<u-icon v-if="thumbUpColor == 0" name="thumb-up" size="36" color="rgba(165, 165, 165, 1)" @click="thumbUp"></u-icon>
					<u-icon v-if="thumbUpColor == 1" name="thumb-up-fill" size="36" color="#0097FF" @click="thumbUp"></u-icon>
				</view>
			</view>
		</view>
		<!-- 清除浮动 -->
		<view style="clear: both;"></view>
		<!-- 详情下部区域 -->
		<view style="margin-top: 40rpx; overflow: hidden; padding: 0 40rpx;background-color: rgba(255, 255, 255, 1);">
			<u-tabs :list="menuInfoList" :current="menuListCurrent" is-scroll="false" @change="menuListChange" bar-width="116"
			 gutter="90" height="80" style="display: block; margin: 30px 0 15px 0;"></u-tabs>
			<view class="swiperBox">
				<view class="listOne" :style="listNum == 0 ? 'display:block' : 'display:none'">
					<view class="menuInfoCon">
						<view class="contentList">
							<view class="contentItem">主料：</view>
							<view style="display: inline;">{{ orderList.ingredient }}</view>
						</view>
						<view class="contentList">
							<view class="contentItem">配料：</view>
							<view style="display: inline;">{{ orderList.burdening }}</view>
						</view>
						<view class="contentList">
							<view class="contentItem">简介：</view>
							<view style="display: inline;word-break: break-all;">{{ orderList.abstract }}</view>
						</view>
					</view>
				</view>
				<view class="listTwo" :style="listNum == 1 ? 'display:block' : 'display:none'">
					<view class="menuInfoCon">
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
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { getcuisine, getGivelike } from '@/api/cuisine/cuisine.js';
export default {
	data() {
		return {
			current: 0,
			swiperCurrent: 0,
			dx: 0,
			list: [],
			orderList: {
				cuisineUuid: '',
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
			url: http.baseUrl + 'UploadFiles/RegistPicture/',
			// 点赞图标
			thumbUpColor: 0,
			numColor: 'color:rgba(165, 165, 165, 1)',
			// 菜单详情tab栏列表
			menuInfoList: [
				{
					name: '菜品详情'
				},
				{
					name: '营养价值'
				}
			],
			// 菜单详情tab栏 默认栏
			menuListCurrent: 0,
			menuConCurrent: 0,
			// 显示的模块
			listNum: 0,
			isCuiPrices: true
		};
	},
	onLoad(opt) {
		if (!uni.getStorageSync('isCuiPrices')) {
			this.isCuiPrices = false;
		}
		this.getOrderList(opt);
	},
	methods: {
		// 点赞图标变化
		thumbUp() {
			if (this.thumbUpColor == 0) {
				this.thumbUpColor = 1;
				this.orderList.likeNum++;
				getGivelike(this.orderList.cuisineUuid).then(res => {
					if (res.data.code == 200) {
						this.$u.toast(res.data.message);
					} else {
						this.$u.toast(res.data.message);
					}
				});
				this.numColor = 'color:#0097FF;';
			}
		},
		// tab栏切换
		menuListChange(index) {
			this.listNum = index;
			this.menuListCurrent = index;
		},
		menuConChange(index) {
			this.menuListCurrent = index;
			this.menuConCurrent = index;
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
			console.log(opt.color);
			if(opt.color=='#D9D9D9'){
				this.thumbUpColor=0;
			}else{
				this.thumbUpColor=1;
				this.numColor = 'color:#0097FF;';
			}
			// this.thumbUpColor = opt.color;
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
	page {
		height: 100%;
		background-color: rgba(249, 250, 250, 1);
	}

	.contentList {
		margin: 10px 0;
	}

	.contentItem {
		display: inline;
		font-weight: 600;
		font-size: 15px;
	}
</style>
