import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';

@Component({
  selector: 'app-slots',
  templateUrl: './slots.component.html'
})
export class SlotsComponent {
  public slots: Slot[];


  constructor(http: HttpClient, private router: Router, @Inject('BASE_URL') baseUrl: string) {
    http.get<Slot[]>(baseUrl + 'api/slot/GetAllSlots').subscribe(result => {
      this.slots = result;
    }, error => console.error(error));
  }

  editSlot(id: string) {
    this.router.navigate(['/slots/edit/' + id]);
  }

}

interface Slot {
  slotId: number;
  roomName: string;
  startTime: Date;
  staffId: string;
  studentId: string;
}
