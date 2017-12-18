using System;
using JetBrains.Annotations;

namespace Manatee.Json.Schema.Generation
{
	internal interface ISchemaGenerationAttribute
	{
		void Update(IJsonSchema schema);
	}

	[AttributeUsage(AttributeTargets.Property)]
	public class MinLengthAttribute : Attribute, ISchemaGenerationAttribute
	{
		private readonly uint _value;

		public MinLengthAttribute(uint value)
		{
			_value = value;
		}
		void ISchemaGenerationAttribute.Update(IJsonSchema schema)
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

	[AttributeUsage(AttributeTargets.Property)]
	public class MaxLengthAttribute : Attribute, ISchemaGenerationAttribute
	{
		private readonly uint _value;

		public MaxLengthAttribute(uint value)
		{
			_value = value;
		}
		void ISchemaGenerationAttribute.Update(IJsonSchema schema)
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

	[AttributeUsage(AttributeTargets.Property)]
	public class RequiredAttribute : Attribute { }
}
