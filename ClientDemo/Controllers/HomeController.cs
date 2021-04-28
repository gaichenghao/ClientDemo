using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ClientDemo.Models;
using Interface;
using System.Net.Http;
using Model;
using Consul;

namespace ClientDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _iUserService=null;

        public HomeController(ILogger<HomeController> logger,IUserService userService)
        {
            _logger = logger;
            this._iUserService = userService;
        }

        public IActionResult Index()
        {
            //代码的一小步程序的一大步 单体变成分布式
            //base.ViewBag.Users= this._iUserService.UserAll();
            //string url = "http://localhost:5726/api/users";
            //string url = "http://localhost:5727/api/users";
            //string url = "http://localhost:5728/api/users";

            string url = "http://UserService/api/users/all";

            #region myregion
            {
                ConsulClient client = new ConsulClient(c=> 
                {
                    c.Address = new Uri("http://localhost:8500/");
                    c.Datacenter = "dc1";
                });
                var response = client.Agent.Services().Result.Response;

                Uri uri = new Uri(url);
                string groupName = uri.Host;

                //var serviceDictionary = response.Where(s => s.Value.Service.Equals(groupName,StringComparison.OrdinalIgnoreCase));
                var serviceDictionary = response.Where(s => s.Value.Service.Equals(groupName, StringComparison.OrdinalIgnoreCase)).ToArray();
                {
                    //url = $"{uri.Scheme}://{serviceDictionary.First().Value.Address}:{serviceDictionary.First().Value.Port}{uri.PathAndQuery}";
                }
                ////负载均衡策略
                AgentService agentService = null;
                //{
                //    //平均策略 多个实例 平均分配---随机就是平均
                //    agentService = serviceDictionary[new Random(DateTime.Now.Millisecond + iSeed
                //        ++).Next(0, serviceDictionary.Length)].Value;                   
                //}
                //{
                //    //轮训策略--僵化
                //    agentService = serviceDictionary[iSeed++%serviceDictionary.Length].Value;
                //}
                {
                    //根据服务器的情况选择--权重--不同地实例权重不同
                    List<KeyValuePair<string, AgentService>> pairList = new List<KeyValuePair<string, AgentService>>();
                    foreach (var pair in serviceDictionary)
                    {
                        int count = int.Parse(pair.Value.Tags?[0]);
                        for (int i = 0; i < count; i++)
                        {
                            pairList.Add(pair);
                        }
                        agentService = pairList.ToArray()[new Random(iSeed++).Next(0, pairList.Count())].Value;
                    }
                }
                url = $"{uri.Scheme}://{agentService.Address}:{agentService.Port}{uri.PathAndQuery}";
            }

            #endregion





            string content = InvokApi(url);
            base.ViewBag.Users = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<User>>(content);
            return View();
        }

        private static int iSeed = 0;

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static string InvokApi(string Url)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                HttpRequestMessage message = new HttpRequestMessage();
                message.Method = HttpMethod.Get;
                message.RequestUri = new Uri(Url);
                var result = httpClient.SendAsync(message).Result;
                string content = result.Content.ReadAsStringAsync().Result;
                return content;
            }
        }
       
    }
}
