import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CategoriaNovaComponent } from './categoria-nova.component';

describe('CategoriaNovaComponent', () => {
  let component: CategoriaNovaComponent;
  let fixture: ComponentFixture<CategoriaNovaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CategoriaNovaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CategoriaNovaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
