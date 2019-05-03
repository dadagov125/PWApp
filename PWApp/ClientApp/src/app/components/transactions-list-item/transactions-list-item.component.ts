import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import {TransactionResponse} from "../../models/responses/transaction.response";

@Component({
  selector: 'transactions-list-item',
  templateUrl: './transactions-list-item.component.html',
  styleUrls: ['./transactions-list-item.component.css']
})
export class TransactionsListItemComponent implements OnInit {

  @Input() transaction: TransactionResponse;

  @Input() canRepeat: boolean;

  @Input() transactionTypeName: string;

  @Output() openTransferDialog = new EventEmitter<TransactionResponse>();

  constructor() {
  }

  ngOnInit() {
  
  }

}
