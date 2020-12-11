<template>
	<view>
		<view
			class="swip-box"
			v-for="(res, index) in flieList"
			:key="index"
			style="padding: 40rpx;margin:40rpx 40rpx 0 40rpx;box-shadow: 0 0 2px rgba(0,0,0,0.2);border-radius: 10rpx; background-color: rgba(249, 250, 250, 1);"
			@click="toShowFile(res.qualityUuid)"
		>
			<view class="box-item" style="font-size: 32rpx; font-weight: 600;">{{ res.flieName }}</view>
			<view style="color: rgba(165, 165, 165, 1);margin-top: 20rpx;">
				<view style="float: left;margin-right: 10rpx;">生效时间</view>
				<view>{{ res.effectiveTime==null?"暂无":res.effectiveTime }}</view>
			</view>
			<view style="width: 100%;overflow: hidden; margin-top: 20rpx;">
				<!-- <image :src="res.accessory" style="min-width: 100%; border-radius: 5px; border: 1rpx solid rgba(233, 233, 233, 1);"></image> -->
				<image :src="res.accessory"  v-if="res.accessory != ''" style="min-width: 100%; border-radius: 5px; border: 1rpx solid rgba(233, 233, 233, 1);"></image>
				<view class="u-line-3">简介: {{ res.jianJie }}</view>
			</view>
		</view>
	</view>
	
</template>

<script>
import http from '@/utils/http.js';
import { getlist } from '@/api/quality/quality.js';
export default {
	data() {
		return {
			stores: {
				schoolUuid: ''
			},
			flieList: []
		};
	},
	onLoad() {
		this.getFliesList();
	},
	computed: {},
	methods: {
		// 页面数据
		getFliesList() {
			this.stores.schoolUuid = uni.getStorageSync('schoolguid');
			console.log(this.stores.schoolUuid);
			getlist(this.stores).then(res => {
				console.log(res.data.data);
				this.flieList = res.data.data;
			});
		},
		download(url) {
			let urls = http.baseUrl + url;
			console.log(urls);
			uni.downloadFile({
				url: urls,
				success: res => {
					console.log('res');
					console.log(res);
					if (res.statusCode === 200) {
						uni.saveFile({
							tempFilePath: res.tempFilePath,
							success: resData => {
								console.log(res.tempFilePath);
								uni.openDocument({
									filePath: resData.savedFilePath
								});
							},
							fail: () => {
								console.log(res.tempFilePath);
								uni.openDocument({
									filePath: res.tempFilePath
								});
							}
						});
					}
				}
			});
		},
		toShowFile(uuid) {
			uni.navigateTo({
				url: '/pages/quality/qualityshow?uuid=' + uuid
			});
			// let urls = http.baseUrl + url;
			// console.log(urls);
			// uni.downloadFile({
			// 	url: urls,
			// 	success: (res) => {
			// 		console.log("res");
			// 		console.log(res);
			// 		if (res.statusCode === 200) {
			// 			uni.openDocument({
			// 				filePath:res.tempFilePath,
			// 			});
			// 		}
			// 	},
			// });
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
