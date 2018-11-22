import { Injectable } from '@angular/core';
import { HttpHeaders, HttpClient, HttpParams } from '@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class HttpService {

  private _headers: HttpHeaders = new HttpHeaders().set('Cache-Control', 'no-cache', ).set('Cache-Control', 'no-store').set('Pragma', 'no-cache');
  constructor(private _httpClient: HttpClient) { }

  get(url: string, token?: string, params?: HttpParams): any {
    return this._httpClient.get<any>('http://localhost:64981/' + url, {headers: this._headers.set('Authorization', 'Bearer ' + token), observe: 'response', params: params, withCredentials: true, responseType: 'json'});
  }

  post(url: string, body: any, params?: HttpParams) {
    return this._httpClient.post<any>('http://localhost:64981/' + url, body, {headers: this._headers, observe: 'response', params: params, withCredentials: true, responseType: 'json'});
  }

  put(url: string, body: any, params?: HttpParams) {
    return this._httpClient.put<any>('http://localhost:64981/' + url, body, {headers: this._headers, params: params, withCredentials: true});
  }
}
