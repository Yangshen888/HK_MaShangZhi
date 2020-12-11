<template>
	<view><web-view :webview-styles="webviewStyles" :src="link"></web-view></view>
</template>

<script>
export default {
	data() {
		return {
			webviewStyles: {
				progress: {
					color: '#FF3333'
				}
			},
			link:"",
		};
	},
	methods: {},
	mounted() {
		this.link=uni.getStorageSync('link');
		// this.link='https://uniapp.dcloud.io/component/web-view';
		console.log(this.link);
	}
};
</script>

<style></style>
