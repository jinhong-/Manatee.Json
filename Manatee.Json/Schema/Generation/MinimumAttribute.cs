using System;

namespace Manatee.Json.Schema.Generation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class MinimumAttribute : Attribute, ISchemaGenerationAttribute
	{
		private readonly double _value;
		private readonly bool _isExclusive;

		public MinimumAttribute(double value, bool isExclusive = false)
		{
			_value = value;
			_isExclusive = isExclusive;
		}
		void ISchemaGenerationAttribute.Update(IJsonSchema schema)
		{
			switch (schema)
			{
				case JsonSchema04 schema04:
					schema04.Minimum = _value;
					schema04.ExclusiveMinimum = true;
					break;
				case JsonSchema06 schema06:
					if (_isExclusive)
						schema06.ExclusiveMinimum = _value;
					else
						schema06.Minimum = _value;
					break;
				case JsonSchema07 schema07:
					if (_isExclusive)
						schema07.ExclusiveMinimum = _value;
					else
						schema07.Minimum = _value;
					break;
			}
		}
	}
}