import {BrowserModule} from '@angular/platform-browser';
import {NgModule} from '@angular/core';
import {FormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';


import {AppComponent} from './app.component';
import {NavMenuComponent} from './components/nav-menu/nav-menu.component';
import {HomeComponent} from './components/home/home.component';

import {FetchDataComponent} from './components/fetch-data/fetch-data.component';
import {AppRoutingModule} from './app-routing.module';
import {AccountService} from "./services/account.service";
import {MaterialModule} from "./material.module";
import {BREAKPOINTS, DEFAULT_BREAKPOINTS, FlexLayoutModule} from "@angular/flex-layout";



@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FetchDataComponent
  ],
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

  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
