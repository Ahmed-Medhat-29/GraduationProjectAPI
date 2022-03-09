using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace GraduationProjectAPI.Utilities
{
	public class FileManager
	{
		private readonly string _directoryPath;
		private readonly IFormFile _file;

		public FileManager(string directoryPath, IFormFile file)
		{
			_directoryPath = directoryPath;
			_file = file;
		}

		public async Task<string> SaveAsync(string fileName)
		{
			if (!IsFileValid())
				return null;

			InitializePath();
			var name = SetFileName(fileName);
			await WriteAsync(_directoryPath + name);
			return name;
		}

		private bool IsFileValid()
		{
			return (_file != null && _file.Length > 0);
		}

		private void InitializePath()
		{
			if (!new FileInfo(_directoryPath).Exists)
				Directory.CreateDirectory(_directoryPath);
		}

		private async Task WriteAsync(string fullPath)
		{
			using (var stream = new FileStream(fullPath, FileMode.Create))
				await _file.CopyToAsync(stream);
		}

		private string SetFileName(string name)
		{
			return name + Path.GetExtension(_file.FileName);
		}
	}
}
