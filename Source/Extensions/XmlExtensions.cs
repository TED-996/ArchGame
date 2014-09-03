using System;
using System.IO;
using System.Xml.Serialization;

namespace ArchGame.Extensions {
	/// <summary>
	/// Static class with extensions for working with XML files.
	/// </summary>
	public static class XmlExtensions {
		/// <summary>
		/// Serialize an object to a string in XML format.
		/// </summary>
		public static string XmlSerialize<T>(this T obj) where T : class, new() {
			if (obj == null) {
				throw new ArgumentNullException("obj", "Cannot serialize a null object");
			}

			XmlSerializer serializer = new XmlSerializer(typeof(T));

			using (StringWriter writer = new StringWriter()) {
				serializer.Serialize(writer, obj);
				return obj.ToString();
			}
		}
		
		/// <summary>
		/// Deserialize an object from a string in XML format.
		/// </summary>
		public static T XmlDeserialize<T>(this string xml) where T : class, new() {
			if (xml == null) {
				throw new ArgumentNullException("xml", "Cannot deserialize from a null string");
			}

			XmlSerializer serializer = new XmlSerializer(typeof(T));

			using (StringReader reader = new StringReader(xml)) {
				try {
					return (T) serializer.Deserialize(reader);
				}
				catch {
					return null;
				}
			}
		}
	}
}