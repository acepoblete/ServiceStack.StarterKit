using ServiceStack.Common;
using ServiceStack.Common.Web;
using ServiceStack.FluentValidation;
using ServiceStack.ServiceHost;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.ServiceModel;

namespace DaSco.StarterKit.Auth
{

    #region Domain Interface
    public interface ILogin
    {
        string UserName { get; set; }
        string Password { get; set; }
        bool RememberMe { get; set; }
        string Continue { get; set; }
    }
    #endregion

    #region Validation
    public class LoginValidator : AbstractValidator<Login>
    {
        public LoginValidator()
        {
            RuleSet(ApplyTo.Post | ApplyTo.Put, () =>
            {
                RuleFor(r => r.UserName).NotEmpty();
                RuleFor(r => r.Password).NotEmpty();
            });
        }
    }
    #endregion

    #region Request
    [Route("/login", "GET,POST")]
    public class Login : ILogin, IReturn<LoginResponse>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Continue { get; set; }
    }
    #endregion

    #region Resoponse
    public class LoginResponse : ILogin, IHasResponseStatus
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool RememberMe { get; set; }
        public string Continue { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
    #endregion

    [DefaultView("Login")]
    public class LoginService : Service
    {
        public IValidator<Login> LoginValidator { get; set; }

        public object Get(Login req)
        {
            return new HttpResult(new LoginResponse());
        }

        public object Post(Login req)
        {
            var auth = req.TranslateTo<ServiceStack.ServiceInterface.Auth.Auth>();
            var authService = TryResolve<AuthService>();
            authService.RequestContext = new HttpRequestContext(Request, Response, null);

            return authService.Post(auth);
        }
    }
}