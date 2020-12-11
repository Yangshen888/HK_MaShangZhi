<template>
	<view style="padding-left:40rpx;">
		<view v-for="(item, index) in flieList" :key="index">
			<view style="letter-spacing: 2rpx; padding: 20px 20px 20px 0; border-bottom: 2rpx solid rgba(234, 234, 234, 1);">
				<view class="trainItem" style="overflow: hidden; line-height:45px;">
					<view style="float: left; font-size: 32rpx; font-weight: 600; width: 52%; overflow: hidden; white-space: nowrap;text-overflow: ellipsis;">
						{{ title[index] }}
					</view>
					<view @click="show(item.trainUuid)" style="width: 140rpx; float: right; position: relative; color: rgba(0, 151, 255, 1);">
						<u-icon name="arrow-right" size="20" style="float: right;position: absolute; top: 50%; right: 0; transform: translateY(-50%); "></u-icon>
						<view style="float: right;letter-spacing: 0; font-size: 24rpx;margin-right: 24rpx;">查看详情</view>
					</view>
				</view>
				<view style="overflow: hidden; font-size: 30rpx; line-height: 60rpx; color: rgba(51, 52, 53, 1); position: relative;">
					<view class="trainnerInfo">授课人：{{ item.speaker }}</view>
					<view style="line-height: 48rpx;overflow: hidden;">
						<view class="trainTime" style="float: left;">{{ item.trainTime }}</view>
						<view class="trainSite" style="float: right;margin-right: 10rpx;">
							<image src="https://msz-b.jiulong.yoruan.com/img/images/dizhi@2x.png" style="width: 26rpx; height: 17px;float: left;margin: 8rpx 10rpx 0 0;"></image>
							<view style="float: left; color: rgba(253, 93, 55, 1);">{{ item.address }}</view>
						</view>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { messageList } from '@/api/canteenManage/message.js';
export default {
	data() {
		return {
			stores: {
				schoolUuid: ''
			},
			flieList: [],
			title: []
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
			messageList(this.stores).then(res => {
				if (res.data.data.length > 0) {
					for (let k = 0; k < res.data.data.length; k++) {
						this.title[k] = res.data.data[k].theme;
						// if (this.title[k].length > 4) {
						// 	this.title[k] = this.title[k].substring(0, 4) + '...';
						// }
					}
				}

				this.flieList = res.data.data;
				console.log(this.flieList[0].trainUuid);
			});
		},
		show(e) {
			console.log(e);
			uni.navigateTo({
				url: '/pages/canteenManage/showdetail?guid=' + e
			});
		}
	}
};
</script>

<style></style>
