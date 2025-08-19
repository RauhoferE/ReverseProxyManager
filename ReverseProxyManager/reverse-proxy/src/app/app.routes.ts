import { Routes } from '@angular/router';
import { BaseComponent } from './views/base/base.component';
import { LoginComponent } from './views/login/login.component';
import { ServersComponent } from './views/servers/servers.component';
import { CertificatesComponent } from './views/certificates/certificates.component';

export const routes: Routes = [
      { path: '', component: BaseComponent, children: [
        {
            path: 'servers', component: ServersComponent
        },
        {
            path: 'certificates', component: CertificatesComponent
        }
      ]},
      // This route navigates to the BaseComponent for the root URL
  // This route navigates to the AboutComponent for the '/about' URL
  { path: 'login', component: LoginComponent },
];
