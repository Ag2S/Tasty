using System;
using System.Collections.Generic;
using System.Linq;

namespace Tasty.MockServer.Configure
{
    public class ConfigureCollection<RequestType, ResponseType>
    {
        private readonly LinkedList<Configure<RequestType, ResponseType>> _list = new LinkedList<Configure<RequestType, ResponseType>>();

        public ConfigureCollection(Action<ResponseType> defaultAction)
        {
            _list.AddLast(new Configure<RequestType, ResponseType>{ WouldPick = request => true, FillUpResponse = defaultAction });
        }

        public void Add(Configure<RequestType, ResponseType> configure)
        {
            _list.AddBefore(_list.Last, configure);
        }

        public Action<ResponseType> GetResponderFor(RequestType request)
        {
            return _list.First(which => which.WouldPick(request)).FillUpResponse;
        }
    }
}
