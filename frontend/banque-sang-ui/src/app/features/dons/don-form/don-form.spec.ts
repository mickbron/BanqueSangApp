import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonForm } from './don-form';

describe('DonForm', () => {
  let component: DonForm;
  let fixture: ComponentFixture<DonForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonForm],
    }).compileComponents();

    fixture = TestBed.createComponent(DonForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
