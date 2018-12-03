import { Component, OnInit } from '@angular/core';
import { FormControl, FormArray } from '@angular/forms';
import { HttpService } from '../../services/http.service';
import { ValidationService } from '../../services/validation.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {

  email = new FormControl('', []);
  constructor(private _httpService: HttpService, private _validationService: ValidationService) { }

  ngOnInit() {
  }

  checkValidation() {
    return this._validationService.checkValidation(new FormArray(
      new Array<FormControl>(this.email)
    ));
  }

  changePassword() {
    const body = {
      Email: this.email.value
    };
    this._httpService.post('api/account/forgotPassword', body).subscribe();
  }
}
