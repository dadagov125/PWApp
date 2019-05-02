import {ApiController, ApiServiceBase} from "./rest";
import {HttpClient, HttpErrorResponse, HttpParams} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {RegisterRequest} from "../models/requests/register.request";
import {UserAccountResponse} from "../models/responses/user-account.response";
import {LoginRequest} from "../models/requests/login.request";
import {TransactionsListResponse} from "../models/responses/transactions-list.response";
import {UsersListResponse} from "../models/responses/users-list.response";
import {TransferRequest} from "../models/requests/transfer.request";
import {TransactionResponse} from "../models/responses/transaction.response";
import {Observable, Subject, BehaviorSubject} from "rxjs";
import {tap, onErrorResumeNext, catchError} from "rxjs/operators"
import {CookieService} from "ngx-cookie-service";


@ApiController('account')
@Injectable()
export class AccountService extends ApiServiceBase {

  isLoggedIn: boolean = false;

  redirectUrl: string;

  userAccount: UserAccountResponse;

  constructor(private http: HttpClient, private cookieService: CookieService) {
    super();
  }


  register(request: RegisterRequest) {
    return this.http.post<UserAccountResponse>(this.getActionUrl('register'), request, {withCredentials: true})
      .pipe(
        tap(data => {
          this.isLoggedIn = true;
          this.userAccount = data;
        }));
  }

  login(request: LoginRequest) {
    return this.http.post<UserAccountResponse>(this.getActionUrl('login'), request, {withCredentials: true})
      .pipe(
        tap(data => {
          this.isLoggedIn = true;

          this.userAccount = data;
        }));
  }

  logout() {
    return this.http.post<any>(this.getActionUrl('logout'), null, {withCredentials: true})
      .pipe(
        tap(data => {
          this.isLoggedIn = false;
          this.userAccount = null;
        }));
  }

  // checkLogin() {
  //   return this.http.get<any>(this.getActionUrl('checklogin'), {withCredentials: true})
  //     .pipe(
  //       tap(data => {
  //         this.isLoggedIn = true;
  //       }));
  // }

  getUserAccount() {
    return this.http.get<UserAccountResponse>(this.getActionUrl('useraccount'), {withCredentials: true})
      .pipe(
        tap(data => {
          this.isLoggedIn = true;
          this.userAccount = data;
        }));
  }

  getBalance() {
    return this.http.get<number>(this.getActionUrl('balance'), {withCredentials: true});
  }

  getTransactions() {
    return this.http.get<TransactionsListResponse>(this.getActionUrl('transactions'), {withCredentials: true})
  }

  getUsers(text?: string) {

    let params: HttpParams = new HttpParams();
    if (text){
      params=params.set('text', text);
    }

    params=params.set('take', '5');
    params=params.set('skip', '0');


    return this.http.get<UsersListResponse>(this.getActionUrl('users'), {
      withCredentials: true,
      params:params
    })
  }

  Transfer(request: TransferRequest) {
    return this.http.post<TransactionResponse>(this.getActionUrl('transfer'), request, {withCredentials: true});
  }


}
