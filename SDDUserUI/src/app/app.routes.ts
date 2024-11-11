import { Routes } from '@angular/router';
  
import { IndexComponent } from './user/index/index.component';
import { ViewComponent } from './user/view/view.component';
import { CreateComponent } from './user/create/create.component';
import { EditComponent } from './user/edit/edit.component';
  
export const routes: Routes = [
      { path: 'user', redirectTo: 'user/index', pathMatch: 'full'},
      { path: 'user/index', component: IndexComponent },
      { path: 'user/:userId/view', component: ViewComponent },
      { path: 'user/create', component: CreateComponent },
      { path: 'user/:userId/edit', component: EditComponent } 
  ];