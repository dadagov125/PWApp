import {Component, OnInit,} from '@angular/core';
import {FormGroup, FormControl, Validators} from '@angular/forms';
import {AccountService} from "../../services/account.service";
import {HttpErrorResponse} from "@angular/common/http";
import {getDefaultSnackBarConfig, getErrorText} from "../../services/functions";
import {MatSnackBar,} from "@angular/material";
import {Router} from "@angular/router";

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent implements OnInit {

  constructor(protected accountService: AccountService, private router: Router, private snackBar: MatSnackBar) {
  }

  ngOnInit() {

  }

  loginForm = new FormGroup({
    emailLog: new FormControl(null, [Validators.required, Validators.email]),
    passwordLog: new FormControl(null, [Validators.required]),
  });


  doLogin() {
    const value = this.loginForm.value;
    const email = value.emailLog;
    const password = value.passwordLog;

    this.accountService.login({email, password}).subscribe(value => {
        this.router.navigate([this.accountService.redirectUrl])
      },
      (err: HttpErrorResponse) => {
        this.snackBar.open(getErrorText(err), null, getDefaultSnackBarConfig());
      })

  }


}
