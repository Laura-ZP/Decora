import { Component, inject } from '@angular/core';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AppUser } from '../../../models/app-user.model';
import { AccountService } from '../../../services/account.service';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';

@Component({
  selector: 'app-register',
  imports: [
    RouterLink,
    FormsModule, ReactiveFormsModule,
    MatButtonModule, MatInputModule, MatFormFieldModule
  ],
  templateUrl: './register.component.html',
  styleUrl: './register.component.scss'
})
export class RegisterComponent {
  fB = inject(FormBuilder);
  accountService = inject(AccountService);

  registerFg = this.fB.group({
    emailCtrl: ['', [Validators.required, Validators.email]],
    nameCtrl: ['', [Validators.required]],
    surnameCtrl: ['', [Validators.required]],
    passwordCtrl: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(16)]],
    confirmPasswordCtrl: ['', [Validators.required, Validators.minLength(8), Validators.maxLength(16)]],
    nationalCodeCtrl: [''],
    ageCtrl: 0,
    cityCtrl: [''] 
  });

  get EmailCtrl(): FormControl {
    return this.registerFg.get('emailCtrl') as FormControl;
  }

  get NameCtrl(): FormControl {
    return this.registerFg.get('nameCtrl') as FormControl;
  }
  
  get SurnameCtrl(): FormControl {
    return this.registerFg.get('surnameCtrl') as FormControl;
  }
  
  get PasswordCtrl(): FormControl {
    return this.registerFg.get('passwordCtrl') as FormControl;
  }
  
  get ConfirmPasswordCtrl(): FormControl {
    return this.registerFg.get('confirmPasswordCtrl') as FormControl;
  }
  
  get NationalCodeCtrl(): FormControl {
    return this.registerFg.get('nationalCodeCtrl') as FormControl;
  }
  
  get AgeCtrl(): FormControl {
    return this.registerFg.get('ageCtrl') as FormControl;
  }

  get CityCtrl(): FormControl {
    return this.registerFg.get('cityCtrl') as FormControl;
  }

  register(): void {
    let user: AppUser = {
      email: this.EmailCtrl.value,
      name: this.NameCtrl.value,
      surname: this.SurnameCtrl.value,
      password: this.PasswordCtrl.value,
      confirmPassword: this.ConfirmPasswordCtrl.value,
      nationalCode: this.NationalCodeCtrl.value,
      age: this.AgeCtrl.value,
      city: this.CityCtrl.value
    }

    this.accountService.register(user).subscribe({
      next: (res) => console.log(res),
      error: (err) => console.log(err)
    });
  }
}
