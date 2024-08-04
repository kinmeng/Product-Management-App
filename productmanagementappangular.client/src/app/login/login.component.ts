import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from '../Services/auth.service';
import { Router } from '@angular/router';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginFormComponent {
  form: FormGroup;
  message: string = '';
  isError: boolean = false;

  @Input() error: string | null = null;

  @Output() submitEM = new EventEmitter<void>();

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router) {
    this.form = this.fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
    });
  }

  submit() {
    if (this.form.valid) {
      this.authService.login(this.form.value).subscribe(
        response => {
          // Handle successful login
          this.message = 'Login successful';
          this.isError = false;
          this.submitEM.emit();
          if (response.redirectUrl) {
            this.router.navigate([response.redirectUrl]);  // Redirect to the URL
          }
        },
        error => {
          // Handle login error
          this.message = 'Login failed: ' + (error.error.message || 'Unknown error');
          this.isError = true;
        }
      );
    }
  }
}
