import http from '@/utils/http.js'

export const  checkusertoken =async function(){
	let token = "";
	uni.getStorage({
	key: 'token',
	success: function(ress) {
	token = ress.data
	}});
	if(this.$store.state.hasLogin==false)
	{
		if(token!=""&&token!=null)
	   {
		await http.httpTokenRequest({url:'api/v1/account/profile',method:'get'}).then(res=>{
			if(res.data.code==200)
			{
				let user ={
					userName:res.data.data.user_name,
					userId:res.data.data.user_guid,
					userType:res.data.data.user_type,
					DepartmentName:res.data.data.user_departmentName,
		            DepartmentGuid:res.data.data.user_departmentGuid,
					RoleName:res.data.data.user_roleName,
					token:token
				};
				this.$store.commit('login', user);
			}
			else
			{
				uni.showModal({
					title:'提示',
					content:res.data.message,
					success() {
						uni.reLaunch({
							url:'../login/login'
						})
					}
				})
			}
		}).catch(res=>{
							uni.showModal({
								title: '提示',
								content: '网络服务报错，请检查您的网络'
							});
					})
		}
		else
		{
			uni.reLaunch({
				url:'../login/login'
			})
		}
	}
	
}
export default checkusertoken