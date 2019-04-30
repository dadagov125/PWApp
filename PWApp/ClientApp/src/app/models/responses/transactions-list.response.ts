import { IPaginationResponse } from "./pagination.response";
import { TransactionResponse } from "./transaction.response";

export class TransactionsListResponse implements IPaginationResponse<TransactionResponse> {
  TotalCount: number;
  list: TransactionResponse[];
  skipped?: number;
  taken?: number;


}
