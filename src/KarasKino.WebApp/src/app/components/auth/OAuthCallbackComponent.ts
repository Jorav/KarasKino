import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-oauth-callback',
  standalone: true,
  template: '<p>Completing login...</p>'
})
export class OAuthCallbackComponent implements OnInit {
  ngOnInit() {
    if (window.opener) {
      window.opener.postMessage('oauth_success', window.location.origin);
      window.close();
    }
  }
}
