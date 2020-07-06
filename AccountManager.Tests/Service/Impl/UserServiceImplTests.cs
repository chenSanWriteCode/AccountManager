using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountManager.Service.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccountManager.DB.Entity;

namespace AccountManager.Service.Impl.Tests
{
    [TestClass()]
    public class UserServiceImplTests
    {
        UserServiceImpl service = new UserServiceImpl();
        [TestMethod()]
        public void GetByNamePasswTest()
        {
            var result = service.GetByNamePassw("aa", "123");
            Assert.IsNull(result);
        }

        [TestMethod()]
        public void GetByPhoneNumTest()
        {
            var reuslt = service.GetByPhoneNum("6873");
            Assert.IsNull(reuslt);
        }

        [TestMethod()]
        public void UpdatePasswordTest()
        {
            User user = new User
            {
                Password = "123456",
                LastUpdateBy = "userServiceTest"
            };
            var result = service.Update("6873", user);
            Assert.IsTrue(result.Success);
        }
    }
}