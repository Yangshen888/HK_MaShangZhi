<template>
	<view>
		<view class="wrap">
			<u-form :model="form" :rules="rules" ref="uForm" :errorType="errorType">
				<u-form-item :label-position="labelPosition" label="举报人" prop="name" label-width="150">
					<u-input v-model="form.name" />
				</u-form-item>
				<u-form-item :label-position="labelPosition" label="联系电话" prop="phone" label-width="150">
					<u-input v-model="form.phone" />
				</u-form-item>
				<u-form-item :label-position="labelPosition" label="举报内容" prop="content" label-width="150">
				</u-form-item>
				<u-field type="textarea" v-model="form.content" label-width="0" placeholder="请输入您要举报的内容(5-500字以内)" placeholder-style="color:rgba(165, 165, 165, 1);"></u-field>
			</u-form>
			<button class="lyBtn" @click="submit">提交</button>
		</view>
	</view>
</template>

<script>
	import {
		createReport
	} from '@/api/messagefeedback/messageboard.js';
	export default {
		data() {
			return {
				labelPosition: 'left',
				actionSheetShow: false,


				jobsitem: {
					unit: "",
					unitName: "",
					site: "",
					require: ""
				},
				form: {
					name: '',
					phone: '',
					content: '',
					schoolUuid: '',
					addPeople: ''
				},
				poorState: "否",
				rules: {
					name: [{
						required: true,
						message: '请输入您的姓名',
						trigger: 'change'
					}, ],
					content: [{
						required: true,
						trigger: 'blur',
					}, ],
					phone: [{
							required: true,
							message: '请输入您的手机号',
							trigger: ['change', 'blur'],
						},
						{
							validator: (rule, value, callback) => {
								// 调用uView自带的js验证规则，详见：https://www.uviewui.com/js/test.html
								return this.$u.test.mobile(value);
							},
							message: '手机号码不正确',
							// 触发器可以同时用blur和change，二者之间用英文逗号隔开
							trigger: ['change','blur'],
						}
					],
					
				},
				
				errorType: ['message'],
			};
		},
		onReady() {
			this.$refs.uForm.setRules(this.rules);
			this.form.addPeople=this.$store.state.userId;
		},
		methods: {
			// 点击actionSheet回调
			actionSheetCallback(index) {
				uni.hideKeyboard();
				this.form.type = this.actionSheetList[index].text;
			},
			
			submit() {
				this.$refs.uForm.validate(valid => {
					if (valid) {
						console.log('验证通过');
						this.docreateReport();
					} else {
						console.log('验证失败');
					}
				});
			},
			docreateReport()
			{
				//this.form.addPeople="";
				this.form.schoolUuid=uni.getStorageSync('schoolguid');
				createReport(this.form).then(res=>
				{
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
		mounted() {
		},
		onLoad:function(options){
			
		 }
	}
</script>

<style>
	.wrap {
		padding: 30rpx;
	}

	.lyBtn {
		position: fixed;
		bottom: 80rpx;
		left: 50%;
		transform: translateX(-50%);
		width: 600rpx;
		height: 80rpx;
		line-height: 80rpx;
		border-radius: 40rpx;
		background-image: linear-gradient(10deg, rgba(184, 226, 255, 1) 0%, rgba(0, 151, 255, 1) 100%);
		color: rgba(255, 255, 255, 1);
		font-weight: 600;
		font-size: 32rpx;
	}
</style>
