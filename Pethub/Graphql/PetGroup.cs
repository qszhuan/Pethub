using System.Linq;
using GraphQL.Types;
using Pethub.Models;

namespace Pethub.Graphql
{
    public class PetGroup : ObjectGraphType<Models.PetGroup>
    {
        public PetGroup()
        {
            Name = "PetGroup";
            Field(x => x.Key).Description("group key");
            Field(x => x.Pets, type:typeof(ListGraphType<PetType>))
                .Argument<StringGraphType>("Type", "type of pet")
                .Resolve(context =>
                {
                    var type = context.GetArgument<string>("type");
                    return context.Source.Pets
                        .Where(x => type == null || x.Type.ToString() == type).OrderBy(x => x.Name)
                        .ToList();
                })
                ; ;
        }
    }
}