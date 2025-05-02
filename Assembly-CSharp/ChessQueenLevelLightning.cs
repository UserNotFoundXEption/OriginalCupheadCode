using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018E RID: 398
public class ChessQueenLevelLightning : AbstractProjectile
{
	// Token: 0x17000254 RID: 596
	// (get) Token: 0x060012F7 RID: 4855 RVA: 0x0000FF22 File Offset: 0x0000E122
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000255 RID: 597
	// (get) Token: 0x060012F8 RID: 4856 RVA: 0x0000FF29 File Offset: 0x0000E129
	// (set) Token: 0x060012F9 RID: 4857 RVA: 0x0000FF31 File Offset: 0x0000E131
	public bool isGone { get; set; }

	// Token: 0x060012FA RID: 4858 RVA: 0x000968FC File Offset: 0x00094AFC
	public ChessQueenLevelLightning Create(float posX, LevelProperties.ChessQueen.Lightning properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = new Vector3(posX, -385f);
		this.properties = properties;
		this.lionsLandDustFX.Create(this.dropDustPos.transform.position);
		base.StartCoroutine(this.move_cr());
		return this;
	}

	// Token: 0x060012FB RID: 4859 RVA: 0x0000FF3A File Offset: 0x0000E13A
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060012FC RID: 4860 RVA: 0x0000FF58 File Offset: 0x0000E158
	public override void OnParry(AbstractPlayerController player)
	{
		this.StopAllCoroutines();
		this.Die();
		this.isGone = true;
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x060012FD RID: 4861 RVA: 0x0009695C File Offset: 0x00094B5C
	public void LateUpdate()
	{
		int num = Mathf.Clamp((int)(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (float)this.deathSparkSprites.Length), 0, this.deathSparkSprites.Length - 1);
		if (num < 0)
		{
			return;
		}
		this.bottomRenderer.sortingOrder = ((!ChessQueenLevelLightning.BottomInFront[num]) ? -1 : 1);
		this.topRenderer.sortingOrder = ((!ChessQueenLevelLightning.MiddleInFront[num]) ? 1 : -1);
	}

	// Token: 0x060012FE RID: 4862 RVA: 0x000969E0 File Offset: 0x00094BE0
	public IEnumerator move_cr()
	{
		this.SFX_KOG_QUEEN_ChessPiecesFall();
		base.transform.localScale = new Vector3(Mathf.Sign(base.transform.position.x), 1f);
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", false, true);
		float delayTime = this.properties.lightningDelayTime - 0.5416667f;
		yield return CupheadTime.WaitForSeconds(this, delayTime);
		this.SFX_KOG_QUEEN_ChessPieceRoar();
		this.speed = Mathf.Sign(base.transform.position.x) * -this.properties.lightningSweepSpeed;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > (float)Level.Current.Left - 400f && base.transform.position.x < (float)Level.Current.Right + 400f)
		{
			base.transform.AddPosition(this.speed * CupheadTime.FixedDelta, 0f, 0f);
			yield return wait;
		}
		this.isGone = true;
		this.Recycle<ChessQueenLevelLightning>();
		yield break;
	}

	// Token: 0x060012FF RID: 4863 RVA: 0x000969FC File Offset: 0x00094BFC
	public IEnumerator death_cr()
	{
		AnimationHelper animationHelper = base.GetComponent<AnimationHelper>();
		animationHelper.Speed = 0f;
		int index = Mathf.Clamp((int)(base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime * (float)this.deathSparkSprites.Length), 0, this.deathSparkSprites.Length - 1);
		if (index < 0)
		{
			index = 0;
		}
		this.bottomRenderer.enabled = false;
		this.deathSparkRenderer.sprite = this.deathSparkSprites[index];
		yield return CupheadTime.WaitForSeconds(this, 0.0416666679f);
		this.deathSparkRenderer.sprite = null;
		this.bottomRenderer.enabled = true;
		animationHelper.Speed = 1f;
		base.animator.Play("Death");
		this.SFX_KOG_QUEEN_ChessPiecesParried();
		base.StartCoroutine(this.SFX_KOG_QUEEN_ChessPieceMeow_cr());
		base.animator.SetTrigger("DustEnd");
		float minSpeed = this.speed * 0.2f;
		float maxSpeed = this.speed * 0.8f;
		SpriteDeathParts part = this.deathParts.CreatePart(this.bottomRenderer.transform.position);
		part.SetVelocityX(minSpeed, maxSpeed);
		part.GetComponent<SpriteRenderer>().sortingOrder = 13;
		part.transform.localScale = base.transform.localScale;
		part = this.deathParts.CreatePart(this.middleRenderer.transform.position);
		part.SetVelocityX(minSpeed, maxSpeed);
		part.GetComponent<SpriteRenderer>().sortingOrder = 14;
		part.transform.localScale = new Vector3(-base.transform.localScale.x, 1f);
		part = this.deathParts.CreatePart(this.topRenderer.transform.position);
		part.SetVelocityX(minSpeed, maxSpeed);
		part.transform.localScale = base.transform.localScale;
		part = this.deathDust.CreatePart(this.dustRenderer.transform.position);
		part.SetVelocityX(this.speed * 0.5f, this.speed * 0.5f);
		part.transform.localScale = base.transform.localScale;
		yield break;
	}

	// Token: 0x06001300 RID: 4864 RVA: 0x0000FF7A File Offset: 0x0000E17A
	public void SFX_KOG_QUEEN_ChessPieceRoar()
	{
		AudioManager.Play("sfx_dlc_kog_queen_chesspieceroar");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspieceroar");
	}

	// Token: 0x06001301 RID: 4865 RVA: 0x0000FF96 File Offset: 0x0000E196
	public void SFX_KOG_QUEEN_ChessPiecesFall()
	{
		AudioManager.Play("sfx_dlc_kog_queen_chesspiecesfall");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspiecesfall");
	}

	// Token: 0x06001302 RID: 4866 RVA: 0x0000FFB2 File Offset: 0x0000E1B2
	public void SFX_KOG_QUEEN_ChessPiecesParried()
	{
		AudioManager.Play("sfx_dlc_kog_queen_chesspiecesparried");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspiecesparried");
	}

	// Token: 0x06001303 RID: 4867 RVA: 0x00096A18 File Offset: 0x00094C18
	public IEnumerator SFX_KOG_QUEEN_ChessPieceMeow_cr()
	{
		AudioManager.Stop("sfx_dlc_kog_queen_chesspieceroar");
		yield return CupheadTime.WaitForSeconds(this, 0.17f);
		AudioManager.Play("sfx_dlc_kog_queen_chesspiecemeow");
		this.emitAudioFromObject.Add("sfx_dlc_kog_queen_chesspiecemeow");
		yield break;
	}

	// Token: 0x04000F4D RID: 3917
	public const float YPosition = -385f;

	// Token: 0x04000F4E RID: 3918
	public static readonly bool[] BottomInFront = new bool[]
	{
		true,
		true,
		true,
		false,
		false,
		false,
		false,
		false,
		true,
		true,
		true,
		false,
		false,
		false,
		false,
		false
	};

	// Token: 0x04000F4F RID: 3919
	public static readonly bool[] MiddleInFront = new bool[]
	{
		false,
		false,
		false,
		false,
		true,
		true,
		true,
		false,
		false,
		false,
		false,
		false,
		true,
		true,
		true,
		false
	};

	// Token: 0x04000F50 RID: 3920
	[SerializeField]
	public SpriteRenderer bottomRenderer;

	// Token: 0x04000F51 RID: 3921
	[SerializeField]
	public SpriteRenderer middleRenderer;

	// Token: 0x04000F52 RID: 3922
	[SerializeField]
	public SpriteRenderer topRenderer;

	// Token: 0x04000F53 RID: 3923
	[SerializeField]
	public SpriteRenderer dustRenderer;

	// Token: 0x04000F54 RID: 3924
	[SerializeField]
	public SpriteRenderer deathSparkRenderer;

	// Token: 0x04000F55 RID: 3925
	[SerializeField]
	public Sprite[] rotatingSprites;

	// Token: 0x04000F56 RID: 3926
	[SerializeField]
	public Sprite[] deathSparkSprites;

	// Token: 0x04000F57 RID: 3927
	[SerializeField]
	public Effect lionsLandDustFX;

	// Token: 0x04000F58 RID: 3928
	[SerializeField]
	public Transform dropDustPos;

	// Token: 0x04000F59 RID: 3929
	[SerializeField]
	public SpriteDeathParts deathParts;

	// Token: 0x04000F5A RID: 3930
	[SerializeField]
	public SpriteDeathPartsDLC deathDust;

	// Token: 0x04000F5C RID: 3932
	public LevelProperties.ChessQueen.Lightning properties;

	// Token: 0x04000F5D RID: 3933
	public float speed;
}
