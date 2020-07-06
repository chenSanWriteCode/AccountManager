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
    public class RoleServiceImplTests
    {
        private IRoleService Service = new RoleServiceImpl();
        [TestMethod()]
        public async Task GetByIdTestAsync()
        {
            var result = await Service.GetByIdAsync("aaa");
            Assert.IsNull(result);
        }
    }
}