using System.Web;
using System.Web.Mvc;

namespace Atendimento.WebApi
{
    /// <summary>
    /// Filter Configuration
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Filter Register
        /// </summary>
        /// <param name="filters"></param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
