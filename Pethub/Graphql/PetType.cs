using GraphQL.Types;
using Pethub.Models;
namespace Pethub.Graphql
{
    public class PetType : ObjectGraphType<Pet>
    {
        public PetType()
        {
            Name = "Pet";
            Field(x => x.Name);
            Field(x => x.Type, type: typeof(PetTypeType));
        }
    }
}