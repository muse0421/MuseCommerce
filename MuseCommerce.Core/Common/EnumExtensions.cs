using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.WebPages.Html;

namespace MuseCommerce.Core.Common
{
    public static class EnumExtensions
    {
        public static IEnumerable<SelectListItem> EnumToEnumerable<TEnum>()
        {
            IEnumerable<TEnum> values = Enum.GetValues(typeof(TEnum))
                .Cast<TEnum>();

            IEnumerable<SelectListItem> items =
                from value in values
                select new SelectListItem
                {
                    Text = value.ToString(),
                    Value = value.ToString()
                };

            return items;
        }
    }
}
