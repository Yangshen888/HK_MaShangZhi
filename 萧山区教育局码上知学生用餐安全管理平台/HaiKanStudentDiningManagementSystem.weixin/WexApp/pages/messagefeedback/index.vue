<template>
	<view class="wrap">
		<view class="list-wrap">
			<u-cell-group title-bg-color="rgb(243, 244, 246)">
				<u-cell-item :title="list[0].name" @click="openPage(list[0].path)">
					<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/liuyanban@3x.png" mode="widthFix"></image>
				</u-cell-item>
			</u-cell-group>
			<u-cell-group title-bg-color="rgb(243, 244, 246)">
				<u-cell-item :title="list[1].name" @click="openPage(list[1].path)">
					<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/lyfk@3x.png" mode="widthFix"></image>
				</u-cell-item>
			</u-cell-group>
			<u-cell-group title-bg-color="rgb(243, 244, 246)">
				<u-cell-item :title="list[2].name" @click="openPage(list[2].path)">
					<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/jbxx@3x.png" mode="widthFix"></image>
				</u-cell-item>
			</u-cell-group>
			<u-cell-group title-bg-color="rgb(243, 244, 246)">
				<u-cell-item :title="list[3].name" @click="openPage(list[3].path)">
					<image slot="icon" class="u-cell-icon" src="https://msz-b.jiulong.yoruan.com/img/images/wjdc@3x.png" mode="widthFix"></image>
				</u-cell-item>
			</u-cell-group>
		</view>
	</view>
</template>

<script>
	export default {
		data() {
			return {
				list: [{
						name: "留言板",
						icon: "edit-pen",
						path: "./messageboard"
					},
					{
						name: "留言反馈",
						icon: "chat-fill",
						path: "./messageback"
					},
					{
						name: "举报信箱",
						icon: "email",
						path: "./report"
					},
					{
						name: "问卷调查",
						icon: "file-text",
						path: "./survey"
					}
				],
				isshowReport:false,
			}
			
		},
		computed: {
			
		},
		onLoad() {
			this.isshowReport=uni.getStorageSync('isshowReport');
			console.log(this.isshowReport);
		},
		methods: {
			openPage(path) {
				uni.navigateTo({
					url:path
				})
			}
		}
	}
</script>

<style>
	.list-wrap {
		position: relative;
		box-sizing: border-box;
		width: 100%;
		font-size: 32rpx;
		line-height: 25px;
		color: #606266;
		background-color: #fff;
		text-align: left;
		padding-left: 40rpx;
	}
</style>
<style lang="scss" scoped>
	.u-cell-icon {
		width: 44rpx;
		height: 44rpx;
		margin-right: 20rpx;
	}
</style>