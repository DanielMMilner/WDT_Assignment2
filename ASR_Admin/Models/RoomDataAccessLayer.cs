using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASR_Admin.Models
{
    public class RoomDataAccessLayer
    {
        private readonly dbContext db = new dbContext();

        public IEnumerable<ApiRoomModel> GetAllRooms()
        {
            return db.Room.Select(x => new ApiRoomModel {
                RoomId = x.RoomId,
                RoomName = x.RoomName
            }).OrderBy(x => x.RoomName);
        }

        public ApiRoomModel GetRoom(int id)
        {
            return GetAllRooms().FirstOrDefault(x => x.RoomId == id);
        }

        public void UpdateRoomName(int roomId, string roomName)
        {
            var room = db.Room.FirstOrDefault(x => x.RoomId == roomId);
            room.RoomName = roomName;

            db.Room.Update(room);

            db.SaveChanges();

        }

        public void AddRoom(string roomName)
        {
            db.Add(new Room { RoomName = roomName });
            db.SaveChanges();
        }

        public void DeleteRoom(int id)
        {
            db.Room.Remove(db.Room.FirstOrDefault(x => x.RoomId == id));

            var slots = db.Slot.Where(x => x.RoomId == id);

            db.RemoveRange(slots);

            db.SaveChanges();
            
        }

        public int GetRoomCount()
        {
            return db.Room.ToList().Count;
        }
    }
}
