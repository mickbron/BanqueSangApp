import { Routes } from '@angular/router';

import { Login } from './features/auth/login/login';
import { Dashboard } from './features/dashboard/dashboard/dashboard';
import { DonneursList } from './features/donneurs/donneurs-list/donneurs-list';
import { DonsList } from './features/dons/dons-list/dons-list';
import { StockDashboard } from './features/stock/stock-dashboard/stock-dashboard';
import { TestsList } from './features/tests/tests-list/tests-list';
import { Layout } from './shared/components/layout/layout';


import { authGuard } from './core/guards/auth-guard';
import { roleGuard } from './core/guards/role-guard';

export const routes: Routes = [
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  {
    path: 'login',
    component: Login
  },
  {
    path: '',
    component: Layout,
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        component: Dashboard
      },
      {
        path: 'donneurs',
        component: DonneursList,
        canActivate: [roleGuard],
        data: {
          roles: ['ADMINISTRATEUR', 'TECHNICIEN']
        }
      },
      {
        path: 'dons',
        component: DonsList,
        canActivate: [roleGuard],
        data: {
          roles: ['ADMINISTRATEUR', 'TECHNICIEN']
        }
      },
      {
        path: 'stock',
        component: StockDashboard
      },
      {
        path: 'tests',
        component: TestsList,
        canActivate: [roleGuard],
        data: {
          roles: ['ADMINISTRATEUR', 'TECHNICIEN', 'MEDECIN']
        }
      }
    ]
  },
  {
    path: '**',
    redirectTo: 'login'
  }
];
