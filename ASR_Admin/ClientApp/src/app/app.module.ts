import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { SlotsComponent } from './slots/slots.component';

import { UserManagementComponent } from './user-management/user-management.component';
import { RoomsComponent } from './rooms/rooms.component';
import { SlotsEditComponent } from './slots/slots-edit.component';
import { RoomsEditComponent } from './rooms/rooms-edit.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    SlotsComponent,
    UserManagementComponent,
    RoomsComponent,
    SlotsEditComponent,
    RoomsEditComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'slots/:id', component: SlotsComponent },
      { path: 'user-management', component: UserManagementComponent },
      { path: 'rooms', component: RoomsComponent },
      { path: 'slots/:id/edit/:slotid', component: SlotsEditComponent },
      { path: 'rooms/:id', component: RoomsEditComponent }
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
