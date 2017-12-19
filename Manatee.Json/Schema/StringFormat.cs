using System;

namespace Manatee.Json.Schema
{
	/// <summary>
	/// Defines various string formatting types used for StringSchema validation.
	/// </summary>
	public enum StringFormat
	{
		NotDefined,
		/// <summary>
		/// Defines a date/time format via <see cref="DateTime.TryParse(string, out DateTime)"/>
		/// </summary>
		DateTime,
		/// <summary>
		/// Defines an email address format.
		/// </summary>
		/// <remarks>
		/// From http://www.regular-expressions.info/email.html
		/// </remarks>
		Email,
		// from [lost the link, sorry]
		/// <summary>
		/// Defines a host name format.
		/// </summary>
		HostName,
		// from [lost the link, sorry]
		/// <summary>
		/// Defines an IPV4 address format.
		/// </summary>
		Ipv4,
		// from [lost the link, sorry]
		/// <summary>
		/// Defines an IPV6 format.
		/// </summary>
		Ipv6,
		/// <summary>
		/// Defines a regular expression format.
		/// </summary>
		Regex,
		/// <summary>
		/// Defines a URI format via <see cref="System.Uri.IsWellFormedUriString(string, UriKind)"/>.
		/// </summary>
		/// <remarks>For draft-06 schema, only use this for absolute URIs.</remarks>
		Uri,
		/// <summary>
		/// Defines a URI format per RFC 3896.
		/// </summary>
		UriReference
	}
}