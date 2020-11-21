using System.Web;
using System.Web.Mvc;

namespace OneNX.ASP.NET.MVC.Validation
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorAttribute());
		}
	}
}
