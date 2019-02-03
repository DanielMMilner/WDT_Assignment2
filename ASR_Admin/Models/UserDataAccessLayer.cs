using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASR_Admin.Models
{
    public class UserDataAccessLayer
    {
        
        private readonly dbContext db = new dbContext();

        public IEnumerable<ApiUserModel> GetAllUsers()
        {
            return db.AspNetUsers.Select(x => new ApiUserModel
            {
                SchoolId = x.SchoolId,
                Name = x.Name,
                Email = x.Email
            }).OrderBy(x => x.SchoolId);
        }

        public IEnumerable<ApiUserModel> GetStudents()
        {
            return db.AspNetUsers.Where(x => x.SchoolId.StartsWith("s")).Select(x => new ApiUserModel
            {
                SchoolId = x.SchoolId,
                Name = x.Name,
                Email = x.Email
            });
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

        public int GetUserCount()
        {
            return db.AspNetUsers.ToList().Count;
        }
    }
}
