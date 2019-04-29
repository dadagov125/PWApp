import {ApiController, ApiServiceBase} from "./rest";
import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";

@ApiController('account')
@@Injectable()
export class AccountService extends ApiServiceBase {

  constructor(private http: HttpClient) {
    super();
  }

  register(){

  }

}
