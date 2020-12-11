<template>
	<view class="wrap">
		<u-form ref="uForm">
			<u-form-item :label-position="labelPosition" label="上传时间" label-width="150">
				<view style=" " @click="openDropdown">
					<view style="float: left; margin-right: 10px;">{{ timetitle }}</view>
					<u-icon name="arrow-down-fill"  top="8px" v-if="dateShow != true"  style="float: right;color: rgba(0, 151, 255, 1);"></u-icon>
					<u-icon name="arrow-up-fill"  top="8px" v-else  style="float: right;color: rgba(0, 151, 255, 1);"></u-icon>
				</view>
			</u-form-item>
			<u-form-item :label-position="labelPosition" label="用餐类型" label-width="150">
				<u-input :border="border" type="select" :select-open="selectShow" v-model="type" placeholder="请选择用餐类型" @click="selectShow = true"></u-input>
			</u-form-item>
			<u-form-item :label-position="labelPosition" label="菜品名称" label-width="150">
				<u-input :border="border" type="select" :select-open="selectShow1" v-model="query.cuisineName" placeholder="请选择菜品名称" @click="selectShow1 = true"></u-input>
			</u-form-item>
		</u-form>
		<u-calendar v-model="dateShow" :mode="'date'" @change="change"></u-calendar>
		<u-select mode="single-column" :list="dateType" v-model="selectShow" @confirm="selectConfirm"></u-select>
		<u-select mode="single-column" :list="cuisines" v-model="selectShow1" @confirm="selectConfirm1"></u-select>

		<view>
			图片：
			<easy-upload
				ref="eupcf"
				:dataList="imageList"
				:uploadUrl="action"
				:types="'image'"
				:deleteUrl="actiondelete"
				:uploadCount="6"
				@successImage="successImage"
				:upload_max="5"
				:uploadList="uplist"
				:header="header"
			></easy-upload>
			<!-- 视频：
			<easy-upload
				ref="eupcf2"
				:dataList="imageList2"
				:uploadUrl="action2"
				:types="'video'"
				:deleteUrl="actiondelete2"
				:uploadCount="6"
				@successVideo="successvideo"
				:upload_max="25"
				:uploadList="uplist2"
				:header="header2"
			></easy-upload> -->
			<button class="dlbutton" hover-class="dlbutton-hover" @click="doUpload()">确认上传</button>
		</view>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { CuisineList, createLiveShot } from '@/api/diningRoom/liveShot.js';
export default {
	data() {
		return {
			action: http.baseUrl + 'api/v1/DiningRoom/LiveShot/UpLoad',
			action2: http.baseUrl + 'api/v1/DiningRoom/LiveShot/VideoUpLoad',
			actiondelete: http.baseUrl + 'api/v1/DiningRoom/LiveShot/DeleteFile',
			actiondelete2: http.baseUrl + 'api/v1/DiningRoom/LiveShot/VideoDeleteFile',
			header: { Authorization: 'Bearer ' + uni.getStorageSync('token') },
			header2: { Authorization: 'Bearer ' + uni.getStorageSync('token') },
			imageList: [],
			imageList2: [],
			uplist: [],
			uplist2: [],
			labelPosition: 'left',
			border: false,
			selectShow: false,
			selectShow1: false,
			type: '',
			typevalue: '',
			dateType: [{ value: 'morn', label: '早餐' }, { value: 'noon', label: '中餐' }, { value: 'night', label: '晚餐' }],
			cuisines: [],
			query: {
				cuisineUuid: '',
				cuisineName: ''
			},
			timetitle:'',
			dateShow:false
		};
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
	},
	methods: {
		
		// 打开或关闭下拉选择框
		openDropdown() {
			if (this.dateShow == false) {
				this.dateShow = true;
			} else {
				this.dateShow = false;
			}
		},
		change(e) {
			this.timetitle = e.result;
		},
		successImage(e) {
			console.log(e);
			// console.log(this.imageList);
			// console.log(this.$refs.eup.uploads);
		},
		successvideo(e) {
			// console.log(e);
			// console.log(this.imageList);
			// console.log(this.$refs.eup.uploads);
		},
		doCuisineList() {
			console.log(this.timetitle)
			CuisineList({ datetime: this.timetitle, type: this.typevalue }).then(res => {
				this.cuisines = [];
				console.log(res)
				for (let i = 0; i < res.data.data.length; i++) {
					let data = res.data.data[i];
					this.cuisines.push({
						value: data.cuisineUuid,
						label: data.cuisineName
					});
				}
			});
		},
		// 选择用餐类型回调
		selectConfirm(e) {
			this.type = '';
			e.map((val, index) => {
				this.type += this.type == '' ? val.label : '-' + val.label;
				this.typevalue = this.typevalue == '' ? val.value : val.value;
			});
			this.doCuisineList();
		},
		// 选择菜品回调
		selectConfirm1(e) {
			this.query.cuisineName = '';
			e.map((val, index) => {
				this.query.cuisineName = this.query.cuisineName == '' ? val.label : '-' + val.label;
				this.query.cuisineUuid = val.value;
			});
			console.log(this.query.cuisineName);
			console.log(this.query.cuisineUuid);
		},
		async doUpload() {
			let imgs=[];
			let list =this.$refs.eupcf.uploadImages;
			// let list2=this.$refs.eupcf2.uploadlist;
			console.log(123);
			// console.log(list);
			// if (list.length <= 0 ) {
			// 	uni.showModal({
			// 		title: '提示',
			// 		content: '请选择图片',
			// 		showCancel: false
			// 	});
			// 	return;
			// }
			if (this.query.cuisineUuid == '' || this.type == '') {
				uni.showModal({
					title: '提示',
					content: '请选择用餐类型及菜品',
					showCancel: false
				});
				return;
			}
			// list.forEach(function(value,index,obj){
			// 	let arr=value.split('\\');
			// 	imgs.push(arr[1]);
			// });
			console.log(imgs);
			console.log(this.type);
			let data = {
				cuisineUuid: this.query.cuisineUuid,
				addPeople: this.$store.state.userName,
				datetype: this.typevalue,
				accessory: list.join('|'),
				datetime: this.timetitle,
				schoolUuid: this.$store.state.schoolGuid,
				//accessoryvido:list2.join('|')
			};
			console.log(data);
			await createLiveShot(data).then(res=>{
			console.log(res);
			if(res.data.code==200){
				uni.redirectTo({
					url: '/pages/diningRoom/LiveShot'
				});
				uni.showModal({
					title: '上传成功',
					showCancel: false
				});
			}else{
				uni.showModal({
					title: res.data.message,
					showCancel: false
				});
			}
			});
		},
		GetDate() {
			let time = new Date();
			let sdate = time.getFullYear() + '-' + (time.getMonth() + 1) + '-' + time.getDate();
			return sdate;
		}
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
</style>
