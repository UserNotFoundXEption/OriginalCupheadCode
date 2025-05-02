using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace DialoguerCore
{
	// Token: 0x020005FA RID: 1530
	[XmlRoot("dictionary")]
	public class DialoguerSerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, IXmlSerializable
	{
		// Token: 0x06003EBD RID: 16061 RVA: 0x0003270B File Offset: 0x0003090B
		public XmlSchema GetSchema()
		{
			return null;
		}

		// Token: 0x06003EBE RID: 16062 RVA: 0x0011D94C File Offset: 0x0011BB4C
		public void ReadXml(XmlReader reader)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			bool isEmptyElement = reader.IsEmptyElement;
			reader.Read();
			if (isEmptyElement)
			{
				return;
			}
			while (reader.NodeType != XmlNodeType.EndElement)
			{
				reader.ReadStartElement("item");
				reader.ReadStartElement("key");
				TKey key = (TKey)((object)xmlSerializer.Deserialize(reader));
				reader.ReadEndElement();
				reader.ReadStartElement("value");
				TValue value = (TValue)((object)xmlSerializer2.Deserialize(reader));
				reader.ReadEndElement();
				base.Add(key, value);
				reader.ReadEndElement();
				reader.MoveToContent();
			}
			reader.ReadEndElement();
		}

		// Token: 0x06003EBF RID: 16063 RVA: 0x0011DA04 File Offset: 0x0011BC04
		public void WriteXml(XmlWriter writer)
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(TKey));
			XmlSerializer xmlSerializer2 = new XmlSerializer(typeof(TValue));
			foreach (TKey tkey in base.Keys)
			{
				writer.WriteStartElement("item");
				writer.WriteStartElement("key");
				xmlSerializer.Serialize(writer, tkey);
				writer.WriteEndElement();
				writer.WriteStartElement("value");
				TValue tvalue = base[tkey];
				xmlSerializer2.Serialize(writer, tvalue);
				writer.WriteEndElement();
				writer.WriteEndElement();
			}
		}
	}
}
