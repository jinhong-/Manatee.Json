using System.Collections.Generic;
using System.Linq;

namespace Manatee.Json.Schema.Validators
{
	internal abstract class AllOfSchemaPropertyValidatorBase<T> : IJsonSchemaPropertyValidator
		where T : IJsonSchema
	{
		protected abstract IEnumerable<IJsonSchema> GetAllOf(T schema);
		
		public bool Applies(IJsonSchema schema, JsonValue json)
		{
			return schema is T typed && GetAllOf(typed) != null;
		}

		public SchemaValidationResults Validate(IJsonSchema schema, JsonValue json, JsonValue root)
		{
            var innerErrors = GetAllOf((T)schema).Select(s => s.Validate(json, root)).SelectMany(r => r.Errors);
            return new SchemaValidationResults(schema, string.Empty, string.Empty, validationKeyword: "allOf",
                innerErrors: innerErrors);
		}
	}

	internal class AllOfSchema04PropertyValidator : AllOfSchemaPropertyValidatorBase<JsonSchema04>
	{
		protected override IEnumerable<IJsonSchema> GetAllOf(JsonSchema04 schema)
		{
			return schema.AllOf;
		}
	}
	
	internal class AllOfSchema06PropertyValidator : AllOfSchemaPropertyValidatorBase<JsonSchema06>
	{
		protected override IEnumerable<IJsonSchema> GetAllOf(JsonSchema06 schema)
		{
			return schema.AllOf;
		}
	}
	
	internal class AllOfSchema07PropertyValidator : AllOfSchemaPropertyValidatorBase<JsonSchema07>
	{
		protected override IEnumerable<IJsonSchema> GetAllOf(JsonSchema07 schema)
		{
			return schema.AllOf;
		}
	}
}
