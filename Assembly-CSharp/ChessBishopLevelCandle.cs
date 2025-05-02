using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000180 RID: 384
public class ChessBishopLevelCandle : AbstractCollidableObject
{
	// Token: 0x17000248 RID: 584
	// (get) Token: 0x06001238 RID: 4664 RVA: 0x0000F6AA File Offset: 0x0000D8AA
	// (set) Token: 0x06001239 RID: 4665 RVA: 0x0000F6B2 File Offset: 0x0000D8B2
	public bool isLit { get; set; }

	// Token: 0x0600123A RID: 4666 RVA: 0x000946F4 File Offset: 0x000928F4
	public void Init(float distToBlowout)
	{
		this.glow.SetActive(false);
		this.distToBlowout = distToBlowout;
		this.basePos = base.transform.position;
		this.shadowPos = this.shadow.transform.position;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x0600123B RID: 4667 RVA: 0x00094748 File Offset: 0x00092948
	public float EaseOvershoot(float start, float end, float value, float overshoot)
	{
		float num = Mathf.Lerp(start, end, value);
		return num + Mathf.Sin(value * 3.14159274f) * ((end - start) * overshoot);
	}

	// Token: 0x0600123C RID: 4668 RVA: 0x00094778 File Offset: 0x00092978
	public IEnumerator intro_cr()
	{
		this.introPos = this.introCandle.transform.position;
		yield return null;
		while (!this.introCandle.moving)
		{
			yield return null;
		}
		float t = 0f;
		while (t < 1f)
		{
			this.introCandle.transform.position = this.introPos + Vector3.up * 800f * EaseUtils.EaseOutSine(0f, 1f, Mathf.InverseLerp(0f, 1f, t));
			t += 0.0416666679f;
			yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		}
		base.transform.position = this.basePos + Vector3.up * 800f;
		base.animator.Play("IntroToIdle");
		t = 0f;
		while (t < 1f)
		{
			base.transform.position = this.basePos + (Vector3.up * 800f * this.EaseOvershoot(1f, 0f, t, this.introOvershoot) + this.floatAmplitude * Vector3.up);
			t += 0.0416666679f;
			yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		}
		this.introPlaying = false;
		yield break;
	}

	// Token: 0x0600123D RID: 4669 RVA: 0x00094794 File Offset: 0x00092994
	public bool PlayerInRange()
	{
		if (this.player1 && !this.player1.IsDead)
		{
			float num = Vector3.SqrMagnitude(this.blowoutRoot.position - this.player1.center);
			if (num < this.distToBlowout * this.distToBlowout && this.player1.center != this.lastPlayer1Position)
			{
				return true;
			}
		}
		if (this.player2 && !this.player2.IsDead)
		{
			float num2 = Vector3.SqrMagnitude(this.blowoutRoot.position - this.player2.center);
			if (num2 < this.distToBlowout * this.distToBlowout && this.player2.center != this.lastPlayer2Position)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x0600123E RID: 4670 RVA: 0x00094884 File Offset: 0x00092A84
	public void Update()
	{
		this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (this.PlayerInRange())
		{
			if (this.isLit)
			{
				this.StopAllCoroutines();
				base.StartCoroutine(this.light_out_cr());
			}
			else if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
			{
				base.animator.Play("Walkby", 0, 0f);
			}
		}
		if (this.player1 && !this.player1.IsDead)
		{
			this.lastPlayer1Position = this.player1.center;
		}
		if (this.player2 && !this.player2.IsDead)
		{
			this.lastPlayer2Position = this.player2.center;
		}
		this.stepTimer += CupheadTime.Delta;
		while (this.stepTimer > 0.0416666679f)
		{
			this.Step();
			this.stepTimer -= 0.0416666679f;
		}
	}

	// Token: 0x0600123F RID: 4671 RVA: 0x000949B4 File Offset: 0x00092BB4
	public void Step()
	{
		this.shadow.transform.position = this.shadowPos;
		if (this.introPlaying)
		{
			return;
		}
		base.transform.position = this.basePos;
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle") || base.animator.GetCurrentAnimatorStateInfo(0).IsName("Lit"))
		{
			this.easeToFloat = Mathf.Clamp(0f, 1f, this.easeToFloat + 0.0416666679f);
			float num = (!this.offsetFloat) ? base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime : (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime + 0.25f);
			base.transform.position += Vector3.up * Mathf.Cos(num * 3.14159274f * 2f) * this.floatAmplitude * this.easeToFloat;
		}
		else
		{
			this.easeToFloat = 0f;
		}
	}

	// Token: 0x06001240 RID: 4672 RVA: 0x0000F6BB File Offset: 0x0000D8BB
	public void LightUp()
	{
		this.isLit = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.light_up_cr());
	}

	// Token: 0x06001241 RID: 4673 RVA: 0x00094AEC File Offset: 0x00092CEC
	public IEnumerator light_up_cr()
	{
		base.animator.Play((!Rand.Bool()) ? "ReigniteB" : "ReigniteA", 1);
		this.glow.SetActive(true);
		yield return base.animator.WaitForAnimationToStart(this, "None", 1, false);
		base.animator.Play("Lit", 0, base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime);
		this.SFX_KOG_Bishop_CandlesLightUp();
		yield break;
	}

	// Token: 0x06001242 RID: 4674 RVA: 0x00094B08 File Offset: 0x00092D08
	public IEnumerator light_out_cr()
	{
		this.isLit = false;
		this.glow.SetActive(false);
		base.animator.Play("Stagger");
		this.SFX_KOG_Bishop_CandleSnuff();
		this.smoke.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(-5, 5));
		this.vanquishFX.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		this.vanquishSpark.transform.eulerAngles = new Vector3(0f, 0f, (float)Random.Range(0, 360));
		base.animator.Play((!Rand.Bool()) ? "SmokeB" : "SmokeA", 2);
		yield return CupheadTime.WaitForSeconds(this, this.staggerLoopTime);
		base.animator.SetTrigger("EndStaggerLoop");
		yield break;
	}

	// Token: 0x06001243 RID: 4675 RVA: 0x0000F6D7 File Offset: 0x0000D8D7
	public void SFX_KOG_Bishop_CandlesLightUp()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_candleslightup");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_candleslightup");
	}

	// Token: 0x06001244 RID: 4676 RVA: 0x0000F6F3 File Offset: 0x0000D8F3
	public void SFX_KOG_Bishop_CandleSnuff()
	{
		AudioManager.Play("sfx_dlc_kog_bishop_candlesnuff");
		this.emitAudioFromObject.Add("sfx_dlc_kog_bishop_candlesnuff");
	}

	// Token: 0x04000EB4 RID: 3764
	[SerializeField]
	public Transform blowoutRoot;

	// Token: 0x04000EB5 RID: 3765
	[SerializeField]
	public GameObject smoke;

	// Token: 0x04000EB6 RID: 3766
	[SerializeField]
	public GameObject vanquishFX;

	// Token: 0x04000EB7 RID: 3767
	[SerializeField]
	public GameObject vanquishSpark;

	// Token: 0x04000EB8 RID: 3768
	[SerializeField]
	public GameObject shadow;

	// Token: 0x04000EB9 RID: 3769
	[SerializeField]
	public float staggerLoopTime;

	// Token: 0x04000EBA RID: 3770
	[SerializeField]
	public float floatAmplitude;

	// Token: 0x04000EBB RID: 3771
	[SerializeField]
	public bool offsetFloat;

	// Token: 0x04000EBC RID: 3772
	public float easeToFloat = 1f;

	// Token: 0x04000EBD RID: 3773
	public Vector3 basePos;

	// Token: 0x04000EBE RID: 3774
	public Vector3 shadowPos;

	// Token: 0x04000EBF RID: 3775
	public Vector3 introPos;

	// Token: 0x04000EC1 RID: 3777
	public float distToBlowout;

	// Token: 0x04000EC2 RID: 3778
	public AbstractPlayerController player1;

	// Token: 0x04000EC3 RID: 3779
	public AbstractPlayerController player2;

	// Token: 0x04000EC4 RID: 3780
	public Vector3 lastPlayer1Position = Vector3.zero;

	// Token: 0x04000EC5 RID: 3781
	public Vector3 lastPlayer2Position = Vector3.zero;

	// Token: 0x04000EC6 RID: 3782
	public float stepTimer;

	// Token: 0x04000EC7 RID: 3783
	[SerializeField]
	public GameObject glow;

	// Token: 0x04000EC8 RID: 3784
	[SerializeField]
	public ChessBishopLevelIntroCandle introCandle;

	// Token: 0x04000EC9 RID: 3785
	public bool introPlaying = true;

	// Token: 0x04000ECA RID: 3786
	[SerializeField]
	public bool isLastIntro;

	// Token: 0x04000ECB RID: 3787
	[SerializeField]
	public float introOvershoot;
}
