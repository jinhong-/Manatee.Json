using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
#if !NET45
using System.Net.Http;
#endif
using Manatee.Json.Internal;

namespace Manatee.Json.Schema
{
	/// <summary>
	/// Defines options associated with JSON Schema.
	/// </summary>
	public static class JsonSchemaOptions
	{
		private static Func<string, string> _download;

		/// <summary>
		/// Gets and sets a method used to download online schema.
		/// </summary>
		public static Func<string, string> Download
		{
			get { return _download ?? (_download = _BasicDownload); }
			set { _download = value; }
		}

		/// <summary>
		/// Gets or sets whether the "format" schema keyword should be validated.  The default is true.
		/// </summary>
		public static bool ValidateFormat { get; set; }

		/// <summary>
		/// Gets or sets whether the "readOnly" schema keyword should be enforced.  The default is true.
		/// </summary>
		public static bool EnforceReadOnly { get; set; }

		/// <summary>
		/// Initializes all properties.
		/// </summary>
		static JsonSchemaOptions()
		{
			ValidateFormat = true;
			EnforceReadOnly = true;
		}

		private static string _BasicDownload(string path)
		{
			var uri = new Uri(path);

			switch (uri.Scheme)
			{
				case "http":
				case "https:":
#if NET45
					return new WebClient().DownloadString(uri);
#else
					return new HttpClient().GetStringAsync(uri).Result;
#endif
				case "file":
					var filename = Uri.UnescapeDataString(uri.AbsolutePath)._AdjustForOs();
					return File.ReadAllText(filename);
				default:
					throw new Exception();
			}
		}

		private static string _AdjustForOs(this string path)
		{
#if NET45
			return Environment.OSVersion.Platform.In(PlatformID.MacOSX, PlatformID.Unix)
#else
			return RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ||
			       RuntimeInformation.IsOSPlatform(OSPlatform.OSX)
#endif
				       ? path.Replace("\\", "/")
				       : path.Replace("/", "\\");
		}
	}
}
