<template>
	<view style="padding: 0 40rpx;height: 100%;">
		<view style="height: 100%;">
			<scroll-view scroll-y style="height: 100%;width: 100%;" @scrolltolower="reachBottom" >
				<view>
					<view v-for="(item, index) in flieList" :key="index" style="border-bottom: 1px solid rgba(236, 236, 236, 1.0);">
						<view style="padding-bottom: 30rpx;">
							<view style="overflow: hidden; line-height: 100rpx;">
								<view style="float: left;font-size: 32rpx; font-weight: 600; margin-right: 20rpx;">{{ item.name }}</view>
								<!-- <view class="typeTag">{{shwoType(item.objectType)}}</view> -->
								<view @click="show(item.id,item.objectType)" style="width: 140rpx; float: right; position: relative; color: rgba(0, 151, 255, 1);">
									<u-icon name="arrow-right" style="float: right;position: absolute; top: 50%; right: 0; transform: translateY(-50%); "></u-icon>
									<view style="float: left;">查看详情</view>
								</view>
							</view>
							<view style="overflow: hidden;margin-top: 40rpx;">
								<view class="typeTag">{{shwoType(item.objectType)}}</view>
								<!-- <view style="float: left;">保质期：{{ item.expirationTime }}</view> -->
								<view style="float: right;color: rgba(165, 165, 165, 1);">{{ item.createdAt }}</view>
							</view>
						</view>
						<!-- <u-line color="rgb(144, 147, 153)" style="padding-top: 20rpx;" /> -->
					</view>
					<u-loadmore  :status="loadStatus" ></u-loadmore>
				</view>
			</scroll-view>
		</view>
	</view>
</template>

<script>
	import http from '@/utils/http.js';
	import {
		moneyList,
		LedgerList
	} from '@/api/canteenManage/money.js';
	export default {
		data() {
			return {
				stores: {
					schoolUuid: ''
				},
				query: {
					totalCount: 0,
					pageSize: 10,
					currentPage: 1,
					kw: '',
					sort: [{
						direct: 'DESC',
						field: 'ID'
					}]
				},
				loadStatus: 'nomore',
				flieList: [],
				title: [],
				schoolName: '',
			};
		},
		onLoad() {
			this.schoolName = uni.getStorageSync('schoolName');
			// this.schoolName=this.schoolName.split('-')[1];
			this.query.kw = this.schoolName;
			console.log(uni.getStorageSync('schoolName'));
			console.log(this.schoolName);
			this.getLedgerList();
			//this.getFliesList();
		},
		computed: {},
		methods: {
			toscroll(e){
				console.log(e);
			},
			reachBottom(e) {
				console.log(111111);
				this.loadStatus = "loading";
				this.query.currentPage++;
				this.getLedgerList();
			},
			getLedgerList() {
				console.log(this.schoolName);

				LedgerList(this.query).then(res => {
					console.log(res);
					if (res.data.data.length > 0) {
						this.flieList.push.apply(this.flieList, res.data.data);

					} else {
						this.query.currentPage--;
					}
					this.loadStatus = "nomore";
				});
			},



			// 页面数据
			getFliesList() {
				this.stores.schoolUuid = uni.getStorageSync('schoolguid');
				moneyList(this.stores).then(res => {
					if (res.data.data.length > 0) {
						for (let k = 0; k < res.data.data.length; k++) {
							this.title[k] = res.data.data[k].supplier;
							if (this.title[k].length > 4) {
								this.title[k] = this.title[k].substring(0, 4) + '...';
							}
						}
					}
					this.flieList = res.data.data;
				});
			},
			shwoType(type) {
				if (type == "disinfection") {
					return "消毒";
				}
				if (type == "inspection") {
					return "晨检";
				}
				if (type == "pesticide") {
					return "农药残留";
				}
				if (type == "purchase") {
					return "采购";
				}
				if (type == "sample") {
					return "留样";
				}
				if (type == "waste") {
					return "废弃物处理";
				}
				if (type == "synthesize") {
					return "综合台账";
				}
			},
			show(id,type) {
				console.log(id);
				console.log(type);
				if (type == "disinfection") {
					console.log("消毒");
					uni.navigateTo({
						url: '/pages/canteenManage/moneyInfo/disinfectioninfo?id='+id,
					});
				}
				if (type == "inspection") {
					console.log("晨检");
					uni.navigateTo({
						url: '/pages/canteenManage/moneyInfo/inspectioninfo?id='+id,
					});
				}
				if (type == "pesticide") {
					console.log("农药残留");
					uni.navigateTo({
						url: '/pages/canteenManage/moneyInfo/pesticideinfo?id='+id,
					});
				}
				if (type == "purchase") {
					console.log("采购");
					uni.navigateTo({
						url: '/pages/canteenManage/moneyInfo/purchaseinfo?id='+id,
					});
				}
				if (type == "sample") {
					console.log("留样");
					uni.navigateTo({
						url: '/pages/canteenManage/moneyInfo/sampleinfo?id='+id,
					});
				}
				if (type == "waste") {
					console.log("废弃物处理");
					uni.navigateTo({
						url: '/pages/canteenManage/moneyInfo/wasteinfo?id='+id,
					});
				}
				if (type == "synthesize") {
					console.log("综合台账");
					uni.navigateTo({
						url: '/pages/canteenManage/moneyInfo/synthesizeinfo?id='+id,
					});
				}
				// uni.navigateTo({
				// 	url: '/pages/canteenManage/moneydetail?guid=' + e
				// });
			}
		}
	};
</script>

<style>
	/* #ifndef H5 */
	page {
		height: 100%;
	}
	/* #endif */
	.typeTag {
		font-size: 22rpx;
		float: left;
		width: 120rpx;
		height: 46rpx;
		border: 1px solid rgba(0, 151, 255, 1);
		border-radius: 8rpx;
		text-align: center;
		line-height: 46rpx;
		color: rgba(0, 151, 255, 1);
		background-color: rgba(0, 151, 255, 0.1);
	}
</style>
