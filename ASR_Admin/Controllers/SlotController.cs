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
        
        // GET: api/<controller>
        [HttpGet]
        [Route("Index")]
        public IEnumerable<string> Get()
        {
            return slotDataAccessLayer.GetSlots().Select(x => x.SlotId.ToString());
        }

        [HttpGet]
        [Route("GetAllSlots")]
        public IEnumerable<Slot> GetAllSlots(DateTime date)
        {
            return slotDataAccessLayer.GetSlots();
        }

        [HttpGet("{date}")]
        [Route("GetSlotsOnDay")]
        public IEnumerable<Slot> GetSlotsOnDay(DateTime date)
        {
            return slotDataAccessLayer.GetSlots().Where(x => x.StartTime.Date == date.Date);
        }

        [Route("GetAllSlots")]
        public IEnumerable<Slot> GetSlotsBetweenDates()
        {
            return slotDataAccessLayer.GetSlots();
        }


        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
