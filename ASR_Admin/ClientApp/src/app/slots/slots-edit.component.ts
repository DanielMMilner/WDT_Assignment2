import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';

@Component({
  selector: 'app-slots-edit',
  templateUrl: './slots-edit.component.html'
})
export class SlotsEditComponent {
  public slot: Slot;
  public students: Student[];
  public studentSelected: string;
  public editorID: string;
  public error: boolean = false;

  constructor(private router: Router, private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, private route: ActivatedRoute) {
    this.route.params.subscribe(f => {
      http.get<Slot>(baseUrl + 'api/slot/getslot?id=' + f.slotid).subscribe(result => {
        this.slot = result;
        this.studentSelected = this.slot.studentId;
        this.editorID = f.id;
      }, error => console.error(error));

      http.get<Student[]>(baseUrl + 'api/user/getstudents').subscribe(result => {
        this.students = result;
      }, error => console.error(error));
    });
  }

  //Cancels the selected booking and reutrns to the slot page
  cancelBooking() {
    this.http.delete<void>(this.baseUrl + 'api/booking/' + this.slot.slotId)
      .subscribe(result => {
        this.router.navigate(['/slots/' + this.editorID]);
      }, error => this.error = true);
  }

  //Updates the selected booking and reutrns to the slot page
  updateBooking() {
    let student = this.students.find(x => x.schoolId == this.studentSelected);

    this.http.put<void>(this.baseUrl + 'api/booking/' + this.slot.slotId, student)
      .subscribe(result => {
        this.router.navigate(['/slots/' + this.editorID]);
      }, error => this.error = true);
    
  }


  back() {
    this.router.navigate(['/slots/' + this.editorID]);
  }

}

interface Student {
  schoolId: string;
  Email: string;
  Name: string;
}

interface Slot {
  slotId: number;
  roomName: string;
  startTime: Date;
  staffId: string;
  studentId: string;
}

