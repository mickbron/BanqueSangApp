import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonneursList } from './donneurs-list';

describe('DonneursList', () => {
  let component: DonneursList;
  let fixture: ComponentFixture<DonneursList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonneursList],
    }).compileComponents();

    fixture = TestBed.createComponent(DonneursList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
