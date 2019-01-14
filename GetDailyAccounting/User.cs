using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using  Newtonsoft.Json;

namespace GetDailyAccounting
{
    class User
    {
        [JsonProperty(PropertyName = "userPwd")]
        public string UserPwd { get; set; }
        [JsonProperty(PropertyName = "userName")]
        public string UserId { get; set; }
        [JsonIgnore]
        public string UserComCode { get; set; }
        [JsonIgnore]
        public string UserIpAddress { get; set; }
        [JsonIgnore]
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
