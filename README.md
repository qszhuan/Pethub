## PetHub

### techniques

* .netcore 3.1
* Angular
* GraphQL
* TDD(Moq)
* docker

## Test

`dotnet test` to run backend tests;

`ng test` in `PetHub` folder to run frontend tests.

## Run

### option 1

run `dotnet run` in `PetHub` folder, and then open `http://localhost:5000` to check the result

### option 2 - docker

run `docker build . -t pethub` to build docker image, then
run `docker run --rm -d -p 5000:5000 pethub` to start application,

and then open `http://localhost:5000` to check the result





