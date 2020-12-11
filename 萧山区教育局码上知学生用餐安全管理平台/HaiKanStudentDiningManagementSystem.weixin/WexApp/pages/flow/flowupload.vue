<template>
	<view class="wrap">
		<u-form ref="uForm">
			<u-form-item :label-position="labelPosition" label="上传时间" label-width="150">
				<view style=" " @click="openDropdown">
					<view style="float: left; margin-right: 10px;">{{ timetitle }}</view>
					<u-icon name="arrow-down-fill" top="8px" v-if="dateShow != true" style="float: right;color: rgba(0, 151, 255, 1);"></u-icon>
					<u-icon name="arrow-up-fill" top="8px" v-else style="float: right;color: rgba(0, 151, 255, 1);"></u-icon>
				</view>
			</u-form-item>
			<u-form-item :label-position="labelPosition" label="成菜流程" label-width="150">
				<u-input :border="border" type="select" :select-open="selectShow" v-model="type" placeholder="请选择成菜流程" @click="selectShow = true"></u-input>
			</u-form-item>
		</u-form>
		<u-select mode="single-column" :list="dateType" v-model="selectShow" @confirm="selectConfirm"></u-select>
		<u-calendar v-model="dateShow" :mode="'date'" @change="change"></u-calendar>
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
			<button class="dlbutton" hover-class="dlbutton-hover" @click="doUpload()">确认上传</button>
		</view>
	</view>
</template>

<script>
import { GetAppCreate } from '@/api/flow/flow.js';
import http from '../../utils/http.js';
export default {
	data() {
		return {
			labelPosition: 'left',
			border: false,
			selectShow: false,
			type: '',
			dateType: [
				{ value: '验收', label: '验收' },
				{ value: '清洗', label: '清洗' },
				{ value: '切配', label: '切配' },
				{ value: '加工', label: '加工' },
				{ value: '成菜', label: '成菜' }
			],
			imageList: [],
			action: http.baseUrl + 'api/v1/FoodProcess/Flow/UpLoad',
			actiondelete: http.baseUrl + 'api/v1/FoodProcess/Flow/DeleteFile',
			header: { Authorization: 'Bearer ' + uni.getStorageSync('token') },
			uplist: [],
			dateShow: false,
			timetitle: ''
		};
	},
	methods: {
		// 选择用餐类型回调
		selectConfirm(e) {
			this.type = '';
			e.map((val, index) => {
				this.type += this.type == '' ? val.label : '-' + val.label;
			});
		},
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
			//console.log(e);
		},
		async doUpload() {
			let imgs = [];
			let list = this.$refs.eupcf.uploadImages;
			if (this.type == '') {
				uni.showModal({
					title: '提示',
					content: '请选择流程类别',
					showCancel: false
				});
				return;
			}
			if (list.length <= 0) {
				uni.showModal({
					title: '提示',
					content: '请选择图片',
					showCancel: false
				});
				return;
			}
			let data = {
				addPeople: this.$store.state.userName,
				flowName: this.type,
				accessory: list.join(','),
				schoolUuid: this.$store.state.schoolGuid,
				datetime:this.timetitle
			};
			console.log(data);
			await GetAppCreate(data).then(res => {
				console.log(777777);
				console.log(res);
				if (res.data.code == 200) {
					uni.navigateBack({
						url: '/pages/flow/flow'
					});
					uni.showModal({
						title: '上传成功',
						showCancel: false
					});
				} else {
					uni.showModal({
						title: res.data.message,
						showCancel: false
					});
				}
			});
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
		this.imageList = [];
	}
};
</script>

<style>
/* #ifndef H5 */
/* page {
	height: 100%;
	background-color: #f2f2f2;
} */
.u-collapse-title {
	margin-left: 10px;
}
.u-icon-wrap {
	margin-right: 10px;
}
/* #endif */
</style>

<style lang="scss" scoped>
.wrap {
	padding-left: 30rpx;
	padding-right: 30rpx;
}
.u-cell-icon {
	width: 44rpx;
	height: 44rpx;
	margin-right: 8rpx;
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
