using System;

namespace Tasty
{
    public interface ITestClause
    {
        IValidateClause That(Action test);
        IValidateClause ThatTestDefault();
    }
}
