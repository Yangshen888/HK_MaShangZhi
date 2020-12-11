<template>
	<view>
		<u-cell-group style="margin-top: 5px;">
			<u-cell-item center :is-link="true" value="点击查看" @click="quanbuclick" title="人员培训信息">
				<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/image/actionSheet.png" mode="widthFix"></image>
				<u-switch slot="right-icon"></u-switch>
			</u-cell-item>
			<u-cell-item center :is-link="true" value="点击查看" @click="teseclick" title="管理方案">
				<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/image/empty.png" mode="widthFix"></image>
				<u-switch slot="right-icon"></u-switch>
			</u-cell-item>
			<u-cell-item center :is-link="true" value="点击查看" @click="huncaiclick" title="电子台账">
				<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/image/queryParams.png" mode="widthFix"></image>
				<u-switch slot="right-icon"></u-switch>
			</u-cell-item>
		</u-cell-group>
	</view>
</template>

<script>
export default {

	methods: {
	quanbuclick(){
		uni.navigateTo({
			url: '/pages/canteenManage/message'
		});			
	},
	teseclick(){
		uni.navigateTo({
			url: '/pages/canteenManage/plan'
		});			
	},
	huncaiclick(){
		uni.navigateTo({
			url: '/pages/canteenManage/money'
		});			
	},
		clear() {
		}
	},
	mounted() {}
};
</script>

<style lang="scss" scoped>
	.u-cell-icon {
		width: 44rpx;
		height: 44rpx;
		margin-right: 8rpx;
	}
</style>
 