using System;
using System.Collections;
using TMPro;
using UnityEngine;

// Token: 0x020000F6 RID: 246
public class TextMeshCurveAndJitter : MonoBehaviour
{
	// Token: 0x170001D9 RID: 473
	// (set) Token: 0x06000B91 RID: 2961 RVA: 0x0000A501 File Offset: 0x00008701
	public byte AlphaValue
	{
		set
		{
			this.applyAlpha = true;
			this.alphaValue = value;
		}
	}

	// Token: 0x06000B92 RID: 2962 RVA: 0x0000A511 File Offset: 0x00008711
	public void Awake()
	{
		this.jitterDelay = 0.0833333358f;
		this.AlphaValue = byte.MaxValue;
		this.m_TextComponent = base.gameObject.GetComponent<TMP_Text>();
	}

	// Token: 0x06000B93 RID: 2963 RVA: 0x0000A53A File Offset: 0x0000873A
	public void Start()
	{
		base.StartCoroutine(this.WarpText());
	}

	// Token: 0x06000B94 RID: 2964 RVA: 0x0007FED4 File Offset: 0x0007E0D4
	public AnimationCurve CopyAnimationCurve(AnimationCurve curve)
	{
		return new AnimationCurve
		{
			keys = curve.keys
		};
	}

	// Token: 0x06000B95 RID: 2965 RVA: 0x0007FEF4 File Offset: 0x0007E0F4
	public IEnumerator WarpText()
	{
		this.VertexCurve.preWrapMode = 1;
		this.VertexCurve.postWrapMode = 1;
		this.m_TextComponent.havePropertiesChanged = true;
		for (;;)
		{
			this.currentJitterDelay -= CupheadTime.Delta;
			if (this.currentJitterDelay <= 0f)
			{
				this.currentJitterDelay = this.jitterDelay;
			}
			this.ApplyChanges(true);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000B96 RID: 2966 RVA: 0x0007FF10 File Offset: 0x0007E110
	public void ApplyChanges(bool jitter)
	{
		this.m_TextComponent.ForceMeshUpdate();
		TMP_TextInfo textInfo = this.m_TextComponent.textInfo;
		int characterCount = textInfo.characterCount;
		if (characterCount == 0 || this.m_TextComponent.text.Length == 0)
		{
			return;
		}
		float x = this.m_TextComponent.bounds.min.x;
		float x2 = this.m_TextComponent.bounds.max.x;
		for (int i = 0; i < characterCount; i++)
		{
			if (textInfo.characterInfo[i].isVisible)
			{
				int vertexIndex = (int)textInfo.characterInfo[i].vertexIndex;
				int materialReferenceIndex = textInfo.characterInfo[i].materialReferenceIndex;
				Vector3[] vertices = textInfo.meshInfo[materialReferenceIndex].vertices;
				Color32[] colors = textInfo.meshInfo[materialReferenceIndex].colors32;
				if (jitter)
				{
					this.ApplyCurveAndJitter(jitter, vertices, vertexIndex, i, textInfo, x, x2);
				}
				if (this.applyAlpha)
				{
					this.ApplyAlpha(colors, vertexIndex);
				}
			}
		}
		jitter = false;
		this.m_TextComponent.UpdateVertexData();
	}

	// Token: 0x06000B97 RID: 2967 RVA: 0x00080050 File Offset: 0x0007E250
	public void ApplyCurveAndJitter(bool jitter, Vector3[] vertices, int vertexIndex, int i, TMP_TextInfo textInfo, float boundsMinX, float boundsMaxX)
	{
		Vector3 vector = new Vector2((vertices[vertexIndex].x + vertices[vertexIndex + 2].x) / 2f, textInfo.characterInfo[i].baseLine);
		float num = (vector.x - Mathf.Min(boundsMinX, -229.3845f)) / (Mathf.Max(boundsMaxX, 226.2879f) - Mathf.Min(boundsMinX, -229.3845f));
		float num2 = this.VertexSpacing.Evaluate(num) * this.SpacingScale;
		vertices[vertexIndex].x = vertices[vertexIndex].x + num2;
		int num3 = vertexIndex + 1;
		vertices[num3].x = vertices[num3].x + num2;
		int num4 = vertexIndex + 2;
		vertices[num4].x = vertices[num4].x + num2;
		int num5 = vertexIndex + 3;
		vertices[num5].x = vertices[num5].x + num2;
		vertices[vertexIndex] += -vector;
		vertices[vertexIndex + 1] += -vector;
		vertices[vertexIndex + 2] += -vector;
		vertices[vertexIndex + 3] += -vector;
		float num6 = (vector.x - boundsMinX) / (boundsMaxX - boundsMinX);
		float num7 = num6 + 0.0001f;
		float num8 = this.VertexCurve.Evaluate(num6) * this.CurveScale;
		float num9 = this.VertexCurve.Evaluate(num7) * this.CurveScale;
		Vector3 vector2;
		vector2..ctor(1f, 0f, 0f);
		Vector3 vector3 = new Vector3(num7 * (boundsMaxX - boundsMinX) + boundsMinX, num9) - new Vector3(vector.x, num8);
		float num10 = Mathf.Acos(Vector3.Dot(vector2, vector3.normalized)) * 57.29578f;
		float num11 = (Vector3.Cross(vector2, vector3).z <= 0f) ? (360f - num10) : num10;
		float num12 = 0f;
		if (jitter)
		{
			num12 = Random.Range(-this.jitterAngleAmplitude, this.jitterAngleAmplitude);
		}
		Matrix4x4 matrix4x = Matrix4x4.TRS(new Vector3(0f, num8, 0f), Quaternion.Euler(0f, 0f, num11 + num12), Vector3.one);
		vertices[vertexIndex] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex]);
		vertices[vertexIndex + 1] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 1]);
		vertices[vertexIndex + 2] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 2]);
		vertices[vertexIndex + 3] = matrix4x.MultiplyPoint3x4(vertices[vertexIndex + 3]);
		vertices[vertexIndex] += vector;
		vertices[vertexIndex + 1] += vector;
		vertices[vertexIndex + 2] += vector;
		vertices[vertexIndex + 3] += vector;
		Vector3 zero = Vector3.zero;
		if (jitter)
		{
			zero..ctor(Random.Range(-this.jitterAmplitude, this.jitterAmplitude), Random.Range(-this.jitterAmplitude, this.jitterAmplitude), 0f);
		}
		vertices[vertexIndex] += zero;
		vertices[vertexIndex + 1] += zero;
		vertices[vertexIndex + 2] += zero;
		vertices[vertexIndex + 3] += zero;
	}

	// Token: 0x06000B98 RID: 2968 RVA: 0x00080460 File Offset: 0x0007E660
	public void ApplyAlpha(Color32[] vertices, int vertexIndex)
	{
		vertices[vertexIndex].a = this.alphaValue;
		vertices[vertexIndex + 1].a = this.alphaValue;
		vertices[vertexIndex + 2].a = this.alphaValue;
		vertices[vertexIndex + 3].a = this.alphaValue;
	}

	// Token: 0x0400092D RID: 2349
	public const float MAX_BOUNDS_TEXT_COMPONENT = 226.2879f;

	// Token: 0x0400092E RID: 2350
	public const float MIN_BOUNDS_TEXT_COMPONENT = -229.3845f;

	// Token: 0x0400092F RID: 2351
	[SerializeField]
	public TMP_Text m_TextComponent;

	// Token: 0x04000930 RID: 2352
	public AnimationCurve VertexCurve = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 0f),
		new Keyframe(0.25f, 2f),
		new Keyframe(0.5f, 0f),
		new Keyframe(0.75f, 2f),
		new Keyframe(1f, 0f)
	});

	// Token: 0x04000931 RID: 2353
	public AnimationCurve VertexSpacing = new AnimationCurve(new Keyframe[]
	{
		new Keyframe(0f, 1.5f),
		new Keyframe(0.5f, 0f),
		new Keyframe(1f, -1.5f)
	});

	// Token: 0x04000932 RID: 2354
	public float AngleMultiplier = 1f;

	// Token: 0x04000933 RID: 2355
	public float SpeedMultiplier = 1f;

	// Token: 0x04000934 RID: 2356
	public float CurveScale = 1f;

	// Token: 0x04000935 RID: 2357
	public float SpacingScale = 1f;

	// Token: 0x04000936 RID: 2358
	public float jitterAmplitude = 0.1f;

	// Token: 0x04000937 RID: 2359
	public float jitterAngleAmplitude = 0.1f;

	// Token: 0x04000938 RID: 2360
	public float jitterDelay = 0.1f;

	// Token: 0x04000939 RID: 2361
	public float currentJitterDelay;

	// Token: 0x0400093A RID: 2362
	public bool applyAlpha;

	// Token: 0x0400093B RID: 2363
	public byte alphaValue;
}
