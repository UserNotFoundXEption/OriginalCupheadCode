using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

// Token: 0x02000071 RID: 113
public class Xml
{
	// Token: 0x060005B4 RID: 1460 RVA: 0x0006D6C4 File Offset: 0x0006B8C4
	public static string Serialize(object obj)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlSerializer xmlSerializer = new XmlSerializer(obj.GetType());
		using (TextWriter textWriter = new StringWriter(stringBuilder))
		{
			xmlSerializer.Serialize(textWriter, obj);
		}
		return stringBuilder.ToString();
	}

	// Token: 0x060005B5 RID: 1461 RVA: 0x0006D71C File Offset: 0x0006B91C
	public static T Deserialize<T>(string xml)
	{
		T result = default(T);
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
		using (TextReader textReader = new StringReader(xml))
		{
			result = (T)((object)xmlSerializer.Deserialize(textReader));
		}
		return result;
	}
}
