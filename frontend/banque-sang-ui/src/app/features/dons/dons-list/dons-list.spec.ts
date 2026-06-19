import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DonsList } from './dons-list';

describe('DonsList', () => {
  let component: DonsList;
  let fixture: ComponentFixture<DonsList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DonsList],
    }).compileComponents();

    fixture = TestBed.createComponent(DonsList);
    component = fixture.componentInstance;
    await fixture.whenStable();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
