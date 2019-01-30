import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-slots',
  templateUrl: './slots.component.html'
})
export class SlotsComponent {
  public slots: Slot[];


  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Slot[]>(baseUrl + 'api/slot/GetAllSlots').subscribe(result => {
      this.slots = result;
    }, error => console.error(error));
  }

}

interface Slot {
  roomId: number;
  startTime: Date;
  staffId: string;
  studentId: string;
}
