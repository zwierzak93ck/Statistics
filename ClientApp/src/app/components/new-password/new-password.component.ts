import { Component, OnInit } from '@angular/core';
import { FormControl, FormArray } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { HttpService } from '../../services/http.service';
import { ValidationService } from '../../services/validation.service';

@Component({
  selector: 'app-new-password',
  templateUrl: './new-password.component.html',
  styleUrls: ['./new-password.component.css']
})
export class NewPasswordComponent implements OnInit {

  password = new FormControl('', []);
  confirmPassword = new FormControl('', []);

  userId;
  token;
  constructor(private _httpService: HttpService, private _validationService: ValidationService, private _activatedRoute: ActivatedRoute) { }

  ngOnInit() {
    this._activatedRoute.queryParams.subscribe(params => {
      this.userId = params['userId'];
      this.token = params['token'];
    });
  }

  checkValidation() {
    return this._validationService.checkValidation(new FormArray(
      new Array<FormControl>(this.password, this.confirmPassword)
    )) && this._validationService.isMatch(new FormArray(
      new Array<FormControl>(this.password, this.confirmPassword)
    ));
  }

  changePassword() {
    this._httpService.get('api/account/resetPassword?userId=' + this.userId + '&token=' + this.token + '&newPassword=' + this.password.value).subscribe(result => console.log(result));
  }
}
