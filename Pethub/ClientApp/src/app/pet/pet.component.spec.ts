import { async, ComponentFixture, TestBed, getTestBed} from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { PetComponent } from './pet.component';
import {GroupedPet} from  '../interfaces/GroupedPet';

describe('PetComponent', () => {
  let component: PetComponent;
  let fixture: ComponentFixture<PetComponent>;
  let injector: TestBed;
  let httpMock: HttpTestingController;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      declarations: [PetComponent],
      providers: [{ provide: 'BASE_URL', useValue: 'http://localhost/', deps: [] }]
    })
    .compileComponents();

    injector = getTestBed();
    httpMock = injector.get(HttpTestingController);
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PetComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create and fetch data', () => {
    const groupedPets = [
      {
        key: 'Male',
        pets: [
          {name:'Jim', type: 'Cat'}
        ]
      }
    ];

    const baseUrl = injector.get('BASE_URL');
    const req = httpMock.expectOne(`${baseUrl}pets?type=Cat`);
    expect(req.request.method).toBe("GET");
    req.flush(groupedPets);

    expect(component).toBeTruthy();
    expect(component.groupedPets.length).toBe(1);
  });


});
