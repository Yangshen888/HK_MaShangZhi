<template>
	<view style="height: 100%;background-color: rgba(249, 250, 250, 1);">
		<view style="height: 100%;">
			<u-back-top :scroll-top="scrollTop"></u-back-top>
			<scroll-view scroll-y style="height: 100%;width: 100%;" @scrolltolower="reachBottom">
				<view style="padding: 0 20rpx;">
					<view style="margin: 20rpx 0;">晨检统计</view>
					<view style="padding: 0 15px;background-color: #fff;border-radius: 10rpx;">
						<view  style="font-size: 24rpx;color: rgba(165, 165, 165, 1);padding: 20rpx; border-bottom: 1rpx dashed #ccc;">检测时间：{{info.entity.create}}</view>
						<view style="overflow: hidden;">
							<view class="topBox" style="border-right: 1rpx solid #ccc;">
								<view style="font-size: 36rpx;color: #e27473;">{{info.entity.shouldCount}}</view>
								<view class="grid-text">应检人数</view>
							</view>
							<view class="topBox" style="border-right: 1rpx solid #ccc;">
								<view style="font-size: 36rpx;color: #a3cf5e;">{{info.entity.actualCount}}</view>
								<view class="grid-text">实检人数</view>
							</view>
							<view class="topBox">
								<view style="font-size: 36rpx;color: #72afe5;">{{info.count}}</view>
								<view class="grid-text">正常人数</view>
							</view>
						</view>
					</view>
				</view>
				<view>
					<view style="padding: 0 20rpx;margin: 20rpx 0;">晨检记录</view>
					<view>
						<view v-for="(item, index) in list" :key="index" style="margin-bottom: 10rpx; padding: 40rpx 0;overflow: hidden;background-color: #fff;" @click="getinspec(item)">
							<view class="scollBox">{{item.normal}}</view>
							<view style="float: left;line-height: 44rpx;">
								<view style="float: left;font-size: 24rpx;color: rgba(165, 165, 165, 1);">{{ item.create }}</view>
								<view>
									<view class="numberBlock">应检人数：{{item.shouldCount}}</view>
									<view class="numberBlock">实检人数：{{item.actualCount}}</view>
								</view>
								<view class="numberBlock">正常人数：{{item.normal}}</view>
							</view>
						</view>
					</view>
				</view>
				<u-loadmore :status="loadStatus"></u-loadmore>
			</scroll-view>
		</view>
	</view>
</template>

<script>
	import {
		LedgerInfo,
		InspectionList
	} from '@/api/canteenManage/money.js';
	export default {
		data() {
			return {
				info:{},
				query: {
					totalCount: 0,
					pageSize: 10,
					currentPage: 1,
					kw: '',
					id: -1,
					sort: [{
						direct: 'DESC',
						field: 'ID'
					}]
				},
				list: [],
				loadStatus: 'nomore',
				scrollTop:0,
			}
		},
		onPageScroll(e) {
			this.scrollTop = e.scrollTop;
		},
		onLoad(opt) {
			console.log(opt);
			this.id = opt.id;
			this.query.id=opt.id;
			this.getInspectionInfo();
			this.getInspectionList();
		},
		methods: {
			reachBottom(){
				console.log(66666);
				this.loadStatus = "loading";
				this.query.currentPage++;
				this.getInspectionList();
			},
			getInspectionInfo() {
				LedgerInfo(this.id).then(res => {
					console.log(111111);
					console.log(res);
					this.info = res.data.data;
					

				});
			},
			getinspec(e){
				console.log(e)
				uni.navigateTo({
					url: '/pages/canteenManage/moneyInfo/inspectionshow?id=' + e.inspectionId
				});
			},
			getInspectionList(){
				InspectionList(this.query).then(res=>{
					console.log(22222);
					console.log(res);
					if(res.data.data.length>0){
						this.list.push.apply(this.list, res.data.data);
					}else{
						this.query.currentPage--;
					}
					this.loadStatus = "nomore";
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

	.numberBlock {
		display: inline-block;
		width: 200rpx;
	}
	.grid-text{
		font-size: 24rpx;
	}
</style>
