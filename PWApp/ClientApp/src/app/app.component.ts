import { ChangeDetectorRef, Component, Inject, OnInit, ViewChild, ViewChildren } from '@angular/core';
import { MediaMatcher } from "@angular/cdk/layout";
import { MatSidenav } from "@angular/material";
import { AccountService } from "./services/account.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  mobileQuery: MediaQueryList;


  constructor(private changeDetectorRef: ChangeDetectorRef,
    private media: MediaMatcher,
    protected accountService: AccountService) {

    this.mobileQuery = media.matchMedia('(max-width: 600px)');

    this.mobileQuery.addListener(this.mobileQueryListener.bind(this));

  }

  ngOnInit(): void {


  }


  fillerContent = Array.from({ length: 2 },
    () =>
    `Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut
       labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco
       laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in
       voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat
       cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.`);

  mobileQueryListener() {
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy() {
    this.mobileQuery.removeListener(this.mobileQueryListener);
  }


}
