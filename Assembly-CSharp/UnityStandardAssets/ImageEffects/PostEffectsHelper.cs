using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C5 RID: 1733
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	public class PostEffectsHelper : MonoBehaviour
	{
		// Token: 0x06004813 RID: 18451 RVA: 0x00039411 File Offset: 0x00037611
		public void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}

		// Token: 0x06004814 RID: 18452 RVA: 0x00162CB8 File Offset: 0x00160EB8
		public static void DrawLowLevelPlaneAlignedWithCamera(float dist, RenderTexture source, RenderTexture dest, Material material, Camera cameraForProjectionMatrix)
		{
			RenderTexture.active = dest;
			material.SetTexture("_MainTex", source);
			bool flag = true;
			GL.PushMatrix();
			GL.LoadIdentity();
			GL.LoadProjectionMatrix(cameraForProjectionMatrix.projectionMatrix);
			float num = cameraForProjectionMatrix.fieldOfView * 0.5f * 0.0174532924f;
			float num2 = Mathf.Cos(num) / Mathf.Sin(num);
			float aspect = cameraForProjectionMatrix.aspect;
			float num3 = aspect / -num2;
			float num4 = aspect / num2;
			float num5 = 1f / -num2;
			float num6 = 1f / num2;
			float num7 = 1f;
			num3 *= dist * num7;
			num4 *= dist * num7;
			num5 *= dist * num7;
			num6 *= dist * num7;
			float num8 = -dist;
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				GL.Begin(7);
				float num9;
				float num10;
				if (flag)
				{
					num9 = 1f;
					num10 = 0f;
				}
				else
				{
					num9 = 0f;
					num10 = 1f;
				}
				GL.TexCoord2(0f, num9);
				GL.Vertex3(num3, num5, num8);
				GL.TexCoord2(1f, num9);
				GL.Vertex3(num4, num5, num8);
				GL.TexCoord2(1f, num10);
				GL.Vertex3(num4, num6, num8);
				GL.TexCoord2(0f, num10);
				GL.Vertex3(num3, num6, num8);
				GL.End();
			}
			GL.PopMatrix();
		}

		// Token: 0x06004815 RID: 18453 RVA: 0x00162E20 File Offset: 0x00161020
		public static void DrawBorder(RenderTexture dest, Material material)
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

		// Token: 0x06004816 RID: 18454 RVA: 0x001630C0 File Offset: 0x001612C0
		public static void DrawLowLevelQuad(float x1, float x2, float y1, float y2, RenderTexture source, RenderTexture dest, Material material)
		{
			RenderTexture.active = dest;
			material.SetTexture("_MainTex", source);
			bool flag = true;
			GL.PushMatrix();
			GL.LoadOrtho();
			for (int i = 0; i < material.passCount; i++)
			{
				material.SetPass(i);
				GL.Begin(7);
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
				GL.TexCoord2(0f, num);
				GL.Vertex3(x1, y1, 0.1f);
				GL.TexCoord2(1f, num);
				GL.Vertex3(x2, y1, 0.1f);
				GL.TexCoord2(1f, num2);
				GL.Vertex3(x2, y2, 0.1f);
				GL.TexCoord2(0f, num2);
				GL.Vertex3(x1, y2, 0.1f);
				GL.End();
			}
			GL.PopMatrix();
		}
	}
}
