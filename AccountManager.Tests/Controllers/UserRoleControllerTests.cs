using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountManager.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using Infrastructure;
using AccountManager.DB.Entity;
using System.Diagnostics;

namespace AccountManager.Controllers.Tests
{
    [TestClass()]
    public class UserRoleControllerTests
    {
        private string address = "https://localhost:44337";
        private HttpClient client;
        public UserRoleControllerTests()
        {
            client = new HttpClient { BaseAddress = new Uri(address) };
        }

        /// <summary>
        /// get test
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetTestAsync()
        {
            using (WebApp.Start<Startup>(address))
            {
               
                var response = await client.GetAsync("/api/UserRole?userId=5af39f5c-4f54-43a6-add9-fd0f5d3544cf");
                var result = await response.Content.ReadAsAsync<List<Role>>();
                Assert.AreEqual(1, result.Count);

                response = await client.GetAsync("/api/UserRole/?roleId=00385bc1-56f7-40a0-bc90-2544c9b032cf");
                var result1 = await response.Content.ReadAsAsync<List<User>>();
                Assert.AreEqual(1, result1.Count);

               
            }
        }

        /// <summary>
        ///  重复 外键约束
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PostTestAsync()
        {
            UserRole model = new UserRole
            {
                UserId = "5af39f5c-4f54-43a6-add9-fd0f5d3544cf",
                RoleId = "00385bc1-56f7-40a0-bc90-2544c9b032cf",
                CreateBy = "PostTest"
            };
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PostAsJsonAsync("/api/UserRole", model);
                var result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsFalse(result.Success);
                Assert.AreEqual(ConstInfo.ERR_AlreadyHave, result.Message);


                //外键限制
                model.UserId = "aaa";
                response = await client.PostAsJsonAsync("/api/UserRole", model);
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsFalse(result.Success);
                Trace.WriteLine("==========================="+result.Message);
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PostAddTestAsync()
        {
            UserRole model = new UserRole
            {
                UserId = "5af39f5c-4f54-43a6-add9-fd0f5d3544cf",
                RoleId = "00385bc1-56f7-40a0-bc90-2544c9b032cf",
                CreateBy = "PostTest"
            };
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PostAsJsonAsync("/api/UserRole", model);
                var result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);
                Assert.AreEqual(1, result.Data);

            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task DeleteTestAsync()
        {
            string Id = "dd3a942d-2596-4044-a7f2-e3710d91dfbe";
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.DeleteAsync($"/api/UserRole/{Id}");
                var result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);
            }
        }
    }
}