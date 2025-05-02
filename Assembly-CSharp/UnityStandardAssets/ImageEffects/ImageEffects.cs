using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006C0 RID: 1728
	[AddComponentMenu("")]
	public class ImageEffects
	{
		// Token: 0x060047F3 RID: 18419 RVA: 0x00161C40 File Offset: 0x0015FE40
		public static void RenderDistortion(Material material, RenderTexture source, RenderTexture destination, float angle, Vector2 center, Vector2 radius)
		{
			bool flag = source.texelSize.y < 0f;
			if (flag)
			{
				center.y = 1f - center.y;
				angle = -angle;
			}
			Matrix4x4 matrix4x = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, angle), Vector3.one);
			material.SetMatrix("_RotationMatrix", matrix4x);
			material.SetVector("_CenterRadius", new Vector4(center.x, center.y, radius.x, radius.y));
			material.SetFloat("_Angle", angle * 0.0174532924f);
			Graphics.Blit(source, destination, material);
		}

		// Token: 0x060047F4 RID: 18420 RVA: 0x000392F9 File Offset: 0x000374F9
		[Obsolete("Use Graphics.Blit(source,dest) instead")]
		public static void Blit(RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest);
		}

		// Token: 0x060047F5 RID: 18421 RVA: 0x00039302 File Offset: 0x00037502
		[Obsolete("Use Graphics.Blit(source, destination, material) instead")]
		public static void BlitWithMaterial(Material material, RenderTexture source, RenderTexture dest)
		{
			Graphics.Blit(source, dest, material);
		}
	}
}
