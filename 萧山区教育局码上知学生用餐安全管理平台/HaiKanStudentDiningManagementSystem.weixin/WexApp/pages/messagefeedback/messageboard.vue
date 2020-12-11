<template>
	<view>
		<view class="wrap">
			<view>
				<u-form :mode="form" :rules="rules" ref="uForm">
					<u-form-item prop="type">
						<view class="chooseTye" @click="actionSheetShow = true">
							<view style="float: left;font-weight: 600;">类型</view>
							<input v-model="form.type" style="caret-color: transparent;padding-left: 40rpx;" />
							<u-icon v-if="actionSheetShow == false" name="arrow-down"></u-icon>
							<u-icon v-else name="arrow-up"></u-icon>
						</view>
					</u-form-item>
					<u-form-item prop="content">
						<view style="padding: 0 36rpx;margin-top: 20rpx;">
							<u-field v-model="form.content" label-width="0" type="textarea" placeholder="请输入您要留言的内容(5-500字以内)"
							 placeholder-style="color: rgba(165, 165, 165, 1);"></u-field>
						</view>
					</u-form-item>
				</u-form>
			</view>
			<button class="lyBtn" @click="submit">提交</button>
			<u-select mode="single-column" :list="actionSheetList" v-model="actionSheetShow" @confirm="selectConfirm"></u-select>
		</view>
	</view>
</template>

<script>
	import {
		createMessageboard
	} from '@/api/messagefeedback/messageboard.js';
	export default {
		data() {
			return {
				labelPosition: 'left',
				actionSheetShow: false,
				actionSheetList: [{
						value: '0',
						label: '校方留言'
					},
					{
						value: '1',
						label: '食堂留言'
					},
					{
						value: '2',
						label: '监管部门留言'
					}
				],

				jobsitem: {
					unit: "",
					unitName: "",
					site: "",
					require: ""
				},
				form: {
					type: '',
					content: '',
					schoolUuid: '',
					addPeople: ''
				},
				poorState:"否",
				rules:{
					content: [
						{ 
							required: true, 
							message: '请输入留言内容', 
							trigger: 'blur' ,
						},
					],
					type: [
						{
							required: true, 
							message: '请选择类型',
							trigger: 'change'
						}, 
					]
					
				},
				
				errorType: ['message'],
				// 箭头方向
				isShow: 0,
				// // 选择的留言类型
				// messageType: '',
				// // 留言内容
				// messageCont: ''
			};
		},
		onReady() {
			this.$refs.uForm.setRules(this.rules);
			this.form.addPeople=this.$store.state.userId;
		},
		methods: {
			// 将用户选择的类型传给输入框
			selectConfirm(e) {
				this.form.type = e[0].label
			},
			// 点击actionSheet回调
			// actionSheetCallback(index) {
			// 	uni.hideKeyboard();
			// 	this.form.type = this.actionSheetList[index].text;
			// },
			submit() {
				console.log(this.form)
				// this.$refs.uForm.validate(valid => {
					if(this.form.type==""){
						this.$u.toast('请选择类型');
						return;
					}
					if(this.form.content==""){
						this.$u.toast('请输入留言');
						return;
					}
					this.docreateMessageboard();
				// });
			},
			docreateMessageboard()
			{
				this.form.addPeople=this.$store.state.userId;
				this.form.schoolUuid=uni.getStorageSync('schoolguid');
				createMessageboard(this.form).then(res=>
				{
					if(res.data.code==200)
					{
						uni.redirectTo({
							url:'./index'
						})
						this.$u.toast(res.data.message);
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

	.chooseTye {
		position: relative;
		padding: 0 40rpx;
		overflow: hidden;
		margin: 20rpx 0;
		height: 44rpx;
		line-height: 44rpx;
		font-size: 30rpx;
	}

	.chooseTye u-icon {
		position: absolute;
		top: 50%;
		right: 40rpx;
		transform: translateY(-50%);
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
