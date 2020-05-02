import { Component, OnInit } from '@angular/core';
import { Apollo } from 'apollo-angular';
import gql from 'graphql-tag';
import { GroupedPet } from "../interfaces/GroupedPet";

@Component({
  selector: 'app-pet-graphql',
  templateUrl: './pet-graphql.component.html',
  styleUrls: ['../pet/pet.component.scss']
})
export class PetGraphqlComponent implements OnInit {
  public groupedPets: GroupedPet[];
  constructor(private apollo: Apollo) { }

  ngOnInit() {
    this.apollo
      .watchQuery({
        query: gql`
          {
            petGroups(key: "Gender") {
              key
              pets(type:"Cat") {
                name
              }
            }
          }
        `,
      })
      .valueChanges.subscribe(result => {
        this.groupedPets = result.data && result.data['petGroups'];
      });
  }

}
