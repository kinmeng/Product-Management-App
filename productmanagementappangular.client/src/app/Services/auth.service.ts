import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
 
  private baseUrl = 'https://localhost:7218/api'

  constructor(private http: HttpClient) {
}

  login(credentials: { username: string, password: string, rememberMe: boolean }): Observable<any> {
    return this.http.post<any>(this.baseUrl + '/login', credentials);
  }

  register(userData: any): Observable<any> {
    return this.http.post<any>( this.baseUrl +'/register' , userData);
  }

  logout() {
    localStorage.removeItem('currentUser');
  }

}

