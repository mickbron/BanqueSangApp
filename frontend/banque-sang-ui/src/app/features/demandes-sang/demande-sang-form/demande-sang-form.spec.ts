import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DemandeSangForm } from './demande-sang-form';

describe('DemandeSangForm', () => {
  let component: DemandeSangForm;
  let fixture: ComponentFixture<DemandeSangForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DemandeSangForm],
    }).compileComponents();

    fixture = TestBed.createComponent(DemandeSangForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
