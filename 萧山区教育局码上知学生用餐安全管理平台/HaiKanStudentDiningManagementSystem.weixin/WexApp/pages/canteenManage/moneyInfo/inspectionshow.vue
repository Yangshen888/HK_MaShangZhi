<template>
	<view style="height: 100%;background-color: rgba(249, 250, 250, 1);">
		<view style="height: 100%;">
			<view style="height: 100%;width: 100%;">
				<view style="padding: 0 20rpx;">
					<view style="margin: 20rpx 0;">晨检统计</view>
					<view style="padding: 0 15px;background-color: #fff;border-radius: 10rpx;">
						<view  style="font-size: 24rpx;color: rgba(165, 165, 165, 1);padding: 20rpx; border-bottom: 1rpx dashed #ccc;">检测时间：{{info.create}}</view>
						<view style="overflow: hidden;">
							<view class="topBox" style="border-right: 1rpx solid #ccc;">
								<view style="font-size: 36rpx;color: #e27473;">{{info.shouldCount}}</view>
								<view class="grid-text">应检人数</view>
							</view>
							<view class="topBox" style="border-right: 1rpx solid #ccc;">
								<view style="font-size: 36rpx;color: #a3cf5e;">{{info.actualCount}}</view>
								<view class="grid-text">实检人数</view>
							</view>
							<view class="topBox">
								<view style="font-size: 36rpx;color: #72afe5;">{{count}}</view>
								<view class="grid-text">正常人数</view>
							</view>
						</view>
					</view>
				</view>
				<view>
					<view style="padding: 0 20rpx;margin: 20rpx 0;">晨检记录</view>
					<view style="margin: 0 40rpx;">
						<view v-for="(item, index) in list" :key="index" style="padding: 20rpx 0;overflow: hidden;background-color: #fff;border-bottom: 1px solid #f0f0f0;">
							<view style="float: left;width: 100%;">
								<view style="float: left;">
									<image src="https://cdn.uviewui.com/uview/example/avatar.png" style="width: 40rpx;height: 40rpx;float: left;"></image>
									<view style="margin-left: 60rpx;">{{ item.userName }}</view>
								</view>
								<view>
									<view style="float: right;margin-right: 10rpx;">{{item.checkStatus}}</view>
								</view>
							</view>
						</view>
					</view>
				</view>
				<u-loadmore :status="loadStatus"></u-loadmore>
			</view>
		</view>
	</view>
</template>

<script>
	import {
		GetinspecInfo
	} from '@/api/canteenManage/money.js';
	export default {
		data() {
			return {
				info:{},
				list: [],
				count:0
			}
		},
		onLoad(opt) {
			console.log(opt);
			this.id = opt.id;
			this.getInspectionInfo();
		},
		methods: {
			getInspectionInfo() {
				GetinspecInfo(this.id).then(res => {
					console.log(111111);
					console.log(res);
					this.info = res.data.data.query;
					this.list=res.data.data.list;
					this.count=res.data.data.count;

				});
			}
		}
	}
</script>

<style>
	/* #ifndef H5 */
	page {
		height: 100%;
	}

	/* #endif */
	.topBox{
		margin: 20rpx 0;
		float: left;
		width: 33%; 
		height: 40px;
		text-align: center;
	}
	.scollBox {
		background-color: #ffb7bb;
		float: left;
		width: 80rpx;
		height: 80rpx;
		border-radius: 50%;
		color: #fff;
		font-weight: 600;
		text-align: center;
		line-height: 80rpx;
		margin: 30rpx;
	}

	.grid-text{
		font-size: 24rpx;
	}
</style>
