<template>
	<view>
		<u-loading :show="showloading" size="50" color="#2979ff" style="position: absolute;top: 50%;left: 50%;transform: translate(-50%,-50%);"></u-loading>
		<view class="wrap">
			<!-- <u-card :head-border-bottom="head" :foot-border-top="head"> -->
				<view class="" slot="body">
					<view class="u-body-item">
						<view  class="u-line-5" style="font-size: 18px;font-weight: 600;margin-top: 20px;">{{ flieList.headline }}</view>
						<view style="font-size: 12px;color: #A5A5A5;margin: 10px 0;">{{ flieList.addtime }}</view>
						<!-- <view style="padding: 5px;" v-show="showsrc1==1"><u-swiper :list="list1" :height="500"></u-swiper></view> -->
						<!-- <view  style="margin-top: 5px;">{{flieList.content }}</view> -->
						
						<!-- <rich-text :nodes="flieList.content"></rich-text> -->
						<view class="u-content" style="letter-spacing: 2px; text-align: justify;">
								<u-parse :html="flieList.content"></u-parse>
						</view>
					</view>
					<view>
						<image></image>
					</view>
				</view>
			<!-- </u-card> -->
		</view>
	</view>
</template>

<script>
	import http from '@/utils/http.js';
	import { SchoolJourGet,getGetMYNew } from '@/api/campusnews/news.js';
	export default {
		data() {
			return {
				labelPosition: 'left',
				head: false,
				guid:'',
				flieList: [],
				list1: [],
				showsrc1:1,
				url: http.baseUrl + 'UploadFiles/Picture/',
				showloading:true
			};
		},
		computed: {},
		methods: {
			// 页面数据
			getFliesList() {
				SchoolJourGet(this.guid).then(res=>{
					this.flieList=res.data.data;
					this.showloading=false;
					// console.log(res.data.data)
					// if (res.data.data.accessory != '') {
					// 	for (let k = 0; k < res.data.data.accessory.split(',').length; k++) {
					// 		let images = {
					// 			image: (this.url + res.data.data.accessory.split(',')[k]).toString()
					// 		};
					// 		this.list1[k] = images;
					// 		this.showsrc1=1;
					// 	}
					// } else {
					// 	this.list1 = [];
					// 	this.showsrc1=0;
					// }
					// console.log(this.list1)
				})
			},
			getMYList() {
				getGetMYNew(this.guid).then(res=>{
					this.flieList=res.data.data[0];
					this.showloading=false;
				})
			}
		},
		onLoad: function(e) {
			console.log(e)
			if(e.type=="内"){
				this.guid = e.guid;
				this.getFliesList();
			}else{
				this.guid = e.guid;
				this.getMYList();
			}
		}
	};
</script>


<style>
/* #ifndef H5 */
page {
	height: 100%;
	/* background-color: #f2f2f2; */
}
.wrap {
	/* padding: 30rpx; */
	word-break: break-all;
	padding: 30rpx 60rpx;
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
}
</style>
