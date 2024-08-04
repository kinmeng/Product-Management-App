import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../Services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  registerForm: FormGroup;
  message: string = '';
  isError: boolean = false;
  errors: string[] = [];

  constructor(private fb: FormBuilder, private authService: AuthService) {
    this.registerForm = this.fb.group({
      preferredName: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required]]
    });
  }

  onSubmit() {
    if (this.registerForm.valid) {
      this.authService.register(this.registerForm.value).subscribe(
        response => {
          this.message = 'Registration successful';
          this.isError = false;
          this.errors = [];
        },
        error => {
          this.message = 'Registration failed:';
          this.isError = true;
          if (error.error && error.error.errors) {
            this.errors = error.error.errors.map((err: { description: string }) => err.description) || [error.message || 'Unknown error'];
          } else {
            this.errors = [error.message || 'Unknown error'];
          }
        }
      );
    }
  }
}
