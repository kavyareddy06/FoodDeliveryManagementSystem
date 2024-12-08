import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeilveryDriversComponent } from './deilvery-drivers.component';

describe('DeilveryDriversComponent', () => {
  let component: DeilveryDriversComponent;
  let fixture: ComponentFixture<DeilveryDriversComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeilveryDriversComponent]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(DeilveryDriversComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
