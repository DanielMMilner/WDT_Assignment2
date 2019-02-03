import { Component, Inject } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  public stats: StatsModel;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
    this.http.get<StatsModel>(this.baseUrl + 'api/stats/').subscribe(result => {
      this.stats = result;
    }, error => console.error(error));
  }
}


interface StatsModel {
  userCount: number;
  slotCount: number;
  bookedSlotCount: number;
  roomCount: number;
}
