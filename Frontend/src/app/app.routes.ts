import { Routes } from '@angular/router';
import { GamePageComponent } from './pages/game-page/game-page.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { HomeComponent } from './pages/home/home.component';

export const routes: Routes = [
    {
        path: "home", 
        title: "Home",
        component:HomeComponent
    },
    {
        path: "",
        redirectTo: "home",
        pathMatch: "full"
    },
    {
        path: "game", 
        title: "Game",
        component: GamePageComponent,      
    },
    {
        path: "**",
        title: "Page Not Found",
        component: PageNotFoundComponent
    },
];
