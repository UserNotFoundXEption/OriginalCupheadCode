using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityEngine.UI
{
	// Token: 0x020006A6 RID: 1702
	[AddComponentMenu("UI/Effects/Letter Spacing", 15)]
	public class LetterSpacing : BaseMeshEffect
	{
		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x06004770 RID: 18288 RVA: 0x00038C23 File Offset: 0x00036E23
		// (set) Token: 0x06004771 RID: 18289 RVA: 0x00038C2B File Offset: 0x00036E2B
		public float spacing
		{
			get
			{
				return this.m_spacing;
			}
			set
			{
				if (this.m_spacing == value)
				{
					return;
				}
				this.m_spacing = value;
				if (base.graphic != null)
				{
					base.graphic.SetVerticesDirty();
				}
			}
		}

		// Token: 0x06004772 RID: 18290 RVA: 0x0015B4EC File Offset: 0x001596EC
		public override void ModifyMesh(VertexHelper vh)
		{
			if (!this.IsActive())
			{
				return;
			}
			List<UIVertex> list = new List<UIVertex>();
			vh.GetUIVertexStream(list);
			this.ModifyVertices(list);
			vh.Clear();
			vh.AddUIVertexTriangleStream(list);
		}

		// Token: 0x06004773 RID: 18291 RVA: 0x0015B528 File Offset: 0x00159728
		public void ModifyVertices(List<UIVertex> verts)
		{
			if (!this.IsActive())
			{
				return;
			}
			Text component = base.GetComponent<Text>();
			string text = component.text;
			IList<UILineInfo> lines = component.cachedTextGenerator.lines;
			for (int i = lines.Count - 1; i > 0; i--)
			{
				text = text.Insert(lines[i].startCharIdx, "\n");
				text = text.Remove(lines[i].startCharIdx - 1, 1);
			}
			string[] array = text.Split(new char[]
			{
				'\n'
			});
			if (component == null)
			{
				Debug.LogWarning("LetterSpacing: Missing Text component");
				return;
			}
			float num = this.spacing * (float)component.fontSize / 100f;
			float num2 = 0f;
			int num3 = 0;
			bool flag = this.useRichText && component.supportRichText;
			IEnumerator enumerator = null;
			Match match = null;
			switch (component.alignment)
			{
			case 0:
			case 3:
			case 6:
				num2 = 0f;
				break;
			case 1:
			case 4:
			case 7:
				num2 = 0.5f;
				break;
			case 2:
			case 5:
			case 8:
				num2 = 1f;
				break;
			}
			foreach (string text2 in array)
			{
				int length = text2.Length;
				if (flag)
				{
					enumerator = this.GetRegexMatchedTagCollection(text2, out length);
					match = null;
					if (enumerator.MoveNext())
					{
						match = (Match)enumerator.Current;
					}
				}
				float num4 = (float)(length - 1) * num * num2;
				int k = 0;
				int num5 = 0;
				while (k < text2.Length)
				{
					if (flag && match != null && match.Index == k)
					{
						k += match.Length - 1;
						num5--;
						num3 += match.Length;
						match = null;
						if (enumerator.MoveNext())
						{
							match = (Match)enumerator.Current;
						}
					}
					else
					{
						int index = num3 * 6;
						int index2 = num3 * 6 + 1;
						int index3 = num3 * 6 + 2;
						int index4 = num3 * 6 + 3;
						int index5 = num3 * 6 + 4;
						int num6 = num3 * 6 + 5;
						if (num6 > verts.Count - 1)
						{
							return;
						}
						UIVertex value = verts[index];
						UIVertex value2 = verts[index2];
						UIVertex value3 = verts[index3];
						UIVertex value4 = verts[index4];
						UIVertex value5 = verts[index5];
						UIVertex value6 = verts[num6];
						Vector3 vector = Vector3.right * (num * (float)num5 - num4);
						value.position += vector;
						value2.position += vector;
						value3.position += vector;
						value4.position += vector;
						value5.position += vector;
						value6.position += vector;
						verts[index] = value;
						verts[index2] = value2;
						verts[index3] = value3;
						verts[index4] = value4;
						verts[index5] = value5;
						verts[num6] = value6;
						num3++;
					}
					k++;
					num5++;
				}
				num3++;
			}
		}

		// Token: 0x06004774 RID: 18292 RVA: 0x0015B8A4 File Offset: 0x00159AA4
		public IEnumerator GetRegexMatchedTagCollection(string line, out int lineLengthWithoutTags)
		{
			MatchCollection matchCollection = Regex.Matches(line, "<b>|</b>|<i>|</i>|<size=.*?>|</size>|<color=.*?>|</color>|<material=.*?>|</material>");
			lineLengthWithoutTags = 0;
			int num = 0;
			if (matchCollection.Count > 0)
			{
				IEnumerator enumerator = matchCollection.GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						Match match = (Match)obj;
						num += match.Length;
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						disposable.Dispose();
					}
				}
			}
			lineLengthWithoutTags = line.Length - num;
			return matchCollection.GetEnumerator();
		}

		// Token: 0x04003820 RID: 14368
		public const string SupportedTagRegexPattersn = "<b>|</b>|<i>|</i>|<size=.*?>|</size>|<color=.*?>|</color>|<material=.*?>|</material>";

		// Token: 0x04003821 RID: 14369
		[SerializeField]
		public bool useRichText;

		// Token: 0x04003822 RID: 14370
		[SerializeField]
		public float m_spacing;
	}
}
