using System;

namespace Tasty
{
    public interface IArrangeClause
    {
        ITestClause With(Action arrange);
        ITestClause WithDefaultArrangement();
        ITestClause WithDefaultArrangementAndAdditionally(Action arrange);
    }
}
