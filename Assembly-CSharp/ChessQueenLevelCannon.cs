using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200018B RID: 395
public class ChessQueenLevelCannon : AbstractCollidableObject
{
	// Token: 0x17000252 RID: 594
	// (get) Token: 0x060012DA RID: 4826 RVA: 0x0000FDEF File Offset: 0x0000DFEF
	// (set) Token: 0x060012DB RID: 4827 RVA: 0x0000FDF7 File Offset: 0x0000DFF7
	public bool IsActive { get; set; }

	// Token: 0x060012DC RID: 4828 RVA: 0x00095EF0 File Offset: 0x000940F0
	public void Start()
	{
		this.parryColliders = new Collider2D[this.parry.Length];
		for (int i = 0; i < this.parry.Length; i++)
		{
			this.parry[i].OnActivate += this.shootCannonball;
			this.parryColliders[i] = this.parry[i].GetComponent<Collider2D>();
		}
		this.SetActive(false);
		this.mouseAnimator.Play("Idle");
		this.mouseLookTime = Random.Range(1f, 3f);
		this.setupWickParabola();
	}

	// Token: 0x060012DD RID: 4829 RVA: 0x0000FE00 File Offset: 0x0000E000
	public void SetProperties(float minAngle, float maxAngle, float rotationTime, ChessQueenLevelCannon.CannonPosition cannonPosition, LevelProperties.ChessQueen.Turret properties, ChessQueenLevelQueen queen)
	{
		this.minAngle = minAngle;
		this.maxAngle = maxAngle;
		this.rotationTime = rotationTime;
		this.cannonPosition = cannonPosition;
		this.properties = properties;
		this.queen = queen;
		this.move();
	}

	// Token: 0x060012DE RID: 4830 RVA: 0x00095F8C File Offset: 0x0009418C
	public void SetActive(bool setActive)
	{
		this.mouseAnimator.SetBool("Idle", !setActive);
		if (setActive)
		{
			this.mouseAnimator.SetBool("LookRight", false);
		}
		this.IsActive = setActive;
		foreach (Collider2D collider2D in this.parryColliders)
		{
			collider2D.enabled = setActive;
		}
	}

	// Token: 0x060012DF RID: 4831 RVA: 0x00095FF4 File Offset: 0x000941F4
	public void LateUpdate()
	{
		if (!this.IsActive)
		{
			this.mouseLookTime -= CupheadTime.Delta;
			if (this.mouseLookTime < 0f)
			{
				this.mouseAnimator.SetBool("LookRight", !this.mouseAnimator.GetBool("LookRight"));
				this.mouseLookTime += Random.Range(1f, 3f);
			}
		}
		int num = Array.IndexOf<Sprite>(this.baseSprites, this.baseRenderer.sprite);
		if (num < 0)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("01"))
			{
				num = 0;
			}
			else if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("05"))
			{
				num = 4;
			}
			else if (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("10"))
			{
				num = 9;
			}
			else
			{
				if (!base.animator.GetCurrentAnimatorStateInfo(0).IsTag("15"))
				{
					return;
				}
				num = 14;
			}
		}
		float num2 = (float)num / (float)(this.baseSprites.Length - 1);
		if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Side)
		{
			num2 = EaseUtils.EaseInOutCubic(0f, 1f, num2);
		}
		float num3 = Mathf.Lerp(this.minAngle, this.maxAngle, num2);
		if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Center)
		{
			num3 *= (float)((!this.baseRenderer.flipX) ? 1 : -1);
		}
		this.barrelTransform.rotation = Quaternion.Euler(0f, 0f, num3);
		this.barrelHighlightRenderer.sprite = this.barrelHighlightSprites[Mathf.RoundToInt((1f - num2) * (float)(this.barrelHighlightSprites.Length - 1))];
		this.barrelHighlightRenderer.flipX = (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Center && this.baseRenderer.flipX);
		this.barrelHighlightRenderer.enabled = base.animator.GetCurrentAnimatorStateInfo(1).IsName("Idle");
		if (this.wickFollowsParabola)
		{
			float num4 = num2 * this.wickParametricDuration;
			Vector3 position = this.wickStartPosition;
			position.x -= 2f * this.wickParabolaParameter * num4 * (float)((!this.baseRenderer.flipX) ? 1 : -1);
			position.y += this.wickParabolaParameter * num4 * num4;
			this.wickTransform.position = position;
		}
		if (Level.Current.Ending)
		{
			foreach (Collider2D collider2D in this.parryColliders)
			{
				collider2D.enabled = false;
			}
		}
	}

	// Token: 0x060012E0 RID: 4832 RVA: 0x0000FE35 File Offset: 0x0000E035
	public void move()
	{
		if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Side)
		{
			base.animator.Play(0, ChessQueenLevelCannon.BaseAnimatorLayer, 0.5f);
		}
		base.StartCoroutine(this.cannonActive_cr());
	}

	// Token: 0x060012E1 RID: 4833 RVA: 0x000962E0 File Offset: 0x000944E0
	public IEnumerator cannonActive_cr()
	{
		for (;;)
		{
			while (!this.IsActive)
			{
				yield return null;
			}
			base.animator.SetBool("Moving", true);
			base.animator.SetFloat("BaseSpeed", ChessQueenLevelCannon.BaseRotationDuration / this.rotationTime);
			base.animator.SetTrigger("WickIgnite");
			this.SFX_KOG_QUEEN_CannonFuseLoop();
			while (this.IsActive)
			{
				foreach (Collider2D collider2D in this.parryColliders)
				{
					if (this.queen.activeLightning == null || this.queen.activeLightning.isGone)
					{
						collider2D.enabled = true;
					}
					else
					{
						collider2D.enabled = (Mathf.Abs(collider2D.transform.position.x + collider2D.offset.x - this.queen.activeLightning.transform.position.x) > this.queen.lightningDisableRange);
					}
				}
				float animTime = base.animator.GetCurrentAnimatorStateInfo(ChessQueenLevelCannon.BaseAnimatorLayer).normalizedTime % 1f;
				if (this.mouseReverses)
				{
					if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Side)
					{
						this.mouseAnimator.SetBool("Reverse", animTime > 0.4f && animTime <= 0.9f);
					}
					else
					{
						this.mouseAnimator.SetBool("Reverse", animTime > 0.5f != this.baseRenderer.flipX);
					}
				}
				yield return null;
			}
			base.animator.SetBool("Moving", false);
		}
		yield break;
	}

	// Token: 0x060012E2 RID: 4834 RVA: 0x000962FC File Offset: 0x000944FC
	public void shootCannonball()
	{
		if (this.IsActive)
		{
			base.animator.SetTrigger("CannonBlast");
			base.animator.SetTrigger("WickBlast");
			this.SFX_KOG_QUEEN_CannonShoot();
			this.SFX_KOG_QUEEN_CannonFuseLoopStop();
			this.SetActive(false);
			base.StartCoroutine(this.shoot_cr());
		}
	}

	// Token: 0x060012E3 RID: 4835 RVA: 0x00096354 File Offset: 0x00094554
	public IEnumerator shoot_cr()
	{
		this.wickFollowsParabola = false;
		this.wickTransform.parent = this.wickBlastPositionerTransform;
		this.blastFXTransform.position = this.blastFXSpawnPoint.position;
		this.blastFXTransform.eulerAngles = this.barrelTransform.eulerAngles;
		base.animator.Play((!((ChessQueenLevel)Level.Current).cannonBlastFXVariant) ? "BlastFXB" : "BlastFXA", ChessQueenLevelCannon.BlastFXAnimatorLayer, 0f);
		((ChessQueenLevel)Level.Current).cannonBlastFXVariant = !((ChessQueenLevel)Level.Current).cannonBlastFXVariant;
		base.animator.Update(0f);
		while (base.animator.GetCurrentAnimatorStateInfo(ChessQueenLevelCannon.CannonAnimatorLayer).normalizedTime < 0.7f)
		{
			yield return null;
		}
		base.animator.SetFloat("BaseSpeed", ChessQueenLevelCannon.BaseRotationDuration / this.rotationTime);
		base.animator.SetBool("Moving", false);
		this.wickTransform.parent = base.transform;
		this.wickFollowsParabola = true;
		this.wickTransform.localEulerAngles = Vector3.zero;
		yield break;
	}

	// Token: 0x060012E4 RID: 4836 RVA: 0x0000FE65 File Offset: 0x0000E065
	public void animationEvent_CannonFinishedCycle()
	{
		if (this.cannonPosition == ChessQueenLevelCannon.CannonPosition.Center)
		{
			this.baseRenderer.flipX = !this.baseRenderer.flipX;
			this.baseTopperRenderer.flipX = this.baseRenderer.flipX;
		}
	}

	// Token: 0x060012E5 RID: 4837 RVA: 0x00096370 File Offset: 0x00094570
	public void animationEvent_FireBullet()
	{
		if (Level.Current.Ending)
		{
			return;
		}
		this.looseMouse.CannonFired(this.cannonBall.Create(this.bulletSpawnPoint.transform.position, MathUtils.DirectionToAngle(this.barrelTransform.up), this.properties.turretCannonballSpeed).gameObject);
	}

	// Token: 0x060012E6 RID: 4838 RVA: 0x000963E0 File Offset: 0x000945E0
	public void setupWickParabola()
	{
		this.wickStartPosition = this.wickTransform.position;
		Vector3 vector = this.wickParabolaEndTransform.position - this.wickStartPosition;
		this.wickParabolaParameter = vector.x * vector.x / (4f * vector.y);
		this.wickParametricDuration = vector.x / (2f * this.wickParabolaParameter);
	}

	// Token: 0x060012E7 RID: 4839 RVA: 0x00096454 File Offset: 0x00094654
	public void SFX_KOG_QUEEN_CannonShoot()
	{
		AudioManager.Stop("sfx_DLC_KOG_Queen_CannonFuse_Loop");
		AudioManager.Play("sfx_DLC_KOG_Queen_CannonShoot");
		AudioManager.Pan("sfx_DLC_KOG_Queen_CannonShoot", (Mathf.Abs(base.transform.position.x) <= 100f) ? 0f : Mathf.Sign(base.transform.position.x));
	}

	// Token: 0x060012E8 RID: 4840 RVA: 0x000964C4 File Offset: 0x000946C4
	public void SFX_KOG_QUEEN_CannonFuseLoop()
	{
		AudioManager.PlayLoop("sfx_DLC_KOG_Queen_CannonFuse_Loop");
		AudioManager.Pan("sfx_DLC_KOG_Queen_CannonFuse_Loop", (Mathf.Abs(base.transform.position.x) <= 100f) ? 0f : Mathf.Sign(base.transform.position.x));
	}

	// Token: 0x060012E9 RID: 4841 RVA: 0x0000FEA2 File Offset: 0x0000E0A2
	public void SFX_KOG_QUEEN_CannonFuseLoopStop()
	{
		AudioManager.Stop("sfx_DLC_KOG_Queen_CannonFuse_Loop");
	}

	// Token: 0x04000F1C RID: 3868
	public static readonly float BaseRotationDuration = 0.625f;

	// Token: 0x04000F1D RID: 3869
	public static readonly int BaseAnimatorLayer;

	// Token: 0x04000F1E RID: 3870
	public static readonly int CannonAnimatorLayer = 1;

	// Token: 0x04000F1F RID: 3871
	public static readonly int BlastFXAnimatorLayer = 3;

	// Token: 0x04000F20 RID: 3872
	[SerializeField]
	public ChessQueenLevelCannonball cannonBall;

	// Token: 0x04000F21 RID: 3873
	[SerializeField]
	public ParrySwitch[] parry;

	// Token: 0x04000F22 RID: 3874
	[SerializeField]
	public SpriteRenderer baseRenderer;

	// Token: 0x04000F23 RID: 3875
	[SerializeField]
	public SpriteRenderer barrelHighlightRenderer;

	// Token: 0x04000F24 RID: 3876
	[SerializeField]
	public SpriteRenderer baseTopperRenderer;

	// Token: 0x04000F25 RID: 3877
	[SerializeField]
	public Transform barrelTransform;

	// Token: 0x04000F26 RID: 3878
	[SerializeField]
	public Transform bulletSpawnPoint;

	// Token: 0x04000F27 RID: 3879
	[SerializeField]
	public Transform blastFXSpawnPoint;

	// Token: 0x04000F28 RID: 3880
	[SerializeField]
	public Transform blastFXTransform;

	// Token: 0x04000F29 RID: 3881
	[SerializeField]
	public Sprite[] baseSprites;

	// Token: 0x04000F2A RID: 3882
	[SerializeField]
	public Sprite[] barrelHighlightSprites;

	// Token: 0x04000F2B RID: 3883
	[SerializeField]
	public Transform wickTransform;

	// Token: 0x04000F2C RID: 3884
	[SerializeField]
	public Transform wickParabolaEndTransform;

	// Token: 0x04000F2D RID: 3885
	[SerializeField]
	public Transform wickBlastPositionerTransform;

	// Token: 0x04000F2E RID: 3886
	[SerializeField]
	public Animator mouseAnimator;

	// Token: 0x04000F2F RID: 3887
	[SerializeField]
	public ChessQueenLevelLooseMouse looseMouse;

	// Token: 0x04000F30 RID: 3888
	[SerializeField]
	public bool mouseReverses;

	// Token: 0x04000F32 RID: 3890
	public Collider2D[] parryColliders;

	// Token: 0x04000F33 RID: 3891
	public LevelProperties.ChessQueen.Turret properties;

	// Token: 0x04000F34 RID: 3892
	public float rotationTime;

	// Token: 0x04000F35 RID: 3893
	public float minAngle;

	// Token: 0x04000F36 RID: 3894
	public float maxAngle;

	// Token: 0x04000F37 RID: 3895
	public ChessQueenLevelCannon.CannonPosition cannonPosition;

	// Token: 0x04000F38 RID: 3896
	public ChessQueenLevelQueen queen;

	// Token: 0x04000F39 RID: 3897
	public float mouseLookTime;

	// Token: 0x04000F3A RID: 3898
	public bool wickFollowsParabola = true;

	// Token: 0x04000F3B RID: 3899
	public Vector3 wickStartPosition;

	// Token: 0x04000F3C RID: 3900
	public float wickParabolaParameter;

	// Token: 0x04000F3D RID: 3901
	public float wickParametricDuration;

	// Token: 0x02000AD3 RID: 2771
	public enum CannonPosition
	{
		// Token: 0x04004F6F RID: 20335
		Side,
		// Token: 0x04004F70 RID: 20336
		Center
	}
}
