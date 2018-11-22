import { Component, OnInit } from '@angular/core';
import { HttpService } from 'src/app/services/http-service.service';
import { CookieService } from 'ngx-cookie-service';
import { HttpResponse } from '@angular/common/http';
import { analyzeAndValidateNgModules } from '@angular/compiler';

@Component({
  selector: 'app-test',
  templateUrl: './test.component.html',
  styleUrls: ['./test.component.css']
})
export class TestComponent implements OnInit {

  constructor(public _httpService: HttpService, public _cookieService: CookieService) { }

  ngOnInit() {}

  login() {
    const body = {
      UserName: 'aaa',
      Password: 'aaa',
      Email: 'example@gmail.com'
    };
    this._httpService.post('api/user/login', body).subscribe(result => {
      console.log(result);
      const keys = result.headers;
      console.log(keys);
      const token = result.headers.get('token');
      console.log(token);
      this._cookieService.set('Auth-Token', token);
      console.log(this._cookieService.get('Auth-Token'));
    }, error => {
      console.log(error.error);
    });
  }
  test() {
    this._httpService.get('api/user/test', this._cookieService.get('Auth-Token')).subscribe(result => {
      console.log(result);
    });
  }

  logout() {
    this._cookieService.delete('Auth-Token');
  }
}
