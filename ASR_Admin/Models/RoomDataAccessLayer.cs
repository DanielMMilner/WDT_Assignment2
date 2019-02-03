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
            // Strip the rooms down removing extra db info
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
            // Find the room with the given roomid and update its roomname
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
            // Find the room and remvoe it from the db
            db.Room.Remove(db.Room.FirstOrDefault(x => x.RoomId == id));

            // Remove all slots connected to the given room
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
