using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASR_Admin.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASR_Admin.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {

        private readonly UserDataAccessLayer users = new UserDataAccessLayer();

        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<ApiUserModel> Get()
        {
            return users.GetAllUsers();
        }

        // GET: api/<controller>
        [HttpGet]
        [Route("GetStudents")]
        public IEnumerable<ApiUserModel> GetStudents()
        {
            return users.GetStudents();
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public AspNetUsers Get(string id)
        {
            return users.GetUser(id);
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            users.RemoveUser(id);
        }
    }
}
