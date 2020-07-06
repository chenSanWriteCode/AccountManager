using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using SqlSugar;

namespace AccountManager.DB
{
    public class BaseEntity
    {
        [SugarColumn(IsPrimaryKey = true, IsNullable = false)]
        public string Id { get; set; }
        public bool ActivityFlag { get; set; } = true;
        public DateTime? CreateTime { get; set; } = DateTime.Now;
        [SugarColumn(IsNullable = true)]
        public string CreateBy { get; set; }
        public DateTime? LastUpdateTime { get; set; } = DateTime.Now;
        [SugarColumn(IsNullable = true)]
        public string LastUpdateBy { get; set; }
    }
}