<template>
	<view class="wrap" style="width: 100%;">
		<view class="list-wrap" v-if="isShow" style="padding: 0 40rpx;">
			<u-form ref="uForm" :errorType="errorType" style="float: left;width: 100%;border-bottom: 1rpx solid rgba(220, 220, 220, 1);margin-bottom: 40rpx;">
				<u-form-item :label-position="labelPosition" label="学校" prop="schoolName" label-width="150" style="font-size: 15px;">
					<u-input
						:border="border"
						type="select"
						:select-open="selectShow"
						v-model="schoolName"
						placeholder="请选择学校"
						style="font-size: 13px;"
						@click="selectShow = true"
					></u-input>
				</u-form-item>
			</u-form>
			<view style="padding: 0 40rpx;">
				<u-button type="primary" shape="circle" @click="toHomeIndex">确认</u-button>
			</view>
			<!-- <u-select mode="single-column" :list="selectList" v-model="selectShow" @confirm="selectConfirm"></u-select> -->
			<u-select mode="mutil-column-auto" :list="selectList" v-model="selectShow" @confirm="selectConfirm"></u-select>
			<!-- <u-button type="primary" @click="topay">支付</u-button> -->
			<u-modal v-model="show" :show-cancel-button="true" @confirm="phones" style="text-align: center;">是否确认拨打18067915778？</u-modal>
		</view>
		<view style="position: fixed; bottom: 0; text-align: center;width: 100%;line-height: 48rpx; color: rgba(165, 165, 165, 1);margin-bottom: 60rpx;">
			<view style="width: 38px; margin: 10px auto ; height: 0; border-top: 1px solid rgba(151, 151, 151, 1);"></view>
			<view>萧山区教育局 &nbsp;荣誉出品</view>
			<view>
				<view style="display: inline;margin-right: 20rpx;">浙江九龙物联科技有限公司</view>
				<view @click="showModel" style="color: rgba(0, 151, 255, 1); display: inline;">技术支持</view>
			</view>
		</view>
	</view>
</template>

<script>
import { getSchoolList, getSchoolLink } from '@/api/school/school.js';

export default {
	data() {
		return {
			labelPosition: 'left',
			selectShow: false,
			schoolName: '',
			border: false,
			show: true,
			schoolguid: '',
			school: [],
			selectList: [
				{
					value:1,
					label:'幼儿园',
					children:[]
				},{
					value:2,
					label:'小学',
					children:[]
				},{
					value:3,
					label:'初中',
					children:[]
				},{
					value:4,
					label:'高中',
					children:[]
				}
			],
			isShow: false,
			// 模态框显示与隐藏
			show: false
		};
	},
	methods: {
		phones() {
			uni.makePhoneCall({
				phoneNumber: '18067915778',
				success: function(res) {
					console.log(res);
				},
				fail: function(res) {
					console.log(res);
				}
			});
		},
		// 打开模态框
		showModel() {
			this.show = true;
		},
		// 加载学校列表
		loadSchoolList() {
			console.log(1);
			getSchoolList().then(res => {
				this.schools = res.data.data;
				// this.selectList[0].children = [];
				console.log(res)
					if(res.data.data.yeyquery.length>0){
						for (let i = 0; i < res.data.data.yeyquery.length; i++) {
							let data = res.data.data.yeyquery[i];
							this.selectList[0].children.push({
								value: data.schoolUuid,
								label: data.schoolName
							});
						}
					}else{
						this.selectList[0].children.push({
							value: "",
							label: ""
						});
					}
					if(res.data.data.xxquery.length>0){
						for (let i = 0; i < res.data.data.xxquery.length; i++) {
							let data = res.data.data.xxquery[i];
							this.selectList[1].children.push({
								value: data.schoolUuid,
								label: data.schoolName
							});
						}
					}else{
						this.selectList[1].children.push({
							value: "",
							label: ""
						});
					}
					if(res.data.data.czquery.length>0){
						for (let i = 0; i < res.data.data.czquery.length; i++) {
							let data = res.data.data.czquery[i];
							this.selectList[2].children.push({
								value: data.schoolUuid,
								label: data.schoolName
							});
						}
					}else{
						this.selectList[2].children.push({
							value: "",
							label: ""
						});
					}
					if(res.data.data.gzquery.length>0){
						for (let i = 0; i < res.data.data.gzquery.length; i++) {
							let data = res.data.data.gzquery[i];
							this.selectList[3].children.push({
								value: data.schoolUuid,
								label: data.schoolName
							});
						}
					}else{
						this.selectList[3].children.push({
							value: "",
							label: ""
						});
					}
				this.isShow = true;
				//this.selectList=res.data.data.map(x=>{value:x.schoolUuid,label:x.schoolName})
				//console.log(this.schools);
				//console.log(this.selectList);
			});
		},
		// 注意返回值为一个数组，单列时取数组的第一个元素即可(只有一个元素)
		confirm(e) {
			//console.log(e);
		},
		selectConfirm(e) {
			this.schoolName = '';
			e.map((val, index) => {
				this.schoolName += this.schoolName == '' ? val.label : '-' + val.label;
				this.schoolguid = val.value;
			});
			//console.log(this.schoolguid);
		},
		toHomeIndex() {
			if (this.schoolguid == null || this.schoolguid == '') {
				uni.showToast({
					title: '请选择学校',
					duration: 2000,
					icon: 'none'
				});
				return;
			}
			// let sch=this.schools.find(x=>x.schoolName==this.picker[this.index]);
			// console.log(sch);

			uni.setStorageSync('schoolguid', this.schoolguid);
			uni.setStorageSync('schoolname',this.schoolName);
			getSchoolLink(this.schoolguid).then(res => {
				console.log(res);
				uni.setStorageSync('link', res.data.data);
			});
			//console.log(uni.getStorageSync('schoolguid'));
			uni.redirectTo({
				url: '/pages/home/index'
			});
		},
		topay() {
			uni.redirectTo({
				url: '/pages/pay/pay'
			});
		}
	},
	mounted() {
		this.isShow = false;
		this.loadSchoolList();
	},
	onLoad() {
		// this.getUserInfoOpenID();
	}
};
</script>

<style>
page {
	width: 100%;
	height: 100%;
}
</style>
