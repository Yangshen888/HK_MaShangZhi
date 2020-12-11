<template>
	<view>
		<view class="wrap">
			<view class="u-tabs-box">
				<u-tabs-swiper activeColor="#f29100" ref="tabs" :list="list" :current="current" @change="change" :is-scroll="false" swiperWidth="750" v-if="tabshow"></u-tabs-swiper>
			</view>
			<swiper class="swiper-box" :current="swiperCurrent" @transition="transition" @animationfinish="animationfinish">
				<swiper-item class="swiper-item"  v-if="monshow">
					<scroll-view scroll-y style="height: 100%;width: 100%;">
						<view class="page-box">
							<view class="order" v-for="(res, index) in monentity" :key="index">
								<view class="top">
									<view class="left">
										<u-icon name="home" :size="30" color="rgb(94,94,94)"></u-icon>
										<view class="store" v-if="stores.cuisineType == '特色菜'">特色菜</view>
										<view class="store" v-if="stores.cuisineType != '特色菜'">{{ res.cuisineType }}</view>
									</view>
									<view class="right" @click="show(res.cuisineUuid)">详情</view>
								</view>
								<view class="item">
									<view class="left"><image :src="monimg[index]"></image></view>
									<view class="content">
										<view class="title u-line-2">菜品名称：{{ res.cuisineName }}</view>
										<view class="type u-line-2">主料：{{ res.ingredient }}</view>
										<view class="type u-line-2">配料：{{ res.burdening }}</view>
									</view>
									<view class="right">
										<view class="price">
											￥{{ priceInt(res.price) }}
											<text class="decimal">.{{ priceDecimal(res.price) }}</text>
										</view>
									</view>
								</view>
								<view class="total">
									<view class="like" :class="{ highlight: isLike[index] }">
										<view class="num">{{ res.likeNum }}</view>
										<u-icon v-if="!isLike[index]" name="thumb-up" :size="30" color="#9a9a9a" @click="getLike(index)"></u-icon>
										<u-icon v-if="isLike[index]" name="thumb-up-fill" :size="30" @click="getLike(index)"></u-icon>
									</view>
								</view>
								<view class="bottom"></view>
							</view>
						</view>
					</scroll-view>
				</swiper-item>
				<swiper-item class="swiper-item" v-if="noonshow">
					<scroll-view scroll-y style="height: 100%;width: 100%;">
						<view class="page-box">
							<view class="order" v-for="(res, index) in noonentity" :key="index">
								<view class="top">
									<view class="left">
										<u-icon name="home" :size="30" color="rgb(94,94,94)"></u-icon>
										<view class="store" v-if="stores.cuisineType == '特色菜'">特色菜</view>
										<view class="store" v-if="stores.cuisineType != '特色菜'">{{ res.cuisineType }}</view>
									</view>
									<view class="right" @click="show(res.cuisineUuid)">详情</view>
								</view>
								<view class="item">
									<view class="left"><image :src="noonimg[index]"></image></view>
									<view class="content">
										<view class="title u-line-2">菜品名称：{{ res.cuisineName }}</view>
										<view class="type u-line-2">主料：{{ res.ingredient }}</view>
										<view class="type u-line-2">配料：{{ res.burdening }}</view>
									</view>
									<view class="right">
										<view class="price">
											￥{{ priceInt(res.price) }}
											<text class="decimal">.{{ priceDecimal(res.price) }}</text>
										</view>
									</view>
								</view>
								<view class="total">
									<view class="like" :class="{ highlight: isLike[index] }">
										<view class="num">{{ res.likeNum }}</view>
										<u-icon v-if="!isLike[index]" name="thumb-up" :size="30" color="#9a9a9a" @click="getLike(index)"></u-icon>
										<u-icon v-if="isLike[index]" name="thumb-up-fill" :size="30" @click="getLike(index)"></u-icon>
									</view>
								</view>
								<view class="bottom"></view>
							</view>
						</view>
					</scroll-view>
				</swiper-item>
				<swiper-item class="swiper-item"  v-if="nightshow">
					<scroll-view scroll-y style="height: 100%;width: 100%;">
						<view class="page-box">
							<view class="order" v-for="(res, index) in nightentity" :key="index">
								<view class="top">
									<view class="left">
										<u-icon name="home" :size="30" color="rgb(94,94,94)"></u-icon>
										<view class="store" v-if="stores.cuisineType == '特色菜'">特色菜</view>
										<view class="store" v-if="stores.cuisineType != '特色菜'">{{ res.cuisineType }}</view>
									</view>
									<view class="right" @click="show(res.cuisineUuid)">详情</view>
								</view>
								<view class="item">
									<view class="left"><image :src="nightimg[index]"></image></view>
									<view class="content">
										<view class="title u-line-2">菜品名称：{{ res.cuisineName }}</view>
										<view class="type u-line-2">主料：{{ res.ingredient }}</view>
										<view class="type u-line-2">配料：{{ res.burdening }}</view>
									</view>
									<view class="right">
										<view class="price">
											￥{{ priceInt(res.price) }}
											<text class="decimal">.{{ priceDecimal(res.price) }}</text>
										</view>
									</view>
								</view>
								<view class="total">
									<view class="like" :class="{ highlight: isLike[index] }">
										<view class="num">{{ res.likeNum }}</view>
										<u-icon v-if="!isLike[index]" name="thumb-up" :size="30" color="#9a9a9a" @click="getLike(index)"></u-icon>
										<u-icon v-if="isLike[index]" name="thumb-up-fill" :size="30" @click="getLike(index)"></u-icon>
									</view>
								</view>
								<view class="bottom"></view>
							</view>
						</view>
					</scroll-view>
				</swiper-item>
			</swiper>
		</view>
		<view><u-toast ref="uToast" /></view>
	</view>
</template>

<script>
import http from '@/utils/http.js'
import { getweekmenuList } from '@/api/weekmenu/menu.js';
import { getGivelike } from '@/api/cuisine/cuisine.js';
export default {
	data() {
		return {
			stores: {
				date: '',
				time: '',
				schoolUuid: ''
			},
			url:http.baseUrl+"UploadFiles/RegistPicture/",
			orderList: [],
			monimg: [],
			noonimg: [],
			nightimg: [],
			isLike: [],
			monentity: [],
			noonentity: [],
			nightentity: [],
			
			tabshow:false,
			monshow:false,
			noonshow:false,
			nightshow:false,
			
			list: [
				{
					name: '早餐'
				},
				{
					name: '午餐'
				},
				{
					name: '晚餐'
				}
			],
			current: 0,
			swiperCurrent: 0,
			tabsHeight: 0,
			dx: 0,
		};
	},
	onLoad(opt) {
		this.stores.schoolUuid = uni.getStorageSync('schoolguid');
		this.stores.date = opt.date;
		this.stores.time = opt.time;
		this.getOrderList();
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
	},
	methods: {
		// 页面数据
		getOrderList() {
			getweekmenuList(this.stores).then(res => {
				if (res.data.message == '暂无数据') {
					this.$refs.uToast.show({
						title: '暂无菜品',
						type: 'warning',
						// url: '/pages/weekmenu/menu'
						//back:true
					});
				} else {
					this.monentity = res.data.data.monentity;
					this.noonentity = res.data.data.noonentity;
					this.nightentity = res.data.data.nightentity;
					console.log(res.data.data)
					if(this.monentity.length==0 &&this.noonentity.length==0 &&this.nightentity.length==0 ){
						this.$refs.uToast.show({
							title: '暂无菜品',
							type: 'warning',
							// url: '/pages/weekmenu/menu'
							back:true
						});
					}
					if(this.monentity.length==0){
						this.list.splice(0,1);
					}else{
						this.monshow=true;
						for (let k = 0; k < res.data.data.monentity.length; k++) {
							if (res.data.data.monentity[k].accessory != '') {
								this.monimg[k] = (this.url + res.data.data.monentity[k].accessory.split(',')[0]).toString();
							} else {
								this.monimg[k] = '';
							}
						}
					}
					if(this.noonentity.length==0){
						this.list.splice(1,1);
					}else{
						this.noonshow=true;
						for (let k = 0; k < res.data.data.noonentity.length; k++) {
							if (res.data.data.noonentity[k].accessory != '') {
								this.noonimg[k] = (this.url + res.data.data.noonentity[k].accessory.split(',')[0]).toString();
							} else {
								this.noonimg[k] = '';
							}
						}
					}
					if(this.nightentity.length==0 ){
						if(this.noonentity.length==0){
							this.list.splice(1,1);
						}else{
							this.list.splice(2,1);
						}
					}else{
						this.nightshow=true;
						for (let k = 0; k < res.data.data.nightentity.length; k++) {
							if (res.data.data.nightentity[k].accessory != '') {
								this.nightimg[k] = (this.url + res.data.data.nightentity[k].accessory.split(',')[0]).toString();
							} else {
								this.nightimg[k] = '';
							}
						}
					}
					this.tabshow=true;
					
					
					
				}
			});
		},
		// 点赞
		getLike(index) {
			if (this.isLike[index] == undefined) {
				this.isLike[index] = true;
				this.orderList[index].likeNum++;
				getGivelike(this.orderList[index].cuisineUuid).then(res => {
					if (res.data.code == 200) {
						this.$u.toast(res.data.message);
					} else {
						this.$u.toast(res.data.message);
					}
				});
			} else {
				this.isLike[index] = false;
				return;
			}
		},
		show(e) {
			uni.redirectTo({
				url: '/pages/cuisine/cuisineshow?guid=' + e
			});
		},
		// tab栏切换
		change(index) {
			this.swiperCurrent = index;
			this.getOrderList();
		},
		transition({ detail: { dx } }) {
			this.$refs.tabs.setDx(dx);
		},
		animationfinish({ detail: { current } }) {
			this.$refs.tabs.setFinishCurrent(current);
			this.swiperCurrent = current;
			this.current = current;
		}
	}
};
</script>

<style>
/* #ifndef H5 */
page {
	height: 100%;
	background-color: #f2f2f2;
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
			width: 274rpx;
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
