import { Component, inject } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { AuthService } from '../../../core/services/auth.service';

interface MenuItem {
  label: string;
  icon: string;
  route: string;
  roles: string[];
}

@Component({
  selector: 'app-sidebar',
  imports: [RouterLink, RouterLinkActive],
  templateUrl: './sidebar.html',
  styleUrl: './sidebar.css',
})
export class Sidebar {
  private readonly authService = inject(AuthService);

  role = this.authService.getRole();

  menuItems: MenuItem[] = [
    {
      label: 'Tableau de bord',
      icon: '📊',
      route: '/dashboard',
      roles: ['ADMINISTRATEUR', 'TECHNICIEN', 'MEDECIN']
    },
    {
      label: 'Donneurs',
      icon: '🩸',
      route: '/donneurs',
      roles: ['ADMINISTRATEUR', 'TECHNICIEN']
    },
    {
      label: 'Dons',
      icon: '➕',
      route: '/dons',
      roles: ['ADMINISTRATEUR', 'TECHNICIEN']
    },
    {
      label: 'Tests',
      icon: '🧪',
      route: '/tests',
      roles: ['ADMINISTRATEUR', 'TECHNICIEN', 'MEDECIN']
    },
    {
      label: 'Stock',
      icon: '📦',
      route: '/stock',
      roles: ['ADMINISTRATEUR', 'TECHNICIEN', 'MEDECIN']
    },
    {
      label: 'Patients',
      icon: '👥',
      route: '/patients',
      roles: ['ADMINISTRATEUR', 'MEDECIN']
    },
    {
      label: 'Demandes',
      icon: '📨',
      route: '/demandes',
      roles: ['ADMINISTRATEUR', 'MEDECIN']
    },
    {
      label: 'Utilisateurs',
      icon: '⚙️',
      route: '/utilisateurs',
      roles: ['ADMINISTRATEUR']
    }
  ];

  /**
   * Vérifie si l'élément de menu peut être affiché pour le rôle connecté.
   */
  canShow(item: MenuItem): boolean {
    if (!this.role) {
      return false;
    }

    return item.roles.includes(this.role);
  }
}
