import {HttpErrorResponse} from "@angular/common/http";
import {MatSnackBarConfig} from "@angular/material";


export function getErrorList(err: HttpErrorResponse): string[] {
  if (!err || !err.error) {
    return [err.message];
  }
  return Object.values(err.error).map(value => {
    if (Array.isArray(value)) {
      return value.join("\n")
    }
    return value;
  }).join('\n').replace('\n\n', '\n').split('\n');
}

export function getErrorText(err: HttpErrorResponse): string {
  return getErrorList(err).join('\n')
}

export function getDefaultSnackBarConfig(): MatSnackBarConfig {
  return {
    duration: 2000,
    verticalPosition: 'top',
    horizontalPosition: 'right',
    panelClass: 'snack-bar-panel'
  };
}
