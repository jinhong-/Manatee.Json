using System.Collections.Generic;
using System.Linq;
using Manatee.Json.Internal;

namespace Manatee.Json.Schema.Validators
{
    internal abstract class ItemsSchemaPropertyValidatorBase<T> : IJsonSchemaPropertyValidator
        where T : IJsonSchema
    {
        protected abstract IJsonSchema GetItems(T schema);
        protected abstract AdditionalItems GetAdditionalItems(T schema);

        public bool Applies(IJsonSchema schema, JsonValue json)
        {
            return schema is T typed && (GetItems(typed) != null || GetAdditionalItems(typed) != null) &&
                   json.Type == JsonValueType.Array;
        }

        public SchemaValidationResults Validate(IJsonSchema schema, JsonValue json, JsonValue root)
        {
            var typed = (T)schema;
            var errors = new List<SchemaValidationError>();
            var array = json.Array;
            if (GetItems(typed) is JsonSchemaCollection items)
            {
                var additionalItems = GetAdditionalItems(typed);
                // have array of schemata: validate in sequence
                var itemsErrors = new List<SchemaValidationError>();
                var i = 0;
                while (i < array.Count && i < items.Count)
                {
                    itemsErrors.AddRange(items[i].Validate(array[i], root).Errors);
                    i++;
                }
                if (itemsErrors.Any())
                {
                    errors.Add(new SchemaValidationError(schema, string.Empty, "", validationKeyword: "items", innerErrors: itemsErrors.ToArray()));
                }

                var additionalItemsErrors = new List<SchemaValidationError>();
                if (i < array.Count && additionalItems != null)
                    if (Equals(additionalItems, AdditionalItems.False))
                    {
                        var message = SchemaErrorMessages.Items.ResolveTokens(new Dictionary<string, object>
                        {
                            ["value"] = json
                        });
                        errors.Add(new SchemaValidationError(schema, string.Empty, message, validationKeyword: "additionalItems"));
                    }
                    else if (!Equals(additionalItems, AdditionalItems.True))
                        additionalItemsErrors.AddRange(array.Skip(i).SelectMany(j => additionalItems.Definition.Validate(j, root).Errors));

                if (additionalItemsErrors.Any())
                {
                    errors.Add(new SchemaValidationError(schema, string.Empty, "", validationKeyword: "additionalItems",
                        innerErrors: additionalItemsErrors.ToArray()));
                }
            }
            else if (GetItems(typed) != null)
            {
                // have single schema: validate all against this
                var itemValidations = array.Select(v => GetItems(typed).Validate(v, root));
                var innerErrors = itemValidations.SelectMany((v, i) => v.Errors.Select(e => e.PrependPropertyName($"[{i}]"))).ToArray();
                errors.Add(new SchemaValidationError(schema, string.Empty, "", 
                    validationKeyword: "items",
                    innerErrors: innerErrors));
            }
            return new SchemaValidationResults(errors);
        }
    }

    internal class ItemsSchema04PropertyValidator : ItemsSchemaPropertyValidatorBase<JsonSchema04>
    {
        protected override IJsonSchema GetItems(JsonSchema04 schema)
        {
            return schema.Items;
        }
        protected override AdditionalItems GetAdditionalItems(JsonSchema04 schema)
        {
            return schema.AdditionalItems;
        }
    }

    internal class ItemsSchema06PropertyValidator : ItemsSchemaPropertyValidatorBase<JsonSchema06>
    {
        protected override IJsonSchema GetItems(JsonSchema06 schema)
        {
            return schema.Items;
        }
        protected override AdditionalItems GetAdditionalItems(JsonSchema06 schema)
        {
            if (schema.AdditionalItems == null) return null;
            if (Equals(schema.AdditionalItems, JsonSchema06.True)) return AdditionalItems.True;
            if (Equals(schema.AdditionalItems, JsonSchema06.False)) return AdditionalItems.False;
            return new AdditionalItems { Definition = schema.AdditionalItems };
        }
    }

    internal class ItemsSchema07PropertyValidator : ItemsSchemaPropertyValidatorBase<JsonSchema07>
    {
        protected override IJsonSchema GetItems(JsonSchema07 schema)
        {
            return schema.Items;
        }
        protected override AdditionalItems GetAdditionalItems(JsonSchema07 schema)
        {
            if (schema.AdditionalItems == null) return null;
            if (Equals(schema.AdditionalItems, JsonSchema07.True)) return AdditionalItems.True;
            if (Equals(schema.AdditionalItems, JsonSchema07.False)) return AdditionalItems.False;
            return new AdditionalItems { Definition = schema.AdditionalItems };
        }
    }
}
