<template>
	<view style="padding-left: 30rpx;">
		<u-cell-group style="margin-top: 5px;" v-for="(res, index) in flieList" :key="index">
			<u-cell-item center :is-link="true" :value="res.addTime" @click="show(res.articleUuid)" :title="titlelist[index]">
				<!-- <image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/kfgl@2x.png" mode="widthFix"></image> -->
				
			</u-cell-item>
		</u-cell-group>
	</view>
</template>
<script>
import http from '@/utils/http.js'
import { GetAppList } from '@/api/acticle/acticle.js';
export default {
	data() {
		return {						
			schoolUuid: '',
			flieList: [],
			titlelist:[],
			
		};
	},
	onLoad() {		
		this.getFliesList();
	},
	computed: {
		
	},
	methods: {
		// 页面数据
		getFliesList() {			
			this.schoolUuid = uni.getStorageSync('schoolguid');				
			GetAppList(this.schoolUuid).then(res => {							
				this.flieList = res.data.data;		
				if(this.flieList.length>0){
					for (let k=0;k<this.flieList.length;k++) {
						this.titlelist[k]=this.flieList[k].title.length>12?this.flieList[k].title.substring(0,12)+"...":this.flieList[k].title; 
					}
				}
			});
		},
		show(e){
			console.log("fwefewfewew");
			console.log(e);
			uni.navigateTo({
			   url: '/pages/acticle/acticleshow?guid='+e
			});
		},
	}
};
</script>

<style>
/* #ifndef H5 */
page {
	height: 100%;
}
/* #endif */
</style>

<style lang="scss" scoped>
	.u-cell-icon {
		width: 44rpx;
		height: 44rpx;
		margin-right: 8rpx;
	}
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