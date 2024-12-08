import { TestBed } from '@angular/core/testing';

import { OrdertrackingService } from './ordertracking.service';

describe('OrdertrackingService', () => {
  let service: OrdertrackingService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(OrdertrackingService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
