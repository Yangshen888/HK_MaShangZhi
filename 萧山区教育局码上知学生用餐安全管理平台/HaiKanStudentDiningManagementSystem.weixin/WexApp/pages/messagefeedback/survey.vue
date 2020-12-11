<template>
	<view style="padding-left: 30rpx;">
		<view v-if='survey.length==0'>
			<view style="text-align: center;margin-top: 50%;">{{'暂无数据'}}</view>
		</view>
		<view v-else v-for="(item, index) in survey" @click="answer(item.surveyUuid)" style="font-size: 30rpx;padding: 15px 15px 15px 0;margin-bottom: 15px;border-bottom: 1rpx solid rgba(234, 234, 234, 1);">
			<view style="overflow: hidden;">
				<view class="u-line-2" style="font-size: 30rpx;font-weight: 600;">
					<view>{{ item.headline }}</view>
					<view style="color: rgba(165, 165, 165, 1);font-weight: normal;">（{{item.type}})</view>
				</view>
				<view style="margin: 10px 0;overflow: hidden;">
					<button class="wjBtn">填写</button>
					<!-- <button class="wjBtn" @click="answer(item.surveyUuid)">已完成</button> -->
				</view> 
			</view>
					
		</view>
		<!-- </scroll-view> -->
	</view>
</template>

<script>
	import {getSurveylist} from '../../api/messagefeedback/survey.js'
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
				survey:{
					
				}
			}
		},
		methods:{
			dogetSurveylist(){
				this.query.schoolUuid=uni.getStorageSync('schoolguid');
				getSurveylist(this.query).then(res=>{
			console.log(res);
					if(res.data.code==200)
					{
						this.survey = res.data.data;
					}
				})
			},
			dogetSurveylistConcat(){
				getSurveylist(this.query).then(res=>{
			console.log(res);
					if(res.data.code==200)
					{
						this.survey = this.survey.concat(res.data.data);
					}
				})
			},
			answer(guid)
			{
				uni.navigateTo({
					url: "./surveyanswer?guid=" + guid
				});
			},
		},
		mounted() {
			this.query.people=this.$store.state.userId;
			this.dogetSurveylist();
		},
		//下拉刷新
		onPullDownRefresh() {
					this.query.currentPage=1;
					this.dogetSurveylist();
				},
		onReachBottom(){//与methods 同级
		     if (this.query.currentPage >= parseInt(this.query.totalCount / this.query.pageSize)) {
		     	this.$u.toast('没有更多了');
		     } else {
		     	this.query.currentPage = this.query.currentPage + 1;
		     	this.dogetSurveylistConcat();
		     }
		}
	}
</script>

<style>
	.wjBtn{
		float: right;
		width: 128rpx;
		height: 48rpx;
		line-height: 48rpx;
		color: rgba(0, 151, 255, 1);
		font-size: 24rpx;
		border: 1rpx solid rgba(0, 151, 255, 1);
		background-color: rgba(0, 151, 255, 0.09);
	}
</style>
