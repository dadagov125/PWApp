import {HttpClient} from "@angular/common/http";
import {Inject, Injectable, Optional} from "@angular/core";
import {environment} from '../../environments/environment';

export abstract class ApiServiceBase {

  //http://baseUrl/apiUrl/controllerUrl/actionUrl

  protected apiUrl: string;

  protected controllerUrl: string;

  protected baseUrl: string;

  getActionUrl(action: string) {

    return `${this.controllerUrl}${action}`
  }
}


export function ApiController(controllerName: string) {
  return (target: any) => {
    target.prototype.baseUrl = environment.baseUrl;
    target.prototype.apiUrl = `${target.prototype.baseUrl}api/`;
    target.prototype.controllerUrl = `${target.prototype.apiUrl}${controllerName}/`;
    return target;
  };
}
