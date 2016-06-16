using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.Practices.Unity;

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
            if (_permissions==null)
            {
                return false;
            }
            if ( _permissions.Length == 0)
            {
                return false;               
            }
            if (!IsAuthorized(SecurityService, IIdentity))
            {
                return false;
            }

            return false;
        }

        [Dependency]
        public ISecurityService SecurityService { set; get; }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            var isAuthorized = IIdentity.IsAuthenticated;

            if (isAuthorized && _permissions.Length > 0)
            {
                isAuthorized = IsAuthorized(SecurityService, IIdentity);
            }

            if (!isAuthorized)
            {
                string path = filterContext.HttpContext.Request.Path;
                var routeValue = new RouteValueDictionary { 
                    { "Controller", "Home"}, 
                    { "Action", "UnAuthorized"},
                    { "ReturnUrl", path}
                };

                filterContext.Result = new RedirectToRouteResult("Default", routeValue);
            }
        }
    }
}
