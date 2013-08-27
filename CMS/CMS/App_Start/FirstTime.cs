using CMS.Filters;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

[assembly: PreApplicationStartMethod(typeof(FirstTime), "Initializer")]

public static class FirstTime
{
    public static void Initializer()
    {
        Microsoft.Web.Infrastructure.DynamicModuleHelper.DynamicModuleUtility
            .RegisterModule(typeof(UserInitializationModule));
    }
}

public class UserInitializationModule : IHttpModule
{
    private static bool initialized;
    private static object lockObject = new object();

    private const string _username = "admin";
    private const string _password = "admin";
    private const string _role = "admin";

    void IHttpModule.Init(HttpApplication context)
    {
        lock (lockObject)
        {
            if (!initialized)
            {
                new InitializeSimpleMembershipAttribute().OnActionExecuting(null);

                if (!WebSecurity.UserExists(_username))
                    WebSecurity.CreateUserAndAccount(_username, _password);

                if (!Roles.RoleExists(_role))
                    Roles.CreateRole(_role);

                if (!Roles.IsUserInRole(_username, _role))
                    Roles.AddUserToRole(_username, _role);
            }
            initialized = true;
        }
    }

    void IHttpModule.Dispose() { }
}