import { Component, OnInit } from '@angular/core';
import { FormControl, FormArray } from '@angular/forms';
import { HttpService } from '../../services/http.service';
import { ValidationService } from '../../services/validation.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  email = new FormControl('', []);
  confirmEmail = new FormControl('', []);
  password = new FormControl('', []);
  confirmPassword = new FormControl('', []);

  constructor(private _httpService: HttpService,
    private _validationService: ValidationService) { }

  ngOnInit() {
  }

  checkValidation() {
    return this._validationService.checkValidation(new FormArray(
      new Array<FormControl>(this.email, this.confirmEmail, this.password, this.confirmPassword)
    )) && this._validationService.isMatch(new FormArray(
      new Array<FormControl>(this.email, this.confirmEmail)
    )) && this._validationService.isMatch(new FormArray(
      new Array<FormControl>(this.password, this.confirmPassword)
    ));
  }

  register() {
    console.log('test');
    const body = {
      UserName: this.email.value,
      Password: this.password.value,
      Email: this.email.value
    };

    this._httpService.post('api/user/users', body).subscribe();
  }

}
