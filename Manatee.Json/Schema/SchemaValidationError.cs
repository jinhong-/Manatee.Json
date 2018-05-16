﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Manatee.Json.Schema
{
    /// <summary>
    /// Represents a single schema validation error.
    /// </summary>
    public class SchemaValidationError : IEquatable<SchemaValidationError>
    {
        public IJsonSchema Schema { get; }
        public string ValidationKeyword { get; }
        public IEnumerable<SchemaValidationError> InnerErrors { get; }

        /// <summary>
        /// The property or property path which failed validation.
        /// </summary>
        public string PropertyName { get; private set; }
        /// <summary>
        /// A message indicating the failure.
        /// </summary>
        public string Message { get; }

        internal SchemaValidationError(IJsonSchema schema, string propertyName, string message,
            string validationKeyword = null, IEnumerable<SchemaValidationError> innerErrors = null)
        {
            Schema = schema;
            PropertyName = propertyName;
            Message = message;
            ValidationKeyword = validationKeyword;
            InnerErrors = innerErrors?.ToArray();
        }

        internal SchemaValidationError PrependPropertyName(string parent)
        {
            if (string.IsNullOrWhiteSpace(PropertyName))
                PropertyName = parent;
            else
                PropertyName = parent + (PropertyName[0] == '[' ? string.Empty : ".") + PropertyName;
            return this;
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>
        /// A string that represents the current object.
        /// </returns>
        public override string ToString()
        {
            return string.IsNullOrWhiteSpace(PropertyName)
                ? Message
                : $"Property: {PropertyName} - {Message}";
        }
        /// <summary>Indicates whether the current object is equal to another object of the same type.</summary>
        /// <returns>true if the current object is equal to the <paramref name="other" /> parameter; otherwise, false.</returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(SchemaValidationError other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(PropertyName, other.PropertyName) && string.Equals(Message, other.Message);
        }
        /// <summary>Determines whether the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />.</summary>
        /// <returns>true if the specified <see cref="T:System.Object" /> is equal to the current <see cref="T:System.Object" />; otherwise, false.</returns>
        /// <param name="obj">The object to compare with the current object. </param>
        public override bool Equals(object obj)
        {
            return Equals(obj as SchemaValidationError);
        }
        /// <summary>Serves as a hash function for a particular type. </summary>
        /// <returns>A hash code for the current <see cref="T:System.Object" />.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return ((PropertyName?.GetHashCode() ?? 0) * 397) ^ (Message?.GetHashCode() ?? 0);
            }
        }
    }
}