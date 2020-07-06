using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountManager.Service.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccountManager.Service.Impl.Tests
{
    [TestClass()]
    public class RoleRightServiceImplTests
    {
        [TestMethod()]
        public void GetByRoleIdTest()
        {
            RoleRightServiceImpl service = new RoleRightServiceImpl();
            var result = service.GetByRoleId("aa", 1, 10);
            Assert.AreEqual(result.Count, 0);
            
        }

        [TestMethod()]
        public void GetByUserIdTest()
        {
            RoleRightServiceImpl service = new RoleRightServiceImpl();
            var result = service.GetByUserId("aa");
            Assert.AreEqual(result.Count, 0);
        }
    }
}