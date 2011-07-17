using System;
using System.Net;
using System.Web.Mvc;
using Ninject;

namespace MvcMusicStoreAdfs.ActionFilters
{
    public class RequiresAdminUserAttribute : ActionFilterAttribute
    {
        private const string AdministratorUserName = "Administrator";

        [Inject]
        public Func<User> UserQuery { get; set; }

        private User User
        {
            get { return UserQuery(); }
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Name.EndsWith(AdministratorUserName)) return;

            filterContext.Result = new ViewResult { ViewName = "Error" };
            filterContext.HttpContext.Response.StatusCode = (int)HttpStatusCode.Forbidden;
        }
    }
}