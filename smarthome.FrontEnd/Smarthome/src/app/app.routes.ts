import { Routes } from '@angular/router';
import { HomePageComponent } from './pages/home-page/home-page.component';
import { UsersPageComponent } from './pages/users-page/users-page.component';
import { DevicePageComponent } from './pages/device-page/device-page.component';
import { AddDevicePageComponent } from './pages/new-device-page/new-device-page.component';
import { DeviceDetailsPageComponent } from './pages/device-details-page/device-details-page.component';
import { NewUserPageComponent } from './pages/new-user-page/new-user-page.component';
import { EditUserPageComponent } from './pages/edit-user-page/edit-user-page.component';
import { LoginPageComponent } from './pages/login-page/login-page.component';
import { SignupPageComponent } from './pages/signup-page/signup-page.component';
import { NewCompanyPageComponent } from './pages/companies/new-company-page/new-company-page.component';
import { CompaniesPageComponent } from './pages/companies/companies-page/companies-page.component';
import { NewHomePageComponent } from './pages/new-home-page/new-home-page.component';
import { HomeDetailPageComponent } from './pages/home-detail-page/home-detail-page.component';
import { AuthGuard } from './auth.guard';
import { UnauthGuard } from './unauth.guard';
import { SupportedDevicesPageComponent } from './pages/supported-devices-page/supported-devices-page.component';
import { NotificationsPageComponent } from './pages/notifications-page/notifications-page.component';
import { HomesPageComponent } from './pages/homes-page/homes-page.component';
import { DummyNewDevicePageComponent } from './pages/dummy-new-device-page/dummy-new-device-page.component';
import { UserRole } from './models/user';
import { MyCompanyPageComponent } from './pages/my-company-page/my-company-page.component';

type AppRoutes = (Routes[number] & {
  data?: {
    roles?: UserRole[];
  };
})[];

const nonAuthedRoutes: AppRoutes = [
  {
    path: 'login',
    title: 'Login',
    component: LoginPageComponent,
  },
  {
    path: 'signup',
    title: 'Signup',
    component: SignupPageComponent,
  },
].map(route => ({ ...route, canActivate: [UnauthGuard] }));

const authedRoutes: AppRoutes = [
  {
    path: '',
    title: 'Home',
    component: HomePageComponent,
  },
  {
    path: 'users',
    title: 'Users',
    component: UsersPageComponent,
    data: { roles: [UserRole.Administrator] },
  },
  {
    path: 'users/new',
    title: 'New User',
    component: NewUserPageComponent,
    data: { roles: [UserRole.Administrator] },
  },
  {
    path: 'users/:id/edit',
    title: 'Edit User',
    component: EditUserPageComponent,
    data: { roles: [UserRole.Administrator] },
  },
  {
    path: 'companies',
    title: 'Companies',
    component: CompaniesPageComponent,
    data: { roles: [UserRole.Administrator] },
  },
  {
    path: 'companies/new',
    title: 'New Company',
    component: NewCompanyPageComponent,
    data: { roles: [UserRole.CompanyOwner] },
  },
  {
    path: 'companies/my',
    title: 'My Company',
    component: MyCompanyPageComponent,
    data: { roles: [UserRole.CompanyOwner] },
  },
  {
    path: 'homes',
    title: 'Homes',
    component: HomesPageComponent,
    data: { roles: [UserRole.Administrator, UserRole.HomeOwner] },
  },
  {
    path: 'homes/new',
    title: 'New Home',
    component: NewHomePageComponent,
    data: { roles: [UserRole.Administrator, UserRole.HomeOwner] },
  },
  {
    path: 'homes/:id',
    title: 'Home',
    component: HomeDetailPageComponent,
    data: { roles: [UserRole.Administrator, UserRole.HomeOwner] },
  },
  {
    path: 'devices/new',
    title: 'Add Device',
    component: AddDevicePageComponent,
    data: { roles: [UserRole.CompanyOwner] },
  },
  {
    path: 'devices/supported-devices',
    title: 'Supported Devices',
    component: SupportedDevicesPageComponent,
  },
  {
    path: 'devices/:deviceId',
    title: 'Device Details',
    component: DeviceDetailsPageComponent,
  },
  {
    path: 'devices',
    title: 'Devices',
    component: DevicePageComponent,
  },
  {
    path: 'homes/:id/notifications',
    title: 'Home Notifications',
    component: NotificationsPageComponent,
    data: { roles: [UserRole.Administrator, UserRole.HomeOwner] },
  },
  {
    path: 'import-devices',
    title: 'Import Devices',
    component: DummyNewDevicePageComponent,
    data: { roles: [UserRole.CompanyOwner] },
  },
].map(route => ({ ...route, canActivate: [AuthGuard] }));

export const routes: AppRoutes = [...nonAuthedRoutes, ...authedRoutes].map(
  route => ({ ...route, title: `SmartHome | ${route.title}` }),
);
