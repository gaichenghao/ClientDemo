using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Model;

namespace ServiceInstance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {

        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _iUserService = null;
        private IConfiguration _iConfiguration;

        public UsersController(ILogger<UsersController> logger, IUserService userService, IConfiguration configuration)
        {
            _logger = logger;
            this._iUserService = userService;
            this._iConfiguration = configuration;
        }
        [HttpGet]
        [Route("Get")]
        public User Get(int id)
        {
            return this._iUserService.FindUser(id);
        }
        [HttpGet]
        [Route("All")]
        public IEnumerable<User> Get()
        {
            Console.WriteLine($"this is {this._iConfiguration["port"]} invoke");
            return this._iUserService.UserAll();
        }
    }
}