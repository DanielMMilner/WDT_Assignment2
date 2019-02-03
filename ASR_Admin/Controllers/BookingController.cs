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
    public class BookingController : Controller
    {
        private readonly SlotDataAccessLayer slotDataAccessLayer = new SlotDataAccessLayer();

        // Updates the given booking with the new student information
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] ApiUserModel student)
        {
            slotDataAccessLayer.UpdateBooking(id, student.SchoolId);
        }

        // Cancels the booking with the given id
        [HttpDelete("{id}")]
        public void CancelBooking(int id)
        {
            slotDataAccessLayer.CancelBooking(id);
        }
    }
}
