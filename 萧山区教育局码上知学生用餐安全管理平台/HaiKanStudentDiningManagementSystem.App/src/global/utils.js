//时间-> yyyy-MM-dd
function DateToString(date) {
  if (date != null) {
    let day=date.getDate();
    let month=date.getMonth()+1;
    if(day<10){
      day="0"+day;
    }
    if(month<10){
      month="0"+month;
    }
    return date.getFullYear() + '-' + month + '-' + day;
  } else {
    date = new Date();
    let day=date.getDate();
    let month=date.getMonth()+1;
    if(day<10){
      day="0"+day;
    }
    if(month<10){
      month="0"+month;
    }
    return date.getFullYear() + '-' + month + '-' + day;
  }
}

export {
  DateToString,
}