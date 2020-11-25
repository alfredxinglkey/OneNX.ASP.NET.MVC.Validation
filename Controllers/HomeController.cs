using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OneNX.ASP.NET.MVC.Validation.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}

		public JsonResult Upload()
		{
			uploadResult result = new uploadResult();
			var oFile = Request.Files["txt_file"];
			result.fileName = oFile.FileName;
			Stream sm = oFile.InputStream;
			byte[] bt = new byte[sm.Length];
			sm.Read(bt, 0, (int)sm.Length);
			FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory + oFile.FileName, FileMode.Create);
			fs.Write(bt, 0, bt.Length);
			fs.Close();
			fs.Dispose();
			sm.Close();
			sm.Dispose();
			return Json(result, JsonRequestBehavior.AllowGet);
		}

		public class uploadResult
		{
			public string fileName { get; set; }
			public string error { get; set; }
		}
	}
}