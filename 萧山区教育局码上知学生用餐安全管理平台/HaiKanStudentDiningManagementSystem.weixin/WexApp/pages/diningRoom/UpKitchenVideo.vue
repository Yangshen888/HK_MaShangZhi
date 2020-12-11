<template>
	<view class="wrap">
		<view class="u-tabs-box">
			<u-tabs-swiper activeColor="#f29100" ref="tabs" :list="list" :current="current" @change="change" :is-scroll="false" swiperWidth="750"></u-tabs-swiper>
		</view>
		<swiper class="swiper-box" :current="swiperCurrent" @transition="transition" @animationfinish="animationfinish">
			<swiper-item class="swiper-item">
				<scroll-view scroll-y style="height: 100%;width: 100%;">
					<view >
						<easy-upload
							ref="eupcf"
							:dataList="imageList"
							:uploadUrl="action"
							:types="'video'"
							:deleteUrl="actiondelete"
							:uploadCount="6"
							@successVideo="successvideo"
							:upload_max="25"
							:uploadList="uplist"
							:header="header"
						></easy-upload>
						<button class="dlbutton" hover-class="dlbutton-hover" @click="doUpload(1)">确认上传</button>
					</view>
				</scroll-view>
			</swiper-item>
			<swiper-item class="swiper-item">
				<scroll-view scroll-y style="height: 100%;width: 100%;">
					<view >
						<easy-upload
							ref="eupct"
							:dataList="imageList"
							:uploadUrl="action"
							:types="'video'"
							:deleteUrl="actiondelete"
							:uploadCount="6"
							@successVideo="successvideo"
							:upload_max="25"
							:uploadList="uplist"
							:header="header"
						></easy-upload>
						<button class="dlbutton" hover-class="dlbutton-hover" @click="doUpload(2)">确认上传</button>
					</view>
				</scroll-view>
			</swiper-item>
		</swiper>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import {createDinningvideo} from '@/api/diningRoom/kitchenVideo.js'
export default {
	data() {
		return {
			current: 0,
			list: [
				{
					name: '厨房视频上传'
				},
				{
					name: '餐厅视频上传'
				}
			],
			swiperCurrent: 0,
			action: http.baseUrl + 'api/v1/DiningRoom/KitchenVideo/UpLoad',
			actiondelete: http.baseUrl + 'api/v1/DiningRoom/KitchenVideo/DeleteFile',
			header: { Authorization: 'Bearer ' + uni.getStorageSync('token') },
			imageList: [],
			uplist: [],
			timetitle:''
		};
	},
	methods: {
		change(index) {
			this.swiperCurrent = index;
			console.log(index);
			if (index == 0) {
				// this.getOrderList('厨房');
				// this.checkselect=true;
			}
			if (index == 1) {
				// this.getOrderList('餐厅');
				// this.checkselect=false;
			}
			//this.getOrderList();
		},
		transition({ detail: { dx } }) {
			this.$refs.tabs.setDx(dx);
		},
		animationfinish({ detail: { current } }) {
			this.$refs.tabs.setFinishCurrent(current);
			this.swiperCurrent = current;
			this.current = current;
		},
		successvideo(e) {
			// console.log(e);
			// console.log(this.imageList);
			// console.log(this.$refs.eup.uploads);
		},
		async doUpload(num) {
			
			let sum=0;
			if (num == 1) {
				let list=this.$refs.eupcf.uploadlist;
				if(list.length<=0){
					uni.showModal({
						title: '提示',
						content: '请选择视频',
						showCancel: false
					});
					return ;
				}
				console.log(this.$refs.eupcf.uploadlist);
				for(let i=0;i<list.length;i++){
					let data={
						name:'厨房视频',
						addPeople:this.$store.state.userName,
						type:'厨房',
						addtime:this.timetitle,
						accessory:list[i],
						schoolUuid:this.$store.state.schoolGuid,
					}
					console.log(data);
					await createDinningvideo(data).then(res=>{
						console.log(res);
						if(res.data.code==200){
							sum++;
							console.log(sum);
						}
					});
					
				}
				if(sum>0){
					uni.navigateTo({
						url: '/pages/diningRoom/diningRoomList'
					});
					uni.showModal({
						title: '视频上传成功',
						showCancel: false
					});
					// uni.navigateTo({
					// 	url: '/pages/diningRoom/diningRoomList'
					// });
				}
				
				
			}
			if (num == 2) {
				console.log(this.$refs.eupct.uploadlist);
				let list=this.$refs.eupct.uploadlist;
				for(let i=0;i<list.length;i++){
					let data={
						name:'餐厅视频',
						addPeople:this.$store.state.userName,
						type:'餐厅',
						addtime:this.timetitle,
						accessory:list[i],
						schoolUuid:this.$store.state.schoolGuid,
					}
					console.log(data);
					await createDinningvideo(data).then(res=>{
						console.log(res);
						if(res.data.code==200){
							sum++;
						}
					});
					
				}
				if(sum>0){
					uni.navigateTo({
						url: '/pages/diningRoom/diningRoomList'
					});
					uni.showModal({
						title: '视频上传成功',
						showCancel: false
					});
					
				}
			}
		}
	},
	onLoad() {
		let date = new Date();
		let day = date.getDate().toString();
		let month = (date.getMonth() + 1).toString();
		let year = date.getFullYear().toString();
		if (day.length < 2) {
			day = '0' + day;
		}
		if (month.length < 2) {
			month = '0' + month;
		}
		this.timetitle = year + '-' + month + '-' + day;
	}
};
</script>

<style>
.wrap {
	padding: 30rpx;
	display: flex;
	flex-direction: column;
	height: calc(100vh - var(--window-top));
	width: 100%;
}
.dlbutton {
	color: #ffffff;
	font-size: 34upx;
	width: 470upx;
	height: 100upx;
	background: linear-gradient(-90deg, rgba(63, 205, 235, 1), rgba(188, 226, 158, 1));
	box-shadow: 0upx 0upx 13upx 0upx rgba(164, 217, 228, 0.2);
	border-radius: 50upx;
	line-height: 100upx;
	text-align: center;
	margin-left: auto;
	margin-right: auto;
	margin-top: 40upx;
}
.dlbutton-hover {
	background: linear-gradient(-90deg, rgba(63, 205, 235, 0.9), rgba(188, 226, 158, 0.9));
}
.swiper-box {
	flex: 1;
}
.swiper-item {
	height: 100%;
}
</style>
