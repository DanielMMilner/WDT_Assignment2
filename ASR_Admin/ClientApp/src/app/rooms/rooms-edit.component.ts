import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

import { Router, ActivatedRoute } from '@angular/router';
@Component({
  selector: 'app-rooms',
  templateUrl: './rooms-edit.component.html'
})
export class RoomsEditComponent {
  public room: Room;
  public newName: string;
  public errMsg: string;

  constructor(private router: Router, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute) {
    
    this.route.params.subscribe(f => {
      http.get<Room>(baseUrl + 'api/room/' + f.id).subscribe(result => {
        this.room = result;
      }, error => console.error(error));
    });
  }

  updateRoomName() {
    if (this.newName) {
      let r = this.room;
      r.roomName = this.newName;
      this.http.put<void>(this.baseUrl + 'api/room/', r).subscribe(result => {
        this.router.navigate(['/rooms']);
      }, error => this.errMsg = 'Unable to update room name');
    } else {
      this.errMsg = 'Room name cannot be blank';
    }
    
  }
}

interface Room {
  roomId: string;
  roomName: string;
}
