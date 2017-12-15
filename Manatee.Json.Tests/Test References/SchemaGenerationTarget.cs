using System.Collections.Generic;
using Manatee.Json.Schema.Generation;

namespace Manatee.Json.Tests.Test_References
{
	public class SchemaGenerationTarget
	{
		[MinLength(10)]
		[MaxLength(20)]
		public string StringProp { get; set; }
		//[Range(0, 50)]
		public int IntProp { get; set; }
		//[Range(0, 50)]
		public double DoubleProp { get; set; }
		public bool BoolProp { get; set; }
		//[Required]
		public TestEnum EnumProp { get; set; }
		public FlagsEnum FlagsEnumProp { get; set; }
		public int MappedProp { get; set; }
		[Minimum(10)]
		public List<int> ReadOnlyListProp { get; } = new List<int>();
		[MaxLength(10)]
		public Dictionary<string, int> ReadOnlyDictionaryProp { get; } = new Dictionary<string, int>();
		//[EmailAddress]
		public string Email { get; set; }
		//[RegularExpression("^[a-zA-Z0-9]*$")]
		public string Alphanumeric { get; set; }
		//[Url]
		public string Website { get; set; }
	}
}