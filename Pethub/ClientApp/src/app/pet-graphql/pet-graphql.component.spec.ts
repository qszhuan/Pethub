import { async, ComponentFixture, TestBed, getTestBed } from '@angular/core/testing';
import gql from 'graphql-tag';
import { PetGraphqlComponent } from './pet-graphql.component';
import {
  ApolloTestingModule,
  ApolloTestingController,
} from 'apollo-angular/testing';


describe('PetGraphqlComponent', () => {
  let component: PetGraphqlComponent;
  let controller: ApolloTestingController;
  let fixture: ComponentFixture<PetGraphqlComponent>;
  let injector: TestBed;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      imports: [ApolloTestingModule],
      declarations: [ PetGraphqlComponent ]
    })
    .compileComponents();

    injector = getTestBed();
    controller = injector.get(ApolloTestingController);
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PetGraphqlComponent);
    component = fixture.componentInstance;
    component.ngOnInit();
    fixture.detectChanges();
    fixture.whenStable();
  });

  afterEach(() => {
    controller.verify();
  });

  it('should create and read data', () => {
    const data = {
      'data': {
        'petGroups': [
          {
            key: 'Male',
            pets: [
              { name: 'Jim', type: 'Cat' }
            ]
          }
        ]

      }
    }
    let query = gql`
          {
            petGroups(key: "Gender") {
              key
              pets(type:"Cat") {
                name
              }
            }
          }
        `;
    controller.expectOne(query).flush(data);
    expect(component).toBeTruthy();
    expect(component.groupedPets.length).toBe(1);
  });
});
