<template>
	<view class="wrap">
		<scroll-view scroll-y style="height: 100%;width: 100%;">
			<u-form ref="uForm">
				<u-form-item :label-position="labelPosition" label="学生身份证" label-width="150">
					<u-field v-model="foodName" :type="'text'" :border-bottom="false" placeholder="请输入身份证号" @blur="changeName" />
				</u-form-item>
				<u-form-item :label-position="labelPosition" label="学生姓名" label-width="150">
					<u-field v-model="stuName" :type="'text'" :border="true" disabled="true" />
				</u-form-item>
				<u-form-item :label-position="labelPosition" label="班级" label-width="150">
					<u-field v-model="stuClass" :type="'number'" :border="true" disabled="true" />
				</u-form-item>
				<view v-show="isShow">
					<u-form-item :label-position="labelPosition" label="支付项目" label-width="150">
						<u-input :border="border" type="select" :select-open="selectShow" v-model="billname" placeholder="请选择支付项目" @click="selectShow = true"></u-input>
					</u-form-item>
					<u-form-item :label-position="labelPosition" label="项目金额" label-width="150">
						<u-field v-model="amountPayable" :type="'text'" :border="true" disabled="true" />
					</u-form-item>
					<u-form-item :label-position="labelPosition" label="已付金额" label-width="150">
						<u-field v-model="orderMoney" :type="'text'" :border="true" disabled="true" />
					</u-form-item>
					<u-form-item :label-position="labelPosition" label="应付金额" label-width="150">
						<u-field v-model="money" :type="'text'" :border="true" disabled="true" />
					</u-form-item>
					<u-select mode="single-column" :list="dateType" v-model="selectShow" @confirm="selectConfirm"></u-select>
				</view>



				<view v-if="isShowPep">

					<u-line color="#d5d9e2" />
				</view>

			</u-form>
			<u-button type="primary" @click="toPay">确认支付</u-button>
		</scroll-view>
	</view>

</template>

<script>
	import {
		getBillInfo,
		toUnifiedorder,
		BillSuccess
	} from '@/api/student/pay.js'
	import md5 from '@/global/md5.js'
	export default {
		data() {
			return {
				selectShow: false,
				stuName: "",
				stuClass: "",
				billname: "",
				studentBillUuid: "",
				amountPayable: 0,
				orderMoney: 0,
				money: 0,
				payList: [],
				dateType: [],
				isShow: false,
				stuidcard:""
			}
		},
		methods: {
			toPay() {
				console.log(this.$store.state.openid);
				//return;
				let data = {
					guid: uni.getStorageSync('schoolguid'),
					body: this.billname,
					totalfee: this.money * 100,
					openid: this.$store.state.openid,
					billGuid:this.studentBillUuid
				}
				toUnifiedorder(data).then(res => {
					if (res.data.code == 200) {
						console.log(res);
						let strtime = String(Date.now());//时间戳
						let outtradeno = res.data.data.outtradeno;
						let signmd5 = md5("appId="+res.data.data.appid+"&nonceStr="+res.data.data.sjcode+"&package=prepay_id="+res.data.data.prepayId
						+"&signType=MD5&timeStamp="+strtime+"&key="+res.data.data.key);//签名验证
						
						
						let successdata={
							Idcard:this.stuidcard,
							OrderMoney:this.money,
							SerialNumber:"",
							OrderNum:outtradeno,
							SystemUserUuid:uni.getStorageSync('userId'),
						};
						
						
						uni.requestPayment({
						    provider: 'wxpay',
						    timeStamp: strtime,
						    nonceStr: res.data.data.sjcode,
						    package: 'prepay_id='+res.data.data.prepayId,
						    signType: 'MD5',
						    paySign: signmd5,
						    success: function (res1) {
								BillSuccess(successdata).then(res=>{
									if(res.data.code==200)
									{
										uni.showToast({
											title: '成功',
											duration: 2000,
											icon: 'none'
										});
										uni.redirectTo({
											url: '/pages/home/index'
										});
									}
									else
									{
										uni.showToast({
											title: res.data.message,
											duration: 2000,
											icon: 'none'
										});
									}
								})
						        console.log('success:' + JSON.stringify(res1));
						    },
						    fail: function (err) {
						        console.log('fail:' + JSON.stringify(err));
						    }
						});
					}
					else
					{
						uni.showToast({
							title: res.data.message,
							duration: 2000,
							icon: 'none'
						});
					}

				});

			},
			changeName(e) {
				this.stuidcard = e.detail.value;
				this.isShow = false;
				console.log(11111);
				console.log(e.detail.value);
				console.log(e);
				if (e.detail.value == '') {
					e.detail.value = null;
				}
				let guid = uni.getStorageSync('schoolguid');
				getBillInfo({
					idcard: e.detail.value,
					suid: guid
				}).then(res => {
					console.log(res);
					if (res.data.data != null) {
						
						this.stuName = res.data.data.ent.studentName;
						this.stuClass = res.data.data.ent.className;
						let list = res.data.data.list;
						this.payList = res.data.data.list;
						this.dateType = [];
						this.billname = "";
						this.studentBillUuid = "";
						this.amountPayable = 0;
						let that = this;
						list.forEach(function(item, index) {
							console.log(item);
							that.dateType.push({
								value: item.studentBillUuid,
								label: item.orderName
							});
						});
						this.billname = this.dateType[0].label;
						this.studentBillUuid = this.dateType[0].value;
						this.amountPayable = list[0].amountPayable;
						this.orderMoney = list[0].orderMoney;
						this.money = (this.amountPayable - this.orderMoney).toFixed(2);
						this.isShow = true;
						// let data = res.data.data;
						// if (data.accessory.length <= 0) {
						// 	this.isShowUp = true;
						// } else {
						// 	this.isShowUp = false;
						// 	this.accessory = data.accessory;
						// }
						// console.log(this.isShowUp);
						// this.ingredientUuid = data.ingredientUuid;
						// this.fType = data.type;
						// this.isFTDisabled = true;
					} else {

					}
				});
			},
			selectConfirm(e) {
				console.log(e);
				e.map((val, index) => {
					this.billname = val.label;
					this.studentBillUuid = val.value;
				});
				let bill = this.payList.find(x => x.studentBillUuid == this.studentBillUuid);
				this.amountPayable = bill.amountPayable;
				this.orderMoney = bill.orderMoney;
				this.money = this.amountPayable - this.orderMoney;
			}
		}
	}
</script>

<style>
	.wrap {
		padding: 30rpx;
		display: flex;
		flex-direction: column;
		height: calc(100vh - var(--window-top));
		width: 100%;
	}
</style>
