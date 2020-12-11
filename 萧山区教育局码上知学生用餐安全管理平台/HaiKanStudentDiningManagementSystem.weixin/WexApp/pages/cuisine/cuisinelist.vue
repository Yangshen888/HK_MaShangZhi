<template>
	<view style="padding-left: 40rpx;">
		<u-cell-group style="margin-top: 5px;" :border="false">
			<u-cell-item  center :is-link="true" @click="quanbuclick" title="全部" :border-bottom="false">
				<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/quanbu@2x.png" mode="widthFix"></image>
				<!-- <u-switch slot="right-icon"></u-switch> -->
			</u-cell-item>
			<u-cell-item center :is-link="true" @click="teseclick" title="特色菜"  :border-bottom="false">
				<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/tese@2x.png" mode="widthFix"></image>
				<!-- <u-switch slot="right-icon"></u-switch> -->
			</u-cell-item>
			<u-cell-item center :is-link="true" @click="huncaiclick" title="荤菜"  :border-bottom="false">
				<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/huncai@2x.png" mode="widthFix"></image>
				<!-- <u-switch slot="right-icon"></u-switch> -->
			</u-cell-item>
			<u-cell-item center :is-link="true" @click="sucaiclick" title="素菜"  :border-bottom="false">
				<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/sucai@2x.png" mode="widthFix"></image>
				<!-- <u-switch slot="right-icon"></u-switch> -->
			</u-cell-item>
			<u-cell-item center :is-link="true" @click="hunsuclick" title="荤素搭配"  :border-bottom="false">
				<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/hunsu@2x.png" mode="widthFix"></image>
				<!-- <u-switch slot="right-icon"></u-switch> -->
			</u-cell-item>
			<u-cell-item center :is-link="true" @click="qitaclick" title="其它"  :border-bottom="false">
				<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/qita@2x.png" mode="widthFix"></image>
				<!-- <u-switch slot="right-icon"></u-switch> -->
			</u-cell-item>
		</u-cell-group>
	</view>
</template>

<script>
import { getcuisineList } from '@/api/cuisine/cuisine.js';
export default {
	data() {
		return {
			value: '',
			stores:{
				cuisineName:"",
				cuisineType:"",
				schoolUuid:""
			}
		};
	},
	computed: {},
	methods: {
		quanbuclick() {
			uni.navigateTo({
			    url: '/pages/cuisine/cuisineview?name='+""+"&type="+""
			});
		},
		teseclick(){
			uni.navigateTo({
			    url: '/pages/cuisine/cuisineview?name='+""+"&type="+"特色菜"
			});
		},
		huncaiclick(){
			uni.navigateTo({
			    url: '/pages/cuisine/cuisineview?name='+""+"&type="+"荤菜"
			});
		},
		hunsuclick(){
			uni.navigateTo({
			    url: '/pages/cuisine/cuisineview?name='+""+"&type="+"半荤菜"
			});
		},
		sucaiclick(){
			uni.navigateTo({
			    url: '/pages/cuisine/cuisineview?name='+""+"&type="+"素菜"
			});
		},
		qitaclick(){
			uni.navigateTo({
			    url: '/pages/cuisine/cuisineview?name='+""+"&type="+"其它"
			});
		},
		clear() {
		},
		search(value) {
			// this.$refs.uToast.show({
			// 	title: '搜索内容为：' + value,
			// 	type: 'success'
			// });
			this.stores.cuisineName=value;
			uni.navigateTo({
			    url: '/pages/cuisine/cuisineview?name='+value+"&type="+""
			});
		},
		custom(value) {
			//this.$u.toast('输入值为：' + value);
			this.stores.cuisineName=value;
			uni.navigateTo({
			    url: '/pages/cuisine/cuisineview?name='+value+"&type="+""
			});
		},
		dogetcuisineList() {
			this.stores.schoolUuid=uni.getStorageSync('schoolguid');
			getcuisineList(this.stores).then(res=>{
				console.log(res);
			})
		}
	},
	mounted() {}
};
</script>

<style>
	/* page {
		background-color: rgb(240, 242, 244);
	} */
</style>

<style lang="scss" scoped>
	.u-cell-icon {
		width: 44rpx;
		height: 44rpx;
		margin-right: 20rpx;
	}
</style>


 