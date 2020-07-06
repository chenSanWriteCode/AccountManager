using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace AccountManager.DB.Entity
{
    [SugarTable("Web_RoleRight")]
    public class RoleRight : BaseEntity
    {
        public string RoleId { get; set; }
        public string RightData { get; set; }
    }
}