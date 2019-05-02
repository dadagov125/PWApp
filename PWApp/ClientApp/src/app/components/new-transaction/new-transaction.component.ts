import {Component, Inject, NgZone, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef, MatSnackBar} from "@angular/material";
import {TransactionResponse} from "../../models/responses/transaction.response";
import {TransferRequest} from "../../models/requests/transfer.request";
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {Observable} from "rxjs/internal/Observable";
import {map, startWith, tap, mergeMap,} from "rxjs/operators";
import {UserResponse} from "../../models/responses/user.response";
import {AccountService} from "../../services/account.service";
import {forkJoin} from "rxjs/internal/observable/forkJoin";
import {BehaviorSubject} from "rxjs/internal/BehaviorSubject";
import {AbstractControl} from "@angular/forms/src/model";
import {getDefaultSnackBarConfig, getErrorText} from "../../services/functions";


@Component({
  selector: 'new-transaction',
  templateUrl: './new-transaction.component.html',
  styleUrls: ['./new-transaction.component.css']
})
export class NewTransactionComponent implements OnInit {

  amount: number = 0;

  toUser: UserResponse;

  form: FormGroup;

  filteredOptions: BehaviorSubject<UserResponse[]> = new BehaviorSubject([]);

  constructor(public dialogRef: MatDialogRef<NewTransactionComponent>,
              private accountSerivce: AccountService,
              private snackBar: MatSnackBar,
              private zone: NgZone,
              @Inject(MAT_DIALOG_DATA)  data: { transaction?: TransactionResponse }) {
    if (data.transaction && data.transaction.toUser) {
      this.toUser = data.transaction.toUser;
      this.amount = data.transaction.amount;
    }
  }

  private filter(search: string) {
    this.accountSerivce.getUsers(search).subscribe(data => this.filteredOptions.next(data.list));
  }

  ngOnInit() {
    this.form = new FormGroup({
      user: new FormControl(this.displayFullUserNameFn()(), [Validators.required, this.userIsNull.bind(this)]),
      amount: new FormControl(this.amount, [Validators.required, Validators.min(0.01), this.checkAmount.bind(this)]),
    });

    this.form.controls['user'].valueChanges.subscribe(data => {
      if (typeof data === 'string') {
        this.filter(data);
      }
    });
  }

  get balance() {
    return this.accountSerivce.userAccount.balance;
  }

  displayFullUserNameFn() {
    return () => this.displayFullUserName(this.toUser);
  }

  displayFullUserName(user: UserResponse) {
    if (!user) return '';
    return `${user.firstName} ${user.lastName}`
  }

  selectUser(user: UserResponse) {
    this.toUser = user;
  }

  onNoClick(): void {
    this.dialogRef.close()
  }

  userIsNull(control: AbstractControl) {
    if (!this.toUser) {
      return {user: true}
    }
    return null
  }

  checkAmount(control: AbstractControl) {
    if (control.value > this.accountSerivce.userAccount.balance) {
      return {balance: true}
    }
    return null

  }

  doTransfer() {

    this.accountSerivce.Transfer({receiverId: this.toUser.id, amount: this.form.controls['amount'].value})
      .subscribe(value => {

        this.zone.run(() => {
          this.accountSerivce.userAccount.balance = value.balance;
        });

        this.snackBar.open(`You transferred ${value.amount} to ${this.displayFullUserName(value.toUser)} successful`, null, getDefaultSnackBarConfig());
        this.dialogRef.close(value);

      }, err => {
        this.snackBar.open(getErrorText(err), null, getDefaultSnackBarConfig());
      })
  }
}
