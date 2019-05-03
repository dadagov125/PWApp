import {Component, OnInit, ViewChild} from '@angular/core';
import {AccountService} from "../../services/account.service";
import {MatPaginator} from "@angular/material";

import {TransactionResponse} from "../../models/responses/transaction.response";

import {TransactionType} from "../../models/transaction-type";
import {MatDialog, MatDialogRef, MatDialogConfig, MAT_DIALOG_DATA} from '@angular/material';
import {NewTransactionComponent} from "../new-transaction/new-transaction.component";

@Component({
  selector: 'transactions-list',
  templateUrl: './transactions-list.component.html',
  styleUrls: ['./transactions-list.component.css']
})
export class TransactionsListComponent implements OnInit {

  transactions: TransactionResponse[] = [];

  @ViewChild(MatPaginator) paginator: MatPaginator;

  constructor(protected accountService: AccountService, protected dialog: MatDialog) {
  }

  ngOnInit() {
    this.accountService.getTransactions().subscribe(value => {
      this.transactions = value.list;
    })
  }

  getTransactionTypeName(transaction: TransactionResponse) {
    switch (transaction.transactionType) {
      case TransactionType.DEPOSIT:
        return "Replenished";
      case TransactionType.WITHDRAW:
        return "Withdrawn";
      case TransactionType.TRANSFER:
        if (this.accountService.userAccount.id === transaction.fromUser.id) {
          return "Transferred";
        } else {
          return "Received";
        }
    }
  }

  canRepeat(transaction: TransactionResponse) {
    return transaction.transactionType == TransactionType.TRANSFER && this.accountService.userAccount.id === transaction.fromUser.id
  }

  openNewTransactionDialog = (transaction: TransactionResponse) => {
    const dialogRef = this.dialog.open(NewTransactionComponent, {
      height: '290px',
      width: '350px',

      data: {transaction}
    });

    dialogRef.afterClosed().subscribe((result: TransactionResponse) => {
      if (result) {
        this.transactions.push(result);
      }
    });
  }

}



