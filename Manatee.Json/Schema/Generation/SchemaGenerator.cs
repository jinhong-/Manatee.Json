using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Manatee.Json.Internal;
using Manatee.Json.Serialization;
using Manatee.Json.Serialization.Internal.Serializers;

namespace Manatee.Json.Schema.Generation
{
	internal static class SchemaGenerator
	{
		public static IJsonSchema Generate<T>(Func<IJsonSchema> getSchema, JsonSerializer serializer, Dictionary<IJsonSchema, string> definitions = null)
		{
			var properties = ReflectionCache.GetMembers(typeof(T), PropertySelectionStrategy.ReadWriteOnly, false)
			                                .Select(s => s.MemberInfo)
			                                .OfType<PropertyInfo>()
			                                .ToList();
			definitions = definitions ?? new Dictionary<IJsonSchema, string>();
			var schema = getSchema();
			var schemaProperties = new Dictionary<string, IJsonSchema>();
			foreach (var propertyInfo in properties)
			{
				var propertySchema = getSchema();
				var attributes = propertyInfo.GetCustomAttributes().OfType<ISchemaGenerationAttribute>().ToList();
				foreach (var attribute in attributes)
				{
					attribute.Update(propertySchema);
				}
				_AssignType(propertySchema, propertyInfo.PropertyType);
				var propertyName = serializer.Options.SerializationNameTransform(propertyInfo.Name);
				schemaProperties[propertyName] = propertySchema;
			}
			_AssignProperties(schema, schemaProperties);
			return schema;
		}

		private static JsonSchemaType _DetermineType(Type propertyType)
		{
			var typeInfo = propertyType.GetTypeInfo();

			if (typeof(IDictionary).GetTypeInfo().IsAssignableFrom(typeInfo) &&
			    typeInfo.IsGenericType &&
			    typeInfo.GenericTypeArguments[0] == typeof(string))
				return JsonSchemaType.Object;

			if (propertyType == typeof(string))
				return JsonSchemaType.String;

			if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo))
				return JsonSchemaType.Array;

			if (propertyType.IsFloat())
				return JsonSchemaType.Number;

			if (propertyType.IsInteger())
				return JsonSchemaType.Integer;

			if (propertyType == typeof(bool))
				return JsonSchemaType.Boolean;

			if (typeInfo.IsEnum)
				return JsonSchemaType.NotDefined;

			return JsonSchemaType.Object;
		}

		private static void _AssignType(IJsonSchema schema, Type propertyType)
		{
			var type = _DetermineType(propertyType);

			switch (schema)
			{
				case JsonSchema04 schema04:
					schema04.Type = type;
					break;
				case JsonSchema06 schema06:
					schema06.Type = type;
					break;
				case JsonSchema07 schema07:
					schema07.Type = type;
					break;
			}
		}

		private static void _AssignProperties(IJsonSchema schema, Dictionary<string, IJsonSchema> properties)
		{
			switch (schema)
			{
				case JsonSchema04 schema04:
					schema04.Properties = properties;
					break;
				case JsonSchema06 schema06:
					schema06.Properties = properties;
					break;
				case JsonSchema07 schema07:
					schema07.Properties = properties;
					break;
			}
		}
	}
}
