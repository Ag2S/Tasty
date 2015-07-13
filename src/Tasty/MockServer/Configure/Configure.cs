using System;

namespace Tasty.MockServer.Configure
{
    public class Configure<RequestType, ResponseType>
    {
        public Predicate<RequestType> WouldPick { get; set; }
        public Action<ResponseType> FillUpResponse { get; set; }
    }
}
