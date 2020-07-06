using Microsoft.VisualStudio.TestTools.UnitTesting;
using AccountManager.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Owin.Hosting;
using AccountManager.DB.Entity;
using Infrastructure;
using System.Diagnostics;

namespace AccountManager.Controllers.Tests
{
    [TestClass()]
    public class RoleRightControllerTests
    {
        private string address = "https://localhost:44337";
        private HttpClient client;
        public RoleRightControllerTests()
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

                var response = await client.GetAsync("/api/RoleRight?userId=5af39f5c-4f54-43a6-add9-fd0f5d3544cf");
                var result = await response.Content.ReadAsAsync<List<Right>>();
                Assert.AreEqual(2, result.Count);

                response = await client.GetAsync("/api/RoleRight/?roleId=00385bc1-56f7-40a0-bc90-2544c9b032cf");
                var result1 = await response.Content.ReadAsAsync<List<Right>>();
                Assert.AreEqual(2, result1.Count);

            }
        }

        /// <summary>
        ///  重复 外键约束
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PostTestAsync()
        {
            RoleRight model = new RoleRight
            {
                RightData = "42a7ae7b-8e65-46b4-94d4-bb663f6d33df",
                RoleId = "00385bc1-56f7-40a0-bc90-2544c9b032cf",
                CreateBy = "PostTest"
            };
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PostAsJsonAsync("/api/RoleRight", model);
                var result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsFalse(result.Success);
                Assert.AreEqual(ConstInfo.ERR_AlreadyHave, result.Message);


                //外键限制
                model.RightData = "aaa";
                response = await client.PostAsJsonAsync("/api/RoleRight", model);
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsFalse(result.Success);
                Trace.WriteLine("===========================" + result.Message);
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PostAddTestAsync()
        {
            RoleRight model = new RoleRight
            {
                RightData = "e0dd1c95-0e22-4393-ba9a-95f7a614d052",
                RoleId = "00385bc1-56f7-40a0-bc90-2544c9b032cf",
                CreateBy = "PostTest"
            };
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PostAsJsonAsync("/api/RoleRight", model);
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
            string Id = "eb117c87-5545-4f65-9c9f-f64cb7d0c15c";
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.DeleteAsync($"/api/RoleRight/{Id}");
                var result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);
            }
        }
    }
}