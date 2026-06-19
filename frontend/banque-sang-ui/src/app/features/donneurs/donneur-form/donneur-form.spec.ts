import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonneurForm } from './donneur-form';

describe('DonneurForm', () => {
  let component: DonneurForm;
  let fixture: ComponentFixture<DonneurForm>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonneurForm],
    }).compileComponents();

    fixture = TestBed.createComponent(DonneurForm);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
