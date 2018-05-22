﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Manatee.Json.Internal;

namespace Manatee.Json.Schema.Validators
{
    internal abstract class PropertiesSchemaPropertyValidatorBase<T> : IJsonSchemaPropertyValidator
        where T : IJsonSchema
    {
        protected abstract IDictionary<string, IJsonSchema> GetProperties(T schema);
        protected abstract IEnumerable<string> GetRequiredProperties(T schema);
        protected abstract AdditionalProperties GetAdditionalProperties(T schema);
        protected abstract Dictionary<Regex, IJsonSchema> GetPatternProperties(T schema);
        protected virtual IEnumerable<SchemaValidationError> AdditionValidation(T schema, JsonValue json, JsonValue root)
        {
            return new SchemaValidationError[] { };
        }

        public virtual bool Applies(IJsonSchema schema, JsonValue json)
        {
            return schema is T typed &&
                   (GetProperties(typed) != null ||
                    GetAdditionalProperties(typed) != null ||
                    GetPatternProperties(typed) != null) &&
                   json.Type == JsonValueType.Object;
        }
        public SchemaValidationResults Validate(IJsonSchema schema, JsonValue json, JsonValue root)
        {
            var typed = (T)schema;
            var obj = json.Object;
            var errors = new List<SchemaValidationError>();
            var properties = GetProperties(typed) ?? new Dictionary<string, IJsonSchema>();
            var additionalProperties = GetAdditionalProperties(typed);
            var patternProperties = GetPatternProperties(typed);
            foreach (var property in properties)
            {
                if (!obj.ContainsKey(property.Key)) continue;
                var result = property.Value?.Validate(obj[property.Key], root);
                if (result != null && !result.Valid)
                    errors.AddRange(result.Errors.Select(e => e.PrependPropertyName(property.Key)));
            }
            var requiredProperties = GetRequiredProperties(typed) ?? new List<string>();
            foreach (var property in requiredProperties)
            {
                if (!obj.ContainsKey(property))
                {
                    var message = SchemaErrorMessages.Required.ResolveTokens(new Dictionary<string, object>
                    {
                        ["property"] = property,
                        ["value"] = json
                    });
                    errors.Add(new SchemaValidationError(schema, property, message, validationKeyword: "required"));
                }
            }
            // if additionalProperties is false, we perform the property elimination,
            // otherwise properties and patternProperties applies to all properties.
            var extraData = Equals(additionalProperties, AdditionalProperties.False)
                                ? obj.Where(kvp => properties.All(p => p.Key != kvp.Key))
                                     .ToDictionary(kvp => kvp.Key, kvp => kvp.Value)
                                : obj;
            if (patternProperties != null)
            {
                var propertiesToRemove = new List<string>();
                foreach (var patternProperty in patternProperties)
                {
                    var pattern = patternProperty.Key;
                    var localSchema = patternProperty.Value;
                    var matches = extraData.Keys.Where(k => pattern.IsMatch(k));
                    foreach (var match in matches)
                    {
                        var matchErrors = localSchema.Validate(extraData[match], root).Errors;
                        errors.Add(new SchemaValidationError(schema, string.Empty, string.Empty, validationKeyword: "patternProperties",
                            innerErrors: matchErrors.Select(e => new SchemaValidationError(localSchema, match, e.Message))));
                    }
                    propertiesToRemove.AddRange(extraData.Keys.Where(k => pattern.IsMatch(k)));
                }
                extraData = extraData.Where(kvp => !propertiesToRemove.Contains(kvp.Key))
                                     .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            }
            // additionalProperties never applies to properties checked by
            // properties or patternProperties.
            extraData = extraData.Where(kvp => properties.All(p => p.Key != kvp.Key))
                                 .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            if (additionalProperties != null)
            {
                if (Equals(additionalProperties, AdditionalProperties.False) && extraData.Any())
                    errors.Add(new SchemaValidationError(schema, string.Empty, string.Empty, validationKeyword: "additionalProperties"));
                //errors.AddRange(extraData.Keys.Select(k =>
                //    {
                //        var message = SchemaErrorMessages.AdditionalProperties_False.ResolveTokens(new Dictionary<string, object>
                //        {
                //            ["property"] = k,
                //            ["value"] = json
                //        });
                //        return new SchemaValidationError(schema, k, message);
                //    }));
                else
                {
                    var localSchema = additionalProperties.Definition;
                    errors.Add(new SchemaValidationError(localSchema, "", "", validationKeyword: "additionalProperties",
                        innerErrors: extraData.Keys.SelectMany(key => localSchema.Validate(extraData[key], root).Errors)));
                }
            }
            var additionalValidation = AdditionValidation(typed, json, root);
            return new SchemaValidationResults(errors.Concat(additionalValidation));
        }
    }

    internal class PropertiesSchema04PropertyValidator : PropertiesSchemaPropertyValidatorBase<JsonSchema04>
    {
        protected override IDictionary<string, IJsonSchema> GetProperties(JsonSchema04 schema)
        {
            return schema.Properties;
        }
        protected override IEnumerable<string> GetRequiredProperties(JsonSchema04 schema)
        {
            return schema.Required;
        }
        protected override AdditionalProperties GetAdditionalProperties(JsonSchema04 schema)
        {
            return schema.AdditionalProperties;
        }
        protected override Dictionary<Regex, IJsonSchema> GetPatternProperties(JsonSchema04 schema)
        {
            return schema.PatternProperties;
        }
    }

    // TODO: extract a base class for 6/7 
    internal class PropertiesSchema06PropertyValidator : PropertiesSchemaPropertyValidatorBase<JsonSchema06>
    {
        public override bool Applies(IJsonSchema schema, JsonValue json)
        {
            return base.Applies(schema, json) || (schema is JsonSchema06 typed && json.Type == JsonValueType.Object);
        }

        protected override IDictionary<string, IJsonSchema> GetProperties(JsonSchema06 schema)
        {
            return schema.Properties;
        }
        protected override IEnumerable<string> GetRequiredProperties(JsonSchema06 schema)
        {
            return schema.Required;
        }
        protected override AdditionalProperties GetAdditionalProperties(JsonSchema06 schema)
        {
            if (schema.AdditionalProperties == null) return null;
            return new AdditionalProperties { Definition = schema.AdditionalProperties };
        }
        protected override Dictionary<Regex, IJsonSchema> GetPatternProperties(JsonSchema06 schema)
        {
            return schema.PatternProperties;
        }
        protected override IEnumerable<SchemaValidationError> AdditionValidation(JsonSchema06 schema, JsonValue json, JsonValue root)
        {
            return schema.PropertyNames == null
                       ? new SchemaValidationError[] { }
                       : json.Object.Keys.SelectMany(k => schema.PropertyNames.Validate(k, root).Errors);
        }
    }

    internal class PropertiesSchema07PropertyValidator : PropertiesSchemaPropertyValidatorBase<JsonSchema07>
    {
        public override bool Applies(IJsonSchema schema, JsonValue json)
        {
            return base.Applies(schema, json) || (schema is JsonSchema07 typed && json.Type == JsonValueType.Object && typed.PropertyNames != null);
        }

        protected override IDictionary<string, IJsonSchema> GetProperties(JsonSchema07 schema)
        {
            return schema.Properties;
        }
        protected override IEnumerable<string> GetRequiredProperties(JsonSchema07 schema)
        {
            return schema.Required;
        }
        protected override AdditionalProperties GetAdditionalProperties(JsonSchema07 schema)
        {
            if (schema.AdditionalProperties == null) return null;
            return new AdditionalProperties { Definition = schema.AdditionalProperties };
        }
        protected override Dictionary<Regex, IJsonSchema> GetPatternProperties(JsonSchema07 schema)
        {
            return schema.PatternProperties;
        }
        protected override IEnumerable<SchemaValidationError> AdditionValidation(JsonSchema07 schema, JsonValue json, JsonValue root)
        {
            return schema.PropertyNames == null
                       ? new SchemaValidationError[] { }
                       : json.Object.Keys.SelectMany(k => schema.PropertyNames.Validate(k, root).Errors);
        }
    }
}
