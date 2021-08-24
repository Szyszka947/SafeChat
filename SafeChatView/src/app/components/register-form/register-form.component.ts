import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormControl, FormGroup } from '@angular/forms';
import { MatButton } from '@angular/material/button';
import { Router } from '@angular/router';
import { Origins } from 'src/app/data/origins';
import { AccountService } from 'src/app/services/account.service';
import { ToastService } from 'src/app/services/toast.service';

@Component({
  selector: 'app-register-form',
  templateUrl: './register-form.component.html',
  styleUrls: ['./register-form.component.css']
})
export class RegisterFormComponent implements OnInit {

  constructor(private _accountService: AccountService, private router: Router, private _toastService: ToastService) { }

  ngOnInit(): void {
  }

  hide: boolean = true;
  hide2: boolean = true;
  loading: boolean = false;

  setFormControlAsValid(formControl: AbstractControl) {
    formControl.setErrors(null);
    return null;
  }

  getErrorForFormControl(controlName: string): string | null {
    const formControl = this.registerForm.controls[controlName];

    return formControl.getError('0') ? formControl.getError('0')[0] : this.setFormControlAsValid(formControl);
  }

  registerForm: FormGroup = new FormGroup({
    Email: new FormControl(''),
    Username: new FormControl(''),
    Password: new FormControl(''),
    RepeatPassword: new FormControl('')
  });

  registerUser(): void {
    if (!this.registerForm.valid) return;

    this.loading = true;

    var userData = JSON.stringify(this.registerForm.value);

    this._accountService.createAccount(userData).subscribe((data: any) => {
      this.loading = false;

      this.router.navigate(['user/login']);

      this._toastService.toast('success', 'Registered successfully. Now log in');

    }, err => {
      this.loading = false;

      const result = err.error.result;

      this.registerForm.controls['Email'].setErrors({ "0": result['Email'] });
      this.registerForm.controls['Username'].setErrors({ "0": result['UserName'] });
      this.registerForm.controls['Password'].setErrors({ "0": result['Password'] });
      this.registerForm.controls['RepeatPassword'].setErrors({ "0": result['PasswordsMatches'] });

    });
  }
}