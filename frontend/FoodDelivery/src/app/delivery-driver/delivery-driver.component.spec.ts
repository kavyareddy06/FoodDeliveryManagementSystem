import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeliveryDriverComponent } from './delivery-driver.component';

describe('DeliveryDriverComponent', () => {
  let component: DeliveryDriverComponent;
  let fixture: ComponentFixture<DeliveryDriverComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeliveryDriverComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeliveryDriverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
