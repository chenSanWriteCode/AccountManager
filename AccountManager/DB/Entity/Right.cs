﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SqlSugar;

namespace AccountManager.DB.Entity
{
    [SugarTable("Web_Right")]
    public class Right : BaseEntity
    {
        public int Parent { get; set; } = 0;
        public string Href { get; set; }
        public string Title { get; set; }

    }
}