using System;
using System.Collections.Generic;
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
    public interface IRegister
    {
        string UserName { get; set; }
        string FirstName { get; set; }
        string LastName { get; set; }
        string DisplayName { get; set; }
        string Email { get; set; }
        string Password { get; set; }
        bool? AutoLogin { get; set; }
        string Continue { get; set; }
    }
    #endregion

    #region Request
    [Route("/register", "GET,POST")]
    public class Register : IRegister, IReturn<RegisterResponse>
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? AutoLogin { get; set; }
        public string Continue { get; set; }
    }
    #endregion

    #region Response
    public class RegisterResponse : IRegister, IHasResponseStatus
    {
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool? AutoLogin { get; set; }
        public string Continue { get; set; }
        public ResponseStatus ResponseStatus { get; set; }
    }
    #endregion

    [DefaultView("Register")]
    public class RegisterService : Service
    {

        public IValidator<Register> RegisterValidator { get; set; }

        public object Get(Register req)
        {
            return new HttpResult(new RegisterResponse());
        }

        public object Post(Register req)
        {
            var registration = req.TranslateTo<Registration>();

            var registrationService = TryResolve<RegistrationService>();
            registrationService.RequestContext = new HttpRequestContext(Request, Response, null);

            try
            {
                return registrationService.Post(registration);
            }
            catch (ArgumentNullException e)
            {
                var resp = req.TranslateTo<RegisterResponse>();

                resp.ResponseStatus = new ResponseStatus{
                    Errors = new List<ResponseError>{
                         new ResponseError
                             {
                                 ErrorCode = e.ToErrorCode(),
                                 FieldName = e.ParamName,
                                 Message = e.Message
                             }
                    }
                };
                                                   
                return resp;
            }
        }
    }
}