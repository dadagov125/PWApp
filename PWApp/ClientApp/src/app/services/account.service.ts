import { ApiController, ApiServiceBase } from "./rest";
import { HttpClient, HttpErrorResponse } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { RegisterRequest } from "../models/requests/register.request";
import { UserAccountResponse } from "../models/responses/user-account.response";
import { LoginRequest } from "../models/requests/login.request";
import { TransactionsListResponse } from "../models/responses/transactions-list.response";
import { UsersListResponse } from "../models/responses/users-list.response";
import { TransferRequest } from "../models/requests/transfer.request";
import { TransactionResponse } from "../models/responses/transaction.response";
import { Observable, Subject, BehaviorSubject } from "rxjs";
import { tap, onErrorResumeNext, catchError } from "rxjs/operators"

@ApiController('account')
@Injectable()
export class AccountService extends ApiServiceBase {

  isLoggedIn: boolean = false;

  redirectUrl: string;

  constructor(private http: HttpClient) {
    super();
  }


  register(request: RegisterRequest) {
    return this.http.post<UserAccountResponse>(this.getActionUrl('register'), request)
      .pipe(
        tap(data => {
          this.isLoggedIn = true;
        }));
  }

  login(request: LoginRequest) {
    return this.http.post<UserAccountResponse>(this.getActionUrl('login'), request)
      .pipe(
        tap(data => {
          this.isLoggedIn = true;
        }));
  }

  logout() {
    return this.http.post<any>(this.getActionUrl('logout'), null)
      .pipe(
        tap(data => {
          this.isLoggedIn = false;
        }));
  }

  checkLogin() {
    return this.http.get<any>(this.getActionUrl('checklogin'))
      .pipe(
        tap(data => {
          this.isLoggedIn = true;
        }));
  }

  getUserAccount() {
    return this.http.get<UserAccountResponse>(this.getActionUrl('useraccount'))
      .pipe(
        tap(data => {
          this.isLoggedIn = true;
        }));
  }

  getBalance() {
    return this.http.get<number>(this.getActionUrl('balance'));
  }

  getTransactions() {
    return this.http.get<TransactionsListResponse>(this.getActionUrl('transactions'))
  }

  getUsers() {
    return this.http.get<UsersListResponse>(this.getActionUrl('users'))
  }

  Transfer(request: TransferRequest) {
    return this.http.post<TransactionResponse>(this.getActionUrl('transfer'), request);
  }

}
