using MuseCommerce.Core;
using MuseCommerce.Core.Log;
using MuseCommerce.Core.Security;
using MuseCommerce.Data.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Practices.Unity;

namespace MuseCommerce.Web
{
    public class PlatformWebBootstrapper : Bootstrapper
    {
        public void Run()
        {
            Container = new UnityContainer();  

            Logger = new EntLibLog();

            Container.RegisterType<ISecurityService, SecurityService>();
        }
    }
}