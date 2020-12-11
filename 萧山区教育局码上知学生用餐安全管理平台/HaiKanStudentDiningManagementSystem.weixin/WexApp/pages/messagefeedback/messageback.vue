<template>
	<view class="wrap">
		<view v-if='messageback.length==0'>
			<view style="text-align: center;margin-top: 200rpx;">
				<image src="https://msz-b.jiulong.yoruan.com/img/images/nothing@3x.png" style="width: 320rpx;height: 320rpx;"></image>
				<view style="font-size: 32rpx;font-weight: 600;letter-spacing: 2rpx;">{{'暂无数据'}}</view>
			</view>
		</view>
		<!-- <scroll-view scroll-y style="width: 100%;height: 100%;" @scrolltolower="onreachBottom" > -->
		<view v-else v-for="(item, index) in messageback" style="margin: 20rpx;font-size: 30rpx;box-shadow: 0px 0px 5px 0px #55aaff;padding: 15px;">
			<view >
				<view>
					<view class="content">
						{{"留言内容："}}{{item.content}}
					</view>
					<view  class="content">
						{{"内容类型："}}{{item.type}}
					</view>
					
					<view  class="content">
						{{"留言时间："}}{{rendeerDdate(item.messageDate)}}
					</view>
					<view  class="content">
						{{"回复内容："}}{{renderMessage(item.response)}}
					</view>
					<view>
						{{"回复时间："}}{{rendeerDdate(item.responseDate)}}
					</view>
				</view>
			</view>
					
		</view>
		<!-- </scroll-view> -->
	</view>
</template>

<script>
	import {getMessagebacklist} from '../../api/messagefeedback/messageboard.js'
	export default{
		data(){
			return{
				query: {
					totalCount: 0,
					pageSize: 20,
					currentPage: 1,
					kw: "",
					schoolUuid: "",
					people:"",
					sort: [{
						direct: "DESC",
						field: "ID"
					}]
				},
				messageback:{
					
				}
			}
		},
		methods:{
			rendeerDdate(date)
			{
				return this.utiils.Time(date);
			},
			renderMessage(message)
			{
				return this.utiils.isNull(message);
			},
			dogetMessagebacklist(){
				this.query.schoolUuid=uni.getStorageSync('schoolguid');
				this.query.people =this.$store.state.userId;
				getMessagebacklist(this.query).then(res=>{
					if(res.data.code==200)
					{
						this.messageback = res.data.data;
					}
				})
			},
			dogetMessagebacklistconcat(){
				getMessagebacklist(this.query).then(res=>{
					if(res.data.code==200)
					{
						this.messageback = this.messageback.concat(res.data.data);
					}
				})
			},
		},
		mounted() {
			this.query.people=this.$store.state.userId;
			this.dogetMessagebacklist();
		},
		//下拉刷新
		onPullDownRefresh() {
					this.query.currentPage=1;
					this.dogetMessagebacklist();
				},
		onReachBottom(){//与methods 同级
		     if (this.query.currentPage >= parseInt(this.query.totalCount / this.query.pageSize)) {
		     	this.$u.toast('没有更多了');
		     } else {
		     	this.query.currentPage = this.query.currentPage + 1;
		     	this.dogetMessagebacklistconcat();
		     }
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
	.content{
		padding-bottom: 3px;
	}
</style>
