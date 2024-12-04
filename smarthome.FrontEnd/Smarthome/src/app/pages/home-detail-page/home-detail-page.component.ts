import { AsyncPipe, CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterModule } from '@angular/router';
import { HomeService } from '../../services/home.service';
import { Observable } from 'rxjs';
import { TableModule } from 'primeng/table';
import { HomeDevice, HomeMember, Room } from '../../models/home';
import { ButtonModule } from 'primeng/button';
import { AddMemberDialogComponent } from '../../components/add-member-dialog/add-member-dialog.component';
import { AddDeviceDialogComponent } from '../../components/add-device-dialog/add-device-dialog.component';
import { AddRoomDialogComponent } from '../../components/add-room-dialog/add-room-dialog.component';
import { DropdownModule } from 'primeng/dropdown';
import { FormsModule } from '@angular/forms';
import { CheckboxModule } from 'primeng/checkbox';

@Component({
  selector: 'app-home-detail-page',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    AsyncPipe,
    TableModule,
    CheckboxModule,
    ButtonModule,
    AddMemberDialogComponent,
    AddDeviceDialogComponent,
    RouterModule,
    AddRoomDialogComponent,
    DropdownModule,
  ],
  templateUrl: './home-detail-page.component.html',
})
export class HomeDetailPageComponent implements OnInit {
  homeId!: number;
  homeMembers$!: Observable<HomeMember[]>;
  addMemberDialogVisible = false;

  homeDevices$!: Observable<HomeDevice[]>;
  addDeviceDialogVisible = false;
  devicesRoomQuery: Room | undefined;

  rooms$!: Observable<Room[]>;
  addRoomDialogVisible = false;

  constructor(
    private route: ActivatedRoute,
    private homeService: HomeService,
  ) {}

  private fetchHomeMembers() {
    this.homeMembers$ = this.homeService.getHomeMembers(this.homeId);
  }

  private fetchHomeDevices() {
    this.homeDevices$ = this.homeService.getHomeDevices(
      this.homeId,
      this.devicesRoomQuery?.roomId
        ? {
            roomId: this.devicesRoomQuery?.roomId,
          }
        : undefined,
    );
  }

  private fetchRooms() {
    this.rooms$ = this.homeService.getRooms(this.homeId);
  }

  ngOnInit() {
    this.homeId = this.route.snapshot.params['id'];
    this.fetchHomeMembers();
    this.fetchHomeDevices();
    this.fetchRooms();
  }

  onAddMember() {
    this.fetchHomeMembers();
  }

  onAddDevice() {
    this.fetchHomeDevices();
  }

  onAddRoom() {
    this.fetchRooms();
  }

  onCanReceiveNotificationsChange(userId: number, event: Event) {
    const isChecked = (event.target as HTMLInputElement).checked; // Safely cast to HTMLInputElement
    this.homeService
      .configureNotifications(this.homeId, userId, isChecked)
      .subscribe({
        next: () => {
          console.log(
            `CanReceiveNotifications updated for user ${userId}: ${isChecked}`,
          );
        },
        error: (err: { message: any }) => {
          console.error(
            `Error updating CanReceiveNotifications: ${err.message}`,
          );
        },
      });
  }

  onSearchDevices() {
    this.fetchHomeDevices();
  }
}
