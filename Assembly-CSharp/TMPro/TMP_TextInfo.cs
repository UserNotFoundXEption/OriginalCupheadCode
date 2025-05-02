using System;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000682 RID: 1666
	[Serializable]
	public class TMP_TextInfo
	{
		// Token: 0x060046ED RID: 18157 RVA: 0x00156A68 File Offset: 0x00154C68
		public TMP_TextInfo()
		{
			this.characterInfo = new TMP_CharacterInfo[8];
			this.wordInfo = new TMP_WordInfo[16];
			this.linkInfo = new TMP_LinkInfo[0];
			this.lineInfo = new TMP_LineInfo[2];
			this.pageInfo = new TMP_PageInfo[16];
			this.meshInfo = new TMP_MeshInfo[1];
		}

		// Token: 0x060046EE RID: 18158 RVA: 0x00156AC8 File Offset: 0x00154CC8
		public TMP_TextInfo(TMP_Text textComponent)
		{
			this.textComponent = textComponent;
			this.characterInfo = new TMP_CharacterInfo[8];
			this.wordInfo = new TMP_WordInfo[4];
			this.linkInfo = new TMP_LinkInfo[0];
			this.lineInfo = new TMP_LineInfo[2];
			this.pageInfo = new TMP_PageInfo[16];
			this.meshInfo = new TMP_MeshInfo[1];
			this.meshInfo[0].mesh = textComponent.mesh;
			this.materialCount = 1;
		}

		// Token: 0x060046EF RID: 18159 RVA: 0x00156B4C File Offset: 0x00154D4C
		public void Clear()
		{
			this.characterCount = 0;
			this.spaceCount = 0;
			this.wordCount = 0;
			this.linkCount = 0;
			this.lineCount = 0;
			this.pageCount = 0;
			this.spriteCount = 0;
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].vertexCount = 0;
			}
		}

		// Token: 0x060046F0 RID: 18160 RVA: 0x00156BB8 File Offset: 0x00154DB8
		public void ClearMeshInfo(bool updateMesh)
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].Clear(updateMesh);
			}
		}

		// Token: 0x060046F1 RID: 18161 RVA: 0x00156BF0 File Offset: 0x00154DF0
		public void ClearAllMeshInfo()
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				this.meshInfo[i].Clear(true);
			}
		}

		// Token: 0x060046F2 RID: 18162 RVA: 0x00156C28 File Offset: 0x00154E28
		public void ClearUnusedVertices(MaterialReference[] materials)
		{
			for (int i = 0; i < this.meshInfo.Length; i++)
			{
				int startIndex = 0;
				this.meshInfo[i].ClearUnusedVertices(startIndex);
			}
		}

		// Token: 0x060046F3 RID: 18163 RVA: 0x00156C64 File Offset: 0x00154E64
		public void ClearLineInfo()
		{
			if (this.lineInfo == null)
			{
				this.lineInfo = new TMP_LineInfo[2];
			}
			for (int i = 0; i < this.lineInfo.Length; i++)
			{
				this.lineInfo[i].characterCount = 0;
				this.lineInfo[i].spaceCount = 0;
				this.lineInfo[i].width = 0f;
				this.lineInfo[i].ascender = TMP_TextInfo.k_InfinityVectorNegative.x;
				this.lineInfo[i].descender = TMP_TextInfo.k_InfinityVectorPositive.x;
				this.lineInfo[i].lineExtents.min = TMP_TextInfo.k_InfinityVectorPositive;
				this.lineInfo[i].lineExtents.max = TMP_TextInfo.k_InfinityVectorNegative;
				this.lineInfo[i].maxAdvance = 0f;
			}
		}

		// Token: 0x060046F4 RID: 18164 RVA: 0x00156D60 File Offset: 0x00154F60
		public static void Resize<T>(ref T[] array, int size)
		{
			int newSize = (size <= 1024) ? Mathf.NextPowerOfTwo(size) : (size + 256);
			Array.Resize<T>(ref array, newSize);
		}

		// Token: 0x060046F5 RID: 18165 RVA: 0x000387E0 File Offset: 0x000369E0
		public static void Resize<T>(ref T[] array, int size, bool isBlockAllocated)
		{
			if (size <= array.Length)
			{
				return;
			}
			if (isBlockAllocated)
			{
				size = ((size <= 1024) ? Mathf.NextPowerOfTwo(size) : (size + 256));
			}
			Array.Resize<T>(ref array, size);
		}

		// Token: 0x040036E4 RID: 14052
		public static Vector2 k_InfinityVectorPositive = new Vector2(1000000f, 1000000f);

		// Token: 0x040036E5 RID: 14053
		public static Vector2 k_InfinityVectorNegative = new Vector2(-1000000f, -1000000f);

		// Token: 0x040036E6 RID: 14054
		public TMP_Text textComponent;

		// Token: 0x040036E7 RID: 14055
		public int characterCount;

		// Token: 0x040036E8 RID: 14056
		public int spriteCount;

		// Token: 0x040036E9 RID: 14057
		public int spaceCount;

		// Token: 0x040036EA RID: 14058
		public int wordCount;

		// Token: 0x040036EB RID: 14059
		public int linkCount;

		// Token: 0x040036EC RID: 14060
		public int lineCount;

		// Token: 0x040036ED RID: 14061
		public int pageCount;

		// Token: 0x040036EE RID: 14062
		public int materialCount;

		// Token: 0x040036EF RID: 14063
		public TMP_CharacterInfo[] characterInfo;

		// Token: 0x040036F0 RID: 14064
		public TMP_WordInfo[] wordInfo;

		// Token: 0x040036F1 RID: 14065
		public TMP_LinkInfo[] linkInfo;

		// Token: 0x040036F2 RID: 14066
		public TMP_LineInfo[] lineInfo;

		// Token: 0x040036F3 RID: 14067
		public TMP_PageInfo[] pageInfo;

		// Token: 0x040036F4 RID: 14068
		public TMP_MeshInfo[] meshInfo;
	}
}
