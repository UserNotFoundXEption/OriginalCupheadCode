using System;
using System.Collections;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x0200009B RID: 155
public abstract class AbstractCupheadGameCamera : AbstractCupheadCamera
{
	// Token: 0x0600075B RID: 1883 RVA: 0x000073B3 File Offset: 0x000055B3
	public AbstractCupheadGameCamera()
	{
	}

	// Token: 0x1700014E RID: 334
	// (get) Token: 0x0600075C RID: 1884 RVA: 0x000073D1 File Offset: 0x000055D1
	// (set) Token: 0x0600075D RID: 1885 RVA: 0x000073D9 File Offset: 0x000055D9
	public float zoom
	{
		get
		{
			return this._zoom;
		}
		set
		{
			this._zoom = value;
			base.camera.orthographicSize = this.OrthographicSize / this._zoom;
		}
	}

	// Token: 0x1700014F RID: 335
	// (get) Token: 0x0600075E RID: 1886 RVA: 0x000073FA File Offset: 0x000055FA
	// (set) Token: 0x0600075F RID: 1887 RVA: 0x00007402 File Offset: 0x00005602
	public bool isShaking { get; set; }

	// Token: 0x17000150 RID: 336
	// (get) Token: 0x06000760 RID: 1888
	public abstract float OrthographicSize { get; }

	// Token: 0x17000151 RID: 337
	// (get) Token: 0x06000761 RID: 1889 RVA: 0x0000740B File Offset: 0x0000560B
	public float Width
	{
		get
		{
			return 1.77777779f * this.OrthographicSize * this.zoom;
		}
	}

	// Token: 0x17000152 RID: 338
	// (get) Token: 0x06000762 RID: 1890 RVA: 0x00007420 File Offset: 0x00005620
	public float Height
	{
		get
		{
			return this.OrthographicSize * this.zoom;
		}
	}

	// Token: 0x17000153 RID: 339
	// (get) Token: 0x06000763 RID: 1891 RVA: 0x00072AF4 File Offset: 0x00070CF4
	public float Top
	{
		get
		{
			return base.camera.ScreenToWorldPoint(new Vector3(0f, (float)Screen.height, 0f)).y;
		}
	}

	// Token: 0x06000764 RID: 1892 RVA: 0x0000742F File Offset: 0x0000562F
	public override void Awake()
	{
		base.Awake();
		base.camera.orthographicSize = this.OrthographicSize;
		this._blurEffect = base.GetComponent<BlurOptimized>();
		this._blurEffect.enabled = false;
		base.camera.clearFlags = 2;
	}

	// Token: 0x06000765 RID: 1893 RVA: 0x0000746C File Offset: 0x0000566C
	public void Move()
	{
		base.transform.position = this._position + this._shakeAdd + this._floatAdd;
	}

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06000766 RID: 1894 RVA: 0x00072B2C File Offset: 0x00070D2C
	// (remove) Token: 0x06000767 RID: 1895 RVA: 0x00072B64 File Offset: 0x00070D64
	public event AbstractCupheadGameCamera.OnShakeHandler OnShakeEvent;

	// Token: 0x06000768 RID: 1896 RVA: 0x00072B9C File Offset: 0x00070D9C
	public void Shake(float amount, float time, bool bypassEvent = false)
	{
		this.isShaking = true;
		this.ResetShake();
		this.shakeAmount = amount;
		if (!bypassEvent && this.OnShakeEvent != null)
		{
			this.OnShakeEvent(amount, time);
		}
		this.shakeCoroutine = this.falloffShake_cr(amount, time);
		base.StartCoroutine(this.shakeCoroutine);
	}

	// Token: 0x06000769 RID: 1897 RVA: 0x00007495 File Offset: 0x00005695
	public void StartShake(float amount)
	{
		this.ResetShake();
		this.shakeAmount = amount;
		this.shakeCoroutine = this.endlessShake_cr(amount);
		base.StartCoroutine(this.shakeCoroutine);
	}

	// Token: 0x0600076A RID: 1898 RVA: 0x000074BE File Offset: 0x000056BE
	public void EndShake(float time)
	{
		this.ResetShake();
		if (time > 0f)
		{
			this.shakeCoroutine = this.falloffShake_cr(this.shakeAmount, time);
			base.StartCoroutine(this.shakeCoroutine);
		}
	}

	// Token: 0x0600076B RID: 1899 RVA: 0x000074F1 File Offset: 0x000056F1
	public void StartSmoothShake(float amount, float period, int octaves)
	{
		this.ResetShake();
		this.shakeCoroutine = this.endlessSmoothShake_cr(amount, period, octaves);
		base.StartCoroutine(this.shakeCoroutine);
	}

	// Token: 0x0600076C RID: 1900 RVA: 0x00007515 File Offset: 0x00005715
	public void ResetShake()
	{
		if (this.shakeCoroutine != null)
		{
			base.StopCoroutine(this.shakeCoroutine);
		}
		this.shakeCoroutine = null;
		this._shakeAdd = Vector3.zero;
	}

	// Token: 0x0600076D RID: 1901 RVA: 0x00072BF8 File Offset: 0x00070DF8
	public IEnumerator falloffShake_cr(float amount, float time)
	{
		float t = 0f;
		float halfAmount = amount / 2f;
		while (t < time)
		{
			float val = 1f - t / time;
			this.shakeAmount = amount * val;
			float x = Random.Range(-halfAmount, halfAmount);
			float y = Random.Range(-halfAmount, halfAmount);
			this._shakeAdd = new Vector3(x * val, y * val, 0f);
			t += CupheadTime.Delta;
			yield return null;
			if (PauseManager.state == PauseManager.State.Paused)
			{
				while (PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
			}
		}
		this.ResetShake();
		this.isShaking = false;
		yield break;
	}

	// Token: 0x0600076E RID: 1902 RVA: 0x00072C24 File Offset: 0x00070E24
	public IEnumerator endlessShake_cr(float amount)
	{
		float halfAmount = amount / 2f;
		for (;;)
		{
			float x = Random.Range(-halfAmount, halfAmount);
			float y = Random.Range(-halfAmount, halfAmount);
			this._shakeAdd = new Vector3(x, y, 0f);
			yield return null;
			if (PauseManager.state == PauseManager.State.Paused)
			{
				while (PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x0600076F RID: 1903 RVA: 0x00072C48 File Offset: 0x00070E48
	public IEnumerator endlessSmoothShake_cr(float amount, float period, int octaves)
	{
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			float x = 0f;
			float y = 0f;
			float scale = 1f;
			for (int i = 0; i < octaves; i++)
			{
				scale = Mathf.Pow(2f, (float)i);
				x += Mathf.PerlinNoise((t + 1000f) * scale / period, 0f) * amount / scale;
				y += Mathf.PerlinNoise((t + 2000f) * scale / period, 0f) * amount / scale;
			}
			this._shakeAdd.x = x;
			this._shakeAdd.y = y;
			this._shakeAdd.z = 0f;
			yield return null;
			if (PauseManager.state == PauseManager.State.Paused)
			{
				while (PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x06000770 RID: 1904 RVA: 0x00007540 File Offset: 0x00005740
	public void StartFloat(float amount, float time)
	{
		this.ResetFloat();
		this.floatCoroutine = this.float_cr(amount, time);
		base.StartCoroutine(this.floatCoroutine);
	}

	// Token: 0x06000771 RID: 1905 RVA: 0x00007563 File Offset: 0x00005763
	public void EndFloat()
	{
		this.ResetFloat();
	}

	// Token: 0x06000772 RID: 1906 RVA: 0x0000756B File Offset: 0x0000576B
	public void ResetFloat()
	{
		if (this.floatCoroutine != null)
		{
			base.StopCoroutine(this.floatCoroutine);
		}
		this.floatCoroutine = null;
		this._floatAdd = Vector3.zero;
	}

	// Token: 0x06000773 RID: 1907 RVA: 0x00007596 File Offset: 0x00005796
	public void SetManualFloat(Vector3 value)
	{
		this._floatAdd = value;
	}

	// Token: 0x06000774 RID: 1908 RVA: 0x00072C78 File Offset: 0x00070E78
	public IEnumerator float_cr(float amount, float time)
	{
		this.floatState = AbstractCupheadGameCamera.FloatState.Float;
		float t = 0f;
		float bottom = 0f;
		for (;;)
		{
			t = 0f;
			while (t < time)
			{
				float val = t / time;
				float y = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, bottom, amount, val);
				this._floatAdd = new Vector3(0f, y, 0f);
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			while (t < time)
			{
				float val2 = t / time;
				float y2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, amount, bottom, val2);
				this._floatAdd = new Vector3(0f, y2, 0f);
				t += CupheadTime.Delta;
				yield return null;
			}
			if (this.floatState == AbstractCupheadGameCamera.FloatState.Stop)
			{
				this.ResetFloat();
			}
		}
		yield break;
	}

	// Token: 0x06000775 RID: 1909 RVA: 0x0000759F File Offset: 0x0000579F
	public void Zoom(float newZoom, float time, EaseUtils.EaseType ease)
	{
		this.StopZoom();
		this.zoomCoroutine = this.zoom_cr(newZoom, time, ease);
		base.StartCoroutine(this.zoomCoroutine);
	}

	// Token: 0x06000776 RID: 1910 RVA: 0x000075C3 File Offset: 0x000057C3
	public void StopZoom()
	{
		if (this.zoomCoroutine != null)
		{
			base.StopCoroutine(this.zoomCoroutine);
		}
		this.zoomCoroutine = null;
	}

	// Token: 0x06000777 RID: 1911 RVA: 0x00072CA4 File Offset: 0x00070EA4
	public IEnumerator zoom_cr(float newZoom, float time, EaseUtils.EaseType ease)
	{
		float oldZoom = this.zoom;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.zoom = EaseUtils.Ease(ease, oldZoom, newZoom, val);
			t += CupheadTime.Delta;
			yield return null;
			if (PauseManager.state == PauseManager.State.Paused)
			{
				while (PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
			}
		}
		this.zoom = newZoom;
		yield return null;
		yield break;
	}

	// Token: 0x06000778 RID: 1912 RVA: 0x000075E3 File Offset: 0x000057E3
	public void StartBlur()
	{
		this.maxBlur = 1.2f;
		this.StartBlurCoroutine(2f, 0.15f, false);
	}

	// Token: 0x06000779 RID: 1913 RVA: 0x00007601 File Offset: 0x00005801
	public void StartBlur(float time)
	{
		this.maxBlur = 1.2f;
		this.StartBlurCoroutine(2f, time, false);
	}

	// Token: 0x0600077A RID: 1914 RVA: 0x0000761B File Offset: 0x0000581B
	public void StartBlur(float time, float amount)
	{
		this.maxBlur = amount;
		this.StartBlurCoroutine(2f, time, false);
	}

	// Token: 0x0600077B RID: 1915 RVA: 0x00007631 File Offset: 0x00005831
	public void EndBlur()
	{
		this.maxBlur = 1.2f;
		this.StartBlurCoroutine(0f, 0.15f, true);
	}

	// Token: 0x0600077C RID: 1916 RVA: 0x0000764F File Offset: 0x0000584F
	public void EndBlur(float time)
	{
		this.maxBlur = 1.2f;
		this.StartBlurCoroutine(0f, time, true);
	}

	// Token: 0x0600077D RID: 1917 RVA: 0x00007669 File Offset: 0x00005869
	public void EndBlur(float time, float amount)
	{
		this.maxBlur = amount;
		this.StartBlurCoroutine(0f, time, true);
	}

	// Token: 0x0600077E RID: 1918 RVA: 0x0000767F File Offset: 0x0000587F
	public void StartBlurCoroutine(float amount, float time, bool disableBlurWhenComplete)
	{
		this.StopBlurCoroutine();
		this._blurCoroutine = this.blur_cr(amount, time, disableBlurWhenComplete);
		base.StartCoroutine(this._blurCoroutine);
	}

	// Token: 0x0600077F RID: 1919 RVA: 0x000076A3 File Offset: 0x000058A3
	public void StopBlurCoroutine()
	{
		if (this._blurCoroutine != null)
		{
			base.StopCoroutine(this._blurCoroutine);
		}
		this._blurCoroutine = null;
	}

	// Token: 0x06000780 RID: 1920 RVA: 0x000076C3 File Offset: 0x000058C3
	public void UpdateBlur()
	{
		this._blurEffect.blurSize = Mathf.Lerp(0f, this.maxBlur, this._currentBlurAmount);
	}

	// Token: 0x06000781 RID: 1921 RVA: 0x00072CD4 File Offset: 0x00070ED4
	public IEnumerator blur_cr(float end, float time, bool disableBlurWhenComplete)
	{
		float start = this._currentBlurAmount;
		this._blurEffect.enabled = true;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this._currentBlurAmount = Mathf.Lerp(start, end, val);
			this.UpdateBlur();
			t += Time.deltaTime;
			yield return null;
		}
		this._currentBlurAmount = end;
		this.UpdateBlur();
		yield return null;
		if (disableBlurWhenComplete)
		{
			this._blurEffect.enabled = false;
		}
		yield break;
	}

	// Token: 0x0400059D RID: 1437
	public Vector3 _shakeAdd;

	// Token: 0x0400059E RID: 1438
	public Vector3 _floatAdd;

	// Token: 0x0400059F RID: 1439
	public Vector3 _position;

	// Token: 0x040005A0 RID: 1440
	public BlurOptimized _blurEffect;

	// Token: 0x040005A1 RID: 1441
	public float _zoom = 1f;

	// Token: 0x040005A3 RID: 1443
	public IEnumerator shakeCoroutine;

	// Token: 0x040005A4 RID: 1444
	public float shakeAmount;

	// Token: 0x040005A6 RID: 1446
	public AbstractCupheadGameCamera.FloatState floatState;

	// Token: 0x040005A7 RID: 1447
	public IEnumerator floatCoroutine;

	// Token: 0x040005A8 RID: 1448
	public IEnumerator zoomCoroutine;

	// Token: 0x040005A9 RID: 1449
	public const float BLUR_TIME_START = 0.15f;

	// Token: 0x040005AA RID: 1450
	public const float BLUR_TIME_END = 0.15f;

	// Token: 0x040005AB RID: 1451
	public IEnumerator _blurCoroutine;

	// Token: 0x040005AC RID: 1452
	public float _currentBlurAmount;

	// Token: 0x040005AD RID: 1453
	public float maxBlur = 1.2f;

	// Token: 0x020008ED RID: 2285
	// (Invoke) Token: 0x060052F9 RID: 21241
	public delegate void OnShakeHandler(float amount, float time);

	// Token: 0x020008EE RID: 2286
	public enum FloatState
	{
		// Token: 0x040043E0 RID: 17376
		Stop,
		// Token: 0x040043E1 RID: 17377
		Float
	}
}
