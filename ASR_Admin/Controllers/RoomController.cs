using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASR_Admin.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASR_Admin.Controllers
{
    [Route("api/[controller]")]
    public class RoomController : Controller
    {
        private readonly RoomDataAccessLayer roomDataAccessLayer = new RoomDataAccessLayer();
        // GET: api/<controller>
        [HttpGet]
        public IEnumerable<ApiRoomModel> Get()
        {
            return roomDataAccessLayer.GetAllRooms();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public ApiRoomModel Get(int id)
        {
            return roomDataAccessLayer.GetRoom(id);
        }

        // PUT api/<controller>/5
        [HttpPut]
        public void Put([FromBody]ApiRoomModel value)
        {
            roomDataAccessLayer.UpdateRoomName(value.RoomId, value.RoomName);
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]ApiRoomModel room)
        {
            roomDataAccessLayer.AddRoom(room.RoomName);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            roomDataAccessLayer.DeleteRoom(id);
        }

    }
}
