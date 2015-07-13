using System;

namespace Tasty
{
    public interface IValidateClause
    {
        void Then(Action validate);
        void ThenValidateDefault();
    }

}
