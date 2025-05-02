using System;
using TMPro;
using UnityEngine;

// Token: 0x020000F7 RID: 247
public class TextMeshRandomAngle : MonoBehaviour
{
	// Token: 0x06000B9A RID: 2970 RVA: 0x000804BC File Offset: 0x0007E6BC
	public void Start()
	{
		this.initialAngles = new float[this.m_TextComponent.text.Length];
		this.jitterAngles = new float[this.m_TextComponent.text.Length];
		for (int i = 0; i < this.initialAngles.Length; i++)
		{
			this.initialAngles[i] = Random.Range(-this.m_AngleAmplitude, this.m_AngleAmplitude);
		}
		this.jitterDelay = 0.0833333358f;
		this.ApplyRotation();
	}

	// Token: 0x06000B9B RID: 2971 RVA: 0x00080544 File Offset: 0x0007E744
	public void Update()
	{
		this.currentJitterDelay -= CupheadTime.Delta;
		if (this.currentJitterDelay > 0f)
		{
			return;
		}
		this.currentJitterDelay = this.jitterDelay;
		for (int i = 0; i < this.initialAngles.Length; i++)
		{
			this.jitterAngles[i] = Random.Range(-this.m_JitterAngleAmplitude, this.m_JitterAngleAmplitude);
		}
		this.ApplyRotation();
	}

	// Token: 0x06000B9C RID: 2972 RVA: 0x000805C0 File Offset: 0x0007E7C0
	public void ApplyRotation()
	{
		this.m_TextComponent.havePropertiesChanged = true;
		this.m_ShadowTextComponent.havePropertiesChanged = true;
		this.m_TextComponent.ForceMeshUpdate();
		this.m_ShadowTextComponent.ForceMeshUpdate();
		TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
		int characterCount = textInfo.characterCount;
		if (characterCount == 0 || this.m_TextComponent.text.Length == 0)
		{
			return;
		}
		for (int i = 0; i < characterCount; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				int vertexIndex = (int)textInfo.characterInfo[i].vertexIndex;
				int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
				Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
				Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, (vertices[vertexIndex].y + vertices[vertexIndex + 2].y) / 2f);
				vertices[vertexIndex] += -vector;
				vertices[vertexIndex + 1] += -vector;
				vertices[vertexIndex + 2] += -vector;
				vertices[vertexIndex + 3] += -vector;
				float num = this.initialAngles[i] + this.jitterAngles[i];
				Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, 0f, 0f), Quaternion.Euler(0f, 0f, num), Vector3.one);
				vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
				vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
				vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
				vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
				vector += new Vector3(Random.Range(-this.m_JitterOffsetAmplitude, this.m_JitterOffsetAmplitude), Random.Range(-this.m_JitterOffsetAmplitude, this.m_JitterOffsetAmplitude), 0f);
				vertices[vertexIndex] += vector;
				vertices[vertexIndex + 1] += vector;
				vertices[vertexIndex + 2] += vector;
				vertices[vertexIndex + 3] += vector;
				this.m_ShadowTextComponent.textInfo.meshInfo[materialReferenceIndex].vertices = vertices;
			}
		}
		this.m_TextComponent.UpdateVertexData();
		this.m_ShadowTextComponent.UpdateVertexData();
	}

	// Token: 0x0400093C RID: 2364
	[SerializeField]
	public TMP_Text m_TextComponent;

	// Token: 0x0400093D RID: 2365
	[SerializeField]
	public TMP_Text m_ShadowTextComponent;

	// Token: 0x0400093E RID: 2366
	public float m_AngleAmplitude = 5f;

	// Token: 0x0400093F RID: 2367
	public float m_JitterAngleAmplitude = 0.7f;

	// Token: 0x04000940 RID: 2368
	public float m_JitterOffsetAmplitude = 0.1f;

	// Token: 0x04000941 RID: 2369
	public float[] initialAngles;

	// Token: 0x04000942 RID: 2370
	public float[] jitterAngles;

	// Token: 0x04000943 RID: 2371
	public float jitterDelay = 0.1f;

	// Token: 0x04000944 RID: 2372
	public float currentJitterDelay;
}
