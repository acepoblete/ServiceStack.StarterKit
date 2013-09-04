using ServiceStack.Razor;
using ServiceStack.ServiceInterface.Auth;

namespace DaSco.StarterKit.Razor
{
	//A customizeable typed UserSession that can be extended with your own properties
	public class CustomUserSession : AuthUserSession
	{
		public string CustomProperty { get; set; }
	}

	public abstract class PageBase : ViewPage
	{
        protected CustomUserSession UserSession
        {
            get
            {
                return GetSession<CustomUserSession>();
            }
        }
	}

    public abstract class PageBase<TModel> : ViewPage<TModel> where TModel : class
    {
        protected CustomUserSession UserSession
        {
            get
            {
                return GetSession<CustomUserSession>();
            }
        }
    }
}