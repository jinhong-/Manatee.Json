using Manatee.Json.Schema;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Manatee.Json.Tests.Schema
{
    [TestFixture]
    public class IfThenElseTest
    {
        public static IEnumerable IfThenElseData
        {
            get
            {
                var objectSchema = new JsonSchema07
                {
                    If = new JsonSchema07
                    {
                        Properties = new Dictionary<string, IJsonSchema>
                        {
                            {
                                "power", new JsonSchema07
                                {
                                    Minimum = 9000
                                }
                            }
                        }
                    },
                    Then = new JsonSchema07
                    {
                        Required = new[] { "disbelief" }
                    },
                    Else = new JsonSchema07
                    {
                        Required = new[] { "confidence" }
                    }
                };

                yield return new TestCaseData(objectSchema, new JsonValue(new JsonObject(new Dictionary<string, JsonValue> { }))).Returns(true);
                yield return new TestCaseData(objectSchema, new JsonValue(new JsonObject(new Dictionary<string, JsonValue>
                {
                    { "power", new JsonValue(10000) },
                    { "disbelief", new JsonValue(true) }
                }))).Returns(true);
                yield return new TestCaseData(objectSchema, new JsonValue(new JsonObject(new Dictionary<string, JsonValue>
                {
                    { "power", new JsonValue(1000) },
                    { "disbelief", new JsonValue(true) }
                }))).Returns(true);

                yield return new TestCaseData(objectSchema, new JsonValue(new JsonObject(new Dictionary<string, JsonValue>
                {
                    { "power", new JsonValue(10000) }
                }))).Returns(false);
                yield return new TestCaseData(objectSchema, new JsonValue(new JsonObject(new Dictionary<string, JsonValue>
                {
                    { "power", new JsonValue(10000) },
                    { "confidence", new JsonValue(true) }
                }))).Returns(false);
                yield return new TestCaseData(objectSchema, new JsonValue(new JsonObject(new Dictionary<string, JsonValue>
                {
                    { "power", new JsonValue(1000) }
                }))).Returns(false);


                var integerSchema = new JsonSchema07
                {
                    Type = JsonSchemaType.Integer,
                    Minimum = 1,
                    Maximum = 1000,
                    If = new JsonSchema07
                    {
                        Minimum = 100
                    },
                    Then = new JsonSchema07
                    {
                        MultipleOf = 100
                    },
                    Else = new JsonSchema07
                    {
                        If = new JsonSchema07
                        {
                            Minimum = 10
                        },
                        Then = new JsonSchema07
                        {
                            MultipleOf = 10
                        }
                    }
                };
                yield return new TestCaseData(integerSchema, new JsonValue(1)).Returns(true);
                yield return new TestCaseData(integerSchema, new JsonValue(5)).Returns(true);
                yield return new TestCaseData(integerSchema, new JsonValue(10)).Returns(true);
                yield return new TestCaseData(integerSchema, new JsonValue(20)).Returns(true);
                yield return new TestCaseData(integerSchema, new JsonValue(50)).Returns(true);
                yield return new TestCaseData(integerSchema, new JsonValue(100)).Returns(true);
                yield return new TestCaseData(integerSchema, new JsonValue(200)).Returns(true);
                yield return new TestCaseData(integerSchema, new JsonValue(500)).Returns(true);
                yield return new TestCaseData(integerSchema, new JsonValue(1000)).Returns(true);

                yield return new TestCaseData(integerSchema, new JsonValue(-1)).Returns(false);
                yield return new TestCaseData(integerSchema, new JsonValue(0)).Returns(false);
                yield return new TestCaseData(integerSchema, new JsonValue(2000)).Returns(false);
                yield return new TestCaseData(integerSchema, new JsonValue(11)).Returns(false);
                yield return new TestCaseData(integerSchema, new JsonValue(57)).Returns(false);
                yield return new TestCaseData(integerSchema, new JsonValue(123)).Returns(false);
            }
        }

        [TestCaseSource(nameof(IfThenElseData))]
        public bool ValidateIfThenElse(IJsonSchema schema, JsonValue value)
        {
            var result = schema.Validate(value, value);
            return result.Valid;
        }
    }
}
