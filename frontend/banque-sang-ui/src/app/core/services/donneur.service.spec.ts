import { TestBed } from '@angular/core/testing';

import { DonneurService } from './donneur.service';

describe('DonneurService', () => {
  let service: DonneurService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(DonneurService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
