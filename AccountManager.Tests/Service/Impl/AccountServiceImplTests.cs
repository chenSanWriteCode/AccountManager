using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountManager.Service.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AccountManager.Service.Impl.Tests
{
    [TestClass()]
    public class AccountServiceImplTests
    {
        [TestMethod()]
        public void LoginTest()
        {
            AccountServiceImpl service = new AccountServiceImpl();
            var result = service.LoginByNickName("亓登海", "hai123456");
            Trace.WriteLine("======================" + (result.Success ? result.Data.ExpireTime + result.Data.Token : result.Message));
            var info = service.Validata(result.Data.Token);
            Trace.WriteLine("======================" + info);
            //1594033697
        }
    }
}