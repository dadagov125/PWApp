import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { AccountService } from "../../services/account.service";

@Injectable({
  providedIn: 'root',
})
export class AuthGuard implements CanActivate {

  constructor(private accountService: AccountService, private router: Router) {

  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {

    const isLoggedIn = this.accountService.isLoggedIn;

    if (isLoggedIn) return true;

    this.accountService.redirectUrl = state.url;

    this.router.navigate(['/auth']);

    return false;

  }
}
