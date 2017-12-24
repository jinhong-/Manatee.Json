using System;
using System.Collections.Generic;
using Manatee.Json.Schema;
using Manatee.Json.Serialization;
using Manatee.Json.Tests.Test_References;
using NUnit.Framework;

namespace Manatee.Json.Tests.Schema
{
	[TestFixture]
	public class GenerationTest
	{
		[Test]
		public void GenerateSchemaFromClass()
		{
			var expected = new JsonSchema07
				{
					Definitions = new Dictionary<string, IJsonSchema>
						{
							["EnumSchemaValue"] = new JsonSchema07
								{
									Enum = new List<EnumSchemaValue>
										{
											"None",
											"BasicEnumValue",
											"enum_value_with_description"
										}
								}
						},
					Properties = new Dictionary<string, IJsonSchema>
						{
							["StringProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.String,
									MinLength = 10,
									MaxLength = 20
								},
							["IntProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.Integer,
									Minimum = 0,
									Maximum = 50
								},
							["DoubleProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.Number,
									Minimum = 0,
									Maximum = 50
								},
							["BoolProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.Boolean
								},
							["EnumProp"] = new JsonSchemaReference("#/Definitions/EnumSchemaValue", typeof(JsonSchema07)),
							["FlagsEnumProp"] = new JsonSchemaReference("#/Definitions/EnumSchemaValue", typeof(JsonSchema07)),
							["MappedProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.Integer
								},
							["ReadOnlyListProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.Array,
									Items = new JsonSchema07
										{
											Type = JsonSchemaType.Integer,
											Minimum = 10
										}
								},
							["ReadOnlyDictionaryProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.Object,
									AdditionalProperties = new JsonSchema07
										{
											Type = JsonSchemaType.String,
											MaxLength = 10
										}
								},
							["Email"] = new JsonSchema07
								{
									Type = JsonSchemaType.String,
									Format = StringFormat.Email
								},
							["Alphanumeric"] = new JsonSchema07
								{
									Type = JsonSchemaType.String,
									Pattern = "^[a-zA-Z0-9]*$",
									MinLength = 10,
									MaxLength = 20
								},
							["Website"] = new JsonSchema07
								{
									Type = JsonSchemaType.String,
									Format = StringFormat.Uri
								}
						}
				};

			var actual = JsonSchema07.GenerateFor<SchemaGenerationTarget>(new JsonSerializer());

			Assert.AreEqual(expected, actual);
		}

		[Test]
		public void GeneratedSchemaValidatesModel()
		{
			var model = new SchemaGenerationTarget
				{
					Alphanumeric = "noon23tn4in2",
					BoolProp = true,
					DoubleProp = 15.4,
					Email = "me@you.com",
					EnumProp = TestEnum.BasicEnumValue,
					FlagsEnumProp = FlagsEnum.EnumValueWithDescription,
					IntProp = 25,
					MappedProp = -6,
					ReadOnlyDictionaryProp =
						{
							["one"] = "1",
							["two"] = "2"
						},
					ReadOnlyListProp =
						{
							11,
							12,
							13,
							14
						},
					StringProp = "1234567890123",
					Website = "http://site.com"
				};
			var serializer = new JsonSerializer
				{
					Options =
						{
							EnumSerializationFormat = EnumSerializationFormat.AsName
						}
				};
			var json = serializer.Serialize(model);
			var schema = JsonSchema07.GenerateFor<SchemaGenerationTarget>(serializer);

			var results = schema.Validate(json);

			Console.WriteLine(schema.ToJson(new JsonSerializer()).ToString());
			Console.WriteLine();
			foreach (var schemaValidationError in results.Errors)
			{
				Console.WriteLine(schemaValidationError);
			}

			Assert.IsTrue(results.Valid);
		}
	}
}
