using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace GymManagementBLL.Services.AttachmentService
{
	public class AttachmentService : IAttachmentService
	{
		private readonly long _maxFileSize = 5 * 1024 * 1024; // 5 MB
		private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png" };
		private readonly IWebHostEnvironment _env;

		public AttachmentService(IWebHostEnvironment env)
		{
			_env = env;
		}
		public bool Delete(string fileName, string folderName)
		{
			try
			{
				if (string.IsNullOrEmpty(fileName) || string.IsNullOrEmpty(folderName))
					return false;

				var fullPath = Path.Combine(_env.WebRootPath, "images", folderName, fileName);

				if (File.Exists(fullPath))
				{
					File.Delete(fullPath);
					return true;
				}

				return false;
			}
			catch
			{
				return false;
			}
		}

		public string? Upload(IFormFile file, string folderName)
		{
			try
			{
				if (file is null || file.Length == 0)
					return null;

				if (file.Length > _maxFileSize)
					return null;

				var extension = Path.GetExtension(file.FileName).ToLower();
				if (!_allowedExtensions.Contains(extension))
					return null;

				var uploadsFolder = Path.Combine(_env.WebRootPath, "images", folderName);
				if (!Directory.Exists(uploadsFolder))
				{
					Directory.CreateDirectory(uploadsFolder);
				}

				var FileName = Guid.NewGuid().ToString() + extension;
				var filePath = Path.Combine(uploadsFolder, FileName);

				using (var fileStream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(fileStream);
				}

				return FileName;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Failed to upload file: {ex}");
				return null;
			}
		}
	}
}
