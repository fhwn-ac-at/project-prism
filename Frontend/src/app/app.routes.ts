import { Routes } from '@angular/router';
import { GamePageComponent } from './pages/game-page/game-page.component';
import { PageNotFoundComponent } from './pages/page-not-found/page-not-found.component';
import { LobbyComponent } from './pages/lobby/lobby.component';
import { LoginComponent } from './pages/login/login.component';
import { StartComponent } from './pages/start/start.component';
import { TestComponent } from './pages/test/test.component';

export const routes: Routes = [
    {
        path: "start", 
        title: "Start",
        component: StartComponent
    },
    {
        path: "login",
        title: "Login",
        component: LoginComponent
    },
    {
        path: "lobby",
        title: "Lobby",
        component: LobbyComponent
    },
    {
        path: "game", 
        title: "Game",
        component: GamePageComponent   
    },
    {
        path: "test",
        title: "Test",
        component: TestComponent
    },
    {
        path: "",
        redirectTo: "start",
        pathMatch: "full"
    },
    {
        path: "**",
        title: "Page Not Found",
        component: PageNotFoundComponent
    },
];
