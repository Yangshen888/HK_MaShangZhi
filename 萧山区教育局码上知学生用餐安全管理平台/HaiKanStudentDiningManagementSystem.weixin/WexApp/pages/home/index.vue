<template>
	<view style="height: 100%;">
		<u-navbar title="萧山码上知" :background="background" :border-bottom="false" back-icon-color="#ffffff" title-color="#ffffff" :is-back="false"></u-navbar>
		<view class="wrap content">
			<scroll-view scroll-y style="height: 100%;width: 100%;" @scrolltolower="reachBottom">
			<view v-if="isShow" style="overflow: hidden;">
				<view style="margin-left: 16rpx;margin-right: 80rpx;float: left;width: 170rpx;height: 100rpx;margin-top: -5rpx;">
					<u-form ref="uForm" :errorType="errorType">
						<u-form-item :label-position="labelPosition" prop="schoolName">
							<u-input
								:border="border"
								type="select"
								:select-open="selectShow"
								v-model="schoolName"
								placeholder="请选择学校"
								@click="selectShow = true"
								placeholder-style="color: #ffffff;"
							></u-input>
						</u-form-item>
					</u-form>
					<u-select mode="mutil-column-auto" :list="selectList" v-model="selectShow" @confirm="selectConfirm"></u-select>
				</view>
				<view style="line-height: 100rpx;padding-right: 20rpx;">
					<u-search placeholder="查询信息" search-icon-color="#909399" :clearabled="true" :show-action="false" height="56"></u-search>
				</view>
			</view>
			<view style="padding: 0 20rpx 0 20rpx;"><u-swiper :list="list" :height="320" mode="round"></u-swiper></view>
			<view style="padding:0 30rpx;">
				<!-- <view class="menu">
					<view style="float: left;margin-top: 40rpx;margin-left: 44rpx;color: #238FE2;font-weight: 600;">通知</view>
					<view style="float: left;"><image src="https://msz-b.jiulong.yoruan.com/img/image/jx14@2x.png" style="width: 4rpx;height: 40rpx;margin-top: 40rpx;margin-left: 16rpx;"></image></view>
					<view style="padding:40rpx; 24rpx 0 24rpx;" class="u-line-1">关于社会主义核心价值观教育进校园活动的相关通告</view>
				</view> -->
				<u-grid :col="4" :border="false">
					<u-grid-item @click="toDiningRoom">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/ygst@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;">阳光食堂</view>
					</u-grid-item>
					<u-grid-item @click="weekmenu" v-if="menushowlist.indexOf('菜单公示')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/cdgs@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">菜单公示</view>
					</u-grid-item>
					<u-grid-item @click="toMessageFeedBack"  v-if="menushowlist.indexOf('信息反馈')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/xxfk@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">信息反馈</view>
					</u-grid-item>
					<u-grid-item @click="quality" v-if="menushowlist.indexOf('光盘行动')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/gpxd2@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">光盘行动</view>
					</u-grid-item>
					<u-grid-item @click="liveShot" v-if="menushowlist.indexOf('每餐实拍')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/mcsp@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">每餐实拍</view>
					</u-grid-item>
					<u-grid-item @click="teseclick" v-if="menushowlist.indexOf('采购价格')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/cgjg@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">采购价格</view>
					</u-grid-item>
					<u-grid-item @click="chencaiclick" v-if="menushowlist.indexOf('成菜流程')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/cclc@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">成菜流程</view>
					</u-grid-item>
					<u-grid-item @click="kitchenVideo" v-if="menushowlist.indexOf('厨房视频')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/cfsp@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">厨房视频</view>
					</u-grid-item>
					<u-grid-item @click="quanbuclick" v-if="menushowlist.indexOf('人员培训')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/rypx@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">人员培训</view>
					</u-grid-item>
					<u-grid-item @click="teseclick2" v-if="menushowlist.indexOf('管理方案')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/glfa@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">管理方案</view>
					</u-grid-item>
					<u-grid-item @click="huncaiclick" v-if="menushowlist.indexOf('电子台账')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/dztz@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">电子台账</view>
					</u-grid-item>
					<u-grid-item @click="cuisine" v-if="menushowlist.indexOf('菜品库')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/cpk@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">菜品库</view>
					</u-grid-item>
					<u-grid-item @click="toWorkStudy" v-if="menushowlist.indexOf('勤工俭学')!=-1" v-show="isptjob==true">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/qgjx@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">勤工俭学</view>
					</u-grid-item>
					<u-grid-item @click="toActicle" v-if="menushowlist.indexOf('财务公开')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/jz5bf7@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">账务公开</view>
					</u-grid-item>
					<u-grid-item @click="toUserInfo" v-if="menushowlist.indexOf('员工管理')!=-1">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/bz9bf2@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">员工管理</view>
					</u-grid-item>
					<u-grid-item @click="toPay" v-if="menushowlist.indexOf('支付')!=-1"  v-show="schoolname">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/zf@3x.png" style="width: 86rpx;height: 86rpx;"></image>
						<view style="margin-top: 16rpx;font-weight: 400;">支付</view>
					</u-grid-item>
				</u-grid>
				<view class="menu">
					<view style="float: left;margin-top: 10rpx;margin-left: 16rpx;">
						<!-- <span style="color: #238FE2;font-weight: 600;font-family: STXingkai;">菜单</span>
						<span style="font-weight: 600;font-family: STXingkai;">公示</span> -->
						<image src="https://msz-b.jiulong.yoruan.com/img/images/cdgsT@3x.png" style="width: 104rpx;height: 32rpx;"></image>
					</view>
					<view style="float: left;"><image src="https://msz-b.jiulong.yoruan.com/img/image/jx14@2x.png" style="width: 2rpx;height: 30rpx;margin-top: 14rpx;margin-left: 16rpx;"></image></view>
					<view style="margin-top: -10rpx;">
						<u-notice-bar mode="horizontal" type="none" color="#b4b4b4" font-size="23rpx" :volume-icon="false" :list="menulist" v-if="menushow"></u-notice-bar>
					</view>
				</view>
			</view>
			<view class="head">
				<ul>
					<li style="overflow: hidden;">
						<image src="https://msz-b.jiulong.yoruan.com/img/image/jx15.png" style="width: 4rpx;height: 26rpx;margin-right: 8rpx;float: left;margin-top: -8rpx;"></image>
						<view style="margin-top: -16rpx;margin-left: 20rpx;font-weight: 600;font-size: 32rpx;">资讯</view>
					</li>
					<!-- <li>
						<view style="color: #0097FF; font-size: 24rpx;">查看更多</view>
						<image src="https://msz-b.jiulong.yoruan.com/img/image/fanhui备份%202.png" style="width: 8rpx;height: 16rpx; margin-top: -24rpx;"></image>
					</li> -->
				</ul>
			</view>
			<view class="d1" style="style1">
				<u-loading :show="showloading" size="50" color="#2979ff" style="position: absolute;top: 50%;left: 50%;transform: translate(-50%,-50%);"></u-loading>
				<view v-for="(item, index) in postjobs" :key="index" style="margin: 20rpx;font-size: 30rpx;" @click="clickshow(item)">
					<view>
						<view style="overflow: hidden;">
							<view style="margin: 10rpx;font-size: 0; float: left;width: 200rpx;height: 150rpx;overflow: hidden;position: relative;border-radius: 10rpx;" >
								<!-- <u-swiper :list="list1[index]" :circular="true" :autoplay="true" :height="180" style="width: 130px; float: right;"></u-swiper> -->
								<image :src="list1[index][0].image" style="width: 360rpx;border-radius: 10rpx;position: absolute;left: 50%;transform: translateX(-50%);" mode="widthFix"></image>
							</view>
							<view style="margin-top: 6px;" :style="style[index]">
								<view style="height: 110rpx;">
									<view class="u-line-1" style="font-size: 30rpx;font-weight: 600;">{{ item.headline }}</view>
									<view v-if="item.digest!=null" class="u-line-1" style="margin-top: 10rpx;margin-bottom: 20rpx;font-size: 26rpx;color: #909399;">
										{{ item.digest }}
									</view>
								</view>
								<view style="float: left;margin-top: -12rpx;">
									<view v-if="item.tag!=''&&item.tag!=null" class="au-tag" :style="item.tag=='健康'?jktagstyle:item.tag=='运动'?ydtagstyle:item.tag=='推荐'?tjtagstyle:item.tag=='广告'?ggtagstyle:qttagstyle">
										{{ item.tag}}
									</view>
								</view>
								<view style="font-size: 20rpx;color: #909399;float: right;">{{ item.addTime }}</view>
							</view>
						</view>
					</view>
					<!-- <u-line color="rgb(204, 208, 216)" /> -->
				</view>
				
				<view v-show="isrecharge" style="margin-bottom: 180rpx;">
				<u-loadmore v-if="isload" :status="loadStatus" ></u-loadmore>
				</view>
				<view v-show="!isrecharge" style="margin-bottom: 20rpx;">
				<u-loadmore v-if="isload" :status="loadStatus" ></u-loadmore>
				</view>
				<!-- <view style="height: 140rpx;"></view> -->
				
			</view>
			<view v-show="isrecharge">
				<view class="bottem">
					<view style="margin-top: 1%;margin-left: 19%;float: left;text-align: center;" @click="toOrderOnline">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/xz@3x.png" style="width: 58rpx;height:58rpx;"></image>
						<view style="color: #00aaff;font-weight: 600;font-size:24rpx;">订餐</view>
					</view>
					<view style="width: 1px;height:40px;float: left;background-color: #9cd4ff;margin: 4% 0 0 22%;"></view>
					<view style="margin-top: 1%;float: right;margin-right: 19%;text-align: center;" @click="toOrderOnline2">
						<image src="https://msz-b.jiulong.yoruan.com/img/images/jiaofei@3x.png" style="width: 58rpx;height:58rpx;"></image>
						<view style="color: #00aaff;font-weight: 600;font-size:24rpx;">缴费</view>
					</view>
				</view>
			</view>
			</scroll-view>
			<u-modal v-model="show" :show-cancel-button="true" @confirm="shouquan" style="text-align: center;">是否确认授权？</u-modal>
		</view>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { getSchoolInfo, getSchoolInfo2, getSchoolLink, getSchoolList } from '@/api/school/school.js';
import { getlist, getlifelist, SchoolJourGet, getschoolNews } from '@/api/campusnews/news.js';
import { Getweekmenu } from '@/api/weekmenu/menu.js';
export default {
	data() {
		return {
			labelPosition: 'left',
			selectShow: false,
			schoolName: '',
			border: false,
			show: false,
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

			ydtagstyle:'color: #00aaff;background-color: #c4e7ff;',
			jktagstyle:'color: #00cf00;background-color: #cdffe5;',
			qttagstyle:'color: #ff9900;background-color: #fdf6ec;',
			tjtagstyle:'color: #fb5252;background-color: #fef0f0;',
			ggtagstyle:'color: #afafaf;background-color: #f4f4f5;',
			style1: '',
			style2: 'display:none',
			menushow: false,
			list: [
				{
					image: 'https://msz-b.jiulong.yoruan.com/img/mz1.png',
					title: '背景1'
				},
				{
					image: 'https://msz-b.jiulong.yoruan.com/img/bmage4.png',
					title: '背景2'
				},
				{
					image: 'https://msz-b.jiulong.yoruan.com/img/mz31.png',
					title: '背景3'
				}
			],
			background: {
				backgroundColor: '#238FE2'
			},
			menulist: [],
			value: '',
			query: {
				totalCount: 0,
				pageSize: 10,
				currentPage: 1,
				kw: '',
				schoolUuid: '',
				name: '',
				sort: [
					{
						direct: 'DESC',
						field: 'ID'
					}
				]
			},
			list1: [],
			mlist1: [],
			showsrc1: [],
			mshowsrc1: [],
			list2: [],
			showsrc2: [],
			style: [],
			mstyle: [],
			styles: [],
			url: http.baseUrl + 'UploadFiles/Picture/',
			postjobs: [],
			mpostjobs:[],
			postjobsappeal: [],
			isptjob: false,
			isrecharge: false,
			loadStatus: 'nomore',
			isload: false,
			pindex:0,
			showloading:true,
			menushowlist:[],
			// isptjob: false,
			schoolname:true
		};
	},
	onLoad(opt) {
		this.isptjob=uni.getStorageSync('isptjob');
		if (typeof(opt.name) != "undefined") {
			this.loadSchoolInfo1(opt.name);
		} else {
			let guid = uni.getStorageSync('schoolguid');
			this.loadSchoolInfo2(guid);
		}
		// const res = uni.getSystemInfoSync();
		// uni.setStorageSync('language', res.platform);
		// this.getList();
		this.getList2();
		// this.getlifeList();
		this.doGetweekmenu();
	},
	methods: {
	 	async reachBottom() {
			this.loadStatus="loading";
			this.pindex++;
			this.query.currentPage++;
			await this.getList2();
			
			
		},
		loadSchoolList() {
			getSchoolList().then(res => {
				this.schools = res.data.data;
					// console.log( res.data.data)
					// console.log(789797)
				// for (let i = 0; i < res.data.data.length; i++) {
				// 	let data = res.data.data[i];
				// 	this.selectList.push({
				// 		value: data.schoolUuid,
				// 		label: data.schoolName
				// 	});
				// 	console.log(789797)
				// 	console.log(uni.getStorageSync('schoolguid'))
				// 	console.log(data.schoolUuid)
				// 	if (data.schoolUuid == uni.getStorageSync('schoolguid')) {
				// 		this.schoolName = data.schoolName.length > 4 ? data.schoolName.substring(0, 4) + '...' : data.schoolName;
				// 		this.schoolguid = uni.getStorageSync('schoolguid');
				// 		console.log(this.schoolName)
				// 	}
				// }
				if(res.data.data.yeyquery.length>0){
								// console.log(999)
					for (let i = 0; i < res.data.data.yeyquery.length; i++) {
								// console.log(99)
						let data = res.data.data.yeyquery[i];
								// console.log(data)
						this.selectList[0].children.push({
							value: data.schoolUuid,
							label: data.schoolName
						});
							if (data.schoolUuid == uni.getStorageSync('schoolguid')) {
								this.schoolName = data.schoolName.length > 4 ? data.schoolName.substring(0, 4) + '...' : data.schoolName;
								this.schoolguid = uni.getStorageSync('schoolguid');
								console.log(this.schoolName)
							uni.setStorageSync('schoolName', data.schoolName);
							}
					}
				}else{
					this.selectList[0].children.push({
						value: "",
						label: ""
					});
				}
				if(res.data.data.xxquery.length>0){
								// console.log(888)
					for (let i = 0; i < res.data.data.xxquery.length; i++) {
						let data = res.data.data.xxquery[i];
						this.selectList[1].children.push({
							value: data.schoolUuid,
							label: data.schoolName
						});
						if (data.schoolUuid == uni.getStorageSync('schoolguid')) {
							this.schoolName = data.schoolName.length > 4 ? data.schoolName.substring(0, 4) + '...' : data.schoolName;
							this.schoolguid = uni.getStorageSync('schoolguid');
							// console.log(this.schoolName)
							uni.setStorageSync('schoolName', data.schoolName);
						}
					}
				}else{
					this.selectList[1].children.push({
						value: "",
						label: ""
					});
				}
				if(res.data.data.czquery.length>0){
								// console.log(777)
					for (let i = 0; i < res.data.data.czquery.length; i++) {
						let data = res.data.data.czquery[i];
						this.selectList[2].children.push({
							value: data.schoolUuid,
							label: data.schoolName
						});
						if (data.schoolUuid == uni.getStorageSync('schoolguid')) {
							this.schoolName = data.schoolName.length > 4 ? data.schoolName.substring(0, 4) + '...' : data.schoolName;
							this.schoolguid = uni.getStorageSync('schoolguid');
							// console.log(this.schoolName)
							uni.setStorageSync('schoolName', data.schoolName);
						}
					}
				}else{
					this.selectList[2].children.push({
						value: "",
						label: ""
					});
				}
				if(res.data.data.gzquery.length>0){
								console.log(666)
					for (let i = 0; i < res.data.data.gzquery.length; i++) {
						let data = res.data.data.gzquery[i];
						this.selectList[3].children.push({
							value: data.schoolUuid,
							label: data.schoolName
						});
						if (data.schoolUuid == uni.getStorageSync('schoolguid')) {
							this.schoolName = data.schoolName.length > 4 ? data.schoolName.substring(0, 4) + '...' : data.schoolName;
							this.schoolguid = uni.getStorageSync('schoolguid');
							console.log(this.schoolName)
							uni.setStorageSync('schoolName', data.schoolName);
						}
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
			console.log(e);
		},
		selectConfirm(e) {
			this.schoolName = '';
			e.map((val, index) => {
				this.schoolName += this.schoolName == '' ? val.label : '-' + val.label;
				this.schoolguid = val.value;
			});
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
			getSchoolLink(this.schoolguid).then(res => {
				console.log(res);
				uni.setStorageSync('link', res.data.data);
			});
			//console.log(uni.getStorageSync('schoolguid'));
			uni.redirectTo({
				url: '/pages/home/index'
			});
			//console.log(this.schoolguid);
		},
		loadSchoolInfo1(name) {
			getSchoolInfo(name).then(res => {
				this.isptjob = res.data.data.isptjob;
				uni.setStorageSync('isptjob', res.data.data.isptjob);
				uni.setStorageSync('schoolName',name);
				console.log(8888888);
				console.log(res.data.data.isshowReport);
				uni.setStorageSync('isshowReport', res.data.data.isshowReport);
				uni.setStorageSync('isCuiPrices', res.data.data.isCuiPrices);
				uni.setStorageSync('isYard', res.data.data.isYard);
				uni.setStorageSync('qrcode', res.data.data.qrcode);
				uni.setStorageSync('accessory', res.data.data.accessory);
				this.isrecharge = res.data.data.isrecharge;
				uni.setStorageSync('schoolguid', res.data.data.schoolUuid);
				if (res.data.data.schoolImg != null) {
					this.list = [];
					let imgs = res.data.data.schoolImg.split('|');
					imgs.forEach(x => {
						this.list.push({ image: http.baseUrl + 'UploadFiles/SchoolImg/' + x, title: '' });
					});
				}
				uni.redirectTo({
					url: '/pages/home/index'
				});
				//this.list=imgs.select(x=>{image:http.baseUrl + 'UploadFiles/SchoolImg/'+x,title:''});
			});
		},
		loadSchoolInfo2(guid) {
			getSchoolInfo2(guid).then(res => {
				this.isptjob = res.data.data.isptjob;
				uni.setStorageSync('isCuiPrices', res.data.data.isCuiPrices);
				uni.setStorageSync('isYard', res.data.data.isYard);
				uni.setStorageSync('qrcode', res.data.data.qrcode);
				uni.setStorageSync('accessory', res.data.data.accessory);
				uni.setStorageSync('isshowReport', res.data.data.isshowReport);
				uni.setStorageSync('isptjob', res.data.data.isptjob);
				this.isrecharge = res.data.data.isrecharge;
				if (res.data.data.schoolImg != null) {
					this.list = [];
					let imgs = res.data.data.schoolImg.split('|');
					imgs.forEach(x => {
						this.list.push({ image: http.baseUrl + 'UploadFiles/SchoolImg/' + x, title: '' });
					});
				}
				if(res.data.data.menus!=""){
					this.menushowlist=res.data.data.menus.split(',');
				}
				// this.list=imgs.select(x=>{image:http.baseUrl + 'UploadFiles/SchoolImg/'+x,title:''});
			});
		},
		Tab(num) {
			if (num == 1) {
				this.style1 = 'display:block';
				this.style2 = 'display:none';
			} else {
				this.style1 = 'display:none';
				this.style2 = 'display:block';
			}
		},
		doGetweekmenu() {
			let data = uni.getStorageSync('schoolguid');
			Getweekmenu(data).then(res => {
				if (res.data.code == 200) {
					for (let k = 0; k < res.data.data.length; k++) {
						this.menulist[k] = res.data.data[k].cuisineName;
						// + '—' + res.data.data[k].price + '元'
					}
					this.menushow = true;
				}
			});
		},
		clickshow(e) {
			if (e.type == 2) {
				uni.navigateTo({
					url: '/pages/campusnews/shownew?guid=' + e.uuid+'&type='+e.inOut
				});
			}
		},
		//根据条件加载新闻数据
		// getList() {
		// 	this.query.schoolUuid = uni.getStorageSync('schoolguid');
		// 	getlist(this.query).then(res => {
		// 		this.postjobs = res.data.data;
		// 		if (res.data.data.length > 0) {
		// 			for (let k = 0; k < res.data.data.length; k++) {
		// 				let imgs = [];
		// 				if (res.data.data[k].accessory != '') {
		// 					for (let j = 0; j < res.data.data[k].accessory.split(',').length; j++) {
		// 						let images = {
		// 							image: (this.url + res.data.data[k].accessory.split(',')[j]).toString()
		// 						};
		// 						imgs[j] = images;
		// 					}
		// 					this.showsrc1[k] = 1;
		// 				} else {
		// 					imgs = [];
		// 					this.showsrc1[k] = 0;
		// 					this.style[k] = 'width: 97%;';
		// 				}
		// 				// this.list1[k] = imgs;
		// 				// this.list1[k].push(imgs[0]);
		// 				// console.log(imgs[0]);
		// 				this.list1[k] = [];
		// 				this.list1[k].push(imgs[0]);
		// 			}
		// 		} else {
		// 			this.list1 = [];
		// 		}
		// 	});
		// },
		getList2() {
			this.query.schoolUuid = uni.getStorageSync('schoolguid');
			getschoolNews(this.query).then(res => {
				console.log(7777777);
				console.log(res);
				console.log(this.postjobs);
				if (res.data.data.length > 0) {
					this.postjobs.push.apply(this.postjobs,res.data.data);
					for (let k = (this.pindex*10); k < (this.pindex*10+res.data.data.length); k++) {
						let imgs = [];
							let dindex=k-(this.pindex*10);
						if (res.data.data[dindex].accessory != '') {
							for (let j = 0; j < res.data.data[dindex].accessory.split(',').length; j++) {
								let images={};
								if(res.data.data[dindex].inOut=='内'){
									images = {
										image: (this.url + res.data.data[dindex].accessory.split(',')[j]).toString()
									};
								}else{
									images= {
										image: (res.data.data[dindex].accessory.split(',')[j]).toString()
									};
								}
								
								imgs[j] = images;
							}
							this.showsrc1[k] = 1;
						} else {
							imgs = [];
							this.showsrc1[k] = 0;
							this.style[k] = 'width: 97%;';
						}
						this.list1[k] = [];
						this.list1[k].push(imgs[0]);
					}
				} 
				this.showloading=false;
				this.isload = true;
				this.loadStatus="nomore";
			});
		},
		getlifeList() {
			this.query.schoolUuid = uni.getStorageSync('schoolguid');
			getlifelist(this.query).then(res => {
				this.postjobsappeal = res.data.data;
				if (res.data.data.length > 0) {
					for (let k = 0; k < res.data.data.length; k++) {
						let imgs = [];
						if (res.data.data[k].accessory != '') {
							for (let j = 0; j < res.data.data[k].accessory.split(',').length; j++) {
								let images = {
									image: (this.url + res.data.data[k].accessory.split(',')[j]).toString()
								};
								imgs[j] = images;
							}
							this.showsrc2[k] = 1;
						} else {
							imgs = [];
							this.showsrc2[k] = 0;
						}
						this.list2[k] = imgs;
					}
				} else {
					this.list2 = [];
				}
			});
		},
		liveShot() {
			uni.navigateTo({
				url: '/pages/diningRoom/LiveShot'
			});
		},
		teseclick() {
			uni.navigateTo({
				url: '/pages/diningRoom/PurchaseList'
			});
		},
		kitchenVideo() {
			uni.navigateTo({
				url: '/pages/diningRoom/kitchenVideo'
			});
		},
		chencaiclick() {
			uni.navigateTo({
				url: '/pages/flow/flow'
			});
		},
		quanbuclick() {
			uni.navigateTo({
				url: '/pages/canteenManage/message'
			});
		},
		teseclick2() {
			uni.navigateTo({
				url: '/pages/canteenManage/plan'
			});
		},
		huncaiclick() {
			uni.navigateTo({
				url: '/pages/canteenManage/money'
			});
		},
		cuisine() {
			uni.navigateTo({
				url: '/pages/cuisine/cuisinelist'
			});
		},
		toWorkStudy() {
			if (this.checkAuth()) {
				this.show = true;
			} else {
				uni.navigateTo({
					url: '../workstudy/home'
				});
			}
		},
		checkAuth() {
			console.log(11111);
			console.log(this.$store.state);
			if (this.$store.state.openid == '' || this.$store.state.openid == null) {
				return true;
			} else {
				return false;
			}
		},
		toActicle(){
			uni.navigateTo({
				url: '/pages/acticle/acticle'
			});
		},
		toUserInfo(){
			uni.navigateTo({
				url: '/pages/userinfo/userinfo'
			});
		},
		toPay() {
			if(uni.getStorageSync('isYard')){
				if (this.checkAuth()) {
					this.show = true;
				} else {
					uni.navigateTo({
						url: '/pages/pay/pay'
					});
				}
			}else{
				uni.navigateTo({
					url: '/pages/diningRoom/QRcode'
				});
			}
			
		},
		canteen() {
			uni.navigateTo({
				url: '/pages/canteenManage/canteenManage'
			});
		},
		quality() {
			uni.navigateTo({
				url: '/pages/quality/quality'
			});
		},
		news() {
			uni.navigateTo({
				url: '/pages/campusnews/news'
			});
		},
		weekmenu() {
			uni.navigateTo({
				url: '/pages/weekmenu/menu'
			});
		},
		toOrderOnline() {
			uni.navigateToMiniProgram({
				appId: 'wx475f3810a680175b',
				path: '',
				extraData: {},
				success(res) {
					// 打开成功
				}
			});
		},
		toOrderOnline2() {
			uni.navigateTo({
				url: '/pages/LinkToWeb'
			});
		},
		toMessageFeedBack() {
			if (this.checkAuth()) {
				this.show = true;
			} else {
				uni.navigateTo({
					url: '../messagefeedback/index'
				});
			}
		},
		toDiningRoom() {
			uni.navigateTo({
				url: '/pages/diningRoom/diningRoomList'
			});
		},
		toChangeIdentity() {
			uni.navigateTo({
				url: '/pages/login/selectSchool'
			});
		},
		shouquan(){
			uni.navigateTo({
				url: '/pages/login/WxAuthLogin'
			});
		},
		// toPay() {
		// 	if (this.checkAuth()) {
		// 		this.show = true;
		// 	} else {
		// 		uni.navigateTo({
		// 			url: '/pages/pay/pay'
		// 		});
		// 	}
		// }
	},
	mounted() {
		this.isShow = false;
		this.loadSchoolList();
	}
};
</script>

<style>
	/* #ifndef H5 */
	page {
		height: 100%;
	}
	/* #endif */
.wrap {
	/* background: url(https://msz-b.jiulong.yoruan.com/img/lj1.png) no-repeat;
	background-position: 92% -190%; */

	height: 100%;
}
.menu {
	/* background: url(https://msz-b.jiulong.yoruan.com/img/image/yjjx.png) no-repeat; */
	border: 8rpx solid #eef7fe;
	border-radius: 50rpx;
	background-size: 100%;
	height: 70rpx;
	width: 100%;
}
.bottem {
	position: fixed;
	bottom: 0;
	background-color: #ffffff;
	/* border-radius: 50px 50px 0 0; */
	height: 140rpx;
	width: 100%;
	z-index: 10;
	border-top: 1px solid rgb(204, 208, 216);
}
.head {
	width: 100%;
	height: 40rpx;
	margin-top: 40rpx;
}
.head ul {
	padding: 0;
	margin: 0;
}
.head ul li {
	list-style: none;
	float: left;
	text-decoration: none;
	display: block;
	width: 100px;
	padding: 5px;
	margin: 0px 0px 0px 10px;
	position: relative;
}
.d1 {
	width: 100%;
	position: absolute;
	z-index: 1;
	padding: 0 20rpx 0 20rpx;
}
.d2 {
	display: none;
	width: 100%;
	position: absolute;
	z-index: 1;
}
.au-tag{
	font-size: 22rpx;
	padding: 6rpx 12rpx;
	box-sizing: border-box;
	align-items: center;
	border-radius: 6rpx;
	display: inline-block;
	line-height: 1;
}
</style>
