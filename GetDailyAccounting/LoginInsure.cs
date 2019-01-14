using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
            this._hhloginresult = new HttpResult();
            HttpHelper httphelper = new HttpHelper();
            this.user = user;
            string strloginname = user.UserId;
            string strloginpwd = user.UserPwd;
            string strlongincomcode = user.UserComCode;
            string ipAddress = user.UserIpAddress;
            string strqrcode = user.UserValidateCode;
            HttpItem hiloginItem = new HttpItem();


            //获取登录页面的cookie http://9.0.9.11/workbench/workbench/login.html
            hiloginItem.URL = string.Format("http://{0}/workbench/workbench/login.html", ipAddress);//URL这里都是测试     必需项
            //hiloginItem.URL = "http://9.0.6.69:7001/prpall/index.jsp";//URL这里都是测试     必需项
            //hiloginItem.Referer = "http://9.0.6.69:7031/claimCar/logonin.do";

            hiloginItem.ContentType = "application/x-www-form-urlencoded";
            
            //hiloginItem.Accept = "image/gif, image/x-xbitmap, image/jpeg, image/pjpeg, application/x-shockwave-flash, application/x-silverlight, application/vnd.ms-excel, application/vnd.ms-powerpoint, application/msword, application/x-ms-application, application/x-ms-xbap, application/vnd.ms-xpsdocument, application/xaml+xml, application/x-silverlight-2-b1, *";
            //hiloginItem.UserAgent = "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 5.1; .NET CLR 2.0.50727; .NET CLR 3.0.04506.648; .NET CLR 3.5.21022)";
            //hiloginItem.Encoding = null;//编码格式（utf-8,gb2312,gbk）     可选项 默认类会自动识别
            //Encoding = Encoding.Default,
            hiloginItem.Method = "Post";//URL     可选项 默认为Get
                                        //hiloginItem.Postdata = string.Format("loginMessage=&alertText=&prpDuserUserCode={0}&prpDuserPassword={1}&prpDuserComCode=0000000000&imageField.x=44&imageField.y=19", loginname, loginpwd);
                                        //hiloginItem.Postdata = string.Format("alertText=&prpDuserUserCode={0}&prpDuserPassword={1}&prpDuserComCode=0000000000&loginSyncRTXFlag=0&imageField.x=38&imageField.y=13", strloginname, strloginpwd);
            hiloginItem.Cookie = "";
            this.Hhloginresult = httphelper.GetHtml(hiloginItem);
            this.cookiealltheway = this.Hhloginresult.Cookie;
            //用户名密码登录
            ///workbench/controller/workbench/login/login?redUrl=
            hiloginItem.URL = string.Format("http://{0}/workbench/controller/workbench/login/login?redUrl=", ipAddress);
            
            //strloginpwd = System.Web.HttpUtility.UrlEncode(strloginpwd);
            //sessionUserCode=&sessionComCode=&sessionUserName=&QRCodeSwitch=1&UserCode=411123199004234524&Password=0.0.0.0.&ComCode=4101943202&qrCode=909647&RiskCode=0511&ClassCode=&ClassCodeSelect=05&RiskCodeSelect=0511&USE0509COM=%2C12%2C&CILIFESPECIALCITY=%2C2102%2C3302%2C3502%2C3702%2C4402%2C&image.x=41&image.y=12
            hiloginItem.Postdata = string.Format(JsonConvert.SerializeObject(user));
            hiloginItem.ContentType = "application/json";
           // hiloginItem.Cookie = this.cookiealltheway;
            this.Hhloginresult = httphelper.GetHtml(hiloginItem);


            this.cookiealltheway = this.Hhloginresult.Cookie;
            var responseJson = JsonConvert.DeserializeObject(Hhloginresult.Html) as JObject;
            string isRightUser= (string) responseJson["flag"];
            string redurl = (string)responseJson["redUrl"];
            string systemUserId = (string) responseJson["userId"];

            if (isRightUser == "true")
            {
                // /workbench/controller/workbench/login/authenticationTools?userId=89979dc56021684301602176c9a83323&redUrl=
                hiloginItem.URL = string.Format("http://{0}/workbench/controller/workbench/login/authenticationTools?userId={1}&redUrl=",ipAddress,systemUserId);
                hiloginItem.ContentType = "application/x-www-form-urlencoded";
                Hhloginresult = httphelper.GetHtml(hiloginItem);
                this.cookiealltheway = Hhloginresult.Cookie;
                string location=  Hhloginresult.Header["Location"];

                hiloginItem.URL = location;
                Hhloginresult = httphelper.GetHtml(hiloginItem);
                this.cookieSSO = Hhloginresult.Cookie;
                Hhloginresult.Html

            }
            return this._islogin = !this._hhloginresult.Html.Contains("302 Moved Temporarily");
        }
    }
}
