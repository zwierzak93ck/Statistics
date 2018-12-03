import { Component, OnInit } from '@angular/core';
import { HttpService } from '../../../services/http.service';
import { Route, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-email-confirm',
  templateUrl: './email-confirm.component.html',
  styleUrls: ['./email-confirm.component.css']
})
export class EmailConfirmComponent implements OnInit {

  constructor(private _activatedRoute: ActivatedRoute, private _httpService: HttpService) { }

  userId;
  token;
  message;
  ngOnInit() {
    this._activatedRoute.queryParams.subscribe(params => {
      this.userId = params['userId'];
      this.token = params['token'];
    });
    console.log('działa');
    this._httpService.get('api/account/confirmEmail?userId=' + this.userId + '&token=' + this.token).subscribe();
  }

}
