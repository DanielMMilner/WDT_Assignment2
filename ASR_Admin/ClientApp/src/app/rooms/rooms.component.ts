import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html'
})
export class RoomsComponent {
  public rooms: Room[];


  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    http.get<Room[]>(baseUrl + 'api/room/').subscribe(result => {
      this.rooms = result;
    }, error => console.error(error));
  }



  editRoomName(id: string) {
    //let r = this.rooms.find(x => x.roomId == id);
    //r.roomName = document.getElementById("roomName-" + id).;
    //this.http.put<void>(this.baseUrl + 'api/room/' + id, r).subscribe(result => {
      
    //}, error => console.error(error));
  }

}

interface Room {
  roomId: string;
  roomName: string;
}
