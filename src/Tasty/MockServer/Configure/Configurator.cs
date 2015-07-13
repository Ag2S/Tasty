
namespace Tasty.MockServer.Configure
{
    public class Configurator<RequestType, ResponseType> : AbstractConfigurator<ConfigureCollection<RequestType, ResponseType>, RequestType, ResponseType>
    {
        public Configurator(ConfigureCollection<RequestType, ResponseType> target)
            : base(target)
        {
        }

        protected override void ApplyConfigure()
        {
            _target.Add(_currentConfigure);
        }
    }
}
