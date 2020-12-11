<template>
	<view>
		<view style="width: 100%;text-align: center;">
			<image :src="qrcode" style="width: 90%;" mode="widthFix" show-menu-by-longpress="true"></image>
		</view>
		<view class="u-content" style="letter-spacing: 2px; text-align: justify;">
				<u-parse :html="accessory"></u-parse>
		</view>
	</view>
</template>

<script>
	import http from '@/utils/http.js';
	export default {
		data() {
			return {
				labelPosition: 'left',
				head: false,
				url: http.baseUrl + 'UploadFiles/Picture/',
				qrcode:'',
				accessory:'',
			};
		},
		computed: {},
		methods: {
		},
		onLoad() {
			this.qrcode=http.baseUrl+'UploadFiles/SchoolImg/'+uni.getStorageSync('qrcode');
			this.accessory=uni.getStorageSync('accessory');
		}
	};
</script>

<style>
/* #ifndef H5 */
page {
	height: 100%;
	/* background-color: #f2f2f2; */
}
.wrap {
	/* padding: 30rpx; */
	word-break: break-all;
	padding: 30rpx 60rpx;
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
}
</style>
