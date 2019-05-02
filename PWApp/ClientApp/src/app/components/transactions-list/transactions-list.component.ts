import {Component, OnInit, ViewChild} from '@angular/core';
import {AccountService} from "../../services/account.service";
import {MatPaginator, MatTableDataSource} from "@angular/material";
import {DataSource} from "@angular/cdk/table";
import {TransactionResponse} from "../../models/responses/transaction.response";
import {CollectionViewer} from "@angular/cdk/collections";
import {Observable} from "rxjs/internal/Observable";
import {BehaviorSubject} from "rxjs/internal/BehaviorSubject";
import {catchError, finalize} from "rxjs/operators";
import {of} from "rxjs/internal/observable/of";
import {TransactionType} from "../../models/transaction-type";

@Component({
  selector: 'transactions-list',
  templateUrl: './transactions-list.component.html',
  styleUrls: ['./transactions-list.component.css']
})
export class TransactionsListComponent implements OnInit {

  // displayedColumns: string[] = ['date', 'user', 'email', 'amount', 'balance'];
  //
  // dataSource: MatTableDataSource<TransactionResponse> = new MatTableDataSource<TransactionResponse>([]);

  transactions: TransactionResponse[];
  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(protected accountService: AccountService) {

  }

  ngOnInit() {

    this.accountService.getTransactions().subscribe(value => {

      this.transactions = value.list;
    })


  }

  getTransactionTypeName(transaction: TransactionResponse) {
    switch (transaction.transactionType) {
      case TransactionType.DEPOSIT:
        return "Replenished"
      case TransactionType.WITHDRAW:
        return "Withdrawn";
      case TransactionType.TRANSFER:
        if (this.accountService.userAccount.id === transaction.fromUser.id) {
          return "Transferred"
        } else {
          return "Received"
        }


    }
  }

  canRepeat(transaction: TransactionResponse) {

    return transaction.transactionType == TransactionType.TRANSFER && this.accountService.userAccount.id === transaction.fromUser.id
  }

}



