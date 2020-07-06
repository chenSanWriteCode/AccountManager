using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AccountManager.DB.Entity;

namespace AccountManager.Models
{
    public class UserRoleModel
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
        public string UserName { get; set; }
        public string NickName { get; set; }
        public string PhoneNum { get; set; }
        public int? VIPLevel { get; set; } = 1;
        public string RoleName { get; set; }
        public int RoleLevel { get; set; }
    }
}