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

  loginForm: FormGroup;
  registerForm: FormGroup;

  ngOnInit() {
    this.loginForm = new FormGroup({
      emailLog: new FormControl(null, [Validators.required, Validators.email]),
      passwordLog: new FormControl(null, [Validators.required]),
    });

    this.registerForm = new FormGroup({
      firstNameReg: new FormControl(null, [Validators.required]),
      lastNameReg: new FormControl(null, [Validators.required]),
      emailReg: new FormControl(null, [Validators.required, Validators.email]),
      passwordReg: new FormControl(null, [Validators.required]),
    });
    this.registerForm.addControl('passwordConfirmReg', new FormControl(null, [this.passwordConfirmValidator.bind(this)]))

    this.accountService.getUserAccount().subscribe(value =>{
      this.router.navigate(['/'])
    })

  }


  passwordConfirmValidator(control: FormControl) {
    const password = this.registerForm.controls['passwordReg'].value;
    const passwordConfirm = control.value;
    if (password === passwordConfirm) {
      return null;
    }
    return {"passwordConfirmReg": true}
  }

  doLogin() {
    const {emailLog, passwordLog} = this.loginForm.value;

    this.accountService.login({email: emailLog, password: passwordLog}).subscribe(value => {
        this.router.navigate(['/'])
      },
      (err: HttpErrorResponse) => {
        this.snackBar.open(getErrorText(err), null, getDefaultSnackBarConfig());
      })
  }

  doRegister() {

    const {firstNameReg, lastNameReg, emailReg, passwordReg, passwordConfirmReg} = this.registerForm.value;

    this.accountService.register({
      firstName: firstNameReg,
      lastName: lastNameReg,
      email: emailReg,
      password: passwordReg,
      passwordConfirm: passwordConfirmReg
    }).subscribe(value => {
      this.router.navigate(['/'])
    }, err => {
      this.snackBar.open(getErrorText(err), null, getDefaultSnackBarConfig());
    })

  }

}
