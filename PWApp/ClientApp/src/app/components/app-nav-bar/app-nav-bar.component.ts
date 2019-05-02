import {ChangeDetectorRef, Component, Input, OnInit} from '@angular/core';
import {Router} from "@angular/router";
import {UserAccountResponse} from "../../models/responses/user-account.response";
import {MediaMatcher} from "@angular/cdk/layout";
import {AccountService} from "../../services/account.service";
import {MatSidenav} from "@angular/material";

@Component({
  selector: 'app-nav-bar',
  templateUrl: './app-nav-bar.component.html',
  styleUrls: ['./app-nav-bar.component.css']
})
export class AppNavBarComponent implements OnInit {


  mobileQuery: MediaQueryList;

  @Input() sideNav: MatSidenav;

  @Input() logout: () => void;

  constructor(private changeDetectorRef: ChangeDetectorRef,
              private media: MediaMatcher,
              private router: Router,
              protected accountService: AccountService) {

    this.mobileQuery = media.matchMedia('(max-width: 600px)');

    this.mobileQuery.addListener(this.mobileQueryListener.bind(this));

  }

  ngOnInit() {

  }

  mobileQueryListener() {
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy() {
    this.mobileQuery.removeListener(this.mobileQueryListener);
  }


}
