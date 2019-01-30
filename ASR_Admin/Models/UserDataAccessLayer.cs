using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASR_Admin.Models
{
    public class UserDataAccessLayer
    {
        
        private readonly dbContext db = new dbContext();

        public IEnumerable<AspNetUsers> GetAllUsers()
        {
            return db.AspNetUsers;
        }


        public AspNetUsers GetUser(string id)
        {
            return db.AspNetUsers.FirstOrDefault(x => x.SchoolId == id);
        }

        public void RemoveUser(string id)
        {
            var user = db.AspNetUsers.FirstOrDefault(x => x.SchoolId == id);
            if(user == null)
            {
                return;
            }

            if(user.SchoolId.StartsWith("e"))
            {
                db.Slot.RemoveRange(user.SlotStaff);
            }
            else
            {
                var slotsToUpdate = db.Slot.Where(x => x.StudentId == user.Id).ToList();
                slotsToUpdate.ForEach(x => x.StudentId = null);

                db.Slot.UpdateRange(slotsToUpdate);
                
            }

            db.SaveChanges();
            db.Remove(user);
            db.SaveChanges();
        }
    }
}
