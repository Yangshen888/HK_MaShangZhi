<template>
	<view class="menuInfoBody" style="height: 100%;">
		<!-- <u-swiper indicator-pos="topRight" :list="list" :effect3d="false" :title="true" :height="420" ></u-swiper> -->
		<!-- 详情上部区域 -->
		<view class="infoTop" style="background-color: rgba(255, 255, 255, 1); padding: 20px;">
			<u-swiper :list="list" :height="480"></u-swiper>
			<view class="infoTitle" style="height: 50px; line-height: 50px;">
				<view style="float: left; display: inline; margin-right: 20px; font-size: 18px; font-weight: 600;">{{ orderList.cuisineName }}</view>
				<u-tag :text="orderList.cuisineType" type="primary" mode="light" size="mini"></u-tag>
			</view>
			<view class="priceInfo" style="overflow: hidden;margin-bottom: 30px;">
				<view style="display: inline; float: left; font-size: 16px;color: #FD5D37; font-weight: 550;"  v-if="isCuiPrices">￥{{ priceInt(orderList.price) }}</view>
				<view style="display: inline; float: right; font-size: 16px; color: #0097FF;">
					<view style="float: right;margin-left: 5px;">{{ orderList.likeNum }}</view>
					<u-icon name="thumb-up-fill" size="36" :color="thumbUpColor"></u-icon>
				</view>
			</view>
		</view>
		<!-- 清除浮动 -->
		<view style="clear: both;"></view>
		<!-- 详情下部区域 :current="menuListCurrent" -->
		<view style="width: 100%; padding: 0 20px;background-color: rgba(255, 255, 255, 1);">
			<u-tabs
			:list="menuInfoList"
			:current="menuListCurrent"
			is-scroll="false"
			@change="menuListChange"
			bar-width="90"
			gutter="90"
			height="90"
			style="display: block; margin: 10px 0 15px 0;"
			></u-tabs>
		</view>
		
		<view class="swiperBox" style="padding: 0 20px;background-color: rgba(255, 255, 255, 1);">
			<view class="listOne" :style="listNum == 0 ? 'display:block' : 'display:none'">
				<view class="menuInfoCon">
					<view class="contentList">
						<view class="contentItem">主料：</view>
						<view style="display: inline;">{{ orderList.ingredient == '' ? '暂无' : orderList.ingredient }}</view>
					</view>
					<view class="contentList">
						<view class="contentItem">配料：</view>
						<view style="display: inline;">{{ orderList.burdening == '' ? '暂无' : orderList.burdening }}</view>
					</view>
					<view class="contentList">
						<view class="contentItem">简介：</view>
						<view style="display: inline;word-break: break-all;">{{ orderList.abstract == '' ? '暂无' : orderList.abstract }}</view>
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
</template>

<script>
import { getLiveShotInfo } from '@/api/diningRoom/liveShot.js';
import { getcuisine } from '@/api/cuisine/cuisine.js';
import http from '../../utils/http.js';
export default {
	data() {
		return {
			accordion: true,
			arrow: true,
			list: [],
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
			url: http.baseUrl + 'UploadFiles/LiveShotPicture/',
			// 点赞图标
			thumbUpColor: '#0097FF',
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
			isCuiPrices:true
		};
	},
	methods: {
		// 点赞图标变化
		thumbUp() {
			if (this.thumbUpColor == '#D9D9D9') {
				this.orderList.likeNum++;
				getGivelike(this.orderList.cuisineUuid).then(res => {
					if (res.data.code == 200) {
						this.$u.toast(res.data.message);
					} else {
						this.$u.toast(res.data.message);
					}
				});
				return (this.thumbUpColor = '#0097FF');
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
		getOrderList(guid) {
			console.log(guid);
			getcuisine(guid).then(res => {
				console.log(132113);
				console.log(res);
				if (res.data.code == 200) {
					this.orderList = res.data.data[0];
				}
			});
		}
	},
	onLoad(option) {
		if(!uni.getStorageSync('isCuiPrices')){
			this.isCuiPrices=false;
		}
		let that = this;
		getLiveShotInfo(option.lsuuid).then(res => {
			console.log(res);
			res.data.data.entity.prctlist.forEach(x => that.list.push({ image: http.baseUrl + 'UploadFiles/LiveShotPicture/' + x, title: res.data.data.entity.cuisineName }));
			this.getOrderList(res.data.data.entity.cuisineUuid);
		});
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
	background-color: rgba(249, 250, 250, 1);
}
/* #endif */
</style>

<style lang="scss" scoped>
page {
	height: 100%;
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
