import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DemandesSangList } from './demandes-sang-list';

describe('DemandesSangList', () => {
  let component: DemandesSangList;
  let fixture: ComponentFixture<DemandesSangList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DemandesSangList],
    }).compileComponents();

    fixture = TestBed.createComponent(DemandesSangList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
