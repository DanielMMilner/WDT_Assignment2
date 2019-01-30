using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASR_Admin.Models
{
    public class ApiUserModel
    {
        public string SchoolId;
        public string Email;
        public string Name;
    }

    public class ApiSlotModel
    {
        public string RoomName;
        public string StudentId;
        public string StaffId;
        public DateTime startTime;
    }

}
