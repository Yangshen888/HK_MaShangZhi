<template>
	<view style="background-color: rgba(249, 250, 250, 1);">
		<view class="u-demo-area">
			<u-toast ref="uToast"></u-toast>
		</view>
		<view style="position: relative; width: 100%;padding:40rpx 16rpx; font-size: 32rpx; background-color: rgba(255, 255, 255, 1);overflow: hidden;line-height: 32rpx;border-bottom: 1rpx solid rgba(234, 234, 234, 1);">
			<view style="position: absolute;left: 50%;transform: translateX(-50%);">
				<!-- 切换上一周按钮 -->
				<u-icon @click="toLast" name="arrow-left" style="display: inline;" :color="currentNum==0?'rgba(165, 165, 165, 1)':''"></u-icon>
				<view style="font-weight: 700;display: inline;margin: 0 20rpx;">{{ showMenu1 }}</view>
				<!-- 切换下一周按钮 -->
				<u-icon @click="toNext" name="arrow-right" :color="currentNum==2?'rgba(165, 165, 165, 1)':''"></u-icon>
			</view>
			<!-- <image src="https://msz-b.jiulong.yoruan.com/img/image/jx15.png" style="width: 2px; height: 32rpx; float: left; margin: 0 10px 0 10px;"></image> -->
			<!-- <view style="text-decoration: underline; color: #0097FF; float: left;" @click="showDateList = true">{{ showDate1 }}</view> -->
			<u-select v-model="showDateList" mode="single-column" :list="dateList" @confirm="dateConfirm"></u-select>
			<view @click="openDropdown" class="sxBtn" :style="sxBtnStyle">
				<view style="float: left;">筛选</view>
				<u-icon v-if="dateDropdownStatus == 0" name="arrow-down-fill" size="20" top="-2rpx"></u-icon>
				<u-icon v-else name="arrow-up-fill" size="20" top="-2rpx"></u-icon>
			</view>
		</view>

		<view class="u-demo-area" v-if="lists">
			<view class="dateSelector">
				<u-dropdown ref="dateDropdown" @close="dropdownClosed" height="0">
					<view class="slot-content" style="background-color: #fff; padding:30rpx;">
						<!-- 选择日期 -->
						<view style="text-align: left; font-size: 16px; font-weight: 700; line-height: 26px; height: 26px;">日期</view>
						<view style="display: flex; justify-content: space-between;">
							<view class="dateTags" @click="chooseDate(1)" :style="dateTagStyle == 1 ? active : ''">周一</view>
							<view class="dateTags" @click="chooseDate(2)" :style="dateTagStyle == 2 ? active : ''">周二</view>
							<view class="dateTags" @click="chooseDate(3)" :style="dateTagStyle == 3 ? active : ''">周三</view>
							<view class="dateTags" @click="chooseDate(4)" :style="dateTagStyle == 4 ? active : ''">周四</view>
							<view class="dateTags" @click="chooseDate(5)" :style="dateTagStyle == 5 ? active : ''">周五</view>
						</view>
						<!-- 选择餐次 -->
						<view style="text-align: left; font-size: 16px; font-weight: 700; line-height: 26px; height: 26px;">餐次</view>
						<view style="display: flex; justify-content: space-between; width: 57%; line-height: 50px;">
							<view class="dateTags" @click="chooseMile(1)" :style="mileTagStyle == 1 ? active : ''">早餐</view>
							<view class="dateTags" @click="chooseMile(2)" :style="mileTagStyle == 2 ? active : ''">中餐</view>
							<view class="dateTags" @click="chooseMile(3)" :style="mileTagStyle == 3 ? active : ''">晚餐</view>
						</view>
					</view>
					<!-- 按钮部分 -->
					<view style="width: 100%;height: 50px;text-align: center;line-height: 50px;overflow: hidden;background-color: #fff;">
						<view @click="reset" style="width: 50%;float: left;background-color: rgba(238, 238, 238, 1);">重置</view>
						<view @click="shaxuan" style="width: 50%;float: right;background-color: rgba(0, 151, 255, 1);color: #FFFFFF;">确定</view>
					</view>
				</u-dropdown>
			</view>
			<view class="menuBody" style="padding: 30rpx 20px 0 20px; ">
				<view v-for="(item, index) in getmenuList" :key="index" style="margin-top: -30rpx;">
					
					<view style="font-size: 32rpx; margin: 30rpx 0; float: left;">{{ item[0].date }}</view>
					<view class="menuCard" v-for="(items, indexs) in item" :key="indexs" style=" background-color: rgba(255, 255, 255, 1);">
					<!-- 卡片头部 -->
					<view class="header" style="overflow: hidden; position: relative;">
						<view class="point"></view>
						<view style="font-size: 32rpx; font-weight: 700; margin-left: 30rpx; float: left;">{{ items.cuisineType }}</view>

						<view @click="openMenuInfo(items.cuisineUuid, thumbUpColor[index][indexs])" style="width: 140rpx; line-height:40rpx;float:right; position: relative; color: rgba(0, 151, 255, 1);">
							<u-icon name="arrow-right" size="24" style="float: right;position: absolute; top: 50%; right: 0; transform: translateY(-50%); "></u-icon>
							<view style="float: left;letter-spacing: 0;font-size: 28rpx;" >查看详情</view>
						</view>
					</view>
					<!-- 菜品详情 -->
					<view class="body" style="margin-top: 30rpx; width: 100%; overflow: hidden;">
						<!-- 右侧菜品图 -->
						<view style="width: 180rpx; height: 180rpx; text-align: center; float: left;">
							<!-- <image src="https://msz-b.jiulong.yoruan.com/img/images/glfa@2x.png" style="width: 91px; height: 91px;"></image> -->
							<image :src="img[index][indexs]" style="max-width: 100%; max-height: 100%;border: 1rpx solid rgba(233, 233, 233, 1);border-radius: 5px;"></image>
						</view>
						<!-- 左侧介绍 -->
						<view style="float: right; width: 66%;padding-right: 8rpx;">
							<view style="width: 100%; height: 60rpx;font-size: 32rpx;">
								<view class="title u-line-2" style="float: left;font-weight: 600;">{{ items.cuisineName }}</view>
								<view class="price" style="float: right; color: #FD5D37;line-height: 40rpx;" v-if="isCuiPrices">
									<view style="float: right;font-weight: 600;font-size: 36rpx;">{{ items.price }}</view>
									<view style="font-size: 24rpx;float: right;line-height: 48rpx;">￥</view>
								</view>
							</view>
							<view style="text-align: left; float: left; line-height: 24px; color: #A5A5A5;">主料：{{ items.ingredient }}</view>
							<view style="text-align: left; clear: both; line-height: 24px; color: #A5A5A5; word-break: break-all;">配料：{{ items.burdening }}</view>
						</view>
					</view>
					<!-- 底部点赞 -->
					<view style="width: 100%; float: left; line-height: 40rpx;position: relative;">
						<view style="float: right; font-size: 16px;margin-left: 10rpx;" :style="numColor[index][indexs]!='color:#0097FF'?'color:rgba(165, 165, 165, 1)':'color:#0097FF'">{{ items.likeNum }}</view>
						<!-- <u-icon name="thumb-up-fill" size="36" top="1px" style="float: right;margin-right: 5px;" :color="thumbUpColor[index]" @click="thumbUp(index)"></u-icon> -->
						<u-icon
							v-if="thumbUpColor[index][indexs] != 1"
							name="thumb-up"
							size="36"
							style="float: right;"
							color="rgba(165, 165, 165, 1)"
							@click="thumbUp(index,indexs)"
						></u-icon>
						<u-icon
							v-if="thumbUpColor[index][indexs] == 1"
							name="thumb-up-fill"
							size="36"
							style="float: right;"
							color="#0097FF"
							@click="thumbUp(index,indexs)"
						></u-icon>
					</view>
					</view>
				</view>
			</view>
		</view>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { weektime, getMenuDateList } from '@/api/weekmenu/menu.js';
import { getGivelike } from '@/api/cuisine/cuisine.js';
export default {
	data() {
		return {
			tdate: '',
			ndate: '',
			sdate: '',
			ttime: '',
			ntime: '',
			stime: '',
			lists: false,
			getmenuList: [],
			img: [],
			url: http.baseUrl + 'UploadFiles/RegistPicture/',
			isLike: [],

			dateDefault: [1],
				dateList: [],
				
			// 日期选择器
			showDateList: false,
			showDate1: '',
			showMenu1: '',
			// 日期下拉选择框状态 0-关闭  1-打开
			dateDropdownStatus: 0,
			// 点赞图标
			thumbUpColor: [],
			numColor:[],
			// 设置日期标签默认值
			dateTagStyle: 0,
			// 设置点击样式
			active: 'background-color: #0097FF;color: #fff;',
			// 设置餐次标签默认值
			mileTagStyle: 0,
			//
			dateChoosed: '',
			dateChoosed2: '',
			dateAry: ['周一', '周二', '周三', '周四', '周五'],
			mileChoosed: '',
			mileChoosed2: '',
			mileAry: ['早餐', '午餐', '晚餐'],
			// 筛选按钮样式
			sxBtnStyle: 'color:#333',
			isCuiPrices:true,
				// 默认周次
				currentNum: 1,
			
			datemenu:[]
		};
	},
	computed: {},
	onLoad() {},
	created() {
		if(!uni.getStorageSync('isCuiPrices')){
			this.isCuiPrices=false;
		}
		this.doweektime();
	},
	methods: {
			// 点击切换上一周
			toLast(){
				if(this.currentNum != 0){
					this.currentNum--;
					this.updateWeek()
					this.showDate1=this.dateList[this.currentNum];
					this.dogetMenuDateList();
				}
			},
			// 点击切换下一周
			toNext(){
				if(this.currentNum != 2){
					this.currentNum++;
					this.updateWeek()
					this.showDate1=this.dateList[this.currentNum];
					this.dogetMenuDateList();
				}
			},
			// 显示当前周次
			updateWeek() {
				if (this.currentNum == 0) {
					this.showMenu1 = '上周菜单';
				}else if(this.currentNum == 1){
					this.showMenu1 = '本周菜单';
				}else{
					this.showMenu1 = '下周菜单';
				}
			},
		dropdownClosed() {
			this.dateDropdownStatus = 0;
		},
		// 选择餐次
		chooseMile(index) {
			// this.mileChoosed=this.mileAry[index-1];
			if (this.mileTagStyle == index) {
				this.mileTagStyle = 0;
				this.mileChoosed2 = '';
			} else {
				this.mileTagStyle = index;
				this.mileChoosed2 = this.mileAry[index - 1];
			}
		},
		// 选择星期几
		chooseDate(index) {
			// this.dateChoosed=this.dateAry[index-1];
			if (this.dateTagStyle == index) {
				this.dateTagStyle = 0;
				this.dateChoosed2 = '';
			} else {
				this.dateTagStyle = index;
				this.dateChoosed2 = this.dateAry[index - 1];
			}
		},
		// 点击查看详情 跳转到menuInfo页面
		openMenuInfo(e, c) {
			uni.navigateTo({
				url: '/pages/weekmenu/menuInfo?guid=' + e + '&color=' + c
			});
		},
		// 点赞图标变化
		thumbUp(index,indexs) {
			if (this.thumbUpColor[index][indexs] != 1) {
				this.getmenuList[index][indexs].likeNum++;
				getGivelike(this.getmenuList[index][indexs].cuisineUuid).then(res => {
					if (res.data.code == 200) {
						this.$u.toast(res.data.message);
					} else {
						this.$u.toast(res.data.message);
					}
				});
				this.thumbUpColor[index][indexs] = 1;
				this.numColor[index][indexs]='color:#0097FF';
			}
		},
		// 打开下拉选择框
		openDropdown() {
			this.$refs.dateDropdown.open();
			this.dateDropdownStatus = 1;
			this.sxBtnStyle = 'color:#0097FF';
		},
		// 关闭下拉选择框
		closeDropdown() {
			this.$refs.dateDropdown.close();
			this.dateDropdownStatus = 0;
			this.sxBtnStyle = 'color:#333';
		},
		doweektime() {
			let guid = uni.getStorageSync('schoolguid');
			weektime(guid).then(res => {
				this.stime = res.data.data.sdate;
				this.ttime = res.data.data.tdate;
				this.ntime = res.data.data.ndate;
				this.showDate1 = this.ttime;
				this.showMenu1 = '本周菜单';
				this.dateList[0] = this.stime;
				this.dateList[1] = this.ttime;
				this.dateList[2] = this.ntime;


				this.lists = true;
				this.dogetMenuDateList();
			});
		},
		shaxuan() {
			this.dateChoosed = this.dateChoosed2;
			this.mileChoosed = this.mileChoosed2;
			this.$refs.dateDropdown.close();
			this.dateDropdownStatus = 0;
			this.sxBtnStyle = 'color:#333';
			this.dogetMenuDateList();
		},
			// 点击重置
			reset() {
				this.dateChoosed = '';
				this.mileChoosed = '';
				this.$refs.dateDropdown.close();
				this.dateDropdownStatus = 0;
				this.sxBtnStyle = 'color:#333';
				this.dogetMenuDateList();
			},
			// 更新菜品查询列表
		dogetMenuDateList() {
			let data = {
				dateChoosed: this.dateChoosed,
				mileChoosed: this.mileChoosed,
				date: this.showDate1,
				schoolUuid: uni.getStorageSync('schoolguid')
			};
			console.log(data)
			getMenuDateList(data).then(res => {
				console.log(1888);
				console.log(res.data.data);
				this.getmenuList = [];
				this.img=[];
				if (res.data.code == 200) {
					if (res.data.data.length > 0) {
						for (let k = 0; k < res.data.data.length; k++) {
							if(res.data.data[k].length>0){
								let img=[];
								let thumbUpColor=[];
								let numColor=[];
								for(let i = 0; i < res.data.data[k].length; i++){
									if (res.data.data[k][i].accessory != '' && res.data.data[k][i].accessory != null) {
										 img.push((this.url + res.data.data[k][i].accessory.split(',')[0]).toString());
									}
									numColor.push('color:rgba(165, 165, 165, 1)');
									thumbUpColor.push(0);
								}
								this.numColor.push(numColor);
								this.img.push(img);
								this.thumbUpColor.push(thumbUpColor);
								this.getmenuList.push(res.data.data[k]);
							}
						}
					} else {
						this.$u.toast(res.data.message);
					}
					console.log(1777);
					console.log(this.img)
					console.log(this.getmenuList)
				} else {
					this.$u.toast('暂无菜品');
				}
			});
		},
		dateConfirm(index) {
			this.showDate1 = index[0].label;
			if (this.showDate1 == this.stime) {
				this.showMenu1 = '上周菜单';
			}
			if (this.showDate1 == this.ttime) {
				this.showMenu1 = '本周菜单';
			}
			if (this.showDate1 == this.ntime) {
				this.showMenu1 = '下周菜单';
			}
			this.dogetMenuDateList();
		}
	},
	mounted() {}
};
</script>

<style>
/* #ifndef H5 */
page {
		height: 100%;
		background-color: (249, 250, 250, 1);
	}
/* #endif */
</style>

<style lang="scss" scoped>
.sxBtn {
		float: right;
		margin-right: 15px;
		color: #0097FF;
	}

	.sxBtn u-icon {
		margin-left: 10rpx;
	}

	.dateTags {
		width: 96rpx;
		height: 52rpx;
		border: 1rpx solid #0097FF;
		border-radius: 10rpx;
		line-height: 48rpx;
		margin: 20rpx 0;
	}

	.active {
		background-color: #0097FF;
		color: #fff;
	}

	.u-cell-icon {
		width: 44rpx;
		height: 44rpx;
		margin-right: 8rpx;
	}

	.menuBody {
		.menuCard {
			border-radius: 5px;
			box-shadow: 0px 0px 2px rgba(0, 0, 0, 0.2);
			width: 100%;
			margin-bottom: 30rpx;
			overflow: hidden;
			padding: 10px;

			.point {
				position: absolute;
				top: 50%;
				left: 0;
				transform: translateY(-50%);
				width: 5px;
				height: 5px;
				background-color: #0097FF;
				border-radius: 50%;
			}
		}
	}
</style>
