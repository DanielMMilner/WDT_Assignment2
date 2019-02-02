import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html'
})
export class RoomsComponent {
  public rooms: Room[];
  public newRoomName: string = "";
  public addStatus: number; //0 Nothing, 1 = Success, 2 = fail

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.updateList();
  }

  updateList() {
    this.http.get<Room[]>(this.baseUrl + 'api/room/').subscribe(result => {
      this.rooms = result;
    }, error => console.error(error));
  }

  addRoom() {
    if (this.newRoomName) {
      this.http.post<Room[]>(this.baseUrl + 'api/room/', { roomName: this.newRoomName }).subscribe(result => {
        this.addStatus = 1;
        this.updateList();
      }, error => this.addStatus = 2);
    }
  }

  deleteRoom(id: number) {
    this.http.delete<void>(this.baseUrl + 'api/room/' + id)
      .subscribe(result => {
        this.updateList();
      }, error => console.log(error));
  }

}

interface Room {
  roomId: string;
  roomName: string;
}
