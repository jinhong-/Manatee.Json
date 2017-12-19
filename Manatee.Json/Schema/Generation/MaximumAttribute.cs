using System;

namespace Manatee.Json.Schema.Generation
{
	[AttributeUsage(AttributeTargets.Property)]
	public class MaximumAttribute : Attribute, ISchemaGenerationAttribute
	{
		private readonly double _value;
		private readonly bool _isExclusive;

		public MaximumAttribute(double value, bool isExclusive = false)
		{
			_value = value;
			_isExclusive = isExclusive;
		}
		void ISchemaGenerationAttribute.Update(IJsonSchema schema)
		{
			switch (schema)
			{
				case JsonSchema04 schema04:
					schema04.Maximum = _value;
					schema04.ExclusiveMaximum = true;
					break;
				case JsonSchema06 schema06:
					if (_isExclusive)
						schema06.ExclusiveMaximum = _value;
					else
						schema06.Maximum = _value;
					break;
				case JsonSchema07 schema07:
					if (_isExclusive)
						schema07.ExclusiveMaximum = _value;
					else
						schema07.Maximum = _value;
					break;
			}
		}
	}
}