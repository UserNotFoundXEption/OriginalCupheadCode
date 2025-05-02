using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002E6 RID: 742
public class OldManLevelSockPuppet : AbstractCollidableObject
{
	// Token: 0x060020E7 RID: 8423 RVA: 0x0001C105 File Offset: 0x0001A305
	public void Start()
	{
		this.rootPosition = base.transform.position;
		this.startPosition = base.transform.position;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060020E8 RID: 8424 RVA: 0x0001C134 File Offset: 0x0001A334
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x060020E9 RID: 8425 RVA: 0x000B91F0 File Offset: 0x000B73F0
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.transform.position = this.rootPosition + Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX * Vector3.right + Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY * Vector3.up;
		this.wobbleTimer += CupheadTime.Delta;
	}

	// Token: 0x060020EA RID: 8426 RVA: 0x0001C142 File Offset: 0x0001A342
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060020EB RID: 8427 RVA: 0x0001C160 File Offset: 0x0001A360
	public void AniEvent_IncCmonCount()
	{
		base.animator.SetInteger("CmonCount", base.animator.GetInteger("CmonCount") + 1);
	}

	// Token: 0x060020EC RID: 8428 RVA: 0x0001C184 File Offset: 0x0001A384
	public void AniEvent_ResetCmonCount()
	{
		base.animator.SetInteger("CmonCount", 0);
	}

	// Token: 0x060020ED RID: 8429 RVA: 0x0001C197 File Offset: 0x0001A397
	public Vector3 throwPosition()
	{
		return this.throwPos.position;
	}

	// Token: 0x060020EE RID: 8430 RVA: 0x0001C1A4 File Offset: 0x0001A3A4
	public Vector3 catchPosition()
	{
		return this.catchPos.position;
	}

	// Token: 0x060020EF RID: 8431 RVA: 0x000B928C File Offset: 0x000B748C
	public float EaseOvershoot(float start, float end, float value, float overshoot)
	{
		float num = Mathf.Lerp(start, end, value);
		return num + Mathf.Sin(value * 3.14159274f) * ((end - start) * overshoot);
	}

	// Token: 0x060020F0 RID: 8432 RVA: 0x0001C1B1 File Offset: 0x0001A3B1
	public void MoveToPos(float endYPos, float distanceToCover)
	{
		if (distanceToCover == 0f && !this.entering)
		{
			this.ready = true;
			return;
		}
		this.ready = false;
		base.StartCoroutine(this.move_to_pos_cr(endYPos, distanceToCover));
	}

	// Token: 0x060020F1 RID: 8433 RVA: 0x0001C1E7 File Offset: 0x0001A3E7
	public float InverseLerpUnclamped(float a, float b, float value)
	{
		return (value - a) / (b - a);
	}

	// Token: 0x060020F2 RID: 8434 RVA: 0x000B92BC File Offset: 0x000B74BC
	public IEnumerator move_to_pos_cr(float endYPos, float distanceToCover)
	{
		float t = 0f;
		float startYPos = this.rootPosition.y;
		float time = (distanceToCover != 1f) ? this.moveTimeLong : this.moveTimeShort;
		if (this.entering)
		{
			time = 0.5f;
			if (this.isLeft)
			{
				this.SFX_OMM_P2_PuppetLeftRaiseUp();
				this.SFX_OMM_P2_PuppetLeftRaiseUpVocal();
			}
			else
			{
				this.SFX_OMM_P2_PuppetRightRaiseUp();
				this.SFX_OMM_P2_PuppetRightRaiseUpVocal();
			}
		}
		YieldInstruction wait = new WaitForFixedUpdate();
		bool startEndAnimation = false;
		bool movingUp = endYPos > this.rootPosition.y;
		string moveBool = (!movingUp) ? ((distanceToCover != 1f) ? "MovingDown" : "MovingDownShort") : "MovingUp";
		base.animator.SetBool(moveBool, true);
		string startAnimation = (!movingUp) ? ((distanceToCover != 1f) ? "Move_Down_Start" : "Move_Down_Short_Start") : "Move_Up_Start";
		if (!this.dead)
		{
			if (movingUp)
			{
				yield return base.animator.WaitForAnimationToStart(this, startAnimation, false);
			}
			else
			{
				yield return base.animator.WaitForAnimationToStart(this, startAnimation, false);
			}
		}
		WaitForFrameTimePersistent wait24fps = new WaitForFrameTimePersistent(0.0416666679f, false);
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			this.rootPosition = new Vector3(this.startPosition.x + this.armBowingXModifier * Mathf.Sin(this.InverseLerpUnclamped(startYPos, endYPos, this.rootPosition.y) * 3.14159274f), this.EaseOvershoot(startYPos, endYPos, t / time, this.moveOvershoot), base.transform.position.z);
			if (t / time >= 0.35f && !startEndAnimation)
			{
				base.animator.SetBool(moveBool, false);
				startEndAnimation = true;
			}
			if (t / time >= 0.6f && this.entering)
			{
				this.entering = false;
				this.colliders = base.GetComponentsInChildren<Collider2D>();
				foreach (Collider2D collider2D in this.colliders)
				{
					collider2D.enabled = true;
				}
			}
			yield return wait24fps;
		}
		this.rootPosition.x = this.startPosition.x;
		this.armsHolding.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
		this.arms.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 10;
		int target = Animator.StringToHash(base.animator.GetLayerName(0) + "." + ((!movingUp) ? ((distanceToCover != 1f) ? "Move_Down_End" : "Move_Down_Short_End") : "Move_Up_End"));
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == target)
		{
			yield return null;
		}
		base.animator.SetBool("TauntA", !base.animator.GetBool("TauntA"));
		this.ready = true;
		yield break;
	}

	// Token: 0x060020F3 RID: 8435 RVA: 0x0001C1F0 File Offset: 0x0001A3F0
	public void StopTaunt()
	{
		base.animator.SetInteger("CmonCount", 5);
	}

	// Token: 0x060020F4 RID: 8436 RVA: 0x0001C203 File Offset: 0x0001A403
	public void AniEvent_Catch()
	{
		this.main.CatchBall();
	}

	// Token: 0x060020F5 RID: 8437 RVA: 0x0001C210 File Offset: 0x0001A410
	public void AnIEvent_HoldingBall()
	{
		this.arms.SetActive(false);
		this.armsHolding.SetActive(true);
	}

	// Token: 0x060020F6 RID: 8438 RVA: 0x0001C22A File Offset: 0x0001A42A
	public void AnIEvent_NotHoldingBall()
	{
		this.arms.SetActive(true);
		this.armsHolding.SetActive(false);
	}

	// Token: 0x060020F7 RID: 8439 RVA: 0x000B92E8 File Offset: 0x000B74E8
	public void Die()
	{
		this.dead = true;
		foreach (Collider2D collider2D in this.colliders)
		{
			collider2D.enabled = false;
		}
		base.animator.Play("Death");
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		if (this.rootPosition.y > 200f)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.move_to_pos_cr(180f, 1f));
		}
	}

	// Token: 0x060020F8 RID: 8440 RVA: 0x0001C244 File Offset: 0x0001A444
	public void AnimationEvent_SFX_OMM_P2_PuppetRightCatch()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_catch");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_catch");
	}

	// Token: 0x060020F9 RID: 8441 RVA: 0x0001C260 File Offset: 0x0001A460
	public void SFX_OMM_P2_PuppetRightRaiseUp()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_raiseup");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_raiseup");
	}

	// Token: 0x060020FA RID: 8442 RVA: 0x0001C27C File Offset: 0x0001A47C
	public void SFX_OMM_P2_PuppetRightRaiseUpVocal()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_raiseup_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_raiseup_vocal");
	}

	// Token: 0x060020FB RID: 8443 RVA: 0x0001C298 File Offset: 0x0001A498
	public void AnimationEvent_SFX_OMM_P2_PuppetRightThrow()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_throw");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_throw");
	}

	// Token: 0x060020FC RID: 8444 RVA: 0x0001C2B4 File Offset: 0x0001A4B4
	public void AnimationEvent_SFX_OMM_P2_PuppetRightThrowVocal()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_throw_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_throw_vocal");
	}

	// Token: 0x060020FD RID: 8445 RVA: 0x0001C2D0 File Offset: 0x0001A4D0
	public void AnimationEvent_SFX_OMM_P2_PuppetLeftCatch()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_catch");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_catch");
	}

	// Token: 0x060020FE RID: 8446 RVA: 0x0001C2EC File Offset: 0x0001A4EC
	public void SFX_OMM_P2_PuppetLeftRaiseUpVocal()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_raiseup");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_raiseup");
	}

	// Token: 0x060020FF RID: 8447 RVA: 0x0001C308 File Offset: 0x0001A508
	public void SFX_OMM_P2_PuppetLeftRaiseUp()
	{
		base.StartCoroutine(this.SFX_OMM_P2_PuppetLeftRaiseUpVocal_cr());
	}

	// Token: 0x06002100 RID: 8448 RVA: 0x000B9370 File Offset: 0x000B7570
	public IEnumerator SFX_OMM_P2_PuppetLeftRaiseUpVocal_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0f);
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_raiseup_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_raiseup_vocal");
		yield break;
	}

	// Token: 0x06002101 RID: 8449 RVA: 0x0001C317 File Offset: 0x0001A517
	public void AnimationEvent_SFX_OMM_P2_PuppetLeftThrow()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_throw");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_throw");
	}

	// Token: 0x06002102 RID: 8450 RVA: 0x0001C333 File Offset: 0x0001A533
	public void AnimationEvent_SFX_OMM_P2_PuppetLeftThrowVocal()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_throw_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_throw_vocal");
	}

	// Token: 0x06002103 RID: 8451 RVA: 0x0001C34F File Offset: 0x0001A54F
	public void WORKAROUND_NullifyFields()
	{
		this.arms = null;
		this.armsHolding = null;
		this.throwPos = null;
		this.catchPos = null;
		this.damageDealer = null;
		this.main = null;
		this.colliders = null;
	}

	// Token: 0x04001B0C RID: 6924
	public const string MOVING_UP = "MovingUp";

	// Token: 0x04001B0D RID: 6925
	public const string MOVING_DOWN = "MovingDown";

	// Token: 0x04001B0E RID: 6926
	public const string MOVING_DOWN_SHORT = "MovingDownShort";

	// Token: 0x04001B0F RID: 6927
	public const string MOVE_UP_START = "Move_Up_Start";

	// Token: 0x04001B10 RID: 6928
	public const string MOVE_DOWN_START = "Move_Down_Start";

	// Token: 0x04001B11 RID: 6929
	public const string MOVE_DOWN_SHORT_START = "Move_Down_Short_Start";

	// Token: 0x04001B12 RID: 6930
	public const string MOVE_UP_END = "Move_Up_End";

	// Token: 0x04001B13 RID: 6931
	public const string MOVE_DOWN_END = "Move_Down_End";

	// Token: 0x04001B14 RID: 6932
	public const string MOVE_DOWN_SHORT_END = "Move_Down_Short_End";

	// Token: 0x04001B15 RID: 6933
	[SerializeField]
	public GameObject arms;

	// Token: 0x04001B16 RID: 6934
	[SerializeField]
	public GameObject armsHolding;

	// Token: 0x04001B17 RID: 6935
	[SerializeField]
	public Transform throwPos;

	// Token: 0x04001B18 RID: 6936
	[SerializeField]
	public Transform catchPos;

	// Token: 0x04001B19 RID: 6937
	public bool ready;

	// Token: 0x04001B1A RID: 6938
	public DamageDealer damageDealer;

	// Token: 0x04001B1B RID: 6939
	[SerializeField]
	public float armBowingXModifier;

	// Token: 0x04001B1C RID: 6940
	[SerializeField]
	public float wobbleX = 5f;

	// Token: 0x04001B1D RID: 6941
	[SerializeField]
	public float wobbleY = 5f;

	// Token: 0x04001B1E RID: 6942
	public Vector3 rootPosition;

	// Token: 0x04001B1F RID: 6943
	public Vector3 startPosition;

	// Token: 0x04001B20 RID: 6944
	[SerializeField]
	public float wobbleTimer;

	// Token: 0x04001B21 RID: 6945
	[SerializeField]
	public float moveTimeShort = 0.375f;

	// Token: 0x04001B22 RID: 6946
	[SerializeField]
	public float moveTimeLong = 0.5f;

	// Token: 0x04001B23 RID: 6947
	[SerializeField]
	public float moveOvershoot = 0.5f;

	// Token: 0x04001B24 RID: 6948
	[SerializeField]
	public bool isLeft;

	// Token: 0x04001B25 RID: 6949
	public bool entering = true;

	// Token: 0x04001B26 RID: 6950
	public bool dead;

	// Token: 0x04001B27 RID: 6951
	[SerializeField]
	public OldManLevelSockPuppetHandler main;

	// Token: 0x04001B28 RID: 6952
	public Collider2D[] colliders;
}
