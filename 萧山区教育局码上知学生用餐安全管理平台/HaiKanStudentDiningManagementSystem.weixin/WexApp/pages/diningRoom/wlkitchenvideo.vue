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
			<u-form-item :label-position="labelPosition" label="视频类型" label-width="150">
				<u-input :border="border" type="select" :select-open="selectShow" v-model="type" placeholder="请选择视频类型" @click="selectShow = true"></u-input>
			</u-form-item>
		</u-form>
		<u-select mode="single-column" :list="dateType" v-model="selectShow" @confirm="selectConfirm"></u-select>
		<view style="height: 73%;">
			<scroll-view scroll-y style="height: 100%;width: 100%;" @scrolltolower="reachBottom">
				<view>
					<view style="color: rgba(51, 52, 53, 1);font-weight: 600;font-size: 32rpx;line-height: 3;">视频：</view>
					<u-loading :show="showloading" size="50" color="#2979ff" style="position: absolute;top: 50%;left: 50%;transform: translate(-50%,-50%);"></u-loading>
					<view v-for="(item, index) in flieList" :key="index" style="margin-top: -30rpx;">
						<view style="font-size: 32rpx; margin: 30rpx 0; float: left;width: 100%;color: rgba(165, 165, 165, 1);">{{ item[0].time }}</view>
						<view v-for="(data, indexs) in item" style="position: relative; width: 50%;height: 300rpx;float: left;padding: 4rpx;">
							<video style="width: 100%;height: 100%;border-radius: 10rpx;"></video>
							<view @click="imgchange(index, indexs)">
								<view
									style="position: absolute;top:16rpx;right: 16rpx; width: 30rpx;height: 30rpx;border-radius: 50%;border: 2rpx solid #ccc;line-height: 30rpx;text-align: center;"
									v-if="!data.ispath"
								></view>
								<view
									style="position: absolute;top:16rpx;right: 16rpx; width: 30rpx;height: 30rpx;border-radius: 50%;line-height: 30rpx;text-align: center;background-color: rgba(25, 161, 255, 1);"
									v-if="data.ispath"
								>
									<u-icon name="checkbox-mark" color="#fff" size="20" style="transform: translateY(-25%);"></u-icon>
								</view>
							</view>
						</view>
					</view>
				</view>
				<u-loadmore :status="loadStatus"></u-loadmore>
			</scroll-view>
		</view>
		<button class="dlbutton" hover-class="dlbutton-hover" @click="doUpload()">确认上传</button>
		<u-calendar v-model="dateShow" :mode="'date'" @change="change"></u-calendar>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { Getscreen, Getwlcreate } from '@/api/flow/flow.js';
export default {
	data() {
		return {
			query: {
				totalCount: 0,
				pageSize: 24,
				currentPage: 1,
				kw: '',
				kw1: '',
				kw2: '',
				sort: [
					{
						direct: 'DESC',
						field: 'ID'
					}
				]
			},
			loadStatus: 'loadmore',
			flieList: [],
			ispath: [],
			imageList: [],
			schoolName: '',
			labelPosition: 'left',
			border: false,
			selectShow: false,
			type: '',
			dateType: [
				{ value: '厨房', label: '厨房视频' },
				{ value: '餐厅', label: '餐厅视频' },
			],
			dateShow: false,
			timetitle: '',
			showloading: true
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
		this.schoolName = uni.getStorageSync('schoolname');
		this.schoolName = this.schoolName.split('-')[1];
		this.query.kw = this.schoolName;
		// this.query.kw = '湘湖小学';
		this.query.kw1 = this.timetitle;
		//this.getLedgerList();
		//this.getFliesList();
	},
	computed: {},
	methods: {
		// 选择用餐类型回调
		selectConfirm(e) {
			this.type = '';
			e.map((val, index) => {
				this.type += this.type == '' ? val.label : '-' + val.label;
			});
			this.query.kw2 = this.type;
			this.flieList = [];
			this.showloading = true;
			//this.getLedgerList();
			console.log(this.type);
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
			this.query.kw1 = e.result;
			//this.getLedgerList();
		},
		reachBottom(e) {
			console.log(111111);
			this.loadStatus = 'loading';
			this.query.currentPage++;
			//this.getLedgerList();
		},
		imgchange(e, f) {
			if (this.flieList[e][f].ispath) {
				this.flieList[e][f].ispath = false;
				var index = this.imageList.indexOf(this.flieList[e][f].path);
				this.imageList.splice(index, 1);
			} else {
				this.flieList[e][f].ispath = true;
				this.imageList.push(this.flieList[e][f].path);
			}
		},
		getLedgerList() {
			Getscreen(this.query).then(res => {
				console.log(res);
				if (res.data.code == 200) {
					for (let k = 0; k < res.data.data.length; k++) {
						if (res.data.data[k].length > 0) {
							this.flieList.push(res.data.data[k]);
						}
					}
				} else {
					this.flieList = [];
					this.query.currentPage--;
				}
				console.log(this.flieList);
				this.showloading = false;
				this.loadStatus = 'nomore';
			});
		},
		doUpload() {
			if (this.type == '') {
				uni.showModal({
					title: '提示',
					content: '请选择视频类别',
					showCancel: false
				});
				return;
			}
			if (this.imageList.length <= 0) {
				uni.showModal({
					title: '提示',
					content: '请选择视频',
					showCancel: false
				});
				return;
			}
			let data={
				name:this.type,
				addPeople:this.$store.state.userName,
				type:this.type.substring(0,1),
				addtime:this.timetitle,
				accessory:this.imageList.join(','),
				schoolUuid:this.$store.state.schoolGuid,
			}
			console.log(data);
			// Getwlcreate(data).then(res => {
			// 	console.log(res);
			// 	if (res.data.code == 200) {
			// 		uni.navigateBack({
			// 			url: '/pages/flow/flow'
			// 		});
			// 		uni.showModal({
			// 			title: '上传成功',
			// 			showCancel: false
			// 		});
			// 	} else {
			// 		uni.showModal({
			// 			title: res.data.message,
			// 			showCancel: false
			// 		});
			// 	}
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
	height: 100%;
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
