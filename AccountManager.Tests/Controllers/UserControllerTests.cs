using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AccountManager.DB.Entity;
using Infrastructure;
using Microsoft.Owin.Hosting;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Owin;

namespace AccountManager.Controllers.Tests
{
    [TestClass()]
    public class UserControllerTests
    {
        private string address = "https://localhost:44337";
        private HttpClient client;
        public UserControllerTests()
        {
            client = new HttpClient { BaseAddress = new Uri(address) };
        }
        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            config.DependencyResolver = UnityWebApiActivator.GetDependencyResolver();
            //UnityWebApiActivator.Start();
            app.UseWebApi(config);
        }
        /// <summary>
        /// get test
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task GetTestAsync()
        {
            using (WebApp.Start(address, Configuration))
            {
                var response = await client.GetAsync("api/user");
                var result =await response.Content.ReadAsAsync<List<User>>();
                Assert.AreEqual(2, result.Count);

                response = await client.GetAsync("/api/user/64b0b681-3cb7-4a03-85dc-7f873e4cb459");
                var result1 = await response.Content.ReadAsAsync<Result<User>>();
                Assert.IsTrue(result1.Success);

                response = await client.GetAsync("/api/user/64b0b681-3cb7-4a03-85dc-7f873e4cb4591111");
                result1 = await response.Content.ReadAsAsync<Result<User>>();
                Assert.IsFalse(result1.Success);

                response = await client.GetAsync("/api/user?phoneNum=18615066873");
                result1 = await response.Content.ReadAsAsync<Result<User>>();
                Assert.IsTrue(result1.Success);
            }
        }

        /// <summary>
        /// phoneNum 重复
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task PostTestAsync()
        {
            User model = new User
            {
                NickName = "米老鼠",
                UserName = "Tom",
                Password = "12345",
                PhoneNum = "13478610634",
                CreateBy = "PostTest"
            };
            using (WebApp.Start(address, Configuration))
            {
                var response = await client.PostAsJsonAsync("/api/user", model);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Result<int>>();
                    Assert.IsFalse(result.Success);
                    Assert.AreEqual(ConstInfo.ERR_AlreadyHave_Phone, result.Message);
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
            string phoneNum = new Random().Next(199).ToString() + new Random().Next(99999999).ToString();
            User model = new User
            {
                NickName = "米老鼠"+ new Random().Next(999).ToString(),
                UserName = "Tom",
                Password = "12345",
                PhoneNum = phoneNum,
                CreateBy = "PostsTest"
            };
            using (WebApp.Start(address, Configuration))
            {
                var response = await client.PostAsJsonAsync("/api/user", model);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsAsync<Result<int>>();
                    Assert.IsTrue(result.Success);
                    Assert.AreEqual(1,result.Data);
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
            string phoneOrId = "64b0b681-3cb7-4a03-85dc-7f873e4cb459";
            User model = new User
            {
                Password = "sixsixsix",
                NickName="唐老鸭put",
                UserName="亓登海put",
                VIPLevel=2,
                LastUpdateBy = "testPut"
            };
            Result<int> result=new Result<int>();
            using (WebApp.Start(address, Configuration))
            {
                var response = await client.PutAsJsonAsync($"/api/user/{phoneOrId}", model);
                if (response.IsSuccessStatusCode)
                {
                    result = await response.Content.ReadAsAsync<Result<int>>();
                    Assert.IsTrue(result.Success);
                }
                else
                {
                    var str = await response.Content.ReadAsStringAsync();
                    Assert.Fail(str);
                }
                //手机号为id 进行修改
                model.VIPLevel = 10;
                phoneOrId = "18615066873";
                response = await client.PutAsJsonAsync($"/api/user/{phoneOrId}", model);
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);

                //不存在测试
                phoneOrId = "13466666666";
                response = await client.PutAsJsonAsync($"/api/user/{phoneOrId}", model);
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsFalse(result.Success);
                Assert.AreEqual(ConstInfo.ERR_NotFound_User, result.Message);

                //null值测试
                model = null;
                phoneOrId = "18615066873";
                response = await client.PutAsJsonAsync($"/api/user/{phoneOrId}", model);
                Assert.AreEqual(response.StatusCode, System.Net.HttpStatusCode.BadRequest);
            }
        }


        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [TestMethod()]
        public async Task DeleteTestAsync()
        {
            string Id = "09f4789b-c899-447c-8f35-405d3418ab4e";
            using (WebApp.Start(address, Configuration))
            {
                var response = await client.DeleteAsync($"/api/user/{Id}");
                var result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);

                Id = "13478610634";
                response = await client.DeleteAsync($"/api/user/{Id}");
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsTrue(result.Success);

                Id = "aaa";
                response = await client.DeleteAsync($"/api/user/{Id}");
                result = await response.Content.ReadAsAsync<Result<int>>();
                Assert.IsFalse(result.Success);
            }
        }
    }
}