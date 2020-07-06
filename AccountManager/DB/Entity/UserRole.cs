using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace AccountManager.DB.Entity
{
    [SugarTable("Web_UserRole")]
    public class UserRole : BaseEntity
    {
        public string UserId { get; set; }
        public string RoleId { get; set; }
    }
}