import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-slots',
  templateUrl: './slots.component.html'
})
export class SlotsComponent implements OnInit {
  public slots: Slot[];
  public user: User;
  public isStaff: boolean;

  constructor(private route: ActivatedRoute, private http: HttpClient, private router: Router, @Inject('BASE_URL') private baseUrl: string) {
    this.updateData();
  }

  ngOnInit() {
    this.updateData();
  }


  updateData() {
    this.route.params.subscribe(f => {
      // Get the updated slots for the given user id
      this.http.get<Slot[]>(this.baseUrl + 'api/slot/getslots?=' + f.id).subscribe(result => {
        this.slots = result;
      }, error => console.error(error));
      // Get the information for the user
      this.http.get<User>(this.baseUrl + 'api/user/' + f.id).subscribe(result => {
        this.user = result;
        this.isStaff = this.user.schoolId.startsWith("e");
      }, error => console.error(error));
    });
  }

  editSlot(id: number) {
    this.router.navigate(['/slots/' + this.user.schoolId + '/edit/' + id]);
  }

  cancelBooking(id: number) {
    // Cancel the booking
    this.http.delete<void>(this.baseUrl + 'api/booking/' + id)
      .subscribe(result => {
        this.updateData();
      }, error => console.log(error));
  }

  deleteSlot(id: number) {
    this.http.delete<void>(this.baseUrl + 'api/slot/' + id)
      .subscribe(result => {
        this.updateData();
      }, error => console.log(error));
  }

  back() {
    this.router.navigate(['/user-management']);
  }
}

interface Slot {
  slotId: number;
  roomName: string;
  startTime: Date;
  staffId: string;
  studentId: string;
}

interface User {
  schoolId: string;
  Email: string;
  Name: string;
}

