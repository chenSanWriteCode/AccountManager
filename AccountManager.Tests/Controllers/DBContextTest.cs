using System;
using System.Collections.Generic;
using AccountManager.DB;
using AccountManager.DB.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AccountManager.Tests.Controllers
{
    [TestClass]
    public class DBContextTest
    {
        [TestMethod]
        public void TestInitTable()
        {
            try
            {
                DBContext context = new DBContext();
                List<Type> list = new List<Type>() { typeof(User), typeof(Role), typeof(UserRole), typeof(RoleRight), typeof(Right) };
                context.InitTables(list);
            }
            catch (Exception err)
            {
                throw;
            }

        }
    }
}
