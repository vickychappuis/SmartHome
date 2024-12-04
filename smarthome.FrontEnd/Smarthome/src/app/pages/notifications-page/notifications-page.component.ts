import { Component, OnInit } from '@angular/core';
import { AsyncPipe } from '@angular/common';
import { ActivatedRoute, RouterLink, RouterModule } from '@angular/router';
import { TableModule } from 'primeng/table';
import { map, Observable } from 'rxjs';
import { NotificationService } from '../../services/notification.service';
import { HomeMemberNotification } from '../../models/notification';
import { FormsModule } from '@angular/forms';
import { DropdownModule } from 'primeng/dropdown';
import { CalendarModule } from 'primeng/calendar';

type ReadOption = { name: string; value: boolean | null };

@Component({
  selector: 'app-notifications-page',
  standalone: true,
  imports: [
    AsyncPipe,
    TableModule,
    FormsModule,
    DropdownModule,
    CalendarModule,
    RouterModule,
  ],
  templateUrl: './notifications-page.component.html',
})
export class NotificationsPageComponent implements OnInit {
  homeId!: number;
  homeMemberNotifications$!: Observable<HomeMemberNotification[]>;
  readQuery: ReadOption = { name: 'All', value: null };
  sinceDate: Date | null = null;

  readOptions: ReadOption[] = [
    { name: 'All', value: null },
    { name: 'Read', value: true },
    { name: 'Unread', value: false },
  ];

  constructor(
    private notificationService: NotificationService,
    private route: ActivatedRoute,
  ) {}

  ngOnInit(): void {
    this.homeId = this.route.snapshot.params['id'];
    this.homeMemberNotifications$ =
      this.notificationService.getHomeMemberNotifications(this.homeId);
    this.fetchNotifications();
  }

  fetchNotifications(): void {
    this.homeMemberNotifications$ =
      this.notificationService.getHomeMemberNotifications(this.homeId, {
        read: this.readQuery.value ?? undefined,
        sinceDate: this.sinceDate ?? undefined,
      });
  }

  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');
    return `${year}-${month}-${day}`;
  }
}
