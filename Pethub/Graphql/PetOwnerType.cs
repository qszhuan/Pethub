using System.Linq;
using GraphQL.Types;
using Pethub.Models;
namespace Pethub.Graphql
{
    public class PetOwnerType : ObjectGraphType<PetOwner>
    {
        public PetOwnerType()
        {
            Name = "owner";
            Field(x => x.Name).Description("The name of people");
            Field(x => x.Gender, type:typeof(GenderType));
            Field(x => x.Age);
            Field(x => x.Pets, type: typeof(ListGraphType<PetType>))
                .Argument<StringGraphType>("Type", "type of pet")
                .Resolve(context =>
                {
                    var type = context.GetArgument<string>("type");
                    return context.Source.Pets
                        .Where(x => type == null || x.Type.ToString() == type).OrderBy(x => x.Name)
                        .ToList();
                });
        }
    }
}