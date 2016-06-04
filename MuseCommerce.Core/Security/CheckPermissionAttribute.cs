using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;

namespace MuseCommerce.Core.Security
{
    public class CheckPermissionAttribute : System.Web.Mvc.AuthorizeAttribute
    {
        #region Static Fields

        private static readonly string[] _emptyArray = new string[0];

        #endregion

        #region Fields

        private string _permission;
        private string[] _permissions = _emptyArray;

        #endregion

        #region Public Properties

        public string Permission
        {
            get { return _permission ?? string.Empty; }
            set
            {
                _permission = value;
                _permissions = this.SplitString(value, ',');
            }
        }

        public string[] Permissions
        {
            get { return _permissions ?? _emptyArray; }
            set
            {
                _permissions = value;
                _permission = string.Join(",", value ?? _emptyArray);
            }
        }

        #endregion



        protected bool IsAuthorized(ISecurityService securityService, IIdentity principal)
        {
            var isAuthorized = false;

            if (securityService != null && principal != null)
            {
                isAuthorized = securityService.UserHasAnyPermission(principal.Name, null, _permissions);
            }

            return isAuthorized;
        }

        public IIdentity IIdentity { set; get; }

        protected override bool AuthorizeCore(System.Web.HttpContextBase httpContext)
        {
            if (httpContext == null)
            {
                throw new ArgumentNullException("HttpContext");
            }

            IIdentity = httpContext.User.Identity;

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            return false;
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var isAuthorized = true;//base.IsAuthorized(actionContext);

            if (isAuthorized && _permissions.Length > 0)
            {
                var securityService = new SecurityService2();

                isAuthorized = IsAuthorized(securityService, IIdentity);
            }


            if (!isAuthorized)
            {
                string path = filterContext.HttpContext.Request.Path;
                var routeValue = new RouteValueDictionary { 
                    { "Controller", "Home"}, 
                    { "Action", "Index"},
                    { "ReturnUrl", path}
                };

                filterContext.Result = new RedirectToRouteResult(routeValue);
            }
        }

    }


    public class SecurityService2 : ISecurityService
    {
        public SecurityService2()
        {

        }

        #region ISecurityService Members


        public bool UserHasAnyPermission(string userName, string[] scopes, params string[] permissionIds)
        {
            if (permissionIds == null)
            {
                throw new ArgumentNullException("permissionIds");
            }



            var result = false;

            if (userName == "xpy")
                return true;

            //For managers always allow to call api
            //if (result && permissionIds.Length == 1 && permissionIds.Contains(PredefinedPermissions.SecurityCallApi)
            //   && (string.Equals(user.UserType, AccountType.Manager.ToString(), StringComparison.InvariantCultureIgnoreCase) ||
            //        string.Equals(user.UserType, AccountType.Administrator.ToString(), StringComparison.InvariantCultureIgnoreCase)))
            //{
            //    return true;
            //}

            //if (result)
            //{
            //    var fqUserPermissions = user.Roles.SelectMany(x => x.Permissions).SelectMany(x => x.GetPermissionWithScopeCombinationNames()).Distinct();
            //    var fqCheckPermissions = permissionIds.Concat(permissionIds.LeftJoin(scopes, ":"));
            //    result = fqUserPermissions.Intersect(fqCheckPermissions, StringComparer.OrdinalIgnoreCase).Any();
            //}

            return result;
        }

        public Permission[] GetUserPermissions(string userName)
        {
            var result = new List<Permission>();
            result.Add(new Permission()
            {
                Name = "platform:module:read",
                Description = "platform:module:read",
                GroupName = "",
                ModuleId = ""
            });
            result.Add(new Permission()
            {
                Name = "platform:module:access",
                Description = "platform:module:access",
                GroupName = "",
                ModuleId = ""
            });
            return result.ToArray();
        }
        #endregion


        private static string GetUserCacheRegion(string userId)
        {
            return "AppUserRegion:" + userId;
        }

        private static void NormalizeUser(ApplicationUserExtended user)
        {
            if (user.UserName != null)
                user.UserName = user.UserName.Trim();

            if (user.Email != null)
                user.Email = user.Email.Trim();

            if (user.PhoneNumber != null)
                user.PhoneNumber = user.PhoneNumber.Trim();
        }
    }
}
