import { HttpClient, HttpHeaders } from '@angular/common/http';
import {
  HttpClient as SignalRHttpClient,
  HttpRequest as SignalRHttpRequest,
  HttpResponse as SignalRHttpResponse,
  HttpTransportType,
} from '@microsoft/signalr';
import { inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class AngularSignalRHttpClient extends SignalRHttpClient {
 
    private httpClient: HttpClient = inject(HttpClient);

  send(request: SignalRHttpRequest): Promise<SignalRHttpResponse> {
    // Convert SignalR request to Angular HttpClient request
    const url = request.url!;
    const headers = new HttpHeaders(request.headers || {});
    const body = request.content || null;

    const options: {
        headers: HttpHeaders;
        observe: 'response';
        responseType: 'text';
    } = {
      headers: headers,
      observe: 'response' as const,
      responseType: 'text', // SignalR expects text response
    };

    // Map SignalR HTTP methods to Angular
    let httpCall;
    switch (request.method) {
      case 'GET':
        httpCall = this.httpClient.get(url, options);
        break;
      case 'POST':
        httpCall = this.httpClient.post(url, body, options);
        break;
      case 'DELETE':
        httpCall = this.httpClient.delete(url, options);
        break;
      case 'PUT':
        httpCall = this.httpClient.put(url, body, options);
        break;
      default:
        return Promise.reject(
          new Error(`Unsupported HTTP method: ${request.method}`)
        );
    }

    return new Promise((resolve, reject) => {
        httpCall.subscribe({
          next: (response) => {
            resolve({
              statusCode: response.status, // Full response provides status
              statusText: response.statusText,
              content: response.body as string, // Ensure body is treated as string
            });
          },
          error: (error) => {
            reject({
              statusCode: error.status,
              statusText: error.statusText || 'Unknown error',
              content: error.message,
            });
          },
        });
      });
  }
}
