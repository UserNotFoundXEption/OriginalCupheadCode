using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C4 RID: 1732
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class PostEffectsBase : MonoBehaviour
	{
		// Token: 0x06004805 RID: 18437 RVA: 0x001628AC File Offset: 0x00160AAC
		public Material CheckShaderAndCreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				base.enabled = false;
				return null;
			}
			if (s.isSupported && m2Create && m2Create.shader == s)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				this.NotSupported();
				return null;
			}
			m2Create = new Material(s);
			m2Create.hideFlags = 52;
			if (m2Create)
			{
				return m2Create;
			}
			return null;
		}

		// Token: 0x06004806 RID: 18438 RVA: 0x00162928 File Offset: 0x00160B28
		public Material CreateMaterial(Shader s, Material m2Create)
		{
			if (!s)
			{
				return null;
			}
			if (m2Create && m2Create.shader == s && s.isSupported)
			{
				return m2Create;
			}
			if (!s.isSupported)
			{
				return null;
			}
			m2Create = new Material(s);
			m2Create.hideFlags = 52;
			if (m2Create)
			{
				return m2Create;
			}
			return null;
		}

		// Token: 0x06004807 RID: 18439 RVA: 0x0003938C File Offset: 0x0003758C
		public void OnEnable()
		{
			this.isSupported = true;
		}

		// Token: 0x06004808 RID: 18440 RVA: 0x00039395 File Offset: 0x00037595
		public bool CheckSupport()
		{
			return this.CheckSupport(false);
		}

		// Token: 0x06004809 RID: 18441 RVA: 0x0003939E File Offset: 0x0003759E
		public virtual bool CheckResources()
		{
			return this.isSupported;
		}

		// Token: 0x0600480A RID: 18442 RVA: 0x000393A6 File Offset: 0x000375A6
		public virtual void Start()
		{
			this.CheckResources();
		}

		// Token: 0x0600480B RID: 18443 RVA: 0x00162998 File Offset: 0x00160B98
		public bool CheckSupport(bool needDepth)
		{
			this.isSupported = true;
			this.supportHDRTextures = SystemInfo.SupportsRenderTextureFormat(2);
			this.supportDX11 = (SystemInfo.graphicsShaderLevel >= 50 && SystemInfo.supportsComputeShaders);
			if (!SystemInfo.supportsImageEffects)
			{
				this.NotSupported();
				return false;
			}
			if (needDepth && !SystemInfo.SupportsRenderTextureFormat(1))
			{
				this.NotSupported();
				return false;
			}
			if (needDepth)
			{
				base.GetComponent<Camera>().depthTextureMode |= 1;
			}
			return true;
		}

		// Token: 0x0600480C RID: 18444 RVA: 0x000393AF File Offset: 0x000375AF
		public bool CheckSupport(bool needDepth, bool needHdr)
		{
			if (!this.CheckSupport(needDepth))
			{
				return false;
			}
			if (needHdr && !this.supportHDRTextures)
			{
				this.NotSupported();
				return false;
			}
			return true;
		}

		// Token: 0x0600480D RID: 18445 RVA: 0x000393D9 File Offset: 0x000375D9
		public bool Dx11Support()
		{
			return this.supportDX11;
		}

		// Token: 0x0600480E RID: 18446 RVA: 0x000393E1 File Offset: 0x000375E1
		public void ReportAutoDisable()
		{
		}

		// Token: 0x0600480F RID: 18447 RVA: 0x000393E3 File Offset: 0x000375E3
		public bool CheckShader(Shader s)
		{
			if (!s.isSupported)
			{
				this.NotSupported();
				return false;
			}
			return false;
		}

		// Token: 0x06004810 RID: 18448 RVA: 0x000393F9 File Offset: 0x000375F9
		public void NotSupported()
		{
			base.enabled = false;
			this.isSupported = false;
		}

		// Token: 0x06004811 RID: 18449 RVA: 0x00162A18 File Offset: 0x00160C18
		public void DrawBorder(RenderTexture dest, Material material)
		{
			RenderTexture.active = dest;
			bool flag = true;
			GL.PushMatrix();
			GL.LoadOrtho();
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				float num;
				float num2;
				if (flag)
				{
					num = 1f;
					num2 = 0f;
				}
				else
				{
					num = 0f;
					num2 = 1f;
				}
				float num3 = 0f;
				float num4 = 1f / ((float)dest.width * 1f);
				float num5 = 0f;
				float num6 = 1f;
				GL.Begin(7);
				GL.TexCoord2(0f, num);
				GL.Vertex3(num3, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num3, num6, 0.1f);
				num3 = 1f - 1f / ((float)dest.width * 1f);
				num4 = 1f;
				num5 = 0f;
				num6 = 1f;
				GL.TexCoord2(0f, num);
				GL.Vertex3(num3, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num3, num6, 0.1f);
				num3 = 0f;
				num4 = 1f;
				num5 = 0f;
				num6 = 1f / ((float)dest.height * 1f);
				GL.TexCoord2(0f, num);
				GL.Vertex3(num3, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num3, num6, 0.1f);
				num3 = 0f;
				num4 = 1f;
				num5 = 1f - 1f / ((float)dest.height * 1f);
				num6 = 1f;
				GL.TexCoord2(0f, num);
				GL.Vertex3(num3, num5, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(num4, num5, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(num4, num6, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(num3, num6, 0.1f);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x0400397E RID: 14718
		public bool supportHDRTextures = true;

		// Token: 0x0400397F RID: 14719
		public bool supportDX11;

		// Token: 0x04003980 RID: 14720
		public bool isSupported = true;
	}
}
