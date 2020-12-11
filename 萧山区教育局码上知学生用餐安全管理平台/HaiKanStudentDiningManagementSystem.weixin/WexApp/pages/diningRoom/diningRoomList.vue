<template>
	<view>
		<!-- <u-navbar title="阳光食堂"></u-navbar> -->
		<view class="wrap content">
			<ul class="con_list">
				<li style="border-right: 2rpx solid #f5f5f5;" @click="liveShot">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/mcsp@3x.png"></image>
					<view class="list_item">每餐实拍</view>
				</li>
				<li style="border-right: 2rpx solid #f5f5f5;"  @click="teseclick">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/cgjg@3x.png"></image>
					<view class="list_item">采购价格</view>
				</li>
				<li  @click="chencaiclick" style="border-right: 2rpx solid #f5f5f5;">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/cclc@3x.png"></image>
					<view class="list_item">成菜流程</view>
				</li>
				<li style="border-right: 2rpx solid #f5f5f5;"  @click="kitchenVideo">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/cfsp@3x.png"></image>
					<view class="list_item">厨房视频</view>
				</li>
				<li style="border-right: 2rpx solid #f5f5f5;" @click="quanbuclick">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/rypx@3x.png"></image>
					<view class="list_item">人员培训</view>
				</li>
				<li @click="teseclick2" style="border-right: 2rpx solid #f5f5f5;">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/glfa@3x.png"></image>
					<view class="list_item">管理方案</view>
				</li>
				<li style="border-right: 2rpx solid #f5f5f5;"  @click="huncaiclick">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/dztz@3x.png"></image>
					<view class="list_item">电子台账</view>
				</li>
				<li style="border-right: 2rpx solid #f5f5f5;" @click="cuisine">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/cpk@3x.png"></image>
					<view class="list_item">菜品库</view>
				</li>
				<li  style="border-right: 2rpx solid #f5f5f5;" @click="toWorkStudy" v-if="isptjob">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/qgjx@3x.png"></image>
					<view class="list_item">勤工俭学</view>
				</li>
				<li  style="border-right: 2rpx solid #f5f5f5;" @click="toActicle">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/jz5bf7@3x.png"></image>
					<view class="list_item">账务公开</view>
				</li>
				<li  style="border-right: 2rpx solid #f5f5f5;" @click="toUserInfo">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/bz9bf2@3x.png"></image>
					<view class="list_item">员工管理</view>
				</li>
				<li @click="toPay" v-show="schoolname"  style="border-right: 2rpx solid #f5f5f5;">
					<image src="https://msz-b.jiulong.yoruan.com/img/images/zf@3x.png"></image>
					<view class="list_item">支付</view>
				</li>
			</ul>
		</view>
		<u-modal v-model="show" :show-cancel-button="true" @confirm="shouquan" style="text-align: center;">是否确认授权？</u-modal>
	</view>
</template>

<script>
export default {
	data() {
		return {
			isptjob: false,
			show:false,
			schoolname:true
		};
	},
	onLoad(){
		this.isptjob=uni.getStorageSync('isptjob');
		console.log(this.isptjob)
	},
	methods: {
		liveShot() {
			uni.navigateTo({
				url: '/pages/diningRoom/LiveShot'
			});
		},
		teseclick() {
			uni.navigateTo({
				url: '/pages/diningRoom/PurchaseList'
			});
		},
		kitchenVideo() {
			uni.navigateTo({
				url: '/pages/diningRoom/kitchenVideo'
			});
		},
		shouquan(){
			uni.navigateTo({
				url: '/pages/login/WxAuthLogin'
			});
		},
		kitchenVideoUpload() {
			if (this.checkAuth()) {
				this.show = true;
			} else {
				uni.navigateTo({
					url: '/pages/diningRoom/UpKitchenVideo'
				});
			}
		},
		chencaiclick() {
			uni.navigateTo({
				url: '/pages/flow/flow'
			});
		},
		chencaiupclick() {
			uni.navigateTo({
				url: '/pages/flow/flowupload'
			});
		},
		checkAuth() {
			console.log(11111);
			console.log(this.$store.state);
			if (this.$store.state.openid == '' || this.$store.state.openid == null) {
				return true;
			} else {
				return false;
			}
		},
		checkShow() {
			console.log(this.$store.state.isUploadV);
			console.log(this.$store.state.schoolGuid);
			return this.$store.state.isUploadV && this.$store.state.schoolGuid != '' && this.$store.state.schoolGuid == uni.getStorageSync('schoolguid');
		},
		checkShow2() {
			console.log(this.$store.state.isAddPR);
			console.log(this.$store.state.schoolGuid);
			return this.$store.state.isAddPR && this.$store.state.schoolGuid != '' && this.$store.state.schoolGuid == uni.getStorageSync('schoolguid');
		},
		checkShow3() {
			console.log(this.$store.state.isUploadP);
			console.log(this.$store.state.schoolGuid);
			return this.$store.state.isUploadP && this.$store.state.schoolGuid != '' && this.$store.state.schoolGuid == uni.getStorageSync('schoolguid');
		},
		checkShow4() {
			return this.$store.state.isFlow && this.$store.state.schoolGuid != '' && this.$store.state.schoolGuid == uni.getStorageSync('schoolguid');
		},
		quanbuclick() {
			uni.navigateTo({
				url: '/pages/canteenManage/message'
			});
		},
		teseclick2() {
			uni.navigateTo({
				url: '/pages/canteenManage/plan'
			});
		},
		huncaiclick() {
			uni.navigateTo({
				url: '/pages/canteenManage/money'
			});
		},
		cuisine() {
			uni.navigateTo({
				url: '/pages/cuisine/cuisinelist'
			});
		},
		toWorkStudy() {
			if (this.checkAuth()) {
				this.show = true;
			} else {
				uni.navigateTo({
					url: '../workstudy/home'
				});
			}
		},
		toActicle(){
			uni.navigateTo({
				url: '/pages/acticle/acticle'
			});
		},
		toUserInfo(){
			uni.navigateTo({
				url: '/pages/userinfo/userinfo'
			});
		},
		quality() {
			uni.navigateTo({
				url: '/pages/quality/quality'
			});
		},
		toPay() {
			if(uni.getStorageSync('isYard')){
				if (this.checkAuth()) {
					this.show = true;
				} else {
					uni.navigateTo({
						url: '/pages/pay/pay'
					});
				}
			}else{
				uni.navigateTo({
					url: '/pages/diningRoom/QRcode'
				});
			}
			
		},
		foodlibrary() {
			uni.navigateTo({
				url: '/pages/cuisine/cuisinelist'
			});
		},
		standard() {
			uni.navigateTo({
				url: '/pages/quality/quality'
			});
		},
		PRecordUpload() {
			uni.navigateTo({
				url: '/pages/diningRoom/purchaseRecordToUp'
			});
		},
		liveShotUpdate() {
			uni.navigateTo({
				url: '/pages/diningRoom/LiveShotUpdate'
			});
		}
	}
};
</script>

<style lang="scss" scoped>
.con_list{
	// margin-top: 30rpx;
	width: 100%;
	border-top: 2rpx solid #F5F5F5;
}
.con_list li {
	box-sizing: border-box;
	width: 250rpx;
	height: 212rpx;
	border-bottom: 2rpx solid #f5f5f5;
	text-align: center;
	float: left;
	padding-top: 50rpx;
	overflow: hidden;
	position: relative;
}
.con_list image {
	position: absolute;
	bottom: 80rpx;
	width: 90rpx;
	height: 90rpx;
	transform: translateX(-50%);
}
.con_list .list_item{
	position: absolute;
	bottom: 20rpx;
	width: 100%;
	height: 34rpx;
	line-height: 34rpx;
	font-size: 30rpx;
}
</style>
