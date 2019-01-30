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
    public class SlotController : Controller
    {

        private readonly SlotDataAccessLayer slotDataAccessLayer = new SlotDataAccessLayer();
       
        [HttpGet]
        [Route("GetAllSlots")]
        public IEnumerable<ApiSlotModel> GetAllSlots()
        {
            return slotDataAccessLayer.GetSlots();
        }
        

    }
}
