using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDailyAccounting
{
    class User
    {
        public string UserPwd { get; set; }
        public string UserId { get; set; }
        public string UserComCode { get; set; }
        public string UserIpAddress { get; set; }

        public  string UserValidateCode { get; set; }

        public User(string userid, string userpwd, string usercomcode, string userIpAddress, string userValidateCode )
        {
            UserId = userid;
            UserPwd = userpwd;
            UserComCode = usercomcode;
            UserIpAddress = userIpAddress;
            UserValidateCode = userValidateCode;
        }
    }
}
