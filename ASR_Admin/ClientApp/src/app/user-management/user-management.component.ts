import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { forEach } from '@angular/router/src/utils/collection';

@Component({
  selector: 'app-user-management',
  templateUrl: './user-management.component.html'
})
export class UserManagementComponent {
  public users: AspUser[];
  public httpClient: HttpClient;
  public baseURL: string;
  
  public deleteFail: number = 0; // 0 nothing, 1 fail, 2 success

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.httpClient = http;
    this.baseURL = baseUrl;
    this.updateList();
  }

  updateList() {
    this.httpClient.get<AspUser[]>(this.baseURL + 'api/user/').subscribe(result => {
      this.users = result;
    }, error => console.error(error));
  }

  deleteUser(id: string) {
    this.httpClient.delete<AspUser[]>(this.baseURL + 'api/user/' + id).subscribe(result => {
      this.deleteFail = 2;
      this.updateList();
    }, error => this.deleteFail = 1);
    
  }

  
}

interface AspUser {
  schoolId: string;
  Email: string;
  Name: string;
}
