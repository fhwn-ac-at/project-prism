import { inject, Injectable } from "@angular/core";
import { HttpClient, HttpRequest, HttpResponse } from "@microsoft/signalr";
import { HttpClient as Client } from '@angular/common/http';

@Injectable({
    providedIn: null
  })
  export class SignalRHttpClient implements HttpClient {
    private httpClient: Client = inject(Client);
    
    get(url: unknown, options?: unknown): Promise<import("@microsoft/signalr").HttpResponse> {
        throw new Error("Method not implemented.");
    }
    post(url: unknown, options?: unknown): Promise<import("@microsoft/signalr").HttpResponse> {
        throw new Error("Method not implemented.");
    }
    delete(url: unknown, options?: unknown): Promise<import("@microsoft/signalr").HttpResponse> {
        throw new Error("Method not implemented.");
    }
    
    getCookieString(url: string): string {
        throw new Error("Method not implemented.");
    }
  
    send(request: HttpRequest): Promise<HttpResponse> {
        throw new Error("Method not implemented.");
    }
}