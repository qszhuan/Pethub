using GraphQL.Types;
using Pethub.Models;

namespace Pethub.Graphql
{
    public class GenderType : EnumerationGraphType<Gender>
    {
        public GenderType()
        {
            Name = "gender";
        }
        protected override string ChangeEnumCase(string val)
        {
            return val;
        }
    }
}