namespace BackendProject.Helpers
{
	public static class FileType
	{
		public static string GetFilePath(string root, string folder, string fileName)
		{
			return Path.Combine(root, folder, fileName);
		}
		public static async Task SaveFile(string path, IFormFile photo)
		{
			using (FileStream stream = new(path, FileMode.Create))
			{
				await photo.CopyToAsync(stream);
			}
		}

	}
}
