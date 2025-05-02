using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000123 RID: 291
public class AirplaneLevelBulldogParachute : LevelProperties.Airplane.Entity
{
	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000DBC RID: 3516 RVA: 0x0000BBE3 File Offset: 0x00009DE3
	// (set) Token: 0x06000DBD RID: 3517 RVA: 0x0000BBEB File Offset: 0x00009DEB
	public bool isMoving { get; set; }

	// Token: 0x06000DBE RID: 3518 RVA: 0x0000BBF4 File Offset: 0x00009DF4
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.pinkString = new PatternString(base.properties.CurrentState.parachute.pinkString, true, true);
	}

	// Token: 0x06000DBF RID: 3519 RVA: 0x0000BC29 File Offset: 0x00009E29
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x06000DC0 RID: 3520 RVA: 0x0000BC37 File Offset: 0x00009E37
	public override void LevelInit(LevelProperties.Airplane properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06000DC1 RID: 3521 RVA: 0x0000BC40 File Offset: 0x00009E40
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06000DC2 RID: 3522 RVA: 0x0000BC5E File Offset: 0x00009E5E
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06000DC3 RID: 3523 RVA: 0x00087CEC File Offset: 0x00085EEC
	public void StartDescent(Vector2 pos, float scale)
	{
		this.isMoving = true;
		base.transform.position = pos;
		base.transform.localScale = new Vector3(scale, 1f);
		this.count = 0;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06000DC4 RID: 3524 RVA: 0x00087D3C File Offset: 0x00085F3C
	public IEnumerator move_cr()
	{
		base.animator.Play("Drop");
		base.animator.Update(0f);
		yield return base.animator.WaitForAnimationToEnd(this, "Drop", false, true);
		this.isMoving = false;
		yield break;
	}

	// Token: 0x06000DC5 RID: 3525 RVA: 0x00087D58 File Offset: 0x00085F58
	public void EarlyExit()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.early_exit_cr());
		if (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.413793117f)
		{
			base.transform.position = new Vector3(base.transform.position.x, this.collider.transform.localPosition.y + 400f);
			base.animator.Play("Drop", 0, 0.87356323f);
			base.animator.Update(0f);
		}
	}

	// Token: 0x06000DC6 RID: 3526 RVA: 0x00087E00 File Offset: 0x00086000
	public IEnumerator early_exit_cr()
	{
		if (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.413793117f)
		{
			base.animator.Play("Drop", 0, 0.87356323f);
		}
		yield return base.animator.WaitForAnimationToStart(this, "None", false);
		this.isMoving = false;
		if (this.main.isDead && this.mainAnimator)
		{
			this.mainAnimator.SetBool("InParachuteATK", false);
		}
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x06000DC7 RID: 3527 RVA: 0x0000BC76 File Offset: 0x00009E76
	public void OnDisable()
	{
		base.GetComponent<HitFlash>().StopAllCoroutinesWithoutSettingScale();
		base.GetComponent<SpriteRenderer>().color = Color.black;
	}

	// Token: 0x06000DC8 RID: 3528 RVA: 0x00087E1C File Offset: 0x0008601C
	public void AniEvent_Shoot()
	{
		LevelProperties.Airplane.Parachute parachute = base.properties.CurrentState.parachute;
		Vector3 vector;
		vector..ctor((this.count != 1) ? this.spawnRoot1.position.x : this.spawnRoot2.position.x, this.shotYPos[this.count]);
		float delay = (this.count != 0) ? ((this.count != 1) ? parachute.shotCReturnDelay.RandomFloat() : parachute.shotBReturnDelay.RandomFloat()) : parachute.shotAReturnDelay.RandomFloat();
		if (this.pinkString.PopLetter() == 'P')
		{
			this.boomerangPink.Create(vector, parachute.speedForward, parachute.easeDistanceForward, parachute.speedReturn, parachute.easeDistanceReturn, delay, base.transform.localScale.x > 0f, 1);
		}
		else
		{
			this.boomerang.Create(vector, parachute.speedForward, parachute.easeDistanceForward, parachute.speedReturn, parachute.easeDistanceReturn, delay, base.transform.localScale.x > 0f, 1);
		}
		this.count++;
		AudioManager.Play("sfx_dlc_dogfight_p1_bulldog_bicepflex");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_p1_bulldog_bicepflex");
		AudioManager.Play("sfx_dlc_dogfight_dogflexhugovocal");
		this.emitAudioFromObject.Add("sfx_dlc_dogfight_dogflexhugovocal");
	}

	// Token: 0x06000DC9 RID: 3529 RVA: 0x0000BC93 File Offset: 0x00009E93
	public void AniEvent_SFX_BulldogPlane_ParachuteEnd()
	{
		AudioManager.Play("sfx_DLC_Dogfight_P1_Bulldog_SpringsUp");
	}

	// Token: 0x06000DCA RID: 3530 RVA: 0x00087FB4 File Offset: 0x000861B4
	public void WORKAROUND_NullifyFields()
	{
		this.shotYPos = null;
		this.spawnRoot1 = null;
		this.spawnRoot2 = null;
		this.boomerang = null;
		this.boomerangPink = null;
		this.pinkString = null;
		this.damageDealer = null;
		this.main = null;
		this.mainAnimator = null;
		this.collider = null;
	}

	// Token: 0x04000AC1 RID: 2753
	public float[] shotYPos = new float[]
	{
		100f,
		-50f,
		-200f
	};

	// Token: 0x04000AC2 RID: 2754
	[SerializeField]
	public Transform spawnRoot1;

	// Token: 0x04000AC3 RID: 2755
	[SerializeField]
	public Transform spawnRoot2;

	// Token: 0x04000AC4 RID: 2756
	[SerializeField]
	public AirplaneLevelBoomerang boomerang;

	// Token: 0x04000AC5 RID: 2757
	[SerializeField]
	public AirplaneLevelBoomerang boomerangPink;

	// Token: 0x04000AC6 RID: 2758
	public PatternString pinkString;

	// Token: 0x04000AC7 RID: 2759
	public DamageDealer damageDealer;

	// Token: 0x04000AC8 RID: 2760
	[SerializeField]
	public AirplaneLevelBulldogPlane main;

	// Token: 0x04000AC9 RID: 2761
	[SerializeField]
	public Animator mainAnimator;

	// Token: 0x04000ACA RID: 2762
	public int count;

	// Token: 0x04000ACB RID: 2763
	[SerializeField]
	public GameObject collider;
}
