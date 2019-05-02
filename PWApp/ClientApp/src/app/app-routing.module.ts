import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {RouterModule} from "@angular/router";
import {HomeComponent} from "./components/home/home.component";
import {AuthGuard} from "./components/auth/auth.guard";
import {AuthComponent} from "./components/auth/auth.component";
import {TransactionsListComponent} from "./components/transactions-list/transactions-list.component";

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forRoot([
      {
        canActivate: [AuthGuard],
        path: '',
        // component: HomeComponent,
        redirectTo:'transactions',
        pathMatch: 'full'
      },
      {
        path: 'auth',
        component: AuthComponent,
      },
      {
        canActivate:[AuthGuard],
        path:'transactions',
        component:TransactionsListComponent
      }
    ]),
  ],
  declarations: [],
  exports: [
    RouterModule
  ]
})
export class AppRoutingModule {
}
