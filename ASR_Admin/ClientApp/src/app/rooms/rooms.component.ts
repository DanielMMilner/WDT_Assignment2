import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html'
})
export class RoomsComponent {
  public rooms: Room[];


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Room[]>(baseUrl + 'api/slot/GetAllSlots').subscribe(result => {
      this.rooms = result;
    }, error => console.error(error));
  }

}

interface Room {
  roomId: string;
  roomName: string;
}
