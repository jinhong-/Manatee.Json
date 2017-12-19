using System;
using JetBrains.Annotations;

namespace Manatee.Json.Schema.Generation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class RegexAttribute : Attribute, ISchemaGenerationAttribute
	{
		private readonly string _regex;

		public RegexAttribute([RegexPattern] string regex)
		{
			_regex = regex;
		}

		void ISchemaGenerationAttribute.Update(IJsonSchema schema)
		{
			switch (schema)
			{
				case JsonSchema04 schema04:
					schema04.Pattern = _regex;
					break;
				case JsonSchema06 schema06:
					schema06.Pattern = _regex;
					break;
				case JsonSchema07 schema07:
					schema07.Pattern = _regex;
					break;
			}
		}
	}
}