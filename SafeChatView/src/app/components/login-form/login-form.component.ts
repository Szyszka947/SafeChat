import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { Router } from '@angular/router';
import { AccountService } from 'src/app/services/account.service';
import { ToastService } from 'src/app/services/toast.service';
import Swal from 'sweetalert2';
@Component({
  selector: 'app-login-form',
  templateUrl: './login-form.component.html',
  styleUrls: ['./login-form.component.css']
})
export class LoginFormComponent implements OnInit {

  constructor(private _accountService: AccountService, private router: Router, private _toastService: ToastService) { }

  hide: boolean = true;

  loading: boolean = false;

  ngOnInit(): void {
  }

  setFormControlAsValid(formControl: AbstractControl) {
    formControl.setErrors(null);
    return null;
  }

  getErrorForFormControl(controlName: string): string | null {
    const formControl = this.logInForm.controls[controlName];

    return formControl.getError('0') ? formControl.getError('0') : this.setFormControlAsValid(formControl);
  }

  logInForm: FormGroup = new FormGroup({
    Email: new FormControl(''),
    Password: new FormControl(''),
  });

  logInUser(): void {
    if (!this.logInForm.valid) return;

    this.loading = true;

    let userData = JSON.stringify(this.logInForm.value);

    this._accountService.signIn(userData).subscribe(data => {
      this.loading = false;

      this._toastService.toast('success', 'Signed in successfully');

      this.router.navigate(['/']);
    }, err => {
      this.loading = false;

      const result = err.error.result;

      this.logInForm.controls['Email'].setErrors({ "0": result['Email'] || result['email'] });
      this.logInForm.controls['Password'].setErrors({ "0": result['Password'] || result['password'] });
    });
  }
}
