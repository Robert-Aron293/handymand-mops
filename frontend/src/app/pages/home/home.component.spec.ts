import { ComponentFixture, TestBed } from '@angular/core/testing';

import { HomeComponent } from './home.component';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let compiled: HTMLElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [HomeComponent],
    }).compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
    compiled = fixture.nativeElement as HTMLElement;
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render motto', () => {
    const fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('#home .logoBox #logo1')).toBeTruthy();
    expect(
      compiled.querySelector('#home .logoBox #logo1')?.textContent
    ).toContain('Make the most');

    expect(
      compiled.querySelector('#home .logoBox #logo2')?.textContent
    ).toContain('of your time');
  });

  it('should show "about" section', () => {
    const fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelector('.aboutContainer .aboutBox')).toBeTruthy();
  });

  it('should show "post a job" about section', () => {
    const fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
  });

  it('should show "need to authenticate" pop-up', () => {
    const fixture = TestBed.createComponent(HomeComponent);
    fixture.detectChanges();
    const compiled = fixture.nativeElement as HTMLElement;
    const hireButton = compiled.querySelector(
      '#home #hirebtn .btn'
    ) as HTMLButtonElement;
    expect(hireButton?.textContent).toContain('Hire Handyman');
    hireButton.click();
    expect(compiled.querySelector('.overlay .popup h2')?.textContent).toContain(
      'You need to authenticate in order to proceed!'
    );
  });

  it('should close "need to authenticate" pop-up', () => {
    const hireButton = compiled.querySelector(
      '#home #hirebtn .btn'
    ) as HTMLButtonElement;
    expect(hireButton?.textContent).toContain('Hire Handyman');
    hireButton.click();
    expect(
      (compiled.querySelector('.overlay') as HTMLDivElement)?.style.visibility
    ).toBe('visible');
    expect(compiled.querySelector('.overlay .popup h2')?.textContent).toContain(
      'You need to authenticate in order to proceed!'
    );
    component.closePopup();
    fixture.detectChanges();
    expect(
      (compiled.querySelector('.overlay') as HTMLDivElement)?.style.visibility
    ).toBe('hidden');
  });
});
