import { Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/account/login/login.component';
import { RegisterComponent } from './components/account/register/register.component';
import { FooterComponent } from './components/footer/footer.component';
import { NavbarComponent } from './components/navbar/navbar.component';
import { NotFoundComponent } from './components/not-found/not-found.component';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'account/login', component: LoginComponent },
    { path: 'account/register', component: RegisterComponent },
    { path: 'footer', component: FooterComponent },
    { path: 'navbar', component: NavbarComponent },
    { path: '**', component: NotFoundComponent }
];
