using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000697 RID: 1687
	public struct TMP_MeshInfo
	{
		// Token: 0x06004756 RID: 18262 RVA: 0x00159D80 File Offset: 0x00157F80
		public TMP_MeshInfo(Mesh mesh, int size)
		{
			if (mesh == null)
			{
				mesh = new Mesh();
			}
			else
			{
				mesh.Clear();
			}
			this.mesh = mesh;
			int num = size * 4;
			int num2 = size * 6;
			this.vertexCount = 0;
			this.vertices = new Vector3[num];
			this.uvs0 = new Vector2[num];
			this.uvs2 = new Vector2[num];
			this.colors32 = new Color32[num];
			this.normals = new Vector3[num];
			this.tangents = new Vector4[num];
			this.triangles = new int[num2];
			int num3 = 0;
			int num4 = 0;
			while (num4 / 4 < size)
			{
				for (int i = 0; i < 4; i++)
				{
					this.vertices[num4 + i] = Vector3.zero;
					this.uvs0[num4 + i] = Vector2.zero;
					this.uvs2[num4 + i] = Vector2.zero;
					this.colors32[num4 + i] = TMP_MeshInfo.s_DefaultColor;
					this.normals[num4 + i] = TMP_MeshInfo.s_DefaultNormal;
					this.tangents[num4 + i] = TMP_MeshInfo.s_DefaultTangent;
				}
				this.triangles[num3] = num4;
				this.triangles[num3 + 1] = num4 + 1;
				this.triangles[num3 + 2] = num4 + 2;
				this.triangles[num3 + 3] = num4 + 2;
				this.triangles[num3 + 4] = num4 + 3;
				this.triangles[num3 + 5] = num4;
				num4 += 4;
				num3 += 6;
			}
			this.mesh.vertices = this.vertices;
			this.mesh.normals = this.normals;
			this.mesh.tangents = this.tangents;
			this.mesh.triangles = this.triangles;
			this.mesh.bounds = new Bounds(Vector3.zero, new Vector3(3840f, 2160f, 0f));
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x00159F90 File Offset: 0x00158190
		public void ResizeMeshInfo(int size)
		{
			int newSize = size * 4;
			int newSize2 = size * 6;
			int num = this.vertices.Length / 4;
			Array.Resize<Vector3>(ref this.vertices, newSize);
			Array.Resize<Vector3>(ref this.normals, newSize);
			Array.Resize<Vector4>(ref this.tangents, newSize);
			Array.Resize<Vector2>(ref this.uvs0, newSize);
			Array.Resize<Vector2>(ref this.uvs2, newSize);
			Array.Resize<Color32>(ref this.colors32, newSize);
			Array.Resize<int>(ref this.triangles, newSize2);
			if (size <= num)
			{
				return;
			}
			for (int i = num; i < size; i++)
			{
				int num2 = i * 4;
				int num3 = i * 6;
				this.normals[num2] = TMP_MeshInfo.s_DefaultNormal;
				this.normals[1 + num2] = TMP_MeshInfo.s_DefaultNormal;
				this.normals[2 + num2] = TMP_MeshInfo.s_DefaultNormal;
				this.normals[3 + num2] = TMP_MeshInfo.s_DefaultNormal;
				this.tangents[num2] = TMP_MeshInfo.s_DefaultTangent;
				this.tangents[1 + num2] = TMP_MeshInfo.s_DefaultTangent;
				this.tangents[2 + num2] = TMP_MeshInfo.s_DefaultTangent;
				this.tangents[3 + num2] = TMP_MeshInfo.s_DefaultTangent;
				this.triangles[num3] = num2;
				this.triangles[1 + num3] = 1 + num2;
				this.triangles[2 + num3] = 2 + num2;
				this.triangles[3 + num3] = 2 + num2;
				this.triangles[4 + num3] = 3 + num2;
				this.triangles[5 + num3] = num2;
			}
			this.mesh.vertices = this.vertices;
			this.mesh.normals = this.normals;
			this.mesh.tangents = this.tangents;
			this.mesh.triangles = this.triangles;
		}

		// Token: 0x06004758 RID: 18264 RVA: 0x0015A184 File Offset: 0x00158384
		public void Clear()
		{
			if (this.vertices == null)
			{
				return;
			}
			Array.Clear(this.vertices, 0, this.vertices.Length);
			this.vertexCount = 0;
			if (this.mesh != null)
			{
				this.mesh.vertices = this.vertices;
			}
		}

		// Token: 0x06004759 RID: 18265 RVA: 0x0015A1DC File Offset: 0x001583DC
		public void Clear(bool uploadChanges)
		{
			if (this.vertices == null)
			{
				return;
			}
			Array.Clear(this.vertices, 0, this.vertices.Length);
			this.vertexCount = 0;
			if (uploadChanges && this.mesh != null)
			{
				this.mesh.vertices = this.vertices;
			}
		}

		// Token: 0x0600475A RID: 18266 RVA: 0x0015A238 File Offset: 0x00158438
		public void ClearUnusedVertices()
		{
			int num = this.vertices.Length - this.vertexCount;
			if (num > 0)
			{
				Array.Clear(this.vertices, this.vertexCount, num);
			}
		}

		// Token: 0x0600475B RID: 18267 RVA: 0x0015A270 File Offset: 0x00158470
		public void ClearUnusedVertices(int startIndex)
		{
			int num = this.vertices.Length - startIndex;
			if (num > 0)
			{
				Array.Clear(this.vertices, startIndex, num);
			}
		}

		// Token: 0x0600475C RID: 18268 RVA: 0x0015A29C File Offset: 0x0015849C
		public void ClearUnusedVertices(int startIndex, bool updateMesh)
		{
			int num = this.vertices.Length - startIndex;
			if (num > 0)
			{
				Array.Clear(this.vertices, startIndex, num);
			}
			if (updateMesh && this.mesh != null)
			{
				this.mesh.vertices = this.vertices;
			}
		}

		// Token: 0x04003756 RID: 14166
		public static readonly Color32 s_DefaultColor = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, byte.MaxValue);

		// Token: 0x04003757 RID: 14167
		public static readonly Vector3 s_DefaultNormal = new Vector3(0f, 0f, -1f);

		// Token: 0x04003758 RID: 14168
		public static readonly Vector4 s_DefaultTangent = new Vector4(-1f, 0f, 0f, 1f);

		// Token: 0x04003759 RID: 14169
		public Mesh mesh;

		// Token: 0x0400375A RID: 14170
		public int vertexCount;

		// Token: 0x0400375B RID: 14171
		public Vector3[] vertices;

		// Token: 0x0400375C RID: 14172
		public Vector3[] normals;

		// Token: 0x0400375D RID: 14173
		public Vector4[] tangents;

		// Token: 0x0400375E RID: 14174
		public Vector2[] uvs0;

		// Token: 0x0400375F RID: 14175
		public Vector2[] uvs2;

		// Token: 0x04003760 RID: 14176
		public Color32[] colors32;

		// Token: 0x04003761 RID: 14177
		public int[] triangles;
	}
}
