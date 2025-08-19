import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CertEditComponent } from './cert-edit.component';

describe('CertEditComponent', () => {
  let component: CertEditComponent;
  let fixture: ComponentFixture<CertEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CertEditComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CertEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
