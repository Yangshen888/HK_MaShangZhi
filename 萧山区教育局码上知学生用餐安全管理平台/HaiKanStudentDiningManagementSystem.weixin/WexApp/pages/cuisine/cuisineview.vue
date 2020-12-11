<template>
	<view>
		<view class="wrap">
			<view class="page-box" style="padding: 0 20px; ">
				<view class="menuCard" v-for="(res, index) in orderList" :key="index" style=" background-color: rgba(255, 255, 255, 1);overflow: hidden;">
					<!-- 卡片头部 -->
					<view class="header" style="overflow: hidden; position: relative;">
						<view class="point"></view>
						<view style="font-size: 32rpx; font-weight: 700; margin-left: 30rpx; float: left;">{{ typeList[index] }}</view>

						<view @click="show(res.cuisineUuid,isLike[index])" style="width: 140rpx; line-height:40rpx;float:right; position: relative; color: rgba(0, 151, 255, 1);">
							<u-icon name="arrow-right" size="24" style="float: right;position: absolute; top: 50%; right: 0; transform: translateY(-50%); "></u-icon>
							<view style="float: left;letter-spacing: 0;font-size: 28rpx;">查看详情</view>
						</view>
					</view>
					<!-- 菜品详情 -->
					<view class="body" style="margin-top: 30rpx; width: 100%; overflow: hidden;">
						<!-- 右侧菜品图 -->
						<view style="width: 180rpx; height: 180rpx; text-align: center; float: left;">
							<!-- <image src="https://msz-b.jiulong.yoruan.com/img/images/glfa@2x.png" style="width: 91px; height: 91px;"></image> -->
							<image :src="img[index]" style="max-width: 100%; max-height: 100%;border: 1rpx solid rgba(233, 233, 233, 1);border-radius: 5px;"></image>
						</view>
						<!-- 左侧介绍 -->
						<view style="float: right; width: 66%;padding-right: 8rpx;">
							<view style="width: 100%; height: 60rpx;font-size: 32rpx;">
								<view class="title u-line-2" style="float: left;font-weight: 600;">{{ res.cuisineName }}</view>
								<view class="price" style="float: right; color: #FD5D37;line-height: 40rpx;" v-if="isCuiPrices">
									<view style="float: right;font-weight: 600;font-size: 36rpx;">{{ res.price }}</view>
									<view style="font-size: 28rpx;font-weight: 600; float: right;line-height: 48rpx;">￥</view>
								</view>
							</view>
							<view style="text-align: left; float: left; line-height: 24px; color: #A5A5A5;">主料：{{ res.ingredient }}</view>
							<view style="text-align: left; clear: both; line-height: 24px; color: #A5A5A5; word-break: break-all;">配料：{{ res.burdening }}</view>
						</view>
					</view>
					<!-- 底部点赞 -->
					<view style="width: 100%; float: left; line-height: 40rpx;position: relative;">
						<view style="float: right; font-size: 16px;margin-left: 10rpx;" :style="numColor[index]!='color:#0097FF'?'color:rgba(165, 165, 165, 1)':'color:#0097FF'">{{ res.likeNum }}</view>
						<!-- <u-icon name="thumb-up-fill" size="36" top="1px" style="float: right;margin-right: 5px;" :color="thumbUpColor[index]" @click="thumbUp(index)"></u-icon> -->
						<u-icon v-if="thumbUpColor[index] != 1" name="thumb-up" size="36" style="float: right;"
						 color="rgba(165, 165, 165, 1)" @click="thumbUp(index)"></u-icon>
						<u-icon v-if="thumbUpColor[index] == 1" name="thumb-up-fill" size="36" style="float: right;"
						 color="#0097FF" @click="thumbUp(index)"></u-icon>
					</view>
				</view>
			</view>
		</view>
		<view>
			<u-toast ref="uToast" />
		</view>
	</view>
</template>

<script>
	import http from '@/utils/http.js'
	import {
		getcuisineList,
		getGivelike
	} from '@/api/cuisine/cuisine.js';
	export default {
		data() {
			return {
				stores: {
					cuisineName: '',
					cuisineType: '',
					schoolUuid: ''
				},
				orderList: [],
				img: [],
				isLike: [],
				typeList: [],
				url: http.baseUrl + "UploadFiles/RegistPicture/",
				isCuiPrices: true,
				// 点赞图标
				thumbUpColor: [],
				numColor:[],
				getmenuList:[]
			};
		},
		onLoad(opt) {
			if (!uni.getStorageSync('isCuiPrices')) {
				this.isCuiPrices = false;
			}
			this.getOrderList(opt);
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
			// 点赞图标变化
			thumbUp(index) {
				if (this.thumbUpColor[index] != 1) {
					this.orderList[index].likeNum++;
					this.isLike[index] = true;
					getGivelike(this.orderList[index].cuisineUuid).then(res => {
						if (res.data.code == 200) {
							this.$u.toast(res.data.message);
						} else {
							this.$u.toast(res.data.message);
						}
					});
					this.thumbUpColor[index] = 1;
					this.numColor[index]='color:#0097FF';
				}
			},
			// 页面数据
			getOrderList(opt) {
				this.stores.schoolUuid = uni.getStorageSync('schoolguid');
				this.stores.cuisineType = opt.type;
				this.stores.cuisineName = opt.name;
				let type = opt.type;
				getcuisineList(this.stores).then(res => {
					console.log(res.data.data)
					if (res.data.data.length > 0) {
						for (let k = 0; k < res.data.data.length; k++) {
							if (res.data.data[k].accessory != "" && res.data.data[k].accessory != null) {
								this.img[k] = (this.url + res.data.data[k].accessory.split(',')[0]).toString();
							} else {
								this.img[k] = "";
							}
							if (this.stores.cuisineType == "" || this.stores.cuisineType == "特色菜") {
								if (k < 3) {
									this.typeList[k] = "特色菜";
								} else {
									this.typeList[k] = res.data.data[k].cuisineType;
								}
							} else {
								this.typeList[k] = res.data.data[k].cuisineType;
							}
						}
						this.orderList = res.data.data;
					} else {
						this.$refs.uToast.show({
							title: '暂无菜品',
							type: 'warning',
							// url: '/pages/cuisine/cuisinelist'
							back: true
						});
					}

				});
			},
			// 点赞
			// getLike(index) {
			// 	if (this.isLike[index] == undefined) {
			// 		this.isLike[index] = true;
			// 		this.orderList[index].likeNum++;
			// 		getGivelike(this.orderList[index].cuisineUuid).then(res => {
			// 			if (res.data.code == 200) {
			// 				this.$u.toast(res.data.message);
			// 			} else {
			// 				this.$u.toast(res.data.message);
			// 			}
			// 		});
			// 	} else {
			// 		this.isLike[index] = false;
			// 		return;
			// 	}
			// },
			show(e, c) {
				let color = '';
				if (c == undefined) {
					color = '#D9D9D9';
				} else {
					color = '#0097FF';
				}
				uni.navigateTo({
					url: '/pages/weekmenu/menuInfo?guid=' + e + '&color=' + color
				});
			},
		}
	};
</script>


<style>
	page {
		height: 100%;
		background-color: #f2f2f2;
	}
	.menuCard {
		border-radius: 5px;
		box-shadow: 0px 0px 2px rgba(0, 0, 0, 0.2);
		width: 100%;
		margin-bottom: 30rpx;
		overflow: hidden;
		padding: 10px;
	}
	.point {
		position: absolute;
		top: 50%;
		left: 0;
		transform: translateY(-50%);
		width: 5px;
		height: 5px;
		background-color: #0097FF;
		border-radius: 50%;
	}

	.wrap {
		display: flex;
		margin-top: 30rpx;
		flex-direction: column;
		height: calc(100vh - var(--window-top));
		width: 100%;
	}
	.like {
		float: right;
		display: flex;
		align-items: center;
		color: #9a9a9a;
		font-size: 26rpx;
	}
	.num {
		margin-right: 4rpx;
		color: #9a9a9a;
	}

	.highlight {
		color: #5677fc;
	}
	.num {
		color: #5677fc;
	}
</style>
