using GraphQL.Types;

namespace Pethub.Graphql
{
    public class PetTypeType : EnumerationGraphType<Models.PetType>
    {
        protected override string ChangeEnumCase(string val)
        {
            return val;
        }
    }
}