using System;
using System.Collections.Generic;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000631 RID: 1585
	public sealed class RenderTextureFactory : IDisposable
	{
		// Token: 0x060040FA RID: 16634 RVA: 0x00034111 File Offset: 0x00032311
		public RenderTextureFactory()
		{
			this.m_TemporaryRTs = new HashSet<RenderTexture>();
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x0012F84C File Offset: 0x0012DA4C
		public RenderTexture Get(RenderTexture baseRenderTexture)
		{
			return this.Get(baseRenderTexture.width, baseRenderTexture.height, baseRenderTexture.depth, baseRenderTexture.format, (!baseRenderTexture.sRGB) ? 1 : 2, baseRenderTexture.filterMode, baseRenderTexture.wrapMode, "FactoryTempTexture");
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x0012F89C File Offset: 0x0012DA9C
		public RenderTexture Get(int width, int height, int depthBuffer = 0, RenderTextureFormat format = 2, RenderTextureReadWrite rw = 0, FilterMode filterMode = 1, TextureWrapMode wrapMode = 1, string name = "FactoryTempTexture")
		{
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, depthBuffer, format, rw);
			temporary.filterMode = filterMode;
			temporary.wrapMode = wrapMode;
			temporary.name = name;
			this.m_TemporaryRTs.Add(temporary);
			return temporary;
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x0012F8DC File Offset: 0x0012DADC
		public void Release(RenderTexture rt)
		{
			if (rt == null)
			{
				return;
			}
			if (!this.m_TemporaryRTs.Contains(rt))
			{
				throw new ArgumentException(string.Format("Attempting to remove a RenderTexture that was not allocated: {0}", rt));
			}
			this.m_TemporaryRTs.Remove(rt);
			RenderTexture.ReleaseTemporary(rt);
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x0012F92C File Offset: 0x0012DB2C
		public void ReleaseAll()
		{
			foreach (RenderTexture renderTexture in this.m_TemporaryRTs)
			{
				RenderTexture.ReleaseTemporary(renderTexture);
			}
			this.m_TemporaryRTs.Clear();
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x00034124 File Offset: 0x00032324
		public void Dispose()
		{
			this.ReleaseAll();
		}

		// Token: 0x040033A2 RID: 13218
		public HashSet<RenderTexture> m_TemporaryRTs;
	}
}
