using System;

namespace Manatee.Json.Schema.Generation
{
	public interface ISchemaGenerationAttribute
	{
		void Update(IJsonSchema schema);
	}

	public class MinLengthAttribute : Attribute, ISchemaGenerationAttribute
	{
		private readonly uint _value;

		public MinLengthAttribute(uint value)
		{
			_value = value;
		}
		public void Update(IJsonSchema schema)
		{
			switch (schema)
			{
				case JsonSchema04 schema04:
					schema04.MinLength = _value;
					break;
				case JsonSchema06 schema06:
					schema06.MinLength = _value;
					break;
				case JsonSchema07 schema07:
					schema07.MinLength = _value;
					break;
			}
		}
	}

	public class MaxLengthAttribute : Attribute, ISchemaGenerationAttribute
	{
		private readonly uint _value;

		public MaxLengthAttribute(uint value)
		{
			_value = value;
		}
		public void Update(IJsonSchema schema)
		{
			switch (schema)
			{
				case JsonSchema04 schema04:
					schema04.MaxLength = _value;
					break;
				case JsonSchema06 schema06:
					schema06.MaxLength = _value;
					break;
				case JsonSchema07 schema07:
					schema07.MaxLength = _value;
					break;
			}
		}
	}

	public class MinimumAttribute : Attribute, ISchemaGenerationAttribute
	{
		private readonly double _value;
		private readonly bool _isExclusive;

		public MinimumAttribute(double value, bool isExclusive = false)
		{
			_value = value;
			_isExclusive = isExclusive;
		}
		public void Update(IJsonSchema schema)
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
