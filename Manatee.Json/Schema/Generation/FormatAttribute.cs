using System;

namespace Manatee.Json.Schema.Generation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class FormatAttribute : Attribute, ISchemaGenerationAttribute
	{
		private readonly StringFormat _format;

		public FormatAttribute(StringFormat format)
		{
			_format = format;
		}

		void ISchemaGenerationAttribute.Update(IJsonSchema schema)
		{
			switch (schema)
			{
				case JsonSchema04 schema04:
					schema04.Format = _format;
					break;
				case JsonSchema06 schema06:
					schema06.Format = _format;
					break;
				case JsonSchema07 schema07:
					schema07.Format = _format;
					break;
			}
		}
	}
}