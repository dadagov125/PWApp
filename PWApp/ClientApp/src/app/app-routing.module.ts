import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from "@angular/router";
import { FetchDataComponent } from "./components/fetch-data/fetch-data.component";
import { HomeComponent } from "./components/home/home.component";
import { AuthGuard } from "./components/auth/auth.guard";
import { AuthComponent } from "./components/auth/auth.component";

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot([
      {
        canActivate: [AuthGuard],
        path: '',
        component: HomeComponent,
        pathMatch: 'full'
      },
      {
        path: 'auth',
        component: AuthComponent,
      },
      {
        canActivate: [AuthGuard],
        path: 'fetch-data',
        component: FetchDataComponent
      },
    ]),
  ],
  declarations: [],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {
}
