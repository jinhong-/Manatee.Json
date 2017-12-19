using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Manatee.Json.Internal;

namespace Manatee.Json.Schema
{
	internal static class StringFormatValidator
	{
		private class StringFormatData
		{
			private readonly Regex _validationRule;
			private readonly Func<string, bool> _validate;

			public string Key { get; }
			public List<Type> SupportedBy { get; }

			public StringFormatData(string key, Regex regex, params Type[] supportedBy)
			{
				Key = key;
				_validationRule = regex;
				SupportedBy = supportedBy.ToList();
			}
			public StringFormatData(string key, Func<string, bool> validate, params Type[] supportedBy)
			{
				_validate = validate;
				Key = key;
				SupportedBy = supportedBy.ToList();
			}
			public bool Validate(string value)
			{
				return _validationRule?.IsMatch(value) ?? _validate == null || _validate(value);
			}
		}

		private static readonly Dictionary<StringFormat, StringFormatData> _formats =
			new Dictionary<StringFormat, StringFormatData>
				{
					[StringFormat.NotDefined] = new StringFormatData(string.Empty, s => true,
					                                                 typeof(JsonSchema04), typeof(JsonSchema06), typeof(JsonSchema07)),
					[StringFormat.DateTime] = new StringFormatData("date-time", s => DateTime.TryParse(s, out _),
					                                               typeof(JsonSchema04), typeof(JsonSchema06), typeof(JsonSchema07)),
					[StringFormat.Email] = new StringFormatData("email",
					                                            new Regex("^(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])$", RegexOptions.IgnoreCase),
					                                            typeof(JsonSchema04), typeof(JsonSchema06), typeof(JsonSchema07)),
					[StringFormat.HostName] = new StringFormatData("hostName",
					                                               new Regex("^(?!.{255,})([a-zA-Z0-9-]{0,63}\\.)*([a-zA-Z0-9-]{0,63})$"),
					                                               typeof(JsonSchema04), typeof(JsonSchema06), typeof(JsonSchema07)),
					[StringFormat.Ipv4] = new StringFormatData("ipv4",
					                                           new Regex("^(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$"),
					                                           typeof(JsonSchema04), typeof(JsonSchema06), typeof(JsonSchema07)),
					[StringFormat.Ipv6] = new StringFormatData("ipv6",
					                                           new Regex("^(([0-9a-fA-F]{1,4}:){7,7}[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,7}:|([0-9a-fA-F]{1,4}:){1,6}:[0-9a-fA-F]{1,4}|([0-9a-fA-F]{1,4}:){1,5}(:[0-9a-fA-F]{1,4}){1,2}|([0-9a-fA-F]{1,4}:){1,4}(:[0-9a-fA-F]{1,4}){1,3}|([0-9a-fA-F]{1,4}:){1,3}(:[0-9a-fA-F]{1,4}){1,4}|([0-9a-fA-F]{1,4}:){1,2}(:[0-9a-fA-F]{1,4}){1,5}|[0-9a-fA-F]{1,4}:((:[0-9a-fA-F]{1,4}){1,6})|:((:[0-9a-fA-F]{1,4}){1,7}|:)|fe80:(:[0-9a-fA-F]{0,4}){0,4}%[0-9a-zA-Z]{1,}|::(ffff(:0{1,4}){0,1}:){0,1}((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]).){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9])|([0-9a-fA-F]{1,4}:){1,4}:((25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]).){3,3}(25[0-5]|(2[0-4]|1{0,1}[0-9]){0,1}[0-9]))$"),
					                                           typeof(JsonSchema04), typeof(JsonSchema06), typeof(JsonSchema07)),
					[StringFormat.Regex] = new StringFormatData("regex", (Regex) null, typeof(JsonSchema04), typeof(JsonSchema06), typeof(JsonSchema07)),
					[StringFormat.Uri] = new StringFormatData("uri", s => Uri.IsWellFormedUriString(s, UriKind.RelativeOrAbsolute),
					                                          typeof(JsonSchema04), typeof(JsonSchema06), typeof(JsonSchema07)),
					[StringFormat.UriReference] = new StringFormatData("uri-reference", Uri3986.IsValid,
					                                                   typeof(JsonSchema06), typeof(JsonSchema07)),
				};

		internal static bool Validate<T>(StringFormat format, string value)
			where T : IJsonSchema
		{
			var data = _formats[format];
			if (!data.SupportedBy.Contains(typeof(T)))
				throw new InvalidOperationException($"Format '{data.Key}' is not supported by {typeof(T).Name}");

			return data.Validate(value);
		}

		internal static void ValidateForDraft<T>(StringFormat format)
			where T : IJsonSchema
		{
			var data = _formats[format];
			if (!data.SupportedBy.Contains(typeof(T)))
				throw new InvalidOperationException($"Format '{data.Key}' is not supported by {typeof(T).Name}");
		}

		internal static string GetString(StringFormat format)
		{
			var data = _formats[format];
			return data.Key;
		}

		internal static StringFormat GetFormat(string key)
		{
			return _formats.FirstOrDefault(kvp => kvp.Value.Key == key).Key;
		}
	}
}