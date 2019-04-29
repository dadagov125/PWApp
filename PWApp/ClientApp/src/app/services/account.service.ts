import {ApiController, ApiServiceBase} from "./rest";
import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";
import {RegisterRequest} from "../models/requests/register.request";
import {UserAccountResponse} from "../models/responses/user-account.response";
import {LoginRequest} from "../models/requests/login.request";
import {TransactionsListResponse} from "../models/responses/transactions-list.response";
import {UsersListResponse} from "../models/responses/users-list.response";
import {TransferRequest} from "../models/requests/transfer.request";
import {TransactionResponse} from "../models/responses/transaction.response";

@ApiController('account')
@Injectable()
export class AccountService extends ApiServiceBase {

  constructor(private http: HttpClient) {
    super();
  }

  register(request: RegisterRequest) {
    return this.http.post<UserAccountResponse>(this.getActionUrl('register'), request);
  }

  login(request: LoginRequest) {
    return this.http.post<UserAccountResponse>(this.getActionUrl('login'), request);
  }

  logout() {
    return this.http.post<any>(this.getActionUrl('logout'), null);
  }

  getUserAccount() {
    return this.http.get<UserAccountResponse>(this.getActionUrl('useraccount'))
  }

  getBalance() {
    return this.http.get<number>(this.getActionUrl('balance'))
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
