import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { App } from './app';
import { Login } from './views/login/login';
import { ServerManager } from './views/server-manager/server-manager';
import { CertManager } from './views/cert-manager/cert-manager';
import { Routes } from '@angular/router';

const routes: Routes = [
  // This is the default route that loads the HomeComponent
  { path: '', component: App, 
    children:[
      {
      path:'server',
      component: ServerManager
      },
      {
        path:'certs',
        component: CertManager
      }

    ]
  },
  // This is the route for the /about URL
  { path: 'login', component: Login },
];

@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ]
})
export class AppRoutingModule { }
