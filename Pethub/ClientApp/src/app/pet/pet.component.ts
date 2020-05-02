import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-pet',
  templateUrl: './pet.component.html',
  styleUrls: ['./pet.component.scss']
})
export class PetComponent {
  public groupedPets: GroupedPet[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<GroupedPet[]>(baseUrl + 'pets', { params: { type: 'Cat' }}).subscribe(result => {
      this.groupedPets = result;
    }, error => console.error(error));
  }

}

interface GroupedPet {
  key: string,
  pets: Pet[]
}

interface Pet {
  name: string,
  type: string
}
