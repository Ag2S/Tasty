using System;

namespace Tasty.MockServer
{
    public interface IDoClause<ResponseType>
    {
        void Do(Action<ResponseType> fillUpResponseAction);
    }
}
