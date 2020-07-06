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

namespace AccountManager.Controllers.Tests
{
    [TestClass()]
    public class RightControllerTests
    {
        private string address = "https://localhost:44337";
        private HttpClient client;
        public RightControllerTests()
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
                var response = await client.GetAsync("api/right");
                var result = await response.Content.ReadAsAsync<List<Right>>();
                Assert.AreEqual(4, result.Count);

                response = await client.GetAsync("/api/right/42a7ae7b-8e65-46b4-94d4-bb663f6d33df");
                var result1 = await response.Content.ReadAsAsync<Result<Right>>();
                Assert.IsTrue(result1.Success);

                response = await client.GetAsync("/api/right/666666");
                result1 = await response.Content.ReadAsAsync<Result<Right>>();
                Assert.IsFalse(result1.Success);

                response = await client.GetAsync("/api/right?title=第");
                result = await response.Content.ReadAsAsync<List<Right>>();
                Assert.AreEqual(3, result.Count);
            }
        }

        /// <summary>
        /// title 重复
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PostTestAsync()
        {
            Right model = new Right
            {
                Parent = 1,
                Href = "/Home",
                Title = "目录",
                CreateBy = "PostTest"
            };
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PostAsJsonAsync("/api/right", model);
                var result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsFalse(result.Success);
                Assert.AreEqual(ConstInfo.ERR_AlreadyHave_Right, result.Message);
            }
        }
        /// <summary>
        /// 新增
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PostAddTestAsync()
        {
            string title = "第" + new Random().Next(199).ToString()+"条";
            Right model = new Right
            {
                Parent = 1,
                Href = "/Value/Index",
                Title = title,
                CreateBy = "PostTest"
            };
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PostAsJsonAsync("/api/right", model);
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
            string id = "42a7ae7b-8e65-46b4-94d4-bb663f6d33df";
            Right model = new Right
            {
                Parent = 0,
                Href = "/Home",
                Title = "目录",
                LastUpdateBy = "PutTest"
            };
            Result<int> result = new Result<int>();
            using (WebApp.Start<Startup>(address))
            {
                var response = await client.PutAsJsonAsync($"/api/right/{id}", model);
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);

                //null值测试
                model = null;
               response = await client.PutAsJsonAsync($"/api/right/{id}", model);
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);

                //不存在测试
                id = "13466666666";
                response = await client.PutAsJsonAsync($"/api/right/{id}", model);
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsFalse(result.Success);
                Assert.AreEqual(ConstInfo.ERR_NotFound, result.Message);


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
                var response = await client.DeleteAsync($"/api/right/{Id}");
                var result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);

                Id = "aaa";
                response = await client.DeleteAsync($"/api/right/{Id}");
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);
            }
        }
    }
}