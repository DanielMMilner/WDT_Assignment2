import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-slots-edit',
  templateUrl: './slots-edit.component.html'
})
export class SlotsEditComponent {
  public slot: Slot;
  
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string, private route: ActivatedRoute) {
    this.route.params.subscribe(f => {
      http.get<Slot>(baseUrl + 'api/slot/getslot?id=' + f.id).subscribe(result => {
        this.slot = result;
      }, error => console.error(error));
    });
  }

}

interface Slot {
  slotId: number;
  roomName: string;
  startTime: Date;
  staffId: string;
  studentId: string;
}

