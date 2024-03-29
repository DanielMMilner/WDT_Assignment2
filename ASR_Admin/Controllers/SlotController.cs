﻿using System;
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
       
        // Get all slots for a given user school id
        [HttpGet("{id}")]
        [Route("GetSlots")]
        public IEnumerable<ApiSlotModel> GetSlotsForId(string id)
        {
            return slotDataAccessLayer.GetSlotsForId(id);
        }

        // Get a slot given a slot id
        [HttpGet("{id}")]
        [Route("GetSlot")]
        public ApiSlotModel GetSlot(int id)
        {
            return slotDataAccessLayer.GetSlot(id);
        }

        // Delete a slot given a slotid
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            slotDataAccessLayer.DeleteSlot(id);
        }
    }
}
