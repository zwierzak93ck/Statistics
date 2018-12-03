import { Component, OnInit } from '@angular/core';
import { FormControl, FormArray } from '@angular/forms';
import { HttpService } from '../../services/http.service';
import { ValidationService } from '../../services/validation.service';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  constructor(private _httpService: HttpService,
    private _validationService: ValidationService, private _cookieService: CookieService) { }

  email = new FormControl('', []);
  password = new FormControl('', []);
  controls = new Array<FormControl>();

  ngOnInit() {
  }

  checkValidation() {
    return this._validationService.checkValidation(new FormArray(
        new Array<FormControl>(this.email, this.password)));
  }

  login() {
    const body = {
      UserName:  this.email.value,
      Password: this.password.value
    };
    this._httpService.post('api/user/login', body).subscribe(result => {
      const token = result.headers.get('token');
      this._cookieService.set('Auth-Token', token);
    }, error => {
      console.log('Error' + error.error);
    });
  }
}
