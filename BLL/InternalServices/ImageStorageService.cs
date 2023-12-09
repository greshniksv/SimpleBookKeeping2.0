using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace BLL.InternalServices
{
	//public class ImageStorageService
	//{
	//	private readonly string _dbPath;

	//	public ImageStorage()
	//	{
	//		_dbPath = System.Web.Hosting.HostingEnvironment.MapPath(@"~/App_Data/Files/");
	//	}

	//	public void Move(string filename, string path)
	//	{
	//		var list = Find(Path.GetFileNameWithoutExtension(filename));
	//		foreach (var fileInfo in list)
	//		{
	//			fileInfo.Delete();
	//		}

	//		var file = GetFilePath(filename);
	//		if (File.Exists(file))
	//		{
	//			File.Delete(file);
	//		}
	//		File.Move(path, file);
	//	}

	//	public void CreateSmallView(string filename)
	//	{
	//		var file = GetFilePath(filename);
	//		if (!File.Exists(file))
	//		{
	//			throw new Exception("File not found");
	//		}

	//		try
	//		{
	//			var image = ScaleImage(file, 1024, 768);
	//			var smallImagePath = GetSmallFilePath(filename);

	//			image.Save(smallImagePath, ImageFormat.Png);
	//		}
	//		catch (Exception)
	//		{
	//		}
	//	}

	//	public FileInfo Get(string filename, bool small = true)
	//	{
	//		var file = GetFilePath(filename);
	//		var smallImagePath = GetSmallFilePath(filename);

	//		if (!File.Exists(file))
	//		{
	//			throw new Exception("File not found");
	//		}

	//		if (small && !File.Exists(smallImagePath))
	//		{
	//			small = false;
	//		}

	//		return small ? new FileInfo(smallImagePath) : new FileInfo(file);
	//	}

	//	public List<FileInfo> Find(string id)
	//	{
	//		var file = GetFilePath(id);

	//		var dir = Path.GetDirectoryName(file);
	//		if (dir == null)
	//		{
	//			return null;
	//		}

	//		var fileItem = Directory.GetFiles(dir).Where(x => Path.GetFileNameWithoutExtension(x) == id);
	//		if (fileItem == null)
	//		{
	//			throw new Exception("File not found");
	//		}

	//		return fileItem.Select(x => new FileInfo(x)).ToList();
	//	}

	//	public void Delete(string filename)
	//	{
	//		var file = GetFilePath(filename);
	//		var smallImagePath = GetSmallFilePath(filename);
	//		if (File.Exists(file))
	//		{
	//			File.Delete(file);
	//		}
	//		if (File.Exists(smallImagePath))
	//		{
	//			File.Delete(smallImagePath);
	//		}
	//	}

	//	private string GetFilePath(string filename)
	//	{
	//		var item = filename.Trim(' ');
	//		var path = Path.Combine(_dbPath, item[0].ToString(), item[1].ToString());
	//		if (!Directory.Exists(path))
	//		{
	//			Directory.CreateDirectory(path);
	//		}
	//		return Path.Combine(path, filename);
	//	}

	//	private string GetSmallFilePath(string filename)
	//	{
	//		var file = GetFilePath(filename);
	//		return string.Concat(
	//			Path.Combine(Path.GetDirectoryName(file), Path.GetFileNameWithoutExtension(file)),
	//			"_small",
	//			Path.GetExtension(file));
	//	}

	//	public static Image ScaleImage(string imagePath, int maxWidth, int maxHeight)
	//	{
	//		using (var image = Image.FromFile(imagePath))
	//		{
	//			var ratioX = (double)maxWidth / image.Width;
	//			var ratioY = (double)maxHeight / image.Height;
	//			var ratio = Math.Min(ratioX, ratioY);

	//			var newWidth = (int)(image.Width * ratio);
	//			var newHeight = (int)(image.Height * ratio);

	//			var newImage = new Bitmap(newWidth, newHeight);

	//			using (var graphics = Graphics.FromImage(newImage))
	//				graphics.DrawImage(image, 0, 0, newWidth, newHeight);
	//			return newImage;
	//		}
	//	}
	//}
}
