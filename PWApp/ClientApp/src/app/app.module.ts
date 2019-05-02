import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';


import {AppComponent} from './app.component';
import {HomeComponent} from './components/home/home.component';

import {AppRoutingModule} from './app-routing.module';
import {AccountService} from "./services/account.service";
import {MaterialModule} from "./material.module";
import {BREAKPOINTS, DEFAULT_BREAKPOINTS, FlexLayoutModule} from "@angular/flex-layout";
import {AuthComponent} from './components/auth/auth.component';
import {CookieService} from "ngx-cookie-service";
import {AppNavBarComponent} from './components/app-nav-bar/app-nav-bar.component';
import {TransactionsListComponent} from './components/transactions-list/transactions-list.component';
import {NewTransactionComponent} from './components/new-transaction/new-transaction.component';


@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    AuthComponent,
    AppNavBarComponent,
    TransactionsListComponent,
    NewTransactionComponent
  ],
  entryComponents: [NewTransactionComponent],
  imports: [
    BrowserModule.withServerTransition({appId: 'ng-cli-universal'}),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    MaterialModule,
    FlexLayoutModule
  ],
  providers: [
    AccountService,
    CookieService
  ],

  bootstrap: [AppComponent]
})
export class AppModule {
}
