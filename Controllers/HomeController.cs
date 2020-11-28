using OneNX.ASP.NET.MVC.Validation.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace OneNX.ASP.NET.MVC.Validation.Controllers
{
	public class HomeController : Controller
	{
		public ActionResult Index()
		{
			var models = DBHelper.GetAll();
			return View(models);
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
			var model = new FileModel();
			//保存到服务器
			var file = Request.Files["txt_file"];
			var fileName = Path.Combine(Server.MapPath("../Uploads"), file.FileName);
			file.SaveAs(fileName);
			//存数据库
			model.FileName = file.FileName;
			model.FileType = "Metrology";
			model.UploadDate = DateTime.UtcNow.ToString();
			model.FileId = DBHelper.Create(model);

			return Json(model);
		}

		public FileStreamResult Download(string id)
		{
			var model = DBHelper.GetById(int.Parse(id));
			string filePath = Server.MapPath($"../Subrecipe/{model.SubrecipeName}");
			FileStream fs = new FileStream(filePath, FileMode.Open);
			return File(fs, "text/plain", model.SubrecipeName);
		}

		public JsonResult Compute(string id, string fileName)
		{
			//Mock doing some compute jos...
			Thread.Sleep(5000);
			// 创建文件
			var basePath = Server.MapPath("../Subrecipe");
			var subrecipeFileName = Guid.NewGuid().ToString() + ".xml";
			var subrecipeFilePath = Path.Combine(basePath, subrecipeFileName);
			FileStream fs = new FileStream(subrecipeFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite); 
			StreamWriter sw = new StreamWriter(fs); // 创建写入流
			sw.WriteLine(DateTime.Now.ToString()); // 写入内容
			sw.Close(); //关闭文件

			var model = new FileModel();
			model.FileId = int.Parse(id);
			model.SubrecipeGenStatus = "Success";
			model.SubrecipeName = subrecipeFileName;
			DBHelper.Update(model);

			return Json(model);
		}
	}
}