using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OneNX.ASP.NET.MVC.Validation.Models
{
	public class FileModel
	{
		public long FileId { get; set; }
		public string FileName { get; set; }
		public string FileType { get; set; }
		public string UploadDate { get; set; }
		public string SubrecipeGenStatus { get; set; }
		public string SubrecipeName { get; set; }
		public string TransferToRMSStatus { get; set; }
	}
}