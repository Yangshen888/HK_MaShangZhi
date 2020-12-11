<template>
	<view>
		<view class="u-demo-area" style="margin-left: 15px;">
			<u-toast ref="uToast"></u-toast>
			<u-search v-model="value" @search="search" @custom="custom" shape="round" :clearabled="true" :show-action="true" input-align="center" @clear="clear"></u-search>
		</view>
		<view class="wrap">
			<view><u-tabs-swiper ref="uTabs" :list="list" :current="current" @change="tabsChange" :is-scroll="false"></u-tabs-swiper></view>
			<swiper class="swiper-box" :current="swiperCurrent" @transition="transition" @animationfinish="animationfinish" autoHeight="auto">
				<swiper-item class="swiper-item">
					<scroll-view scroll-y style="width: 100%;height: 100%;" @scrolltolower="onreachBottomjobs">
						<view v-for="(item, index) in postjobs" style="margin: 20rpx;font-size: 30rpx;" @click="clickshow(item.schoolJourUuid)">
							<view>
								<view style="overflow: hidden;">
									<view style="float: left;margin-left: 3px;margin-top: 6px;" :style="style[index]">
										<u-tag text="标题" type="primary" size="mini" mode="dark" style="float: left;margin:0 5px 0 0;" />
										<view class="u-line-1" style="font-size: 16px;font-weight: 600;">{{ item.headline }}</view>
										<view style="font-size: 12px;margin: 5px 0 0 0;color: #909399;">{{ item.addtime }}</view>
										<view class="u-line-1" style="margin-top: 5px;margin-bottom: 10px;">{{ '内容：' }}{{ item.content }}</view>
									</view>
									<view style="padding: 5px;" v-show="showsrc1[index] == 1">
										<u-swiper
											:list="list1[index]"
											:circular="true"
											:autoplay="true"
											:height="160"
											style="width: 130px; float: right;margin-bottom: 10px;"
										></u-swiper>
									</view>
								</view>
							</view>
							<u-line color="rgb(144, 147, 153)" style="padding-top: 25px;" />
						</view>
					</scroll-view>
				</swiper-item>
				<swiper-item class="swiper-item">
					<scroll-view scroll-y style="height: 100%;width: 100%;" @scrolltolower="onreachBottomappeal">
						<view v-for="(item, index) in postjobsappeal" style="margin: 20rpx;font-size: 30rpx;">
							<view>
								<view style="overflow: hidden;">
									<view style="float: left;margin-left: 3px;margin-top: 6px;" :style="style[index]">
										<u-tag text="标题" type="primary" size="mini" mode="dark" style="float: left;margin:0 5px 0 0;" />
										<view class="u-line-2" style="font-size: 16px;font-weight: 600;height: 45px;">{{ item.headline }}</view>
										<view style="font-size: 12px;margin: 5px 0 0 0;color: #909399;margin-bottom: 10px;">{{ item.addTime }}</view>
									</view>
									<view style="padding: 5px;" v-show="showsrc2[index] == 1">
										<u-swiper
											:list="list2[index]"
											:circular="true"
											:autoplay="true"
											:height="160"
											style="width: 130px; float: right;"
										></u-swiper>
									</view>
								</view>
							</view>
							<u-line color="rgb(144, 147, 153)" style="padding-top: 25px;" />
						</view>
					</scroll-view>
				</swiper-item>
			</swiper>
		</view>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { getlist, getlifelist, SchoolJourGet } from '@/api/campusnews/news.js';
export default {
	data() {
		return {
			value: '',
			query: {
				totalCount: 0,
				pageSize: 5,
				currentPage: 1,
				kw: '',
				schoolUuid: '',
				sort: [
					{
						direct: 'DESC',
						field: 'ID'
					}
				]
			},
			list1: [],
			showsrc1: [],
			list2: [],
			showsrc2: [],
			style: [],
			url: http.baseUrl + 'UploadFiles/Picture/',
			list: [
				{
					name: '校园文化'
				},
				{
					name: '校园生活'
				}
			],
			postjobs: [],
			postjobsappeal: [],
			// 因为内部的滑动机制限制，请将tabs组件和swiper组件的current用不同变量赋值
			current: 0, // tabs组件的current值，表示当前活动的tab选项
			swiperCurrent: 0 // swiper组件的current值，表示当前那个swiper-item是活动的
		};
	},
	onLoad() {
		this.getList();
		this.getlifeList();
	},
	methods: {
		clickshow(e) {
			uni.navigateTo({
				url: '/pages/campusnews/shownew?guid=' + e
			});
		},
		//根据条件加载新闻数据
		getList() {
			this.query.schoolUuid = uni.getStorageSync('schoolguid');
			getlist(this.query).then(res => {
				this.postjobs = res.data.data;

				console.log(this.postjobs);
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
							this.showsrc1[k] = 1;
							this.style[k] = 'width: 166px';
						} else {
							imgs = [];
							this.showsrc1[k] = 0;
							this.style[k] = '';
						}
						this.list1[k] = imgs;
					}
				} else {
					this.list1 = [];
				}
			});
		},
		//加载全部的新闻数据
		getListcount() {
			getlist(this.query).then(res => {
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
							this.showsrc1[k] = 1;
						} else {
							imgs = [];
							this.showsrc1[k] = 0;
						}
						this.list1[k] = imgs;
					}
				} else {
					this.list1 = [];
				}
				this.postjobs = res.data.data;
			});
		},
		//加载全部的新闻数据
		getLifecount() {
			getlifelist(this.query).then(res => {
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
				this.postjobsappeal = res.data.data;
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
		clear() {},
		search(value) {
			// this.$refs.uToast.show({
			// 	title: '搜索内容为：' + value,
			// 	type: 'success'
			// });
			this.query.headline = value;
			this.getlifeList();
			this.getList();
		},
		custom(value) {
			//this.$u.toast('输入值为：' + value);
			this.query.headline = value;
			this.getlifeList();
			this.getList();
		},

		// tabs通知swiper切换
		tabsChange(index) {
			this.current = index;
			this.swiperCurrent = index;
		},
		// swiper-item左右移动，通知tabs的滑块跟随移动
		transition(e) {
			let dx = e.detail.dx;
			this.$refs.uTabs.setDx(dx);
		},
		// 由于swiper的内部机制问题，快速切换swiper不会触发dx的连续变化，需要在结束时重置状态
		// swiper滑动结束，分别设置tabs和swiper的状态
		animationfinish(e) {
			let current = e.detail.current;
			this.$refs.uTabs.setFinishCurrent(current);
			this.swiperCurrent = current;
			this.current = current;
		},
		// scroll-view到底部加载更多
		onreachBottomjobs() {
			if (this.query.currentPage >= parseInt(this.query.totalCount / this.query.pageSize)) {
				this.$u.toast('没有更多了');
			} else {
				this.query.currentPage = this.query.currentPage + 1;
				this.getLifecount();
			}
		},
		// scroll-view到底部加载更多
		onreachBottomappeal() {
			if (this.query.currentPage >= parseInt(this.query.totalCount / this.query.pageSize)) {
				this.$u.toast('没有更多了');
			} else {
				this.query.currentPage = this.query.currentPage + 1;
				this.getListcount();
			}
		}
	}
};
</script>

<style>
.wrap {
	display: flex;
	flex-direction: column;
	height: calc(100vh - var(--window-top));
	width: 100%;
}
.swiper-box {
	flex: 1;
}
.swiper-item {
	height: 100%;
}
</style>
