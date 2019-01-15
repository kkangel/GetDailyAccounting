using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetDailyAccounting
{


    public class CurrentLoginStructure
    {
        public string structureId { get; set; }
        public string structureName { get; set; }
        public string structurePath { get; set; }
    }

    public class Role
    {
        public string ownerUserId { get; set; }
        public string createUserId { get; set; }
        public string createDate { get; set; }
        public string updateUserId { get; set; }
        public string updateDate { get; set; }
        public string sequenceId { get; set; }
        public string roleId { get; set; }
        public string kind { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string approvalLevel { get; set; }
        public string category { get; set; }
        public string typeId { get; set; }
        public string roleLevel { get; set; }
        public string businessType { get; set; }
        public string sourcePath { get; set; }
        public string ownedStructureId { get; set; }
        public string ownedStructureName { get; set; }
        public string ownedStructurePath { get; set; }
    }

    public class Actors
    {
        public List<Role> Role { get; set; }
    }

    public class RbacUserProfile
    {
        public string ownerUserId { get; set; }
        public string createUserId { get; set; }
        public string createDate { get; set; }
        public string updateUserId { get; set; }
        public string updateDate { get; set; }
        public string userId { get; set; }
        public string loginCount { get; set; }
    }

    public class LoginStructures
    {
        public string structureId { get; set; }
        public string structureName { get; set; }
        public string structurePath { get; set; }
    }

    public class Rbac_loginStructure
    {
        public string ownerUserId { get; set; }
        public string createUserId { get; set; }
        public string createDate { get; set; }
        public string updateUserId { get; set; }
        public string updateDate { get; set; }
        public string sequenceId { get; set; }
        public string userId { get; set; }
        public string loginName { get; set; }
        public string name { get; set; }
        public string status { get; set; }
        public string startDate { get; set; }
        public string endDate { get; set; }
        public string identifyType { get; set; }
        public string identifyNumber { get; set; }
        public string ownedStructureId { get; set; }
        public string ownedStructureName { get; set; }
        public string ownedStructurePath { get; set; }
        public string employeeId { get; set; }
        public CurrentLoginStructure currentLoginStructure { get; set; }
        public Actors actors { get; set; }
        public RbacUserProfile rbacUserProfile { get; set; }
        public List<LoginStructures> loginStructures { get; set; }
    }

    public class UserInfoVO信息
    {
        public string userId { get; set; }
        public string loginStructure { get; set; }
        public string onlineStatus { get; set; }
        public string userName { get; set; }
        public string loginName { get; set; }
        public string company { get; set; }
        public string leaveCause { get; set; }
        public string leaveTimeHour { get; set; }
        public string leaveTimeMinutes { get; set; }
        public string jobNumber { get; set; }
        public string lastLoginDate { get; set; }
        public string insideAndOutside { get; set; }
        public string locale { get; set; }
        public string pageTabCount { get; set; }
        public string refreshCycle { get; set; }
    }

    public class SessionInfoByUserId
    {
        public string session信息 { get; set; }
        public Rbac_loginStructure rbac_loginStructure { get; set; }
        public string loginStructureId { get; set; }
        public UserInfoVO信息 UserInfoVO信息 { get; set; }
        public string sessionId { get; set; }
        public string userName { get; set; }
        public string userId { get; set; }
    }

}
