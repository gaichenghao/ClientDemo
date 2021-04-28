using Consul;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServiceInstance.Utility
{
    public static  class ConsulHelper
    {
        public static void ConsulRegist(this IConfiguration configuration)
        {
            ConsulClient client = new ConsulClient(c => { c.Address = new Uri("http://localhost:8500"); c.Datacenter = "dcl"; });
            string ip = configuration["ip"];
            int port = int.Parse(configuration["port"]);//命令行参数必须传入
            //int port = 8001;
            int weight = string.IsNullOrWhiteSpace(configuration["weight"]) ? 1 : int.Parse(configuration["weight"]);
            client.Agent.ServiceRegister(new AgentServiceRegistration()
            {
                ID="service"+Guid.NewGuid(),//惟一的
                Name="UserService",//组名称 group
                Address=ip,//Ip地址
                Port=port,//不同实例
                Tags=new string[]{weight.ToString() },//标签
                Check=new AgentServiceCheck()
                {
                    Interval=TimeSpan.FromSeconds(3),
                    HTTP=$"http://{ip}:{port}/api/Health/Index",
                    Timeout = TimeSpan.FromSeconds(5),
                    DeregisterCriticalServiceAfter= TimeSpan.FromSeconds(60),
                }
            });
            Console.WriteLine($"{ip}:{port}--weight:{weight}");

        }
    }
}
