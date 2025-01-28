import { Injectable, NgZone } from '@angular/core';
import { BehaviorSubject, Observable, fromEvent } from 'rxjs';
import { debounceTime } from 'rxjs/operators';

@Injectable({
  providedIn: null,
})
export class ResizeService {
  private resizeSubject = new BehaviorSubject<{ width: number; height: number }>({
    width: window.innerWidth,
    height: window.innerHeight,
  });

  constructor(private ngZone: NgZone) {
    // Listen to window resize events
    fromEvent(window, 'resize')
      .pipe(debounceTime(200)) // Debounce to avoid too many events
      .subscribe(() => {
        this.ngZone.run(() => {
          this.resizeSubject.next({
            width: window.innerWidth,
            height: window.innerHeight,
          });
        });
      });
  }

  // Observable to expose the resize event
  get onResize$(): Observable<{ width: number; height: number }> {
    return this.resizeSubject.asObservable();
  }
}