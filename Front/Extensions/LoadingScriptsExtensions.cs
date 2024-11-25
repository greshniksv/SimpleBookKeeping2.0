using System.Reflection;
using System.Text;

namespace Front.Extensions
{
	public static class LoadingScriptsExtensions
	{
		public async static Task LoadFilesAsync(this WebApplication app)
		{
			var pwd = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);

			var priorityList = new List<KeyValuePair<string, int>>() {
				new KeyValuePair<string, int>("session.js", 0),
				new KeyValuePair<string, int>("tools.js", 1),
				new KeyValuePair<string, int>("dialog-base.js", 2),
			};

			var output = new StreamWriter("C:\\Data\\Projects\\SimpleBookKeeping2.0\\Front\\src\\libs\\sbk\\sbk.min.js", Encoding.UTF8,
				new FileStreamOptions() { Mode = FileMode.Create, Access = FileAccess.Write });

			await output.WriteLineAsync($"\n\n// -= Auto generated {DateTime.Now:dd-MM-yyy HH:mm:ss} =-");
			var directory = new DirectoryInfo("C:\\Data\\Projects\\SimpleBookKeeping2.0\\Front\\src\\libs\\sbk");

			var fileList = new List<KeyValuePair<FileInfo, int>>();
			int position = 100;
			foreach (FileInfo file in directory.GetFiles("*.js", SearchOption.AllDirectories))
			{
				var prior = priorityList.FirstOrDefault(x => file.Name.ToLower().Contains(x.Key));

				fileList.Add(!default(KeyValuePair<string, int>).Equals(prior)
					? new KeyValuePair<FileInfo, int>(file, prior.Value)
					: new KeyValuePair<FileInfo, int>(file, position++));
			}

			foreach (KeyValuePair<FileInfo, int> fileItem in fileList.OrderBy(x => x.Value))
			{
				if (fileItem.Key.Name.Contains("sbk.min.js"))
				{
					continue;
				}

				await output.WriteLineAsync($"\n\n// #### {fileItem.Key.Name}");
				await output.FlushAsync();

				var openStream = fileItem.Key.OpenRead();
				await openStream.CopyToAsync(output.BaseStream);
				await output.FlushAsync();
			}

			await output.FlushAsync();
			output.Close();
		}

	}
}
