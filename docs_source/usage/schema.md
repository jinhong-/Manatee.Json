# JSON Schema

The occasion may arise when you wish to validate that a JSON object is in the correct form (has the appropriate keys and the right types of values).  Enter JSON Schema.  Much like XML Schema with XML, JSON Schema defines a pattern for JSON data.  A JSON Schema validator can verify that a given JSON object meets the requirements as defined by the JSON Schema.  This validation can come in handy as a precursor step before deserializing.

More information about JSON Schema can be found at [json-schema.org](http://json-schema.org).

## Building JSON Schema

In Manatee.Json, JSON Schema are defined using the `JsonSchema` class, which can be found in the `Manatee.Json.Schema` namespace.  This object contains the properties you'll need to validate any JSON object.  Let's take a look at this object in a bit more detail to understand all of the parts of a JSON Schema.

<table>
<thead>
<tr>
    <th align="center">Property</th>
    <th align="center">Validates</th>
    <th align="left">Supported by draft</th>
    <th align="left">Description</th>
</tr>
</thead>
<tbody>
<tr>
    <td align="center"><code>Id</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">This is a string value which defines an id for this schema.  Value must be a URI.  When deserialized, a schema will automatically register itself with JsonSchemaRegistry.
    
For draft 4, this is represented in JSON as the `id` property; for draft 6, this has been updated to `$id`.</td>
</tr>
<tr>
    <td align="center"><code>Schema</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">This will usually be the same value as the <code>Id</code> property, but may be different.  Value must be a URI.  It is used to identify a schema which governs the valid shape of this schema.  If left null, the default to be used is specified in the `JsonSchemaFactory` static class.</td>
</tr>
<tr>
    <td align="center"><code>Title</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Provides a title for this schema.  This is informational only and does not affect validation.</td>
</tr>
<tr>
    <td align="center"><code>Description</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Provides a description for this schema.  This is informational only and does not affect validation.</td>
</tr>
<tr>
    <td align="center"><code>Default</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a default value.  This may be any value.</td>
</tr>
<tr>
    <td align="center"><code>MultipleOf</code></td>
    <td align="center">Numbers</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a required divisor for accepted values.</td>
</tr>
<tr>
    <td align="center"><code>Maximum</code></td>
    <td align="center">Numbers</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a maximum for accepted values.</td>
</tr>
<tr>
    <td align="center"><code>ExclusiveMaximum</code></td>
    <td align="center">Numbers</td>
    <td align="left">4/6/7</td>
    <td align="left">draft 4 (boolean): Defines whether the value in the <code>Maximum</code> property is acceptable.

draft 6/7 (number): Defines an exclusive maximum.</td>
</tr>
<tr>
    <td align="center"><code>Minimum</code></td>
    <td align="center">Numbers</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a minimum for accepted values.</td>
</tr>
<tr>
    <td align="center"><code>ExclusiveMinimum</code></td>
    <td align="center">Numbers</td>
    <td align="left">4/6/7</td>
    <td align="left">draft 4 (boolean): Defines whether the value in the <code>Minimum</code> property is acceptable.

draft 6/7 (number): Defines an exclusive minimum.</td>
</tr>
<tr>
    <td align="center"><code>MaxLength</code></td>
    <td align="center">Strings</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a maximum number of characters.</td>
</tr>
<tr>
    <td align="center"><code>MinLength</code></td>
    <td align="center">Strings</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a minimum number of characters.</td>
</tr>
<tr>
    <td align="center"><code>Pattern</code></td>
    <td align="center">Strings</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a regular expression for which acceptable values must pass validation.</td>
</tr>
<tr>
    <td align="center"><code>AdditionalItems</code></td>
    <td align="center">Arrays</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines whether additional items are allowed in acceptable values.  May be <code>True</code>, <code>False</code>, or a JSON schema.</td>
</tr>
<tr>
    <td align="center"><code>Items</code></td>
    <td align="center">Arrays</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a JSON schema or sequence of JSON schemata to validate contained elements.</td>
</tr>
<tr>
    <td align="center"><code>MaxItems</code></td>
    <td align="center">Arrays</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a maximum number of elements allowed.</td>
</tr>
<tr>
    <td align="center"><code>MinItems</code></td>
    <td align="center">Arrays</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a minimum number of elements allowed.</td>
</tr>
<tr>
    <td align="center"><code>UniqueItems</code></td>
    <td align="center">Arrays</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines whether each element must be unique.</td>
</tr>
<tr>
    <td align="center"><code>MaxProperties</code></td>
    <td align="center">Objects</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a maximum number of properties allowed.</td>
</tr>
<tr>
    <td align="center"><code>MinProperties</code></td>
    <td align="center">Objects</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a minimum number of properties allowed.</td>
</tr>
<tr>
    <td align="center"><code>AdditionalProperties</code></td>
    <td align="center">Objects</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines whether any keys not listed in the <code>Properties</code> property may appear as well as the schema to validate any such properties.  See <em><code>AdditionalProperties</code> and <code>PatternProperties</code></em> below for more information.</td>
</tr>
<tr>
    <td align="center"><code>Properties</code></td>
    <td align="center">Objects</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a collection of keys which may appear in the object being validated as well as the schema to validate the value under that key.</td>
</tr>
<tr>
    <td align="center"><code>Definitions</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a collection of type definitions that may be referenced throughout this schema.</td>
</tr>
<tr>
    <td align="center"><code>PatternProperties</code></td>
    <td align="center">Objects</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines criteria which any additional keys must adhere as well as the schema to validate those properties.  See <em><code>AdditionalProperties</code> and <code>PatternProperties</code></em> below for more information.</td>
</tr>
<tr>
    <td align="center"><code>Dependencies</code></td>
    <td align="center">Objects</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines any keys on which other keys depend.  For instance, the JSON Schema draft 4 document defines that the <code>exclusiveMaximum</code> property requires that the <code>maximum</code> property be present in the object being validated.</td>
</tr>
<tr>
    <td align="center"><code>Enum</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a discrete collection of acceptable values.</td>
</tr>
<tr>
    <td align="center"><code>Type</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a type or a collection of simple JSON Schema types acceptable.</td>
</tr>
<tr>
    <td align="center"><code>AllOf</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a collection of JSON Schema, all of which acceptable values must pass.</td>
</tr>
<tr>
    <td align="center"><code>AnyOf</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a collection of JSON Schema, one or more of which acceptable values must pass.</td>
</tr>
<tr>
    <td align="center"><code>OneOf</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a collection of JSON Schema, exactly one of which acceptable values must pass.</td>
</tr>
<tr>
    <td align="center"><code>Not</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Defines a JSON Schema for which acceptable values must fail.</td>
</tr>
<tr>
    <td align="center"><code>Format</code></td>
    <td align="center">Strings</td>
    <td align="left">4/6/7</td>
    <td align="left">Allows special validation on strings.</td>
</tr>
<tr>
    <td align="center"><code>Required</code></td>
    <td align="center">Objects</td>
    <td align="left">4/6/7</td>
    <td align="left">Lists names of required properties.</td>
</tr>
<tr>
    <td align="center"><code>ExtraneousDetails</code></td>
    <td align="center">Any</td>
    <td align="left">4/6/7</td>
    <td align="left">Stores any properties not explicitly defined in the JSON Schema specifications to be added to the schema.  There is no validation on these properties.</td>
</tr>
<tr>
    <td align="center"><code>PropertyNames</code></td>
    <td align="center">Any</td>
    <td align="left">6/7</td>
    <td align="left">Defines a schema for any property names that aren't explicitly defined.  Differs from AdditionalProperties in that this looks at the property name instead of the property value.</td>
</tr>
<tr>
    <td align="center"><code>If/Then/Else</code></td>
    <td align="center">Any</td>
    <td align="left">7</td>
    <td align="left">Each property defines a schema.  If the `If` schema passes validation, the `Then` schema is validated, otherwise the `Else` schema is validated.</td>
</tr>
</tbody>
</table>

### Type Definitions

Type definitions are typically used to extend the basic definitions: array, boolean, integer, null, number, object, and string.  Each of these schema types have their own distinct properties.  For instance, the string schema type can have a minimum length requirement.  Type definitions provide a method for naming a string type with a minimum length so that it can be referenced anywhere within the JSON Schema document.

To reference a type definition from within the JSON Schema document, the relative path to the definition must be given using a slash-delimited property string with a pound sign specifying the root of the document.  Typically a reference will appear in the following form:

```
#/definitions/[type name]
```

Ideally all type definitions will be contained in the root schema object's `Definitions` collection, however they could technically appear in non-schema-standard properties (These will appear in the `ExtraneousDetails` collection on the `JsonSchema` object.).  References to type definitions can even be made from other type definitions.

See the *References* section below for more information on references.

### Properties

The most important part of any JSON Schema is the collection of properties it defines.  This is what allows the schema to validate that a JSON object contains the right kind of data.

Let's start with an example.  Suppose we have a schema which defines a single property:

```json
{"properties":{"myProperty":{}}}
```

which can declared in Manatee.Json by

```csharp
var schema = new JsonSchema07
    {
        Properties = new Dictionary<string, IJsonSchema>
            {
                ["myProperty"] = new JsonSchema07()
            }
    }
```

This schema is not very interesting (or useful) as it only defines that the property should exist (although it's not required), and it doesn't define a type for it.  Suppose we pass the following JSON objects through the validator:

```json
{}
{"myProperty":false}
{"myProperty":"some string"}
{"otherProperty":35.4}
```

The schema will validate all of them as compliant because it has no type restrictions for the property.  Let's specify it as string.

```json
{"properties":{"myProperty":{"type":"string"}}}
```

which is declared in Manatee.Json by

```csharp
var schema = new JsonSchema07
    {
        Properties = new Dictionary<string, IJsonSchema>
            {
                ["myProperty"] = new JsonSchema07 {Type = JsonSchemaType.String}
            }
    }
```

Now, the JSON object with a value of `false` will pass validation; the others will pass.  (The ones missing the property pass because it's not listed as required.)

The `JsonSchemaType` enumeration is marked with a `Flags` attribute, so that they can be combined.  For instance, the JSON Schema draft 6 metaschema declares a type of both `boolean` and `object`.  This is done in an array

```json
{"type": ["object", "boolean"]}
```

This is declared in code using the `|` operator.

```csharp
var schema = new JsonSchema07 { Type = JsonSchemaType.Object | JsonSchemaType.Boolean };
```

### `AdditionalProperties` and `PatternProperties`

According to the JSON Schema documentation, these two properties have a somewhat unique interaction.  In short, the rules governing the validation of these properties are as follows:

- `PatternProperties` defines a collection of Regular Expression/JSON Schema pairs.  If a property name in an object matches a Regular Expression, it must also pass the defined JSON Schema.
- `AdditionalProperties` may be true, false, or a valid schema.
    - For `JsonSchema04`, this is implemented via two static fields on the `AdditionalProperties` type (`True` and `False`) and a general constructor which allows the injection of an `IJsonSchema` implementation.
    - For `JsonSchema06` and `JsonSchema07`, the property is merely `IJsonSchema` since drafts 6 and 7 support booleans as valid schemas.

Now here's the tricky part.  The behavior of these properties depends on the value of `AdditionalProperties`.

- If `AdditionalProperties` is true
    - The properties listed in `Properties` are evaluated per normal.
    - `PatternProperties` applies to all properties, even those in `Properties`.
- If `AdditionalProperties` is a valid schema
    - The properties listed in `Properties` are evaluated per normal.
    - `PatternProperties` applies to all properties, even those in `Properties`.
    - The `AdditionalProperties` schema is used to validate all properties not handled by `Properties` or `PatternProperties`.
- If `AdditionalProperties` is false
    - The properties listed in `Properties` are evaluated per normal.
    - `PatternProperties` applies only to properties not defined in `Properties`.
    - No other properties are allowed.

The default value for `AdditionalProperties` is the empty schema, which validates all additional properties (like `True`).

### References

Suppose we want to impose a minimum string length of 10 on the `myProperty` value from the above schema.  We can define the schema in two ways:

By explicitly defining the type in the property definition

```json
{
    "properties":{
        "myProperty":{
            "type":"string",
            "minLength":10
        }
    }
}
```

or by defining the type and creating a reference to it:

```json
{
    "definitions":{
        "stringWithMinLength10":{
            "type":"string",
            "minLength":10
        }
    },
    "properties":{
        "myProperty":{"$ref":"#/definitions/stringWithMinLength10"}
    }
}
```

As you can see, the reference is defined by an object with a special key: `"$ref"`.  The value is the path to the definition relative to the root of the schema.  The value may also be a URI where the schema can be downloaded, such as [`http://json-schema.org/geo`](http://json-schema.org/geo).  Now that you understand how type definitions and references work, let's take a look at the above schema declared with Manatee.Json.

```csharp
var schema = new JsonSchema07
    {
        Definitions = new Dictionary<string, object>
            {
                ["stringWithMinLenth10"] = new JsonSchema06
                    {
                        Type = JsonSchemaTypeDefinition.String},
                        MinLength = 10
                    }
            },
        Properties = new Dictionary<string, object>
            {
                ["myProperty"] = new JsonSchemaReference(
                    "#/definitions/stringWithMinLength10",
                    typeof(JsonSchema06))
            }
    }
```

> **NOTE** We need to tell the `JsonSchemaReference` class what version of schema we'd like constructed if the schema doesn't itself declare a version.

The declaration is very similar to the schema itself.  Defining a new type is typically done when that type is used for several properties.  In the above example, it is somewhat unnecessary.

External references are a useful way to organize complex type hierarchies; they allow a type defined in one schema to be referenced in another schema. For example, the following are all valid syntax:
* `{"$ref": "http://example.com/otherSchema.json"}`
* `{"$ref": "http://example.com/otherSchema.json#/definitions/exampleType"}`
* `{"$ref": "./otherSchema.json"}`
* `{"$ref": "../otherSchema.json#/definitions/exampleType"}`

In other words, references can use either absolute or relative URIs to indicate external schemas, in addition to an optional JSON pointer to the part of the file being referenced. External references do not need to comply to the same meta-schema as the file doing the referencing; a schema complying to draft 4 can reference an external schema complying to draft 7, and vice versa. Manatee.Json can automatically access external references using the JsonSchemaFactory.FromJson function, which has an optional parameter for the document's URI &mdash; necessary information for resolving relative URIs.

As a final note on properties, let's take a look at how the latest version of JSON Schema (draft 4) defines properties which are required by making the `myProperty` property required.  The schema itself would look like this:

```json
{
    "properties":{
        "myProperty":{
            "type":"string",
            "minLength":10
        }
    },
    "required":["myProperty"]
}
```

The schema object has a property which declares which properties are required.  Manatee.Json is very similar:

```csharp
var schema = new JsonSchema06
    {
        Properties = new Dictionary<string, IJsonSchema>
            {
                ["myProperty")] = new JsonSchema06
                    {
                        Type = JsonSchemaTypeDefinition.String,
                        MinLength = 10
                    }
            },
        Required = new [] {"myProperty"}
    };
```

### One Schema to Rule Them All

It may be appropriate for a schema to accept multiple kinds of data.  For instance, if you're reading from an API, and your DTOs (Data Transfer Objects) can sometimes be represented as the full object and sometimes by only a string (like an ID), it's useful to have a schema for that object that can validate both cases.

Let's consider the following DTO:

```csharp
public class SampleDto
{
    public string Id { get; set; }
    public double Score { get; set; }
    public string Username { get; set; }
}
```

A schema could be defined that could accept the complete object

```json
{
    "Id" : "b5e72f0a-cf1b-4732-bb83-3bb756c77ee1",
    "Score" : 9.5,
    "Username" : "gregsdennis"
}
```

or simply a string (interpreted to be the ID)

```
"b5e72f0a-cf1b-4732-bb83-3bb756c77ee1"
```

Such a schema could look like

```json
{
    "type" : [ "object", "string" ],
    "properties" : {
        "Id" : {
            "type" : "string",
            "pattern" : "^[a-fA-F0-9]{8}-([a-fA-F0-9]{4}-){3}[a-fA-F0-9]{12}$"
        },
        "Score" : {
            "type" : "number",
            "minimum" : 0
        }
        "Username" : { "type" : "string" }
    },
    "pattern" : "^[a-fA-F0-9]{8}-([a-fA-F0-9]{4}-){3}[a-fA-F0-9]{12}$"
}
```

This schema defines:

- that the JSON could be a string or an object
- that, if an object, it should have properties
    - `Id` as a string matching a GUID RegEx
    - `Score` as a number greater than or equal to 0
    - `Username` as any string
- that, if a string, it should match a GUID RegEx

This would be built in Manatee.Json as

```csharp
var guidPattern = "^[a-fA-F0-9]{8}-([a-fA-F0-9]{4}-){3}[a-fA-F0-9]{12}$";
var schema = new JsonSchema
    {
        Type = JsonSchemaType.String | JsonSchemaType.Object,
        Properties = new Dictionary<string, object>
            {
                ["Id"] = new JsonSchema06
                    {
                        Type = JsonSchemaType.String,
                        Pattern = guidPattern
                    },
                ["Score"] = new JsonSchema06
                    {
                        Type = JsonSchemaType.Number,
                        Minimum = 0
                    },
                ["Username"] = new JsonSchema06
                    {
                        Type = JsonSchemaType.String
                    },
            },
        Pattern = guidPattern
    }
```

The key here to remember is that `Properties` only validates when the JSON is an object, and `Pattern` only validates when the JSON is a string.  Otherwise, these are ignored.  The `Type` property ensures that it fails for anything except strings or objects.

That covers all of the basics for defining and building schema directly.  As expected, schema can also be downloaded and deserialized.  This will be covered in the *Schema Serialization* section below.

> **Note:** On occasion, an object may have a property which is of the same type.  In order to declare this kind of property, you should use the static property `Root` defined on the appropriate schema class.  This will instruct the reference to validate the property against the root schema.

## Validation

The purpose of JSON Schema is to validate that a JSON value meets certain criteria.  Let's take a look at how Manatee.Json schema objects provide this service.

Every schema object is required to implement the `IJsonSchema` interface.  This interface defines a single method:

```csharp
SchemaValidationResults Validate(JsonValue json, JsonValue root = null)
```

It is this method which we will use to validate incoming JSON values.  Let's begin with our aforementioned schema and JSON objects:

```json 
{
    "properties":{
        "myProperty":{
            "type":"string",
            "minLength":10
        }
    },
    "required":["myProperty"]
}

{}
{"myProperty":false}
{"myProperty":"some string"}
{"otherProperty":35.4}
"nonObject"
```

To validate these, all we have to do is pass these into our schema's `Validate()` method.

```csharp
var schema = GetSchema(); // defines the schema we have listed above.
var emptyJson = new JsonObject();
var booleanJson = new JsonObject { {"myProperty", false} };
var stringJson = new JsonObject { {"myProperty", "some string"} };
var numberJson = new JsonObject { {"otherProperty", 35.4} };
var nonObject = (JsonValue)"nonObject";

var emptyResults = schema.Validate(emptyJson);
var booleanResults = schema.Validate(booleanJson);
var stringResults = schema.Validate(stringJson);
var numberResults = schema.Validate(numberJson);
var nonObjectResults = schame.Validate(nonObject);
```

> **IMPORTANT** You may have noticed that the second parameter is not used.  This parameter is used internally for validating subschema and resolving references and should not be used explicitly.

The various results objects are of type `SchemaValidationResults`, which has two properties:

- IsValid - Indicates if the tested JSON value is valid,
- Errors - A collection of errors found while validating.  Each error lists a JSONPath to the problem element and a message which describes the issue.

In the above example, the following would be reported:

- `emptyJson` and `numberJson` failed because `"myProperty"` was not found.
- `booleanJson` failed because `"myProperty"` is of the wrong type.
- `stringJson` passed validation.
- `nonObject` also passed validation because `properties` and `required` ignore non-object JSON.

## Schema Serialization

Of what use would JSON Schema objects be to a developer if they were not serializable to and from JSON?  Not a whole lot.  Therefore, Manatee.Json's schema objects are designed to be completely serializable to and from `JsonObject`s (and subsequently to and from strings).

```csharp
var schema = GetSchema();
var json = schema.ToJson(null);
```

This can also be done through a `JsonSerializer` instance, of course.  The serializer will yield the same results.

> **Note:** The `null` passed into the `ToJson()` method indicates that a serializer instance is not required to serialize the schema classes.

For deserialization, you will need to go through the `JsonSchemaFactory.FromJson()` static method.  Simply pass your JSON in, and you get the appropriate JSON schema out.

```csharp
var json = GetSchemaJson();
var schema = JsonSchemaFactory.FromJson(json);
```

You may prefer to use deserialization combined with JSON object construction to generate your schema.

```csharp
var json = new JsonObject
    {
        {"properties", new JsonObject
            {
                {"myProperty", new JsonObject
                    {
                        {"type", "string"},
                        {"minLength", 10}
                    }
                }
            }
        }
        {"required", new JsonArray {"myProperty"}}
    };
var serializer = new JsonSerializer();
var schema = serializer.Deserialize<IJsonSchema>(json);
```

As you can see, the schema definition is easy to read.  The downside to this approach is that you don't get the compile-time checking of the `JsonSchema0x` classes.

This is also useful when your schema is defined at a URI.  Simply download the content, parse it, and run it through the schema factory.

```csharp
var json = JsonValue.Parse(new WebClient().DownloadString("http://json-schema.org/geo"));
var schema = JsonSchemaFactory.FromJson(json);
```

## Schema Version Selection

`JsonSchemaFactory` will make a best guess when deserializing schemata, but many times, both drafts may work for a given schema.  In these cases a default implementation must be used.

To determine the schema version, use `JsonSchemaFactory.SetDefaultSchemaVersion<T>()` and pass in either `JsonSchema04`, `JsonSchema06`, or `JsonSchema07`.  This will instruct the system to create instances of this type when deserializing and dereferencing schemata.

> **NOTE** This method will throw an exception if any type outside of these two are used.

Somewhat related to schema serialization is the ability to download schema directly.  When you need to do this, it is suggested that you use the `JsonSchemaRegistry.Get()` static method.  Just pass in a URI, and it will download and deserialize the schema automatically.  The benefit to using this method is that the class will also cache the schema for future use.  If there are any references to that URI, it will simply return this schema rather than downloading and deserializing again. 

This static class also exposes additional methods which allow you to manage the cache of schemata.

- `Register(JsonSchema)` - Adds the schema to the cache.
- `Unregister(JsonSchema)` - Removes the schema from the cache.
- `Unregister(string)` - Removes a schema registered to the specified URI from the cache.

> **NOTE** To register a schema, it must have a valid `id`.

## Final Notes

Manatee.Json's schema implementation has been designed using the latest two drafts of JSON Schema: draft 4 and draft 6.  Included with these schemas are self-validating schema (metaschema) which serve as a basis for all other schema.  This means that any schema you create must pass validation by this base schema.  Furthermore, both schema classes define their metaschema and the empty schema as static properties.

As drafts 6 and 7 support booleans as valid schema, implicit casts have been created from `bool` to `JsonSchema06` and `JsonSchema07`.  These are translated to the static properties `True` and `False` on their respective types.  These casts will not work implicitly when assigning to a property of type `IJsonSchema`; an explicit cast will be required.