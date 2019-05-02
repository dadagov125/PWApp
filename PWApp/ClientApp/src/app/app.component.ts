import {ChangeDetectorRef, Component, Inject, OnInit, ViewChild, ViewChildren} from '@angular/core';
import {MediaMatcher} from "@angular/cdk/layout";
import {MatSidenav} from "@angular/material";
import {AccountService} from "./services/account.service";
import {Router} from "@angular/router";
import {UserAccountResponse} from "./models/responses/user-account.response";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {

  mobileQuery: MediaQueryList;

  constructor(private changeDetectorRef: ChangeDetectorRef,
              private media: MediaMatcher,
              private router: Router,
              protected accountService: AccountService) {

    this.mobileQuery = media.matchMedia('(max-width: 600px)');

    this.mobileQuery.addListener(this.mobileQueryListener.bind(this));

  }


  ngOnInit(): void {

  }

  logout() {
    this.accountService.logout().subscribe(value => {
      this.router.navigate(['/auth'])
    })
  }

  mobileQueryListener() {
    this.changeDetectorRef.detectChanges();
  }

  ngOnDestroy() {
    this.mobileQuery.removeListener(this.mobileQueryListener);
  }


}
