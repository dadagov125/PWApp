import {UserResponse} from "./user.response";
import {TransactionType} from "../transaction-type";

export class TransactionResponse {

  id: string;
  fromUser: UserResponse;
  toUser: UserResponse;
  amount: number;
  balance: number;
  created: string;
  transactionType: TransactionType;
  comment: string;

}

