using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using CsharpHttpHelper;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

//using System.Web.HttpUtility;

namespace GetDailyAccounting
{
    class LoginInsure
    {
        private bool _islogin;//登录状态
        private string _cookiealltheway;//新收付平台用的cookie
        private string _cookieSSO;//uam用的sso的cookie
        private HttpResult _hhloginresult;
        public HttpResult Hhloginresult
        {
            get
            {
                return this._hhloginresult;
            }
            set
            {
                this._hhloginresult = value;
            }
        }
        public bool islogin
        {
            get
            {
                return this._islogin;
            }
            set
            {
                this._islogin = value;
            }
        }
        public string cookiealltheway
        {
            get
            {
                return this._cookiealltheway;
            }
            set
            {
                this._cookiealltheway = value;
            }
        }

        public string cookieSSO
        {
            get { return this._cookieSSO; }
            set { this._cookieSSO = value; }
        }

        public User  user { get; set; }
        //1 先访问login.html获得cookie
        //    2 请求/workbench/controller/workbench/login/login? redUrl =
        public bool login(User user)
        {
            Hhloginresult = new HttpResult();
            HttpHelper httphelper = new HttpHelper();
            islogin = false;
            this.user = user;
            string strloginname = user.UserId;
            string strloginpwd = user.UserPwd;
            string strlongincomcode = user.UserComCode;
            string ipAddress = user.UserIpAddress;
            string strqrcode = user.UserValidateCode;
            HttpItem hiloginItem = new HttpItem();


            //1 获取登录页面的cookie http://9.0.9.11/workbench/workbench/login.html
            hiloginItem.URL = string.Format("http://{0}/workbench/workbench/login.html", ipAddress);

            hiloginItem.ContentType = "application/x-www-form-urlencoded";
            
            
            hiloginItem.Method = "Post";
            hiloginItem.Cookie = "";
            this.Hhloginresult = httphelper.GetHtml(hiloginItem);
            this.cookiealltheway = this.Hhloginresult.Cookie;
            cookiealltheway = Regex.Replace(cookiealltheway, "path=/[,;]*", "",RegexOptions.IgnoreCase);
            cookiealltheway = Regex.Replace(cookiealltheway, "httponly", "", RegexOptions.IgnoreCase);
            //2 用户名密码登录
            ///workbench/controller/workbench/login/login?redUrl=
            hiloginItem.URL = string.Format("http://{0}/workbench/controller/workbench/login/login?redUrl=", ipAddress);
            string urlFirst = hiloginItem.URL;
            hiloginItem.Postdata =JsonConvert.SerializeObject(user);
            hiloginItem.Cookie = cookiealltheway;
            hiloginItem.ContentType = "application/json";
            hiloginItem.Header.Add("Origin", "http://9.0.9.11");
            this.Hhloginresult = httphelper.GetHtml(hiloginItem);
            //this.cookiealltheway = this.Hhloginresult.Cookie;

            var responseJson = JsonConvert.DeserializeObject(Hhloginresult.Html) as JObject;
            bool isRightUser= (bool) responseJson["flag"];
            string redurl = (string)responseJson["redUrl"];
            string systemUserId = (string) responseJson["userId"];

            if (isRightUser == true)
            {
                // /workbench/controller/workbench/login/authenticationTools?userId=89979dc56021684301602176c9a83323&redUrl=
                //3 获取uam地址及返回的requestToken
                hiloginItem.URL = string.Format("http://{0}/workbench/controller/workbench/login/authenticationTools?userId={1}&redUrl=",ipAddress,systemUserId);
                hiloginItem.Referer = "http://9.0.9.11/workbench/workbench/login.html";
                hiloginItem.ContentType = "application/x-www-form-urlencoded";
                hiloginItem.Cookie = cookiealltheway;
                hiloginItem.Header.Add("Origin", "http://9.0.9.11");
                Hhloginresult = httphelper.GetHtml(hiloginItem);
                //this.cookiealltheway = Hhloginresult.Cookie;
                string location=  Hhloginresult.Header["Location"];
                string requestToken = location.Split('?')[1];
                
                //4 访问uam地址 获取uam的cookie
                hiloginItem.URL = location;
                hiloginItem.Accept ="text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8";
                hiloginItem.Referer = "http://9.0.9.11/workbench/workbench/login.html";
                hiloginItem.UserAgent ="Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/61.0.3163.79 Safari/537.36 Maxthon/5.2.5.4000";

                hiloginItem.Cookie = "";
                Hhloginresult = httphelper.GetHtml(hiloginItem);
                this.cookieSSO = Hhloginresult.Cookie;
                cookieSSO = Regex.Replace(cookieSSO, "path=/[,;]*", "", RegexOptions.IgnoreCase);
                cookieSSO = Regex.Replace(cookieSSO, "httponly[,;]*", "", RegexOptions.IgnoreCase);
                cookieSSO = Regex.Replace(cookieSSO, @"domain=.gpic.com.cn[,;]*", "", RegexOptions.IgnoreCase);
                //HtmlDocument uamHtmlDocument = new HtmlDocument();
                //uamHtmlDocument.LoadHtml(Hhloginresult.Html);

                //var requestTokenNode = uamHtmlDocument.DocumentNode.SelectSingleNode("//input[@name='requestToken']");
                //string requestToken=  requestTokenNode.GetAttributeValue("value","");

                //5 访问uam/sso获得responseToken 返回工作台
                hiloginItem.URL = string.Format("http://9.0.8.93/UAM/SSO");
                hiloginItem.Postdata = string.Format(requestToken + "&redUrlnew=redUrlnew");
                hiloginItem.Cookie = cookieSSO;
                hiloginItem.Referer = location;
                hiloginItem.Method = "post";
                hiloginItem.Header.Add("Origin", "http://uam.gpic.com.cn");
                hiloginItem.ContentType = "application/x-www-form-urlencoded";
                Hhloginresult = httphelper.GetHtml(hiloginItem);
                
                HtmlDocument ssoHtmlDocument= new HtmlDocument();
                ssoHtmlDocument.LoadHtml(Hhloginresult.Html);

                var responseTokenNode = ssoHtmlDocument.DocumentNode.SelectSingleNode("//input[@name='responseToken']");
                string responseToken = responseTokenNode.GetAttributeValue("value", "");
                responseToken =
                    System.Web.HttpUtility.UrlEncode(responseToken, System.Text.Encoding.GetEncoding("GB2312"));
                var formActionNode = ssoHtmlDocument.DocumentNode.SelectSingleNode("//form[@name='submitForm']");
                string actionUrl = formActionNode.GetAttributeValue("action", "");
                //6 访问 /workbench/cas/login?param=null 返回控制台index页面
                hiloginItem.URL = actionUrl;
                hiloginItem.Cookie = cookiealltheway;
                hiloginItem.ContentType = "application/x-www-form-urlencoded";
                hiloginItem.Header.Add("Origin", "http://uam.gpic.com.cn");
                hiloginItem.Method = "post";
                hiloginItem.Referer = "http://uam.gpic.com.cn/UAM/SSO";
                
                hiloginItem.Postdata = string.Format("responseToken=" + responseToken + "&redUrl=");
                Hhloginresult = httphelper.GetHtml(hiloginItem);

            }
            return islogin = Hhloginresult.Html.Contains("/workbench/workbench/index.html");
        }
    }
}
