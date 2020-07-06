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
using AccountManager.DB;

namespace AccountManager.Controllers.Tests
{
    [TestClass()]
    public class RoleControllerTests
    {
        private string address = "https://localhost:44337";
        private HttpClient client;
        public RoleControllerTests()
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
                var response = await client.GetAsync("api/role");
                var result = await response.Content.ReadAsAsync<List<Role>>();
                DBContext context = new DBContext();
                Assert.AreEqual(3, result.Count);

                response = await client.GetAsync("/api/role/00385bc1-56f7-40a0-bc90-2544c9b032cf");
                var result1 = await response.Content.ReadAsAsync<Result<Role>>();
                Assert.IsTrue(result1.Success);

                response = await client.GetAsync("/api/role/666666");
                result1 = await response.Content.ReadAsAsync<Result<Role>>();
                Assert.IsFalse(result1.Success);

                response = await client.GetAsync("/api/role?roleName=班长");
                result1 = await response.Content.ReadAsAsync<Result<Role>>();
                Assert.IsTrue(result1.Success);
            }
        }

        /// <summary>
        /// roleName 重复
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PostTestAsync()
        {
            Role model = new Role
            {
                RoleName="admin",
                RoleLevel=1,
                CreateBy = "PostTest"
            };
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PostAsJsonAsync("/api/role", model);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Result<int>>();
                    Assert.IsFalse(result.Success);
                    Assert.AreEqual(ConstInfo.ERR_AlreadyHave_Role, result.Message);
                }
                else
                {
                    var str = await response.Content.ReadAsStringAsync();
                    Assert.Fail(str);
                }
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PostAddTestAsync()
        {
            string RoleName = "admin"+new Random().Next(199).ToString();
            Role model = new Role
            {
                RoleName = RoleName,
                RoleLevel = 2,
                CreateBy = "PostTest"
            };
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PostAsJsonAsync("/api/role", model);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Result<int>>();
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(1, result.Data);
                }
                else
                {
                    var str = await response.Content.ReadAsStringAsync();
                    Assert.Fail(str);
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PutTestAsync()
        {
            //正常流程 id修改
            string RoleId = "00385bc1-56f7-40a0-bc90-2544c9b032cf";
            Role model = new Role
            {
                RoleName = "班长",
                RoleLevel = 1,
                LastUpdateBy = "PostTest"
            };
            Result<int> result = new Result<int>();
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PutAsJsonAsync($"/api/role/{RoleId}", model);
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);

                //null值测试
                model = null;
                response = await client.PutAsJsonAsync($"/api/role/{RoleId}", model);
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);

                //不存在测试
                RoleId = "13466666666";
                response = await client.PutAsJsonAsync($"/api/role/{RoleId}", model);
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsFalse(result.Success);
                Assert.AreEqual(ConstInfo.ERR_NotFound_Role, result.Message);

                
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task DeleteTestAsync()
        {
            string Id = "4f85dfd2-d0ec-4e24-bd35-d68aa666c136";
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.DeleteAsync($"/api/role/{Id}");
                var result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);

                Id = "aaa";
                response = await client.DeleteAsync($"/api/role/{Id}");
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);
            }
        }
    }
}