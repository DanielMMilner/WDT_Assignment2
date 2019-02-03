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
    public class StatsController : Controller
    {
        private readonly RoomDataAccessLayer roomDataAccessLayer = new RoomDataAccessLayer();
        private readonly UserDataAccessLayer userDataAccessLayer = new UserDataAccessLayer();
        private readonly SlotDataAccessLayer slotDataAccessLayer = new SlotDataAccessLayer();
        

        // GET: api/<controller>
        // Return a stats object
        [HttpGet]
        public ApiStatsModel Get()
        {
            return new ApiStatsModel {
                BookedSlotCount = slotDataAccessLayer.GetBookedCount(),
                RoomCount = roomDataAccessLayer.GetRoomCount(),
                SlotCount = slotDataAccessLayer.GetSlotCount(),
                UserCount = userDataAccessLayer.GetUserCount()
            };
        }

    }
}
