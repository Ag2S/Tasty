using System;

namespace Tasty.MockServer
{
    public interface IWhenClause<RequestType, ResponseType>
    {
        IDoClause<ResponseType> When(Predicate<RequestType> wouldPickPredicate);
    }
}
