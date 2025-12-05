using Microsoft.AspNetCore.Http;

namespace GymManagementBLL.Services.AttachmentService
{
	public interface IAttachmentService
	{
		// Upload 
		string? Upload(IFormFile file, string FolderName);
		bool Delete(string fileName, string folderName);
	}
}
