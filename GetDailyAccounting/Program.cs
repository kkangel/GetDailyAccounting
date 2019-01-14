using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CsharpHttpHelper;
using Newtonsoft.Json;

namespace GetDailyAccounting
{
    class Program
    {
        static void Main(string[] args)
        {
            //互动二部樊荣 410105198709140028-  123456-
            string userIpAddress = "9.0.9.11";
            string userId = "410105198709140028";
            string userPwd = "123456";
            string userCom = "";
            string userValidateCode = "";


            User hd2user = new User(userId,userPwd,userCom,userIpAddress,userValidateCode);
            Console.WriteLine( JsonConvert.SerializeObject(hd2user));
            Console.ReadLine();
        }
    }
}
