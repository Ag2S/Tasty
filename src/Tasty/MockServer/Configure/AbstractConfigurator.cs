using System;

namespace Tasty.MockServer.Configure
{
    public abstract class AbstractConfigurator<TargetType, RequestType, ResponseType> : IWhenClause<RequestType, ResponseType>, IDoClause<ResponseType>
    {
        protected readonly TargetType _target;
        protected readonly Configure<RequestType, ResponseType> _currentConfigure;

        public AbstractConfigurator(TargetType target)
        {
            _target = target;
            _currentConfigure = new Configure<RequestType, ResponseType>();
        }

        public virtual IDoClause<ResponseType> When(Predicate<RequestType> wouldPickPredicate)
        {
            _currentConfigure.WouldPick = wouldPickPredicate;
            return this;
        }

        public virtual void Do(Action<ResponseType> fillUpResponseAction)
        {
            _currentConfigure.FillUpResponse = fillUpResponseAction;
            ApplyConfigure();
        }

        protected abstract void ApplyConfigure();
    }
}
