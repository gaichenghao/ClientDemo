using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public UsersController(ILogger<UsersController> logger, IUserService userService)
        {
            _logger = logger;
            this._iUserService = userService;
        }
        [HttpGet]
        [Route("[Get]")]
        public User Get(int id)
        {
            return this._iUserService.FindUser(id);
        }
        [HttpGet]
        [Route("[All]")]
        public IEnumerable<User> Get()
        {
            return this._iUserService.UserAll();
        }
    }
}