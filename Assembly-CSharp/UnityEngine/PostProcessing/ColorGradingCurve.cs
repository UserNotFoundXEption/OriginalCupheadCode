using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x0200062E RID: 1582
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x060040EC RID: 16620 RVA: 0x00034094 File Offset: 0x00032294
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x0012F2EC File Offset: 0x0012D4EC
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe keyframe = this.curve[length - 1];
			keyframe.time -= this.m_Range;
			Keyframe keyframe2 = this.curve[0];
			keyframe2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(keyframe);
			this.m_InternalLoopingCurve.AddKey(keyframe2);
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x0012F3A4 File Offset: 0x0012D5A4
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x0400339A RID: 13210
		public AnimationCurve curve;

		// Token: 0x0400339B RID: 13211
		[SerializeField]
		public bool m_Loop;

		// Token: 0x0400339C RID: 13212
		[SerializeField]
		public float m_ZeroValue;

		// Token: 0x0400339D RID: 13213
		[SerializeField]
		public float m_Range;

		// Token: 0x0400339E RID: 13214
		public AnimationCurve m_InternalLoopingCurve;
	}
}
