<template>
	<view class="wrap">
		<scroll-view scroll-y style="height: 100%;width: 100%;">
			<u-form ref="uForm">
				<u-form-item :label-position="labelPosition" label="食材名称" label-width="150">
					<u-field v-model="foodName" :type="'text'" :border-bottom="false" placeholder="请输入食材名称" @blur="changeName" />
				</u-form-item>
				<view v-show="!isFTDisabled">
					<u-form-item :label-position="labelPosition" label="食材类型" label-width="150">
						<u-input :border="border" type="select" :select-open="selectShow2" v-model="fType" placeholder="请选择食材类型" @click="selectShow2 = true"></u-input>
					</u-form-item>
				</view>
				<u-form-item :label-position="labelPosition" label="供应商" label-width="150">
					<u-input v-model="supplier" :type="'text'" :border="true" placeholder="请输入供应商" />
				</u-form-item>
				<u-form-item :label-position="labelPosition" label="采购时间" label-width="150">
					<u-input
						:border="border"
						type="select"
						:select-open="selectShow3"
						v-model="purchaseDate"
						placeholder="请选择采购时间(时/分)"
						@click="selectShow3 = true"
					></u-input>
				</u-form-item>
				<u-form-item :label-position="labelPosition" label="采购数量" label-width="150">
					<u-input v-model="purchaseNum" :type="'text'" :border="true" placeholder="请输入采购数量" />
				</u-form-item>
				<u-form-item :label-position="labelPosition" label="采购单价" label-width="150">
					<u-input v-model="price" :type="'text'" :border="true" placeholder="请输入采购单价" />
				</u-form-item>
				<u-form-item :label-position="labelPosition" label="采购人员" label-width="150">
					<u-input v-model="systemUserUuid" :type="'text'" :border="true" placeholder="请输入采购人员" />
				</u-form-item>
				<!-- <view v-if="isShowPep">
					<u-collapse>
						<u-collapse-item :title="'采购人员'" :key="index">
							<view>
								<u-checkbox-group @change="pepchange">
									<u-checkbox v-model="item.checkbox" v-for="(item, index) in peopleList" :key="index" :name="item.systemUserUuid">
										{{ item.realName }}
									</u-checkbox>
								</u-checkbox-group>
							</view>
						</u-collapse-item>
					</u-collapse>
					<u-line color="#d5d9e2" />
				</view> -->
				<!-- <u-form-item :label-position="labelPosition" label="用餐类型" label-width="150">
				<u-input :border="border" type="select" :select-open="selectShow" v-model="type" placeholder="请选择用餐类型" @click="selectShow = true"></u-input>
			</u-form-item>
			<u-form-item :label-position="labelPosition" label="菜品名称" label-width="150">
				<u-input :border="border" type="select" :select-open="selectShow1" v-model="query.cuisineName" placeholder="请选择菜品名称" @click="selectShow1 = true"></u-input>
			</u-form-item> -->

				<u-select mode="single-column" :list="dateType" v-model="selectShow" @confirm="selectConfirm"></u-select>
				<u-select mode="single-column" :list="cuisines" v-model="selectShow1" @confirm="selectConfirm1"></u-select>
				<u-select mode="single-column" :list="foodType" v-model="selectShow2" @confirm="selectConfirm2"></u-select>
				<u-select v-model="selectShow3" mode="mutil-column" :list="timelist" @confirm="selectConfirm3"></u-select>
				<view v-if="isShowUp">
					<easy-upload
						ref="prup"
						:dataList="imageList"
						:uploadUrl="action"
						:types="'image'"
						:deleteUrl="actiondelete"
						:uploadCount="6"
						@successImage="successImage"
						:upload_max="5"
						:uploadList="uplist"
						:header="header"
					></easy-upload>
				</view>
				<button class="dlbutton" hover-class="dlbutton-hover" @click="doUpload()">确认上传</button>
			</u-form>
		</scroll-view>
	</view>
</template>

<script>
import http from '@/utils/http.js';
import { loadIngredient2, getStaffList, createPurchaseRecord,getFoodTypeList } from '@/api/diningRoom/PurchaseRecord.js';
export default {
	data() {
		return {
			foodName: '',
			fType: '',
			supplier: '',
			purchaseNum: '',
			purchaseDate: '',
			price: '',
			ingredientUuid: '',
			systemUserUuid: '',
			accessory: '',
			labelPosition: 'left',
			border: false,
			selectShow: false,
			selectShow1: false,
			selectShow2: false,
			selectShow3: false,
			isShowUp: true,
			isFTDisabled: false,
			isShowPep: false,
			type: '',
			typevalue: '',
			dateType: [{ value: 'morn', label: '早餐' }, { value: 'noon', label: '中餐' }, { value: 'night', label: '晚餐' }],
			foodType: [
			],
			timelist: [
				[
					{
						value: '1',
						label: '1'
					}
				],
				[
					{
						value: '00',
						label: '00'
					}
				]
			],
			peopleList: [],

			action: http.baseUrl + 'api/v1/Ingredient/PurchaseRecord/UpLoad',
			actiondelete: http.baseUrl + 'api/v1/Ingredient/PurchaseRecord/DeleteFile',
			header: { Authorization: 'Bearer ' + uni.getStorageSync('token') },
			imageList: [],
			uplist: []
		};
	},
	methods: {
		dogetFoodTypeList(){
			getFoodTypeList().then(res=>{
				if(res.data.code==200){
					let foodType=[];
					for(let k=0;k<res.data.data.length;k++){
						foodType[k]={
							value:res.data.data[k].typeUuid,
							label:res.data.data[k].name,
						}
					}
				this.foodType=foodType;
				}
			});
		},
		async doUpload() {
			let imgs = [];
			if (this.isShowUp) {
				imgs = this.$refs.prup.uploadImages;
			}
			console.log(this.fType)
			if(this.ingredientUuid==""){
				uni.showModal({
					title: '提示',
					content: '暂无该食材',
					showCancel: false
				});
				return;
			}
			if (this.foodName.trim() == '') {
				uni.showModal({
					title: '提示',
					content: '请输入食材名称',
					showCancel: false
				});
				return;
			}
			if (this.fType == '') {
				uni.showModal({
					title: '提示',
					content: '请输入食材类型',
					showCancel: false
				});
				return;
			}
			if (this.purchaseDate.trim() == '') {
				uni.showModal({
					title: '提示',
					content: '请输入采购时间',
					showCancel: false
				});
				return;
			}
			if (this.purchaseNum.trim() == '') {
				uni.showModal({
					title: '提示',
					content: '请输入采购数量',
					showCancel: false
				});
				return;
			}
			if (this.price.trim() == '') {
				uni.showModal({
					title: '提示',
					content: '请输入价格',
					showCancel: false
				});
				return;
			}
			if (this.isShowUp && imgs.length <= 0) {
				uni.showModal({
					title: '提示',
					content: '请选择图片',
					showCancel: false
				});
				return;
			}
			var priceReg = /(^[1-9]\d*(\.\d{1,2})?$)|(^0(\.\d{1,2})?$)/;
			if(!priceReg.test( this.price.trim())){
				uni.showModal({
					title: '提示',
					content: '请输入正确的单价',
					showCancel: false
				});
				return;
			}
			//return ;
			if (this.isShowUp && imgs.length > 0) {
				this.accessory = imgs.join('|');
			}
			let date = new Date();
			let time = date.getFullYear() + '-' + (date.getMonth() + 1) + '-' + date.getDate() + ' ' + this.purchaseDate + ':00';
			let data = {
				ingredientUuid: this.ingredientUuid,
				foodName: this.foodName,
				type: this.fType,
				supplier: this.supplier,
				purchaseDate: time,
				purchaseNum: this.purchaseNum,
				addPeople: this.$store.state.userName,
				accessory: this.accessory,
				systemUserUuid: this.systemUserUuid,
				price:this.price,
			};
			console.log(22222222);
			console.log(data);
			await createPurchaseRecord(data).then(res => {
				console.log(res);
				if (res.data.code == 200) {
					uni.navigateTo({
						url: '/pages/diningRoom/diningRoomList'
					});
					uni.showModal({
						title: '添加成功',
						showCancel: false
					});
				}
			});
		},
		changeName(e) {
			console.log(e.detail.value);
			console.log(e);
			if (e.detail.value == '') {
				e.detail.value = null;
			}
			loadIngredient2({ name: e.detail.value }).then(res => {
				console.log(res);
				if (res.data.data != null) {
					let data = res.data.data;
					if (data.accessory.length <= 0) {
						this.isShowUp = true;
					} else {
						this.isShowUp = false;
						this.accessory = data.accessory;
					}
					console.log(this.isShowUp);
					this.ingredientUuid = data.ingredientUuid;
					this.fType = data.type;
					this.isFTDisabled = true;
				} else {
					this.isFTDisabled = false;
					this.isShowUp = true;
					this.fType = '';
					this.ingredientUuid = '';
				}
			});
		},

		// 选择用餐类型回调
		selectConfirm(e) {
			this.type = '';
			console.log(e);
			e.map((val, index) => {
				this.type += this.type == '' ? val.label : '-' + val.label;
				this.typevalue = this.typevalue == '' ? val.value : val.value;
			});
			this.doCuisineList();
		},
		// 选择菜品回调
		selectConfirm1(e) {
			this.query.cuisineName = '';
			e.map((val, index) => {
				this.query.cuisineName = this.query.cuisineName == '' ? val.label : '-' + val.label;
				this.query.cuisineUuid = val.value;
			});
			console.log(this.query.cuisineName);
			console.log(this.query.cuisineUuid);
		},
		selectConfirm2(e) {
			console.log(e);
			this.fType = e[0].value;
		},
		selectConfirm3(e) {
			this.purchaseDate = '';
			console.log(e);
			let h = e[0].value;
			let m = e[1].value;
			this.purchaseDate = h + ':' + m;
		},
		pepchange(e) {
			console.log(e);
			this.systemUserUuid = e.join(',');
			console.log(this.systemUserUuid);
		},
		// checkboxGroupChange(e){
		// 	console.log(e);
		// },
		successImage(e) {
			console.log(e);
			// console.log(this.imageList);
			// console.log(this.uplist);
			// console.log(this.$refs.prup.uploadlist);
			console.log(this.$refs.prup.uploadImages);
		}
	},
	onLoad() {
		this.dogetFoodTypeList();
		this.timelist[0] = [];
		this.timelist[1] = [];
		for (let i = 0; i < 24; i++) {
			let num = '';
			if (i < 10) {
				num = '0' + i;
			} else {
				num = i.toString();
			}
			this.timelist[0].push({ value: num, label: num });
		}
		for (let i = 0; i < 60; i++) {
			let num = '';
			if (i < 10) {
				num = '0' + i;
			} else {
				num = i.toString();
			}
			this.timelist[1].push({ value: num, label: num });
		}
		let that = this;
		getStaffList().then(res => {
			console.log(1111111);
			console.log(res);
			that.peopleList = res.data.data;
			that.isShowPep = true;
		});
	}
};
</script>

<style>
.wrap {
	padding: 30rpx;
	display: flex;
	flex-direction: column;
	height: calc(100vh - var(--window-top));
	width: 100%;
}
.dlbutton {
	color: #ffffff;
	font-size: 34upx;
	width: 470upx;
	height: 100upx;
	background: linear-gradient(-90deg, rgba(63, 205, 235, 1), rgba(188, 226, 158, 1));
	box-shadow: 0upx 0upx 13upx 0upx rgba(164, 217, 228, 0.2);
	border-radius: 50upx;
	line-height: 100upx;
	text-align: center;
	margin-left: auto;
	margin-right: auto;
	margin-top: 40upx;
}
.dlbutton-hover {
	background: linear-gradient(-90deg, rgba(63, 205, 235, 0.9), rgba(188, 226, 158, 0.9));
}
</style>
