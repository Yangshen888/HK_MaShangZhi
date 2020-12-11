<template>
      <view>
		<view class="wrap">
			<u-card :head-border-bottom="head" :foot-border-top="head">
				<u-loading :show="showloading" size="50" color="#2979ff" style="position: absolute;top: 50%;left: 50%;transform: translate(-50%,-50%);"></u-loading>
				<view class="" slot="body">
					<view class="u-body-item">
						<view  class="u-line-5" style="font-size: 18px;font-weight: 600;margin-top: 10px;text-align: center;">{{ flie.flieName }}</view>
						<view style="font-size: 12px;color: #909399;text-align: center;margin-top: 5px;">{{ flie.effectiveTime }}生效</view>
						<view style="padding: 5px;" v-show="showsrc1==1"><u-swiper :list="list1" :height="500"></u-swiper></view>
						<!-- <view  style="margin-top: 5px;">{{flieList.content }}</view> -->
						
						<!-- <rich-text :nodes="flieList.content"></rich-text> -->
						<view class="u-content">
								<u-parse :html="flie.accessory"></u-parse>
							</view>
					</view>
				</view>
			</u-card>
		</view>
		</view>
</template>

<script>
import http from '@/utils/http.js'
import { getFileInfo } from '@/api/quality/quality.js';
export default {
	data() {
		return {
			stores: {							
				schoolUuid: ''
			},
			flie: {},
			showloading:true,
		};
	},
	onLoad(opt) {	
		let that=this;
		console.log(opt.uuid);
		//this.getFliesList();
		getFileInfo(opt.uuid).then(res=>{
			console.log(res);
			that.flie=res.data.data;
			this.showloading=false;
		});
	},
	computed: {
		
	},
	methods: {
		// 页面数据
		getFliesList() {			
			this.stores.schoolUuid = uni.getStorageSync('schoolguid');				
			console.log(this.stores.schoolUuid);
			getlist(this.stores).then(res => {							
				this.flieList = res.data.data;						
			});
		},
		downLoad(){
			
		},				
	}
};
</script>



<style>
/* #ifndef H5 */
page {
	height: 100%;
	background-color: #f2f2f2;
}
.wrap {
	word-break: break-all;
}
/* #endif */
</style>

<style lang="scss" scoped>
.u-card-wrap {
	background-color: $u-bg-color;
	padding: 1px;
}
.u-body-item {
	font-size: 32rpx;
	color: #333;
	padding: 20rpx 10rpx;
	margin-top: -45px;
}
</style>
