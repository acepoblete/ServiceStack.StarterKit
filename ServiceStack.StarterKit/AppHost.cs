using DaSco.StarterKit.Auth;
using DaSco.StarterKit.Razor;
using ServiceStack.Common;
using ServiceStack.Configuration;
using ServiceStack.FluentValidation;
using ServiceStack.OrmLite;
using ServiceStack.Razor;
using ServiceStack.ServiceInterface;
using ServiceStack.ServiceInterface.Auth;
using ServiceStack.ServiceInterface.Validation;
using ServiceStack.WebHost.Endpoints;

namespace DaSco.StarterKit
{
    public class AppHost : AppHostBase
    {
        public AppHost() //Tell ServiceStack the name and where to find your web services
            : base("SerivceStack Starter Kit", typeof (AppHost).Assembly)
        {
        }

        public override void Configure(Funq.Container container)
        {
            //Set JSON web services to return idiomatic JSON camelCase properties
            ServiceStack.Text.JsConfig.EmitCamelCaseNames = true;

            //Uncomment to change the default ServiceStack configuration
            //SetConfig(new EndpointHostConfig {
            //});

            //Enable Authentication
            ConfigureAuth(container);

            //Register all your dependencies
            //container.Register(new TodoRepository());		

            //Enable the validation feature
            Plugins.Add(new ValidationFeature());
            //this will auto wire up validation
//            /container.RegisterValidators(typeof(AppHost).Assembly);

            //load up Razor View Engin for HTML 
            Plugins.Add(new RazorFormat());
        }

        private void ConfigureAuth(Funq.Container container)
        {
            var appSettings = new AppSettings();

            //Default route: /auth/{provider}
            Plugins.Add(new AuthFeature(() => new CustomUserSession(),
                                        new IAuthProvider[]
                                            {
                                                new CredentialsAuthProvider(appSettings), 
//					new FacebookAuthProvider(appSettings), 
//					new TwitterAuthProvider(appSettings), 
//					new BasicAuthProvider(appSettings), 
                                            }));

            //Default route: /register
            Plugins.Add(new RegistrationFeature());

            //Requires ConnectionString configured in Web.Config
            container.Register<IDbConnectionFactory>(c =>
                                                     new OrmLiteConnectionFactory(":memory:", false,
                                                                                  SqliteDialect.Provider));

            container.Register<IUserAuthRepository>(c =>
                                                    new OrmLiteAuthRepository(c.Resolve<IDbConnectionFactory>()));

            var authRepo = (OrmLiteAuthRepository) container.Resolve<IUserAuthRepository>();
            authRepo.CreateMissingTables();
        }
    }
}
