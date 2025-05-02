using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200045F RID: 1119
public class PlatformingLevelMovingPlatform : AbstractPausableComponent
{
	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06002FBA RID: 12218 RVA: 0x00027C2A File Offset: 0x00025E2A
	// (set) Token: 0x06002FBB RID: 12219 RVA: 0x00027C32 File Offset: 0x00025E32
	public float[] allValues { get; set; }

	// Token: 0x06002FBC RID: 12220 RVA: 0x000E2964 File Offset: 0x000E0B64
	public virtual void Start()
	{
		this._offset = base.transform.position;
		base.StartCoroutine(this.pingpong_cr());
		AudioManager.PlayLoop("level_platform_propellor_loop");
		this.emitAudioFromObject.Add("level_platform_propellor_loop");
		base.StartCoroutine(this.check_sound_cr());
	}

	// Token: 0x06002FBD RID: 12221 RVA: 0x000E29B8 File Offset: 0x000E0BB8
	public IEnumerator check_sound_cr()
	{
		bool inRange = false;
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
			{
				if (!inRange)
				{
					AudioManager.PlayLoop("level_platform_propellor_loop");
					this.emitAudioFromObject.Add("level_platform_propellor_loop");
					inRange = true;
				}
			}
			else if (inRange)
			{
				AudioManager.Stop("level_platform_propellor_loop");
				inRange = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002FBE RID: 12222 RVA: 0x000E29D4 File Offset: 0x000E0BD4
	public float CalculateTime()
	{
		return this.path.Distance / this.speed;
	}

	// Token: 0x06002FBF RID: 12223 RVA: 0x000E29F8 File Offset: 0x000E0BF8
	public float CalculateRemainingTime(float t)
	{
		float num = this.CalculateTime();
		return (!this.goingUp) ? (t * num) : ((1f - t) * num);
	}

	// Token: 0x06002FC0 RID: 12224 RVA: 0x00027C3B File Offset: 0x00025E3B
	public void MoveCallback(float value)
	{
		base.transform.position = this._offset + this.path.Lerp(value);
	}

	// Token: 0x06002FC1 RID: 12225 RVA: 0x000E2A28 File Offset: 0x000E0C28
	public virtual IEnumerator pingpong_cr()
	{
		for (;;)
		{
			if (this.goingUp)
			{
				yield return base.TweenValue(0f, 1f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			}
			else
			{
				yield return base.TweenValue(1f, 0f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			}
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			this.goingUp = !this.goingUp;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002FC2 RID: 12226 RVA: 0x00027C5F File Offset: 0x00025E5F
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06002FC3 RID: 12227 RVA: 0x00027C72 File Offset: 0x00025E72
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06002FC4 RID: 12228 RVA: 0x000E2A44 File Offset: 0x000E0C44
	public void DrawGizmos(float a)
	{
		if (Application.isPlaying)
		{
			this.path.DrawGizmos(a, this._offset);
			return;
		}
		this.path.DrawGizmos(a, base.baseTransform.position);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawSphere(this.path.Lerp(0f) + base.baseTransform.position, 10f);
		Gizmos.DrawWireSphere(this.path.Lerp(0f) + base.baseTransform.position, 11f);
	}

	// Token: 0x04002788 RID: 10120
	public const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x0400278A RID: 10122
	public int pathIndex;

	// Token: 0x0400278B RID: 10123
	public float loopRepeatDelay;

	// Token: 0x0400278C RID: 10124
	public float speed = 100f;

	// Token: 0x0400278D RID: 10125
	public VectorPath path;

	// Token: 0x0400278E RID: 10126
	public bool goingUp;

	// Token: 0x0400278F RID: 10127
	public SpriteRenderer sprite;

	// Token: 0x04002790 RID: 10128
	public EaseUtils.EaseType _easeType = EaseUtils.EaseType.linear;

	// Token: 0x04002791 RID: 10129
	public Vector3 _offset;
}
