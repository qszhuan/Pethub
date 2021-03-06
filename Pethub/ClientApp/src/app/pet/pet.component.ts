import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { GroupedPet } from "../interfaces/GroupedPet";

@Component({
  selector: 'app-pet',
  templateUrl: './pet.component.html',
  styleUrls: ['./pet.component.scss']
})
export class PetComponent {
  groupedPets: GroupedPet[];
  errorMessage: string;

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<GroupedPet[]>(baseUrl + 'pets', { params: { type: 'Cat' }}).subscribe(result => {
      this.groupedPets = result;
    }, error => {
        this.errorMessage = error;

    });
  }

}

