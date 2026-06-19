import { TestBed } from '@angular/core/testing';

import { TestBiologiqueService } from './test-biologique.service';

describe('TestBiologiqueService', () => {
  let service: TestBiologiqueService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TestBiologiqueService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
