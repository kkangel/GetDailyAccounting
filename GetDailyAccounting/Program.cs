using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using CsharpHttpHelper;
using Newtonsoft.Json;

namespace GetDailyAccounting
{
    class Program
    {
        static void Main(string[] args)
        {

            string cookiealltheway;
            //互动二部樊荣 410105198709140028-  123456-
            string userIpAddress = "9.0.9.11";
            string userId = "410105198709140028";
            //string userPwd = "Fan123123";
            string userPwd = "fanrong0908";
            string userCom = "";
            string userValidateCode = "";


            User hd2user = new User(userId,userPwd,userCom,userIpAddress,userValidateCode);
            LoginInsure login= new LoginInsure();
            bool islogin = login.login(hd2user);
            if (islogin == true)
            {

                cookiealltheway = login.cookiealltheway;
                Console.WriteLine("用户:{0} 登录成功",userId);
                Console.Write("请输入要查询缴费清单的起始日期（YYYY-MM-DD）：");
                string strDateStart = Console.ReadLine();
                //string strDateStart = "2019-1-1";
                DateTime dtDateStart = Convert.ToDateTime(strDateStart);
                Console.Write("请输入要查询缴费清单的截止日期（YYYY-MM-DD）：");
                string strDateEnd = Console.ReadLine();
                //string strDateEnd = "2019-1-1";
                DateTime dtDateEnd = Convert.ToDateTime(strDateEnd);
                GetPayDailyList payDailyList =new GetPayDailyList(dtDateStart,dtDateEnd);
                payDailyList.CookieAllTheWay = cookiealltheway;
                payDailyList.GetList();
            }

            Console.WriteLine("数据提取完毕");
            Console.ReadLine();
        }
    }
}
