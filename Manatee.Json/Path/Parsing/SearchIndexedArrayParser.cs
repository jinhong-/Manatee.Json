﻿using System.Linq;

namespace Manatee.Json.Path.Parsing
{
	internal class SearchIndexedArrayParser : IJsonPathParser
	{
		public bool Handles(string input, int index)
		{
			if (index + 3 >= input.Length)
				return false;

			return input[index] == '.' &&
			       input[index + 1] == '.' &&
			       input[index + 2] == '[' &&
			       (char.IsDigit(input[index + 3]) || input[index + 3] == '-' || input[index + 3] == ':');
		}

		public string TryParse(string source, ref int index, ref JsonPath path)
		{
			if (path == null) return "Start token not found.";

			index += 2;
			var error = source.GetSlices(ref index, out var slices);
			if (error != null) return error;

			path = path.SearchArray(slices.ToArray());
			return null;
		}
	}
}
