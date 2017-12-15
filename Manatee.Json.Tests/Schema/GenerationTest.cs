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
							["TestEnum"] = new JsonSchema07
								{
									Enum = new List<EnumSchemaValue>
										{
											"None",
											"BasicEnumValue",
											"enum_value_with_description"
										}
								},
							["FlagsEnum"] = new JsonSchema07
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
							["EnumProp"] = new JsonSchemaReference("#/Definitions/TestEnum", typeof(JsonSchema07)),
							["FlagsEnumProp"] = new JsonSchemaReference("#/Definitions/FlagsEnum", typeof(JsonSchema07)),
							["MappedProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.Integer
								},
							["ReadOnlyListProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.Array,
									Items = new JsonSchema07
										{
											Type = JsonSchemaType.Integer
										}
								},
							["ReadOnlyDictionaryProp"] = new JsonSchema07
								{
									Type = JsonSchemaType.Object,
									AdditionalProperties = new JsonSchema07
										{
											Type = JsonSchemaType.Integer
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
	}
}
