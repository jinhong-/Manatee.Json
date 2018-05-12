using Manatee.Json.Schema;
using NUnit.Framework;

namespace Manatee.Json.Tests.Schema
{
	[TestFixture]
	public class OtherSchemaTests
	{
		[Test]
		[Ignore("Not certain the expected result of this test.")]
		public void LocationIndependentReferences()
		{
			JsonSchemaFactory.SetDefaultSchemaVersion<JsonSchema07>();

			var fileName = System.IO.Path.Combine(TestContext.CurrentContext.TestDirectory, @"Files\Location-independent-schema.json").AdjustForOS();
			var schema = JsonSchemaRegistry.Get(fileName);

			JsonSchemaRegistry.Register(schema);

			var json = new JsonObject
				{
					["X"] = new JsonObject()
				};

			var results = schema.Validate(json);

			Assert.IsFalse(results.Valid);
		}
	}
}
