import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html'
})
export class UserManagementComponent {
  public users: AspUser[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<AspUser[]>(baseUrl + 'api/user/').subscribe(result => {
      this.users = result;
    }, error => console.error(error));
  }
}

interface AspUser {
  schoolId: string;
  Email: string;
  Name: string;
}
