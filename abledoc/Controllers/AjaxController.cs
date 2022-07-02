using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace abledoc.Controllers
{
    public class AjaxController : Controller
    {
		private readonly IWebHostEnvironment env;

		public AjaxController(IWebHostEnvironment env) => this.env = env;


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<string> UploadImages() // MUST BE "ASYNC"
		{
			return await Task.Run(() =>
			{
				var dir = env.WebRootPath + "\\uploads";
				try
				{
					if (!Directory.Exists(dir))
						Directory.CreateDirectory(dir); // make sure there are appropriate permissions on the wwwroot folder
				}
				catch (Exception ex)
				{
					Response.StatusCode = 500; // SERVER ERROR
					return ex.Message.ToString();
				}
				var ret = string.Empty; // return value
				for (int i = 0; i < Request.Form.Files.Count; i++)
				{
					if (Request.Form.Files[i].Length > 0)
					{
						if (Request.Form.Files[i].ContentType.ToLower().StartsWith("image/")) // make sure it is an image; can be omitted
						{
							try
							{
								// make sure that this directory has write permissions; use GUID in name to generate unique file names
								var file = '\\' + System.Guid.NewGuid().ToString("N") + '-' + Request.Form.Files[i].FileName;
								using (FileStream fs = new FileStream(dir + file, FileMode.Create, FileAccess.Write))
								{
									const int bufsize = 2048000;
									byte[] buffer = new byte[bufsize];
									using (Stream stream = Request.Form.Files[i].OpenReadStream())
									{
										int b = stream.Read(buffer, 0, bufsize);
										int written = b;
										while (b > 0)
										{
											fs.Write(buffer, 0, b);
											b = stream.Read(buffer, 0, bufsize);
											written += b;
										}
									}
								}
								ret += (i > 0 ? "|" : "") + "\\uploads" + file; // just return a string with the file names separated by a | because it is less code than JSON
							}
							catch (Exception ex)
							{
								Response.StatusCode = 500; // SERVER ERROR
								return ex.Message.ToString();
							}
						}
					}
				}
				return ret;
			});
		}
	}
}
