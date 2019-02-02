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
            });
        }

        public ApiRoomModel GetRoom(string id)
        {
            return GetAllRooms().FirstOrDefault(x => x.RoomId == id);
        }

        public void UpdateRoomName(string roomId, string roomName)
        {
            var room = db.Room.FirstOrDefault(x => x.RoomId == roomId);
            room.RoomName = roomName;

            db.Room.Update(room);

            db.SaveChanges();

        }
    }
}
