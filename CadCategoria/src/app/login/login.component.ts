import { Component, OnInit } from '@angular/core';
import { FormsModule,FormGroup, Validators, NgForm, FormBuilder } from '@angular/forms';
import { Router } from '@angular/router';
import { ApiService } from 'src/services/api.service';
import { Usuario } from 'src/model/usuario';

@Component({
  selector: 'loginForm',
  templateUrl: './login.component.html'
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  email: String = '';
  password: String = '';
  dataSource: Usuario;
  isLoadingResults = false;

  constructor(private router: Router, private api: ApiService,
     private formBuilder: FormBuilder) { }

  ngOnInit() {
     this.loginForm = this.formBuilder.group({
    'email' : [null, Validators.required],
    'password' : [null, Validators.required]
  });
  }

  addLogin(form: NgForm) {
    this.isLoadingResults = true;
    this.api.Login(form)
      .subscribe(res => {
          this.dataSource = res;
          localStorage.setItem("jwt", this.dataSource.token);
          this.isLoadingResults = false;
          this.router.navigate(['/categorias']);
        }, (err) => {
          console.log(err);
          this.isLoadingResults = false;
        });
  }
}