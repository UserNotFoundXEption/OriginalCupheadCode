using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x020006CE RID: 1742
	public class Triangles
	{
		// Token: 0x0600483B RID: 18491 RVA: 0x001648EC File Offset: 0x00162AEC
		public static bool HasMeshes()
		{
			if (Triangles.meshes == null)
			{
				return false;
			}
			for (int i = 0; i < Triangles.meshes.Length; i++)
			{
				if (null == Triangles.meshes[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600483C RID: 18492 RVA: 0x00164934 File Offset: 0x00162B34
		public static void Cleanup()
		{
			if (Triangles.meshes == null)
			{
				return;
			}
			for (int i = 0; i < Triangles.meshes.Length; i++)
			{
				if (null != Triangles.meshes[i])
				{
					Object.DestroyImmediate(Triangles.meshes[i]);
					Triangles.meshes[i] = null;
				}
			}
			Triangles.meshes = null;
		}

		// Token: 0x0600483D RID: 18493 RVA: 0x00164990 File Offset: 0x00162B90
		public static Mesh[] GetMeshes(int totalWidth, int totalHeight)
		{
			if (Triangles.HasMeshes() && Triangles.currentTris == totalWidth * totalHeight)
			{
				return Triangles.meshes;
			}
			int num = 21666;
			int num2 = totalWidth * totalHeight;
			Triangles.currentTris = num2;
			int num3 = Mathf.CeilToInt(1f * (float)num2 / (1f * (float)num));
			Triangles.meshes = new Mesh[num3];
			int num4 = 0;
			for (int i = 0; i < num2; i += num)
			{
				int triCount = Mathf.FloorToInt((float)Mathf.Clamp(num2 - i, 0, num));
				Triangles.meshes[num4] = Triangles.GetMesh(triCount, i, totalWidth, totalHeight);
				num4++;
			}
			return Triangles.meshes;
		}

		// Token: 0x0600483E RID: 18494 RVA: 0x00164A34 File Offset: 0x00162C34
		public static Mesh GetMesh(int triCount, int triOffset, int totalWidth, int totalHeight)
		{
			Mesh mesh = new Mesh();
			mesh.hideFlags = 52;
			Vector3[] array = new Vector3[triCount * 3];
			Vector2[] array2 = new Vector2[triCount * 3];
			Vector2[] array3 = new Vector2[triCount * 3];
			int[] array4 = new int[triCount * 3];
			for (int i = 0; i < triCount; i++)
			{
				int num = i * 3;
				int num2 = triOffset + i;
				float num3 = Mathf.Floor((float)(num2 % totalWidth)) / (float)totalWidth;
				float num4 = Mathf.Floor((float)(num2 / totalWidth)) / (float)totalHeight;
				Vector3 vector;
				vector..ctor(num3 * 2f - 1f, num4 * 2f - 1f, 1f);
				array[num] = vector;
				array[num + 1] = vector;
				array[num + 2] = vector;
				array2[num] = new Vector2(0f, 0f);
				array2[num + 1] = new Vector2(1f, 0f);
				array2[num + 2] = new Vector2(0f, 1f);
				array3[num] = new Vector2(num3, num4);
				array3[num + 1] = new Vector2(num3, num4);
				array3[num + 2] = new Vector2(num3, num4);
				array4[num] = num;
				array4[num + 1] = num + 1;
				array4[num + 2] = num + 2;
			}
			mesh.vertices = array;
			mesh.triangles = array4;
			mesh.uv = array2;
			mesh.uv2 = array3;
			return mesh;
		}

		// Token: 0x040039BD RID: 14781
		public static Mesh[] meshes;

		// Token: 0x040039BE RID: 14782
		public static int currentTris;
	}
}
