using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000630 RID: 1584
	public sealed class MaterialFactory : IDisposable
	{
		// Token: 0x060040F7 RID: 16631 RVA: 0x000340FE File Offset: 0x000322FE
		public MaterialFactory()
		{
			this.m_Materials = new Dictionary<string, Material>();
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x0012F778 File Offset: 0x0012D978
		public Material Get(string shaderName)
		{
			Material material;
			if (!this.m_Materials.TryGetValue(shaderName, out material))
			{
				Shader shader = Shader.Find(shaderName);
				if (shader == null)
				{
					throw new ArgumentException(string.Format("Shader not found ({0})", shaderName));
				}
				material = new Material(shader)
				{
					name = string.Format("PostFX - {0}", shaderName.Substring(shaderName.LastIndexOf("/") + 1)),
					hideFlags = 52
				};
				this.m_Materials.Add(shaderName, material);
			}
			return material;
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x0012F800 File Offset: 0x0012DA00
		public void Dispose()
		{
			foreach (KeyValuePair<string, Material> keyValuePair in this.m_Materials)
			{
				Material value = keyValuePair.Value;
				GraphicsUtils.Destroy(value);
			}
			this.m_Materials.Clear();
		}

		// Token: 0x040033A1 RID: 13217
		public Dictionary<string, Material> m_Materials;
	}
}
