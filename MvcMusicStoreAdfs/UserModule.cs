using System;
using System.Linq;
using System.Threading;
using System.Web;
using Microsoft.IdentityModel.Claims;
using Ninject;
using Ninject.Modules;

namespace MvcMusicStoreAdfs
{
    public class UserModule : NinjectModule
    {
        public override void Load()
        {
            Bind<Claim>().ToMethod(ctx => LoadLoggedInUserClaims());
            Bind<User>().ToSelf().InRequestScope();
            Bind<Func<User>>().ToMethod(ctx => () => ctx.Kernel.Get<User>());
        }

        private Claim LoadLoggedInUserClaims()
        {
            if (HttpContext.Current.Session["claim"] != null)
            {
                return (Claim) HttpContext.Current.Session["claim"];
            }

            var id = Thread.CurrentPrincipal.Identity as IClaimsIdentity;

            string userName = null;

            if (id != null)
                foreach (var claim in id.Claims.Where(claim => claim.ClaimType == MusicStoreClaimTypes.UserName))
                {
                    userName = claim.Value;
                }

            return new Claim
            {
                UserName = userName,
            };
        }
    }
}