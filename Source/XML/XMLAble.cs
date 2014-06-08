using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using SFMLGame.Services;

namespace SFMLGame.XML {
	/// <summary>
	/// This class, when inherited, will be able to save all Public, instance, value-type or String, members inplementing
	/// IConvertible to XML and load them.
	/// </summary>
	public abstract class XMLAble {
		readonly string filename;
		readonly FieldInfo[] validFields;

		protected XMLAble(string newFilename) {
			filename = newFilename;

			Type iConvertible = typeof (IConvertible);
			Type stringType = typeof (String);
			validFields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance)
				.Where(field => iConvertible.IsAssignableFrom(field.FieldType) &&
					   (field.FieldType.IsValueType || field.FieldType == stringType)).ToArray();
		}

		public void SaveToFile() {
			string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filename);
			string directoryName = Path.GetDirectoryName(fullPath);
			if (directoryName == null) {
				throw new ArgumentException("Filename is invalid");
			}
			if (!Directory.Exists(directoryName)) {
				Directory.CreateDirectory(directoryName);
			}

			XDocument document = new XDocument(GetXElement());
			using (XmlWriter writer = new XmlTextWriter(fullPath, null)) {
				document.WriteTo(writer);
			}
			ServiceLocator.GetService<ILogger>().Log("Saved to file.", "XMLAble");
		}

		public XElement GetXElement() {
			Type type = GetType();
			XElement root = new XElement(type.Name);

			foreach (FieldInfo field in validFields) {
// ReSharper disable AssignNullToNotNullAttribute
				root.Add(new XElement(field.Name,
					((IConvertible) field.GetValue(this) ?? "null").ToString(CultureInfo.InvariantCulture)));
// ReSharper restore AssignNullToNotNullAttribute
			}
			return root;
		}

		public void LoadFromFile() {
			string fullPath = Path.Combine(Directory.GetCurrentDirectory(), filename);
			if (!File.Exists(fullPath)) {
				return;
			}
// ReSharper disable PossibleNullReferenceException
			XDocument document = XDocument.Load(fullPath);
			XElement root = document.Root;

			Type stringType = typeof(String);

			foreach (XElement element in root.Elements()) {
				string fieldName = element.Name.LocalName;
				string value = element.Value;
				try {
					FieldInfo field = validFields.First(f => f.Name == fieldName);
					Type fieldType = field.FieldType;
					field.SetValue(this, fieldType == stringType ? value : Convert.ChangeType(value, fieldType));
				}
				catch (InvalidOperationException) {
					ServiceLocator.GetService<ILogger>() .Log("Field not found.", "XMLAble, LoadFromFile, " + filename,
						LogMessageType.Warning);
				}
				catch (InvalidCastException) {
					ServiceLocator.GetService<ILogger>().Log("Invalid type to cast.", "XMLAble, LoadFromFile, " + filename,
						LogMessageType.Warning);
				}

			}
// ReSharper restore PossibleNullReferenceException
			ServiceLocator.GetService<ILogger>().Log("Loaded from file.", "XMLAble");
		}
	}
}