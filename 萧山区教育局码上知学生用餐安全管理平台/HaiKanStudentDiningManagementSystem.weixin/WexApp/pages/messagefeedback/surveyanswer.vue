<template>
	<view style="padding: 0 40rpx;">
		<view v-for="(item,index) in surveydata.surveyQuestionDetail">
			<view class="title" style="font-weight: 30rpx;font-weight: 600;margin: 30rpx 0;">
				{{index+1}}{{'、'}}{{item.questionTitle}}
			</view>
			<view>
				<!-- 单选题-->
				<view v-if="item.questionType==0&&item.isMuti==0">
					<u-radio-group v-model="messagelist[index].content" @change="radioGroupChange($event,index)" wrap=true>
						<u-radio @change="radioChange($event,index)" v-for="(item1, index1) in item.surveyQuestionItemDetail" :key="index1"
						 :name="item1.optionts" :disabled='false' :shape="'circle'">
							{{item1.optionts}}{{'. '}}{{item1.questionStr}}
						</u-radio>
					</u-radio-group>
				</view>
				<!-- 多选题-->
				<view v-if="item.questionType==0&&item.isMuti==1">
					<u-checkbox-group @change="checkboxGroupChange($event,index)" wrap=true>
						<u-checkbox @change="checkboxChange" v-model="item2.checkbox" v-for="(item2, index2) in item.surveyQuestionItemDetail"
						 :key="index2" :name="item2.optionts" :shape="'circle'" :disabled='item2.disabled'>{{item2.optionts}}{{'. '}}{{item2.questionStr}}</u-checkbox>
					</u-checkbox-group>
				</view>
				<!-- 主观题-->
				<view v-else-if="item.questionType==1">
					<u-input v-model="messagelist[index].content" :type="type" :border="border" />
				</view>
			</view>
		</view>
		<button class="lyBtn" @click="submitanswer">提交</button>
	</view>
</template>

<script>
	import {
		getSurveydetail,
		createSurveyAnswer
	} from '../../api/messagefeedback/survey.js'
	export default {
		data() {
			return {
				surveydata: {},
				messagelist: [],
				answermessage:'',
				form: {
					surveyUuid: '',
					answerStr:'',
					addPeople:'',
					schoolUuid:''
				},
				value: 'orange',
				head: false,
				isgetdata: false
			}
		},
		methods: {
			// 选中某个单选框时，由radio时触发
			radioChange(e, index) {
				console.log(index);
				console.log(e);
				console.log(this.messagelist[index])
				this.messagelist[index].content = e;
			},
			// 选中任一radio时，由radio-group触发
			radioGroupChange(e, index) {
				// console.log(index);
				// console.log(e);
			},


			// 选中某个复选框时，由checkbox时触发
			checkboxChange(e) {},
			// 选中任一checkbox时，由checkbox-group触发
			checkboxGroupChange(e, index) {
				console.log(this.surveydata.surveyQuestionDetail[index].surveyQuestionItemDetail)
				console.log(index);
				console.log(e);
				this.messagelist[index].content = e;
				console.log(111);
				console.log(this.messagelist[index].content);
			},
			surveydetail(guid) {
				this.messagelist = [];
				getSurveydetail({
					guid: guid
				}).then(res => {
					if (res.data.code == 200) {
						this.surveydata = res.data.data;
						for (let i = 0; i < this.surveydata.surveyQuestionDetail.length; i++) {
							if (this.surveydata.surveyQuestionDetail[i].questionType == 0 && this.surveydata.surveyQuestionDetail[i].isMuti ==
								1) {
								this.messagelist.push({
									content: []
								});
							} else {
								this.messagelist.push({
									content: ''
								});
							}
						}
						this.isgetdata = true;
					} else {
						this.$u.toast(res.data.message);
					}

				})
			},
			submitanswer() {
				this.answermessage='';
				for(let i=0;i<this.messagelist.length;i++)
				{
					let everypoint ='';
					if(this.surveydata.surveyQuestionDetail[i].questionType == 0 && this.surveydata.surveyQuestionDetail[i].isMuti ==1)
					{
						everypoint=this.messagelist[i].content.join(',');
					}
					else
					{
						everypoint=this.messagelist[i].content
					}
					
					if(everypoint=='')
					{
						this.$u.toast('请全部填写');
						return;
					}
					if(i==0)
					{
						this.answermessage=everypoint;
					}
					else
					{
						this.answermessage=this.answermessage+"||"+everypoint;
					}
					
				}
				console.log(this.messagelist);
				console.log(this.answermessage);
				this.docreateSurveyAnswer();
			},
			docreateSurveyAnswer()
			{
				this.form.addPeople=this.$store.state.userId;
				this.form.answerStr=this.answermessage;
				createSurveyAnswer(this.form).then(res=>{
					if(res.data.code==200)
					{
						this.$u.toast(res.data.message);
						uni.redirectTo({
							url:'./index'
						})
					}
					else
					{
						this.$u.toast(res.data.message);
					}
				})
				
			}
		},
		mounted() {},
		onLoad: function(options) {
			this.form.addPeople=this.$store.state.userId;
			if (!this.utiils.isEmpty(options)) {
				if (!this.utiils.isEmpty(options.guid)) {
					this.form.surveyUuid = options.guid;
					this.surveydetail(options.guid);
				}
			}
		}
	}
</script>

<style>
/* #ifndef H5 */
page {
	height: 100%;
	background-color: #f2f2f2;
}
.wrap {
	padding: 30rpx;
	word-break: break-all;
}
/* #endif */
</style>

<style lang="scss" scoped>
.u-card-wrap {
	background-color: $u-bg-color;
	padding: 1px;
}
.u-body-item {
	font-size: 32rpx;
	color: #333;
	padding: 20rpx 10rpx;
	margin-top: -45px;
}
.lyBtn{
		position: fixed;
		bottom: 80rpx;
		left: 50%;
		transform: translateX(-50%);
		width: 600rpx;
		height: 80rpx;
		line-height: 80rpx;
		border-radius: 40rpx;
		background-image: linear-gradient(10deg,rgba(184, 226, 255, 1) 0%, rgba(0, 151, 255, 1) 100%);
		color: rgba(255, 255, 255, 1);
		font-weight: 600;
		font-size: 32rpx;
	}
</style>
