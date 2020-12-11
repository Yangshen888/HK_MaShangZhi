<template>
	<view>
		<view class="wrap">
			<view><u-tabs-swiper ref="uTabs" :list="list" :current="current" @change="tabsChange" :is-scroll="false"></u-tabs-swiper></view>
			<swiper class="swiper-box" :current="swiperCurrent" @transition="transition" @animationfinish="animationfinish" autoHeight="auto">
				<swiper-item class="swiper-item">
					<scroll-view scroll-y style="width: 100%;height: 100%;" @scrolltolower="onreachBottomjobs">
						<view v-if="postjobs.length == 0">
							<view style="text-align: center;margin-top: 50%;">{{ '暂无数据' }}</view>
						</view>
						<view v-else v-for="(item, index) in postjobs" style="margin: 20rpx;font-size: 30rpx;box-shadow: 0px 0px 5px 0px #55aaff;padding: 15px;">
							<view>
								<view>
									<view class="content">{{ '用人单位：' }}{{ item.unit }}</view>
									<view class="content">{{ '岗位名称：' }}{{ item.unitName }}</view>
									<view class="content">{{ '工作要求：' }}{{ item.require }}</view>
									<view class="content">{{ '发布日期：' }}{{ rendeerDdate(item.addTime) }}</view>
									<view class="content">{{ '招收人数：' }}{{ item.passNum }}{{ '/' }}{{ item.num }}</view>
									<view style="margin-top: 5px;">
										<u-button shape="circle" type="primary" :plain="true" @click="appeal(item.postUuid)">申请</u-button></view>
								</view>
							</view>
						</view>
					</scroll-view>
				</swiper-item>
				<swiper-item class="swiper-item">
					<scroll-view scroll-y style="height: 100%;width: 100%;" @scrolltolower="onreachBottomappeal">
						<view v-if="postjobsappeal.length == 0">
							<view style="text-align: center;margin-top: 50%;">{{ '暂无数据' }}</view>
						</view>
						<view v-else v-for="(item, index) in postjobsappeal" style="margin: 20rpx;font-size: 30rpx;box-shadow:0px 0px 5px 0px #55aaff;padding: 15px;">
							<view>
								<view class="content">{{ '用人单位：' }}{{ item.unit }}</view>
								<view class="content">{{ '岗位名称：' }}{{ item.unitName }}</view>
								<view class="content">{{ '学生姓名：' }}{{ item.stuName }}</view>
								<view class="content">{{ '申请时间：' }}{{ rendeerDdate(item.addTime) }}</view>
								<view>{{ '审核状态：' }}{{ redendtate(item.state) }}</view>
							</view>
						</view>
					</scroll-view>
				</swiper-item>
			</swiper>
		</view>

	</view>
</template>

<script>
	import {
		getPostjobsList,
		getPostjobsAppealList
	} from '@/api/workstudy/postjobs.js';
	export default {
		data() {
			return {
				query: {
					totalCount: 0,
					pageSize: 5,
					currentPage: 1,
					kw: "",
					schoolUuid: "",
					appealPeople:"",
					sort: [{
						direct: "DESC",
						field: "ID"
					}]
				},
				queryappeal: {
					totalCount: 0,
					pageSize: 5,
					currentPage: 1,
					kw: "",
					appealPeople: "",
					sort: [{
						direct: "DESC",
						field: "ID"
					}]
				},
				list: [{
					name: '岗位列表'
				}, {
					name: '申请反馈'
				}],
				postjobs: [],
				postjobsappeal: [],
				// 因为内部的滑动机制限制，请将tabs组件和swiper组件的current用不同变量赋值
				current: 0, // tabs组件的current值，表示当前活动的tab选项
				swiperCurrent: 0, // swiper组件的current值，表示当前那个swiper-item是活动的
			};
		},
		methods: {
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
			redendtate(state) {
				let statetext = "未知";
				switch (state) {
					case 0:
						statetext = "待审核";
						break;
					case 1:
						statetext = "通过";
						break;
					case 2:
						statetext = "未通过";
						break;
				}
				return statetext;
			},
			// scroll-view到底部加载更多
			onreachBottomjobs() {
				if (this.query.currentPage >= parseInt(this.query.totalCount / this.query.pageSize)) {
					this.$u.toast('没有更多了');
				} else {
					this.query.currentPage = this.query.currentPage + 1;
					this.dogetPostjobsListConcat();
				}
			},
			// scroll-view到底部加载更多
			onreachBottomappeal() {
				if (this.queryappeal.currentPage >= parseInt(this.queryappeal.totalCount / this.queryappeal.pageSize)) {
					this.$u.toast('没有更多了');
				} else {
					this.queryappeal.currentPage = this.queryappeal.currentPage + 1;
					this.dogetPostjobsAppealListConcat();
				}
			},
			dogetPostjobsList() {
				let uuid = uni.getStorageSync('schoolguid');
				console.log(uuid)
				this.query.schoolUuid = uuid;
				getPostjobsList(this.query).then(res => {
					if (res.data.code == 200) {
						this.postjobs = res.data.data;
						this.query.totalCount = res.data.totalCount;
					}
				})
			},
			dogetPostjobsListConcat() {
				getPostjobsList(this.query).then(res => {
					if (res.data.code == 200) {
						this.postjobs = this.postjobs.concat(res.data.data);
						this.query.totalCount = res.data.totalCount;
					}
				})
			},
			dogetPostjobsAppealList() {
				getPostjobsAppealList(this.queryappeal).then(res => {
					if (res.data.code == 200) {
						this.postjobsappeal = res.data.data;
						this.queryappeal.totalCount = res.data.totalCount;
					}
				})
			},
			dogetPostjobsAppealListConcat() {
				getPostjobsAppealList(this.queryappeal).then(res => {
					if (res.data.code == 200) {
						this.postjobsappeal = this.postjobsappeal.concat(res.data.data);
						this.queryappeal.totalCount = res.data.totalCount;
					}
				})
			},
			appeal(guid) {
				uni.navigateTo({
					url: "./postjobsappeal?guid=" + guid
				});
			},
			rendeerDdate(date) {
				return this.utiils.Time(date);
			}
		},
		onLoad() {
			this.query.appealPeople = this.$store.state.userId;
			this.queryappeal.appealPeople = this.$store.state.userId;
			//console.log(uni.getStorageSync('schoolguid'))
			this.dogetPostjobsList();
			this.dogetPostjobsAppealList();
		}

	}
</script>

<style>
	.wrap {
		display: flex;
		flex-direction: column;
		height: calc(100vh - var(--window-top));
		width: 100%;
		word-break:break-all;
	}

	.swiper-box {
		flex: 1;
	}

.swiper-item {
	height: 100%;
}
.content {
	padding-bottom: 3px;
}
</style>
