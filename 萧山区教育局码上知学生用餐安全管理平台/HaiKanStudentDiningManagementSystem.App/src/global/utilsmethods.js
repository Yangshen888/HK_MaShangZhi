// 时间格式的转换
function Time(value) {
  if(value==undefined||value==null||value=="")
    return "";
  var dateee = new Date(value).toJSON();
  var date = new Date(+new Date(dateee)+8*3600*1000).toISOString().replace(/T/g,' ').replace(/\.[\d]{3}Z/,'')
  return date
}
export default {
  Time
}
