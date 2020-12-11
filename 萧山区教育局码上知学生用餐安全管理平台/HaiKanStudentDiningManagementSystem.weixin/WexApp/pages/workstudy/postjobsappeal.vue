<template>
	<view>
		<view class="wrap">
			<u-form :model="form" :rules="rules" ref="uForm" :errorType="errorType">
				<u-form-item label="用工单位" :label-position="labelPosition" prop="unit" label-width="150"><u-input v-model="jobsitem.unit" disabled="true" type="textarea" height="35"  /></u-form-item>
				<u-form-item label="岗位名称" :label-position="labelPosition" prop="unitName" label-width="150">
					<u-input v-model="jobsitem.unitName" disabled="true" type="textarea" height="35"  />
				</u-form-item>
				<u-form-item label="工作地点" :label-position="labelPosition" prop="site" label-width="150">
					<u-input v-model="jobsitem.site" disabled="true" type="textarea" height="35"  /></u-form-item>
				<u-form-item label="工作要求" :label-position="labelPosition" prop="require" label-width="150">
					<u-input v-model="jobsitem.require" disabled="true" type="textarea" height="35"  /></u-form-item>
				<u-form-item label="学生姓名" :label-position="labelPosition" prop="stuName" label-width="150"><u-input v-model="form.stuName" /></u-form-item>
				<u-form-item :label-position="labelPosition" label="性别" prop="sex" label-width="150">
					<u-input :border="border" type="select" :select-open="actionSheetShow" v-model="form.sex" placeholder="请选择性别" @click="actionSheetShow = true"></u-input>
				</u-form-item>
				<u-form-item :label-position="labelPosition" label="年级" prop="grade" label-width="150"><u-input v-model="form.grade" /></u-form-item>
				<u-form-item :label-position="labelPosition" label="班级" prop="class" label-width="150"><u-input v-model="form.class" /></u-form-item>
				<u-form-item :label-position="labelPosition" label="是否贫困生" prop="poorState" label-width="150">
					<u-radio-group v-model="poorState" @change="radioGroupChange">
						<u-radio v-for="(item, index) in radioList" :key="index" :name="item.name" :disabled="item.disabled">{{ item.name }}</u-radio>
					</u-radio-group>
				</u-form-item>

				<u-form-item :label-position="labelPosition" label="监护人姓名" prop="guardianName" label-width="150"><u-input v-model="form.guardianName" /></u-form-item>
				<u-form-item :label-position="labelPosition" label="监护人电话" prop="guardianPhone" label-width="150"><u-input v-model="form.guardianPhone" /></u-form-item>
			</u-form>
			<u-button @click="submit" :ripple="true"  ripple-bg-color="#909399" shape="circle">提交</u-button>
			<u-action-sheet :list="actionSheetList" v-model="actionSheetShow" @click="actionSheetCallback"></u-action-sheet>
		</view>
		
	</view>

</template>

<script>
	import {
		getPostjobsDetial,
		createPostjobsAppeal
	} from '@/api/workstudy/postjobs.js';
	export default {
		data() {
			return {
				labelPosition: 'left',
				actionSheetShow: false,
				actionSheetList: [
					{
						text: '男'
					},
					{
						text: '女'
					}
				],
				jobsitem: {unit:"",unitName:"",site:"",require:""},
				form:{
					postUuid:'',
					stuName:'',
					sex:'',
					grade:'',
					class:'',
					poorState:1,
					guardianName:'',
					guardianPhone:'',
					appealPeople:''
				},
				poorState:"否",
				rules:{
					stuName: [
						{ 
							required: true, 
							message: '请输入姓名', 
							trigger: 'blur' ,
						},
					],
					sex: [
						{
							required: true, 
							message: '请选择性别',
							trigger: 'change'
						}, 
					],
					grade: [
						{ 
							required: true, 
							message: '请输入年级', 
							trigger: 'blur' ,
						},
					],
					class: [
						{ 
							required: true, 
							message: '请输入班级', 
							trigger: 'blur' ,
						},
					],
					guardianName: [
						{ 
							required: true, 
							message: '请输入监护人姓名', 
							trigger: 'blur' ,
						},
					],
					guardianPhone: [
						{
							required: true, 
							message: '请输入手机号',
							trigger: ['change','blur'],
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
				radioList:[
					{name:'是',value:0,disabled:false},
					{name:'否',value:1,disabled:false}
				],
				errorType: ['message'],
			};
		},
		onReady() {
			this.$refs.uForm.setRules(this.rules);
		},
		methods: {
			jobsdetail(guid) {
				getPostjobsDetial({
					guid: guid
				}).then(res => {
					if (res.data.code == 200) {
						this.jobsitem=res.data.data;
					} else {

					}
				})
			},
			// 点击actionSheet回调
			actionSheetCallback(index) {
				uni.hideKeyboard();
				this.form.sex = this.actionSheetList[index].text;
			},
			// checkbox选择发生变化
			radioGroupChange(e) {
				console.log("e:"+e);
				if(e=="是")
				{
					this.form.poorState = 0;
				}
				else if(e=="否")
				{
					this.form.poorState = 1;
				}
				
			},
			submit() {
				this.$refs.uForm.validate(valid => {
					if (valid) {
						console.log('验证通过');
						this.docreatePostjobsAppeal();
					} else {
						console.log('验证失败');
					}
				});
			},
			docreatePostjobsAppeal()
			{
				this.form.appealPeople=this.$store.state.userId;;
				createPostjobsAppeal(this.form).then(res=>
				{
					if(res.data.code==200)
					{
						this.$u.toast(res.data.message);
						uni.redirectTo({
							url:'./home'
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
			if(!this.utiils.isEmpty(options))
			{
				if(!this.utiils.isEmpty(options.guid))
				{
					this.form.postUuid=options.guid;
					this.jobsdetail(options.guid);
				}
			}
		 }
	}
</script>

<style>
	.wrap {
		padding: 30rpx;
		word-break:break-all;
	}
</style>
