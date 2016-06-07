using Microsoft.Practices.Unity;
using MuseCommerce.Core.Log;
using MuseCommerce.Core.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core
{
    public class Bootstrapper
    {
        public IUnityContainer Container { get; protected set; }

        protected ILog Logger { get; set; }

    }
}
