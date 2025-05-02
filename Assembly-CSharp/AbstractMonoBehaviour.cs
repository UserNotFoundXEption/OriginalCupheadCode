using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000053 RID: 83
public abstract class AbstractMonoBehaviour : MonoBehaviour
{
	// Token: 0x06000489 RID: 1161 RVA: 0x000052F9 File Offset: 0x000034F9
	public AbstractMonoBehaviour()
	{
	}

	// Token: 0x1700011F RID: 287
	// (get) Token: 0x0600048A RID: 1162 RVA: 0x00005301 File Offset: 0x00003501
	public Transform baseTransform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x17000120 RID: 288
	// (get) Token: 0x0600048B RID: 1163 RVA: 0x00005309 File Offset: 0x00003509
	public Transform transform
	{
		get
		{
			if (!this._transformCached)
			{
				this._transform = this.baseTransform;
				this._transformCached = true;
			}
			return this._transform;
		}
	}

	// Token: 0x17000121 RID: 289
	// (get) Token: 0x0600048C RID: 1164 RVA: 0x0000532F File Offset: 0x0000352F
	public RectTransform baseRectTransform
	{
		get
		{
			return base.transform as RectTransform;
		}
	}

	// Token: 0x17000122 RID: 290
	// (get) Token: 0x0600048D RID: 1165 RVA: 0x0000533C File Offset: 0x0000353C
	public RectTransform rectTransform
	{
		get
		{
			if (this._rectTransform == null)
			{
				this._rectTransform = this.baseRectTransform;
			}
			return this._rectTransform;
		}
	}

	// Token: 0x17000123 RID: 291
	// (get) Token: 0x0600048E RID: 1166 RVA: 0x00005361 File Offset: 0x00003561
	public Rigidbody baseRigidbody
	{
		get
		{
			return base.GetComponent<Rigidbody>();
		}
	}

	// Token: 0x17000124 RID: 292
	// (get) Token: 0x0600048F RID: 1167 RVA: 0x00005369 File Offset: 0x00003569
	public Rigidbody rigidbody
	{
		get
		{
			if (this._rigidbody == null)
			{
				this._rigidbody = this.baseRigidbody;
			}
			return this._rigidbody;
		}
	}

	// Token: 0x17000125 RID: 293
	// (get) Token: 0x06000490 RID: 1168 RVA: 0x0000538E File Offset: 0x0000358E
	public Rigidbody2D baseRigidbody2D
	{
		get
		{
			return base.GetComponent<Rigidbody2D>();
		}
	}

	// Token: 0x17000126 RID: 294
	// (get) Token: 0x06000491 RID: 1169 RVA: 0x00005396 File Offset: 0x00003596
	public Rigidbody2D rigidbody2D
	{
		get
		{
			if (this._rigidbody2D == null)
			{
				this._rigidbody2D = this.baseRigidbody2D;
			}
			return this._rigidbody2D;
		}
	}

	// Token: 0x17000127 RID: 295
	// (get) Token: 0x06000492 RID: 1170 RVA: 0x000053BB File Offset: 0x000035BB
	public Animator baseAnimator
	{
		get
		{
			return base.GetComponent<Animator>();
		}
	}

	// Token: 0x17000128 RID: 296
	// (get) Token: 0x06000493 RID: 1171 RVA: 0x000053C3 File Offset: 0x000035C3
	public Animator animator
	{
		get
		{
			if (this._animator == null)
			{
				this._animator = this.baseAnimator;
			}
			return this._animator;
		}
	}

	// Token: 0x06000494 RID: 1172 RVA: 0x000053E8 File Offset: 0x000035E8
	public virtual void Awake()
	{
		base.useGUILayout = false;
	}

	// Token: 0x06000495 RID: 1173 RVA: 0x000053F1 File Offset: 0x000035F1
	public virtual void Reset()
	{
	}

	// Token: 0x06000496 RID: 1174 RVA: 0x000053F3 File Offset: 0x000035F3
	public virtual void OnDrawGizmos()
	{
	}

	// Token: 0x06000497 RID: 1175 RVA: 0x000053F5 File Offset: 0x000035F5
	public virtual void OnDrawGizmosSelected()
	{
	}

	// Token: 0x06000498 RID: 1176 RVA: 0x0006A94C File Offset: 0x00068B4C
	public virtual T InstantiatePrefab<T>() where T : MonoBehaviour
	{
		GameObject gameObject = Object.Instantiate<GameObject>(base.gameObject);
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
		return gameObject.GetComponent<T>();
	}

	// Token: 0x06000499 RID: 1177 RVA: 0x000053F7 File Offset: 0x000035F7
	public Coroutine FrameDelayedCallback(Action callback, int frames)
	{
		return base.StartCoroutine(this.frameDelayedCallback_cr(callback, frames));
	}

	// Token: 0x0600049A RID: 1178 RVA: 0x0006A988 File Offset: 0x00068B88
	public IEnumerator frameDelayedCallback_cr(Action callback, int frames)
	{
		for (int i = 0; i < frames; i++)
		{
			yield return null;
		}
		if (callback != null)
		{
			callback();
		}
		yield break;
	}

	// Token: 0x17000129 RID: 297
	// (get) Token: 0x0600049B RID: 1179 RVA: 0x00005407 File Offset: 0x00003607
	public float LocalDeltaTime
	{
		get
		{
			if (this.ignoreGlobalTime)
			{
				return Time.deltaTime;
			}
			return CupheadTime.Delta[this.timeLayer];
		}
	}

	// Token: 0x0600049C RID: 1180 RVA: 0x0000542A File Offset: 0x0000362A
	public Coroutine TweenValue(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenValue_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x0600049D RID: 1181 RVA: 0x0006A9AC File Offset: 0x00068BAC
	public IEnumerator tweenValue_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			if (updateCallback != null)
			{
				updateCallback(EaseUtils.Ease(ease, start, end, val));
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		if (updateCallback != null)
		{
			updateCallback(end);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600049E RID: 1182 RVA: 0x0000543F File Offset: 0x0000363F
	public Coroutine TweenScale(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenScale_cr(start, end, time, ease, null));
	}

	// Token: 0x0600049F RID: 1183 RVA: 0x00005453 File Offset: 0x00003653
	public Coroutine TweenScale(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenScale_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004A0 RID: 1184 RVA: 0x0006A9EC File Offset: 0x00068BEC
	public IEnumerator tweenScale_cr(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetScale(new float?(start.x), new float?(start.y), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(ease, start.x, end.x, val);
			float y = EaseUtils.Ease(ease, start.y, end.y, val);
			this.transform.SetScale(new float?(x), new float?(y), null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetScale(new float?(end.x), new float?(end.y), null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004A1 RID: 1185 RVA: 0x00005468 File Offset: 0x00003668
	public Coroutine TweenPosition(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenPosition_cr(start, end, time, ease, null));
	}

	// Token: 0x060004A2 RID: 1186 RVA: 0x0000547C File Offset: 0x0000367C
	public Coroutine TweenPosition(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenPosition_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004A3 RID: 1187 RVA: 0x0006AA2C File Offset: 0x00068C2C
	public IEnumerator tweenPosition_cr(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.position = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(ease, start.x, end.x, val);
			float y = EaseUtils.Ease(ease, start.y, end.y, val);
			this.transform.SetPosition(new float?(x), new float?(y), new float?(0f));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.position = end;
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004A4 RID: 1188 RVA: 0x00005491 File Offset: 0x00003691
	public Coroutine TweenLocalPosition(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalPosition_cr(start, end, time, ease, null));
	}

	// Token: 0x060004A5 RID: 1189 RVA: 0x000054A5 File Offset: 0x000036A5
	public Coroutine TweenLocalPosition(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalPosition_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004A6 RID: 1190 RVA: 0x0006AA6C File Offset: 0x00068C6C
	public IEnumerator tweenLocalPosition_cr(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.localPosition = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(ease, start.x, end.x, val);
			float y = EaseUtils.Ease(ease, start.y, end.y, val);
			this.transform.SetLocalPosition(new float?(x), new float?(y), new float?(0f));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.localPosition = end;
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004A7 RID: 1191 RVA: 0x000054BA File Offset: 0x000036BA
	public Coroutine TweenPositionX(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenPositionX_cr(start, end, time, ease, null));
	}

	// Token: 0x060004A8 RID: 1192 RVA: 0x000054CE File Offset: 0x000036CE
	public Coroutine TweenPositionX(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenPositionX_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004A9 RID: 1193 RVA: 0x0006AAAC File Offset: 0x00068CAC
	public IEnumerator tweenPositionX_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetPosition(new float?(start), null, null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetPosition(new float?(EaseUtils.Ease(ease, start, end, val)), null, null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetPosition(new float?(end), null, null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004AA RID: 1194 RVA: 0x000054E3 File Offset: 0x000036E3
	public Coroutine TweenLocalPositionX(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalPositionX_cr(start, end, time, ease, null));
	}

	// Token: 0x060004AB RID: 1195 RVA: 0x000054F7 File Offset: 0x000036F7
	public Coroutine TweenLocalPositionX(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalPositionX_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004AC RID: 1196 RVA: 0x0006AAEC File Offset: 0x00068CEC
	public IEnumerator tweenLocalPositionX_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetLocalPosition(new float?(start), null, null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetLocalPosition(new float?(EaseUtils.Ease(ease, start, end, val)), null, null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetLocalPosition(new float?(end), null, null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004AD RID: 1197 RVA: 0x0000550C File Offset: 0x0000370C
	public Coroutine TweenPositionY(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenPositionY_cr(start, end, time, ease, null));
	}

	// Token: 0x060004AE RID: 1198 RVA: 0x00005520 File Offset: 0x00003720
	public Coroutine TweenPositionY(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenPositionY_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004AF RID: 1199 RVA: 0x0006AB2C File Offset: 0x00068D2C
	public IEnumerator tweenPositionY_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetPosition(null, new float?(start), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetPosition(null, new float?(end), null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004B0 RID: 1200 RVA: 0x00005535 File Offset: 0x00003735
	public Coroutine TweenLocalPositionY(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalPositionY_cr(start, end, time, ease, null));
	}

	// Token: 0x060004B1 RID: 1201 RVA: 0x00005549 File Offset: 0x00003749
	public Coroutine TweenLocalPositionY(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalPositionY_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004B2 RID: 1202 RVA: 0x0006AB6C File Offset: 0x00068D6C
	public IEnumerator tweenLocalPositionY_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetLocalPosition(null, new float?(start), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetLocalPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetLocalPosition(null, new float?(end), null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004B3 RID: 1203 RVA: 0x0000555E File Offset: 0x0000375E
	public Coroutine TweenPositionZ(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenPositionZ_cr(start, end, time, ease, null));
	}

	// Token: 0x060004B4 RID: 1204 RVA: 0x00005572 File Offset: 0x00003772
	public Coroutine TweenPositionZ(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenPositionZ_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004B5 RID: 1205 RVA: 0x0006ABAC File Offset: 0x00068DAC
	public IEnumerator tweenPositionZ_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetPosition(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetPosition(null, null, new float?(EaseUtils.Ease(ease, start, end, val)));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetPosition(null, null, new float?(end));
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004B6 RID: 1206 RVA: 0x00005587 File Offset: 0x00003787
	public Coroutine TweenLocalPositionZ(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalPositionZ_cr(start, end, time, ease, null));
	}

	// Token: 0x060004B7 RID: 1207 RVA: 0x0000559B File Offset: 0x0000379B
	public Coroutine TweenLocalPositionZ(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalPositionZ_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004B8 RID: 1208 RVA: 0x0006ABEC File Offset: 0x00068DEC
	public IEnumerator tweenLocalPositionZ_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetLocalPosition(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetLocalPosition(null, null, new float?(EaseUtils.Ease(ease, start, end, val)));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetLocalPosition(null, null, new float?(end));
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004B9 RID: 1209 RVA: 0x000055B0 File Offset: 0x000037B0
	public Coroutine TweenRotation2D(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenRotation2D_cr(start, end, time, ease, null));
	}

	// Token: 0x060004BA RID: 1210 RVA: 0x000055C4 File Offset: 0x000037C4
	public Coroutine TweenRotation2D(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenRotation2D_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004BB RID: 1211 RVA: 0x0006AC2C File Offset: 0x00068E2C
	public IEnumerator tweenRotation2D_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetEulerAngles(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetEulerAngles(null, null, new float?(EaseUtils.Ease(ease, start, end, val)));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetEulerAngles(null, null, new float?(end));
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004BC RID: 1212 RVA: 0x000055D9 File Offset: 0x000037D9
	public Coroutine TweenLocalRotation2D(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalRotation2D_cr(start, end, time, ease, null));
	}

	// Token: 0x060004BD RID: 1213 RVA: 0x000055ED File Offset: 0x000037ED
	public Coroutine TweenLocalRotation2D(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalRotation2D_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x060004BE RID: 1214 RVA: 0x0006AC6C File Offset: 0x00068E6C
	public IEnumerator tweenLocalRotation2D_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetLocalEulerAngles(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetLocalEulerAngles(null, null, new float?(EaseUtils.Ease(ease, start, end, val)));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetLocalEulerAngles(null, null, new float?(end));
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060004BF RID: 1215 RVA: 0x00005602 File Offset: 0x00003802
	public virtual void StopAllCoroutines()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x04000464 RID: 1124
	public Transform _transform;

	// Token: 0x04000465 RID: 1125
	public bool _transformCached;

	// Token: 0x04000466 RID: 1126
	public RectTransform _rectTransform;

	// Token: 0x04000467 RID: 1127
	public Rigidbody _rigidbody;

	// Token: 0x04000468 RID: 1128
	public Rigidbody2D _rigidbody2D;

	// Token: 0x04000469 RID: 1129
	public Animator _animator;

	// Token: 0x0400046A RID: 1130
	public bool ignoreGlobalTime;

	// Token: 0x0400046B RID: 1131
	public CupheadTime.Layer timeLayer;

	// Token: 0x02000899 RID: 2201
	// (Invoke) Token: 0x060051AA RID: 20906
	public delegate void TweenUpdateHandler(float value);
}
