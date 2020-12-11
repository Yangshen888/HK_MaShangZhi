using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HaiKan3.Utils
{
    public static class Electroniccode
    {
        /// <summary>  
        /// 将c# DateTime时间格式转换为Unix时间戳格式  
        /// </summary>  
        /// <param name="time">时间</param>  
        /// <returns>long</returns>  
        public static int ConvertDateTimeToInt(System.DateTime time)
        {
            System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
            return (int)(time - startTime).TotalSeconds;
        }
        public static string getString(int count)
        {
            int number;
            string checkCode = String.Empty;     //存放随机码的字符串 

            System.Random random = new Random();

            for (int i = 0; i < count; i++) //产生4位校验码 
            {
                number = random.Next();
                number = number % 36;
                if (number < 10)
                {
                    number += 48;    //数字0-9编码在48-57 
                }
                else
                {
                    number += 55;    //字母A-Z编码在65-90 
                }

                checkCode += ((char)number).ToString();
            }
            return checkCode;
        }
        /// <summary>
        /// 签名算法，ASCII码字典序排序0,A,B,a,b
        /// </summary>
        /// <param name="InDict">待签名名键值对</param>
        /// <param name="TenPayV3_Key">用于签名的Key</param>
        /// <returns>MD5签名字符串</returns>
        public static string Sign(Dictionary<string, string> InDict, string TenPayV3_Key)
        {
            string[] arrKeys = InDict.Keys.ToArray();
            Array.Sort(arrKeys, string.CompareOrdinal);  //参数名ASCII码从小到大排序；0,A,B,a,b;

            var StrA = new StringBuilder();

            foreach (var key in arrKeys)
            {
                string value = InDict[key];
                if (!String.IsNullOrEmpty(value)) //空值不参与签名
                {
                    StrA.Append(key + "=")
                       .Append(value + "&");
                }
            }

            //foreach (var item in InDict.OrderBy(x => x.Key))//参数名字典序；0,A,a,B,b;
            //{
            //    if(!String.IsNullOrEmpty(item.Value)) //空值不参与签名
            //    {
            //        StrA.Append(item.Key + "=")
            //           .Append(item.Value + "&");
            //    }
            //}

            StrA.Append("key=" + TenPayV3_Key); //注：key为商户平台设置的密钥key
            var s = getMd5(StrA.ToString());
            return s.ToUpper();
        }
        //创建getMd5方法以获得userPwd的Md5值
        public static string getMd5(string str)
        {

            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X2");
            }
            return pwd;
        }
    }
}
