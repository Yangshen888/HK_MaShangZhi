<template>
	<view class="wrap">
		<view class="list-wrap">
		<view style="background-color: #FFFFFF;margin-left: -40rpx;margin-right: -40rpx;">
			<view style="width: 310rpx; color: #000000;margin-left: 30rpx;font-weight: 600;">
				<u-form ref="uForm">
					<u-form-item :label-position="'left'" prop="time">
						<u-input :border="border" type="select" :select-open="selectShow" v-model="timetitle" placeholder="请选择时间" @click="selectShow = true"></u-input>
					</u-form-item>
				</u-form>
			</view>
		</view>
		<u-select v-model="selectShow" mode="mutil-column" :default-value="datedefault" :list="list" @confirm="confirm"></u-select>
			<view v-for="(item, index) in prilist" style="margin-bottom: 20rpx;" :key="index" >
					<image :src="item.list[0].image" mode="widthFix" style="width: 100%;border-radius: 5px;" @click="clickpic(index)"></image>
				<view style="width: 100%;height: 80rpx;font-size: 32rpx;line-height: 80rpx;">
					<view style="float: left;font-weight: 600;">{{item.list[0].title}}</view>
					<view style="float: right;">赞</view>
					<!-- 点赞 -->
					<image src="../../static/xin1.png" mode="widthFix" v-if="thumbStatue[index]==0" style="width: 30rpx;float: right;margin-right: 10px;margin-top: 14px;" @click="thumbChange(index)"></image>
					<image src="../../static/xin2.png" mode="widthFix" v-if="thumbStatue[index]==1" style="width: 30rpx;float: right;margin-right: 10px;margin-top: 14px;"></image>
				</view>
			</view>
		</view>
		<button v-if="checkShow3()" style="position: fixed;bottom: 30px;right: 30px;border-radius: 50px;border: 1px solid #2979ff;" @click="geturl">
			<u-icon name="plus" color="#2979ff" size="38"></u-icon>
		</button>
	</view>
</template>

<script>
import { getTimeHorizon, getPictureList } from '@/api/diningRoom/liveShot.js';
import {
		getGivelike
	} from '@/api/cuisine/cuisine.js';
import http from '../../utils/http.js';
export default {
	data() {
		return {
			datedefault: [],
			selectShow: false,
			timetitle: '',
			timedata: [],
			suuid: '',
			prilist: [],
			datalist: [],
			list: [
				[],
				[
					{
						value: 'morn',
						label: '早餐'
					},
					{
						value: 'noon',
						label: '中餐'
					},
					{
						value: 'night',
						label: '晚餐'
					}
				]
			],
			thumbStatue:[],
		};
	},
	methods: {
		thumbChange(index){
			console.log('nsda')
			console.log(this.prilist)
			console.log(this.thumbStatue)
			this.thumbStatue[index]=1;
			this.prilist[index].list[0].likeNum++;
			getGivelike(this.prilist[index].list[0].cuisineUuid).then(res => {
				if (res.data.code == 200) {
					this.$u.toast(res.data.message);
				} else {
					this.$u.toast(res.data.message);
				}
			});
			console.log(this.thumbStatue)
		},
		confirm(e) {
			console.log(e);
			this.timetitle = '';
			this.timetitle = e[0].value + ' ' + e[1].label;
			console.log(this.timetitle);
			this.timedata = [];
			this.timedata.push(e[0].value);
			this.timedata.push(e[1].value);
			console.log(this.timedata);
			this.getPictList();
		},
		async getPictList() {
			await getPictureList({ date: this.timedata[0], type: this.timedata[1], uuid: this.suuid, change: 1 }).then(res => {
				// console.log(res);
				if (res.data.code == '200') {
					let ls = res.data.data.list;
					this.datalist = ls;
					// this.timedata[0]=res.data.data.date;
					// this.timedata[1]=res.data.data.type;
					let ent = this.list[1].find(x => x.value == res.data.data.type);
					this.timetitle = res.data.data.date + ' ' + ent.label;
					console.log(ls);
					console.log(21212);
					this.prilist = [];
					this.thumbStatue=[]
					for (let i = 0; i < ls.length; i++) {
						this.thumbStatue.push(0)
						let arr = [];
						ls[i].prctlist.forEach(x => arr.push({cuisineUuid:ls[i].cuisineUuid, image: http.baseUrl + 'UploadFiles/LiveShotPicture/' + x, title: ls[i].cuisineName, name: ls[i].liveShotUuid,likeNum:ls[i].likeNum }));
						//arr=ls[i].prctlist.map(x=>new {image:baseUrl+"UploadFiles/LiveShotPicture/"+x,title:ls.cuisineName,name:ls.liveShotUuid});
						this.prilist.push({ list: arr });
					}
				}
			});
		},
		clickpic(e) {
			let lsuuid = this.datalist[e].liveShotUuid;
			uni.navigateTo({
				url: '/pages/diningRoom/LiveShotShowInfo?lsuuid=' + lsuuid
			});
		},
		geturl() {
			uni.redirectTo({
				url: '/pages/diningRoom/LiveShotUpdate'
			});
		},
		checkShow3() {
			return this.$store.state.isUploadP && this.$store.state.schoolGuid != '' && this.$store.state.schoolGuid == uni.getStorageSync('schoolguid');
		}
	},
	async onLoad() {
		console.log(http.baseUrl);
		let that = this;
		await getTimeHorizon().then(res => {
			that.datedefault = [res.data.index, 0];
			let ls = res.data.timelist;
			that.timetitle = ls[res.data.index] + ' ' + that.list[1][0].label;
			that.timedata.push(ls[res.data.index]);
			that.timedata.push(that.list[1][0].value);
			for (let i = 0; i < ls.length; i++) {
				that.list[0].push({ value: ls[i], label: ls[i] });
			}
		});
		this.suuid = uni.getStorageSync('schoolguid');
		console.log(that.timedata);
		await getPictureList({ date: that.timedata[0], type: that.timedata[1], uuid: this.suuid, change: 0 }).then(res => {
			if (res.data.code == '200') {
				let ls = res.data.data.list;
				console.log(ls);
				// that.timedata[0]=res.data.data.date;
				// that.timedata[1]=res.data.data.type;
				let ent = that.list[1].find(x => x.value == res.data.data.type);
				that.timetitle = res.data.data.date + ' ' + ent.label;
				that.datalist = ls;
				that.prilist = [];
				
				for (let i = 0; i < ls.length; i++) {
					this.thumbStatue.push(0)
					let arr = [];
					ls[i].prctlist.forEach(x => arr.push({cuisineUuid: ls[i].cuisineUuid,  image: http.baseUrl + 'UploadFiles/LiveShotPicture/' + x, title: ls[i].cuisineName, name: ls[i].liveShotUuid,likeNum:ls[i].likeNum }));
					//arr=ls[i].prctlist.map(x=>new {image:baseUrl+"UploadFiles/LiveShotPicture/"+x,title:ls.cuisineName,name:ls.liveShotUuid});
					let arr1 = [];
					arr1.push(arr[0]);
					that.prilist.push({ list: arr1 });
				}
			}
			console.log(that.prilist);
		});
	}
};
</script>

<style>
page {
	background-color: rgb(240, 242, 244);
}
.wrap {
	padding: 0 40rpx;
}
</style>
