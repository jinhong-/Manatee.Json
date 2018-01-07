using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Manatee.Json.Internal;
using Manatee.Json.Serialization;
using Manatee.Json.Serialization.Internal;
using Manatee.Json.Serialization.Internal.Serializers;

namespace Manatee.Json.Schema.Generation
{
	internal class SchemaGenerator
	{
		private static readonly Dictionary<Type, JsonSchema07> _rawSchemas = new Dictionary<Type, JsonSchema07>();
		private bool _generateProperties = true;

		public JsonSchema07 Generate<T>(JsonSerializer serializer)
		{
			return Generate(typeof(T), serializer);
		}

		public JsonSchema07 Generate(Type type, JsonSerializer serializer)
		{
			if (_rawSchemas.TryGetValue(type, out var schema)) return schema;

			var properties = ReflectionCache.GetMembers(type, PropertySelectionStrategy.ReadWriteOnly, false)
			                                .Select(s => s.MemberInfo)
			                                .OfType<PropertyInfo>()
			                                .ToList();
			schema = new JsonSchema07();
			_AssignType(schema, type, serializer);
			var schemaProperties = new Dictionary<string, IJsonSchema>();
			var required = new List<string>();
			if (_generateProperties)
			{
				foreach (var propertyInfo in properties)
				{
					var propertySchema = new SchemaGenerator().Generate(propertyInfo.PropertyType, serializer);
					var attributes = propertyInfo.GetCustomAttributes()
					                             .OfType<ISchemaGenerationAttribute>()
					                             .ToList();
					JsonSchema07 schemaToUpdate;
					switch (propertySchema.Type)
					{
						case JsonSchemaType.Array:
							schemaToUpdate = (JsonSchema07) propertySchema.Items;
							break;
						case JsonSchemaType.Object:
							schemaToUpdate = (JsonSchema07) propertySchema.AdditionalProperties;
							break;
						default:
							schemaToUpdate = propertySchema;
							break;
					}
					foreach (var attribute in attributes)
					{
						attribute.Update(schemaToUpdate);
					}
					var propertyName = serializer.Options.SerializationNameTransform(propertyInfo.Name);
					schemaProperties[propertyName] = propertySchema;
					if (propertyInfo.GetCustomAttribute<RequiredAttribute>() != null)
						required.Add(propertyName);
				}
			}
			if (schemaProperties.Count != 0)
				schema.Properties = schemaProperties;
			if (required.Count != 0)
				schema.Required = required;

			//var allSchemas = _CollectSchemas(schema);

			return Equals(schema, JsonSchema07.Empty)
				       ? JsonSchema07.True
				       : schema;
		}

		//private Dictionary<JsonSchema07, int> _CollectSchemas(JsonSchema07 schema)
		//{
		//	var subschemas = new List<IJsonSchema>();
		//	if (schema.Items != null)
		//		subschemas.Add(schema.Items);
		//	if (schema.AdditionalProperties != null)
		//		subschemas.Add(schema.AdditionalProperties);
		//	if (schema.Properties != null)
		//		subschemas.AddRange(schema.Properties.Values);
		//	if (schema.Definitions != null)
		//		subschemas.AddRange(schema.Definitions.Values);

		//}

		private void _AssignType(JsonSchema07 schema, Type type, JsonSerializer serializer)
		{
			var typeInfo = type.GetTypeInfo();

			if (typeInfo.IsGenericType &&
			    typeInfo.GetGenericTypeDefinition() == typeof(Dictionary<,>) &&
			    typeInfo.GenericTypeArguments[0] == typeof(string))
			{
				schema.Type = JsonSchemaType.Object;
				schema.AdditionalProperties = new SchemaGenerator().Generate(typeInfo.GenericTypeArguments[1], serializer);
				_generateProperties = false;
			}
			else if (type == typeof(string))
				schema.Type = JsonSchemaType.String;
			else if (typeof(IEnumerable).GetTypeInfo().IsAssignableFrom(typeInfo))
			{
				schema.Type = JsonSchemaType.Array;
				if (typeInfo.IsGenericType &&
				    typeInfo.GetGenericTypeDefinition() == typeof(List<>))
				{
					schema.Items = new SchemaGenerator().Generate(typeInfo.GenericTypeArguments[0], serializer);
					_generateProperties = false;
				}
			}
			else if (type.IsFloat())
				schema.Type = JsonSchemaType.Number;
			else if (type.IsInteger() || (typeInfo.IsEnum && serializer.Options.EnumSerializationFormat == EnumSerializationFormat.AsInteger))
				schema.Type = JsonSchemaType.Integer;
			else if (type == typeof(bool))
				schema.Type = JsonSchemaType.Boolean;
			else if (typeInfo.IsEnum && serializer.Options.EnumSerializationFormat == EnumSerializationFormat.AsName)
			{
				var defaultOption = serializer.Options.EncodeDefaultValues;
				serializer.Options.EncodeDefaultValues = true;
				var serializerMethod = SerializerCache.GetSerializeMethod(type);
				schema.Enum = Enum.GetValues(type)
				                  .Cast<object>()
				                  .Select(v => new EnumSchemaValue(((JsonValue)serializerMethod.Invoke(serializer, new[]{v})).String))
				                  .ToList();
				serializer.Options.EncodeDefaultValues = defaultOption;
			}
			else schema.Type = JsonSchemaType.Object | JsonSchemaType.Null;
		}
	}
}