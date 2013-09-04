using ServiceStack.Common.Web;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.ServiceModel;

namespace DaSco.StarterKit.ServiceInterface
{

    #region Request
    [Route("/protected")]
    public class Protected { }
    #endregion

    #region Response
    public class ProtectedResponse : IHasResponseStatus
    {
        public ResponseStatus ResponseStatus { get; set; }
    }
    #endregion

    public class ProtectedService : Service
    {
        public object Get(Protected req)
        {
            return new HttpResult(new ProtectedResponse());
        }
    }
}