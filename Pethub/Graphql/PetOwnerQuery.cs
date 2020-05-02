using System.Linq;
using GraphQL.Types;
using Pethub.Services;
namespace Pethub.Graphql
{
    public class PetOwnerQuery : ObjectGraphType
    {
        public PetOwnerQuery(IPetOwnerService petOwnerService)
        {
            var petOwners = petOwnerService.GetOwners().Result;

            Field<PetOwnerType>("owner",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "name", Description = "name of pet owner" }
                ),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    return petOwners.FirstOrDefault(x => x.Name == name);
                });

            Field<ListGraphType<PetOwnerType>>(
                name: "Owners",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "name", Description = "name of pet owner" },
                    new QueryArgument<StringGraphType> { Name = "gender" },
                    new QueryArgument<StringGraphType> { Name = "age" }
                ),
                resolve: context =>
                {
                    var name = context.GetArgument<string>("name");
                    var gender = context.GetArgument<string>("gender");
                    var age = context.GetArgument<int?>("age");
            
                    return petOwners
                        .Where(x => name == null || x.Name == name )
                        .Where(x => gender == null || x.Gender.ToString() == gender)
                        .Where(x => age == null || x.Age == age);
                });
            
            Field<ListGraphType<PetGroup>>(
                name: "PetGroups",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "key" }
                ),
                resolve: context =>
                {
                    var key = context.GetArgument<string>("key");
            
                    var petGroup = petOwners
                        .GroupBy(x => key switch
                            {
                                "name" => x.Name,
                                "age" => x.Age.ToString(),
                                _ => x.Gender.ToString()
                            }, 
                            x => x.Pets,
                            (k, enumerable) =>
                            {
                                return new Models.PetGroup { Key = k.ToString(), Pets = enumerable.SelectMany(x=>x).ToList() };
                            })
                        .OrderByDescending(x=>x.Key);
            
            
                    return petGroup.ToList();
                });
        }
    }
}