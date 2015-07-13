using System;
using Tasty.TestData;
using Tasty.Utility;

namespace Tasty
{
    public abstract class Tests
    {
        protected virtual void ArrangeDefault()
        {
        }

        protected virtual void TestDefault()
        {
        }

        protected virtual void ValidateDefault()
        {
        }

        protected virtual void InitializeTest()
        {
            _state = this.Create().NestedNew<ArrangeState>();
            TestData = new TestDataContainer();
        }

        protected virtual void CleanUpTest()
        {
        }

        protected void Arrange(Action arrange)
        {
            _state.Arrange(arrange);
        }

        protected void Test(Action test)
        {
            _state.Test(test);
        }

        protected void Validate(Action validate)
        {
            _state.Validate(validate);
        }

        protected IArrangeClause StartTest()
        {
            return this.Create().NestedNew<Servant>();
        }

        protected ITestDataContainer TestData { get; private set; }

        #region Servant
        private class Servant : InnerClass<Tests>, IArrangeClause, ITestClause, IValidateClause
        {
            public ITestClause With(Action arrange)
            {
                Outer.Arrange(arrange);
                return this;
            }

            public ITestClause WithDefaultArrangement()
            {
                return With(Outer.ArrangeDefault);
            }

            public ITestClause WithDefaultArrangementAndAdditionally(Action arrange)
            {
                return With(() =>
                {
                    Outer.ArrangeDefault();
                    arrange();
                });
            }

            public IValidateClause That(Action test)
            {
                Outer.Test(test);
                return this;
            }

            public IValidateClause ThatTestDefault()
            {
                return That(Outer.TestDefault);
            }

            public void Then(Action validate)
            {
                Outer.Validate(validate);
            }

            public void ThenValidateDefault()
            {
                Then(Outer.ValidateDefault);
            }
        }
        #endregion

        #region State
        private abstract class State : InnerClass<Tests>
        {
            public virtual void Arrange(Action arrange)
            {
                throw new InvalidOperationException();
            }

            public virtual void Test(Action test)
            {
                throw new InvalidOperationException();
            }

            public virtual void Validate(Action validate)
            {
                throw new InvalidOperationException();
            }
        }

        private class ArrangeState : State
        {
            public override void Arrange(Action arrange)
            {
                arrange();
                Outer._state = Outer.Create().NestedNew<TestState>();
            }
        }

        private class TestState : State
        {
            public override void Test(Action test)
            {
                test();
                Outer._state = Outer.Create().NestedNew<ValidationState>();
            }
        }

        private class ValidationState : State
        {
            public override void Validate(Action validate)
            {
                validate();
                Outer._state = Outer.Create().NestedNew<EndState>();
            }
        }

        private class EndState : State
        {
        }

        private State _state;
        #endregion
    }
}
