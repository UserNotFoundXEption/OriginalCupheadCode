using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003FC RID: 1020
public class TreePlatformingLevelMosquito : AbstractCollidableObject
{
	// Token: 0x17000356 RID: 854
	// (get) Token: 0x06002CA9 RID: 11433 RVA: 0x000254D7 File Offset: 0x000236D7
	// (set) Token: 0x06002CAA RID: 11434 RVA: 0x000254DF File Offset: 0x000236DF
	public bool isActive { get; set; }

	// Token: 0x06002CAB RID: 11435 RVA: 0x000DAC10 File Offset: 0x000D8E10
	public void Start()
	{
		this.startPos = base.transform.position;
		AudioManager.PlayLoop("level_platform_mosquito_loop");
		this.emitAudioFromObject.Add("level_platform_mosquito_loop");
		this.YPositionDown = this.YPositionUp - 30f;
		this.YFall = this.YPositionUp - 35f;
		this.endPos = base.transform.position;
		this.endPos.y = this.YPositionDown;
		base.StartCoroutine(this.delay_start_cr(Random.Range(0f, 3f)));
	}

	// Token: 0x06002CAC RID: 11436 RVA: 0x000DACAC File Offset: 0x000D8EAC
	public IEnumerator delay_start_cr(float delay)
	{
		yield return new WaitForSeconds(delay);
		base.StartCoroutine(this.activate_cr());
		yield break;
	}

	// Token: 0x06002CAD RID: 11437 RVA: 0x000254E8 File Offset: 0x000236E8
	public void SetLetters(int one, int two)
	{
		base.animator.SetInteger("FirstLetter", one);
		base.animator.SetInteger("SecondLetter", two);
	}

	// Token: 0x06002CAE RID: 11438 RVA: 0x000DACD0 File Offset: 0x000D8ED0
	public IEnumerator check_platform_cr()
	{
		for (;;)
		{
			while (this.platform.transform.childCount <= 0)
			{
				yield return null;
			}
			this.StopMoveCoroutines();
			base.StartCoroutine(this.fall_cr());
			base.animator.SetBool("Struggling", true);
			AudioManager.Play("level_platform_mosquito_step_on");
			this.emitAudioFromObject.Add("level_platform_mosquito_step_on");
			AudioManager.Stop("level_platform_mosquito_loop");
			AudioManager.PlayLoop("level_platform_mosquito_struggle_loop");
			this.emitAudioFromObject.Add("level_platform_mosquito_struggle_loop");
			if (!this.projectileShooting)
			{
				base.StartCoroutine(this.shoot_up_cr());
			}
			while (this.platform.transform.childCount > 0)
			{
				yield return null;
			}
			this.StopMoveCoroutines();
			this.StartUp();
			base.animator.SetBool("Struggling", false);
			AudioManager.Stop("level_platform_mosquito_struggle_loop");
			AudioManager.PlayLoop("level_platform_mosquito_loop");
			this.emitAudioFromObject.Add("level_platform_mosquito_loop");
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CAF RID: 11439 RVA: 0x0002550C File Offset: 0x0002370C
	public override void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemyProjectile(hit, phase);
		if (hit.GetComponent<TreePlatformingLevelDragonflyProjectile>())
		{
			this.KillPlatform();
		}
	}

	// Token: 0x06002CB0 RID: 11440 RVA: 0x000DACEC File Offset: 0x000D8EEC
	public IEnumerator activate_cr()
	{
		base.animator.Play("Pick_Type");
		base.transform.position = new Vector3(this.startPos.x, 1200f);
		float t = 0f;
		base.GetComponent<Collider2D>().enabled = true;
		this.platform.gameObject.SetActive(true);
		while (t < this.returnTime)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / this.returnTime);
			base.transform.position = Vector2.Lerp(base.transform.position, this.startPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.isActive = true;
		base.transform.position = this.startPos;
		this.StartDown();
		base.StartCoroutine(this.check_platform_cr());
		switch (this.type)
		{
		case TreePlatformingLevelMosquito.Type.AA:
			this.SetLetters(1, 1);
			break;
		case TreePlatformingLevelMosquito.Type.AB:
			this.SetLetters(1, 2);
			break;
		case TreePlatformingLevelMosquito.Type.AC:
			this.SetLetters(1, 3);
			break;
		case TreePlatformingLevelMosquito.Type.BA:
			this.SetLetters(2, 1);
			break;
		case TreePlatformingLevelMosquito.Type.BB:
			this.SetLetters(2, 2);
			break;
		case TreePlatformingLevelMosquito.Type.BC:
			this.SetLetters(2, 3);
			break;
		case TreePlatformingLevelMosquito.Type.CA:
			this.SetLetters(3, 1);
			break;
		case TreePlatformingLevelMosquito.Type.CB:
			this.SetLetters(3, 2);
			break;
		case TreePlatformingLevelMosquito.Type.CC:
			this.SetLetters(3, 3);
			break;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002CB1 RID: 11441 RVA: 0x000DAD08 File Offset: 0x000D8F08
	public void KillPlatform()
	{
		if (this.explosion != null)
		{
			this.explosion.Create(base.transform.position, new Vector3(0.85f, 0.85f, 0.85f));
		}
		this.platform.transform.DetachChildren();
		this.platform.gameObject.SetActive(false);
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetBool("Struggling", false);
		AudioManager.Stop("level_platform_mosquito_loop");
		AudioManager.Stop("level_platform_mosquito_struggle_loop");
		AudioManager.Play("level_platform_mosquito_death");
		this.emitAudioFromObject.Add("level_platform_mosquito_death");
		this.isActive = false;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06002CB2 RID: 11442 RVA: 0x000DADD8 File Offset: 0x000D8FD8
	public IEnumerator die_cr()
	{
		base.animator.SetTrigger("Death");
		float velocity = 0f;
		float gravity = 2250f;
		while (base.transform.position.y > -CupheadLevelCamera.Current.Height - 200f)
		{
			base.transform.AddPosition(0f, velocity * CupheadTime.Delta, 0f);
			velocity -= gravity * CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.reappearDelay);
		this.projectileShooting = false;
		base.StartCoroutine(this.activate_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002CB3 RID: 11443 RVA: 0x000DADF4 File Offset: 0x000D8FF4
	public IEnumerator sine_cr()
	{
		float time = Random.Range(1f, 1.5f);
		float t = Random.Range(0f, 0.5f);
		float val = 0.5f;
		for (;;)
		{
			if (CupheadTime.Delta != 0f)
			{
				t += CupheadTime.Delta;
				float num = Mathf.Sin(t / time);
				base.transform.AddPosition(0f, num * val, 0f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CB4 RID: 11444 RVA: 0x000DAE10 File Offset: 0x000D9010
	public IEnumerator shoot_up_cr()
	{
		this.projectileShooting = true;
		yield return CupheadTime.WaitForSeconds(this, this.projectileShootUpTime);
		if (this.projectile != null)
		{
			this.projectile.Create(new Vector2(base.transform.position.x, base.transform.position.y - 500f), 90f, this.projectileSpeed);
		}
		this.projectileShooting = false;
		yield return null;
		yield break;
	}

	// Token: 0x06002CB5 RID: 11445 RVA: 0x000DAE2C File Offset: 0x000D902C
	public void StopMoveCoroutines()
	{
		if (this.upCoroutine != null)
		{
			base.StopCoroutine(this.upCoroutine);
			this.upCoroutine = null;
		}
		if (this.downCoroutine != null)
		{
			base.StopCoroutine(this.downCoroutine);
			this.downCoroutine = null;
		}
		if (this.fallCoroutine != null)
		{
			base.StopCoroutine(this.fallCoroutine);
			this.fallCoroutine = null;
		}
		if (this.gotoCoroutine != null)
		{
			base.StopCoroutine(this.gotoCoroutine);
			this.gotoCoroutine = null;
		}
	}

	// Token: 0x06002CB6 RID: 11446 RVA: 0x0002552C File Offset: 0x0002372C
	public void StartDown()
	{
		this.StopMoveCoroutines();
		this.downCoroutine = base.StartCoroutine(this.down_cr());
	}

	// Token: 0x06002CB7 RID: 11447 RVA: 0x00025546 File Offset: 0x00023746
	public void StartUp()
	{
		this.StopMoveCoroutines();
		this.upCoroutine = base.StartCoroutine(this.up_cr());
	}

	// Token: 0x06002CB8 RID: 11448 RVA: 0x000DAEB4 File Offset: 0x000D90B4
	public IEnumerator down_cr()
	{
		yield return new WaitForSeconds(0f);
		this.gotoCoroutine = base.StartCoroutine(this.goTo_cr(this.YPositionUp, this.YPositionDown, 1.5f, EaseUtils.EaseType.easeInOutSine));
		yield return this.gotoCoroutine;
		this.StartUp();
		yield break;
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x000DAED0 File Offset: 0x000D90D0
	public IEnumerator up_cr()
	{
		yield return new WaitForSeconds(0f);
		this.gotoCoroutine = base.StartCoroutine(this.goTo_cr(this.YPositionDown, this.YPositionUp, 1.5f, EaseUtils.EaseType.easeInOutSine));
		yield return this.gotoCoroutine;
		this.StartDown();
		yield break;
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x000DAEEC File Offset: 0x000D90EC
	public IEnumerator fall_cr()
	{
		float time = (1f - (base.transform.position.y - this.startPos.y) / this.YFall) * 0.13f;
		this.gotoCoroutine = base.StartCoroutine(this.goTo_cr(base.transform.position.y - this.startPos.y, this.YFall, time, EaseUtils.EaseType.easeOutSine));
		yield return this.gotoCoroutine;
		this.gotoCoroutine = base.StartCoroutine(this.goTo_cr(this.YFall, this.YPositionDown, 0.12f, EaseUtils.EaseType.easeInOutSine));
		yield return this.gotoCoroutine;
		yield break;
	}

	// Token: 0x06002CBB RID: 11451 RVA: 0x000DAF08 File Offset: 0x000D9108
	public IEnumerator goTo_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		float t = 0f;
		base.transform.SetPosition(null, new float?(this.startPos.y + start), null);
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(null, new float?(this.startPos.y + EaseUtils.Ease(ease, start, end, val)), null);
			t += Time.deltaTime;
			yield return base.StartCoroutine(base.WaitForPause_CR());
		}
		base.transform.SetPosition(null, new float?(this.startPos.y + end), null);
		yield break;
	}

	// Token: 0x040024D5 RID: 9429
	[Header("Projectile Variables")]
	[SerializeField]
	public BasicProjectile projectile;

	// Token: 0x040024D6 RID: 9430
	[SerializeField]
	public float projectileSpeed;

	// Token: 0x040024D7 RID: 9431
	[SerializeField]
	public bool projectileShootsUP;

	// Token: 0x040024D8 RID: 9432
	[SerializeField]
	public float projectileShootUpTime;

	// Token: 0x040024D9 RID: 9433
	[Space(10f)]
	[SerializeField]
	public LevelPlatform platform;

	// Token: 0x040024DA RID: 9434
	[SerializeField]
	public float reappearDelay = 1f;

	// Token: 0x040024DB RID: 9435
	[SerializeField]
	public PlatformingLevelGenericExplosion explosion;

	// Token: 0x040024DC RID: 9436
	public float returnTime = 1.5f;

	// Token: 0x040024DE RID: 9438
	public bool projectileShooting;

	// Token: 0x040024DF RID: 9439
	public TreePlatformingLevelMosquito.Type type;

	// Token: 0x040024E0 RID: 9440
	public float YPositionUp;

	// Token: 0x040024E1 RID: 9441
	public const float TIME = 1.5f;

	// Token: 0x040024E2 RID: 9442
	public const float FALL_TIME = 0.13f;

	// Token: 0x040024E3 RID: 9443
	public const float FALL_BOUNCE_TIME = 0.12f;

	// Token: 0x040024E4 RID: 9444
	public const float DELAY = 0f;

	// Token: 0x040024E5 RID: 9445
	public const EaseUtils.EaseType FLOAT_EASE = EaseUtils.EaseType.easeInOutSine;

	// Token: 0x040024E6 RID: 9446
	public const EaseUtils.EaseType FALL_EASE = EaseUtils.EaseType.easeOutSine;

	// Token: 0x040024E7 RID: 9447
	public const EaseUtils.EaseType FALL_BOUNCE_EASE = EaseUtils.EaseType.easeInOutSine;

	// Token: 0x040024E8 RID: 9448
	[SerializeField]
	public TreePlatformingLevelMosquito.State state;

	// Token: 0x040024E9 RID: 9449
	public Vector3 startPos;

	// Token: 0x040024EA RID: 9450
	public Vector3 endPos;

	// Token: 0x040024EB RID: 9451
	public float YPositionDown;

	// Token: 0x040024EC RID: 9452
	public float YFall;

	// Token: 0x040024ED RID: 9453
	public Coroutine upCoroutine;

	// Token: 0x040024EE RID: 9454
	public Coroutine downCoroutine;

	// Token: 0x040024EF RID: 9455
	public Coroutine fallCoroutine;

	// Token: 0x040024F0 RID: 9456
	public Coroutine gotoCoroutine;

	// Token: 0x0200103A RID: 4154
	public enum Type
	{
		// Token: 0x040073C0 RID: 29632
		AA,
		// Token: 0x040073C1 RID: 29633
		AB,
		// Token: 0x040073C2 RID: 29634
		AC,
		// Token: 0x040073C3 RID: 29635
		BA,
		// Token: 0x040073C4 RID: 29636
		BB,
		// Token: 0x040073C5 RID: 29637
		BC,
		// Token: 0x040073C6 RID: 29638
		CA,
		// Token: 0x040073C7 RID: 29639
		CB,
		// Token: 0x040073C8 RID: 29640
		CC
	}

	// Token: 0x0200103B RID: 4155
	public enum State
	{
		// Token: 0x040073CA RID: 29642
		Up,
		// Token: 0x040073CB RID: 29643
		Down,
		// Token: 0x040073CC RID: 29644
		PlayerOn
	}
}
