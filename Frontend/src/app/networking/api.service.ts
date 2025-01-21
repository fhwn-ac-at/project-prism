import { Injectable } from '@angular/core';
import { Client } from './generatedAPI/GeneratedApiClient';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: null
})
export class ApiService {

  constructor(httpClient: HttpClient) 
  { 
    this.ApiClient = new Client(httpClient, "http://localhost:5164");
  }

  public ApiClient: Client;
}
