using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MuseCommerce.Core.Security
{
    public static class PredefinedPermissions
    {
        public const string ModuleQuery = "platform:module:read",
             ModuleAccess = "platform:module:access",
             ModuleManage = "platform:module:manage";
    }
}
