using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002F6 RID: 758
public class PirateLevelShark : LevelProperties.Pirate.Entity
{
	// Token: 0x170002E6 RID: 742
	// (get) Token: 0x060021AF RID: 8623 RVA: 0x0001CCED File Offset: 0x0001AEED
	// (set) Token: 0x060021B0 RID: 8624 RVA: 0x0001CCF5 File Offset: 0x0001AEF5
	public PirateLevelShark.State state { get; set; }

	// Token: 0x060021B1 RID: 8625 RVA: 0x000BB17C File Offset: 0x000B937C
	public override void Awake()
	{
		base.Awake();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.shark.SetActive(false);
		this.shark.transform.SetLocalPosition(new float?(-950f), null, null);
		this.splash.SetActive(false);
	}

	// Token: 0x060021B2 RID: 8626 RVA: 0x000BB1EC File Offset: 0x000B93EC
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state != PirateLevelShark.State.Exit && this.state != PirateLevelShark.State.Exit_Shot)
		{
			return;
		}
		if (this.shotCoroutine != null)
		{
			base.StopCoroutine(this.shotCoroutine);
		}
		this.shotCoroutine = this.shot_cr();
		base.StartCoroutine(this.shotCoroutine);
	}

	// Token: 0x060021B3 RID: 8627 RVA: 0x0001CCFE File Offset: 0x0001AEFE
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060021B4 RID: 8628 RVA: 0x000BB244 File Offset: 0x000B9444
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.sharkProperties = (propertyGroup as LevelProperties.Pirate.Shark);
		base.StartCoroutine(this.shark_cr());
		base.StartCoroutine(this.collider_cr());
		this.state = PirateLevelShark.State.Swim;
		this.damageDealer = new DamageDealer(1f, 1f);
		this.damageDealer.SetDirection(DamageDealer.Direction.Right, base.transform);
		Vector3 position = base.transform.position;
		position.x = this.sharkProperties.x;
		base.transform.position = position;
	}

	// Token: 0x060021B5 RID: 8629 RVA: 0x0001CD1C File Offset: 0x0001AF1C
	public void OnBiteAnimComplete()
	{
		this.state = PirateLevelShark.State.Exit;
		base.StartCoroutine(this.exit_cr());
	}

	// Token: 0x060021B6 RID: 8630 RVA: 0x0001CD32 File Offset: 0x0001AF32
	public void OnBiteAudio()
	{
	}

	// Token: 0x060021B7 RID: 8631 RVA: 0x0001CD34 File Offset: 0x0001AF34
	public void OnBiteShake()
	{
		CupheadLevelCamera.Current.Shake(12f, 0.5f, false);
	}

	// Token: 0x060021B8 RID: 8632 RVA: 0x0001CD4B File Offset: 0x0001AF4B
	public void Splash()
	{
		this.splash.SetActive(true);
		base.animator.Play("Splash", 3);
	}

	// Token: 0x060021B9 RID: 8633 RVA: 0x0001CD6A File Offset: 0x0001AF6A
	public void End()
	{
		AudioManager.Stop("level_pirate_shark_exit_normal_loop");
		this.state = PirateLevelShark.State.Complete;
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x060021BA RID: 8634 RVA: 0x000BB2D8 File Offset: 0x000B94D8
	public IEnumerator shot_cr()
	{
		this.state = PirateLevelShark.State.Exit_Shot;
		base.animator.SetLayerWeight(1, 1f);
		float t = 0f;
		while (t < 1f)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetLayerWeight(1, 0f);
		this.state = PirateLevelShark.State.Exit;
		yield break;
	}

	// Token: 0x060021BB RID: 8635 RVA: 0x000BB2F4 File Offset: 0x000B94F4
	public IEnumerator shark_cr()
	{
		AudioManager.Play("level_pirate_shark_warning");
		yield return base.StartCoroutine(this.fin_cr());
		this.shark.SetActive(true);
		this.state = PirateLevelShark.State.Attack;
		base.animator.Play("Attack");
		AudioManager.Play("levels_pirate_shark_attack");
		this.emitAudioFromObject.Add("levels_pirate_shark_attack");
		yield break;
	}

	// Token: 0x060021BC RID: 8636 RVA: 0x000BB310 File Offset: 0x000B9510
	public IEnumerator exit_cr()
	{
		base.animator.Play("Exit");
		AudioManager.PlayLoop("level_pirate_shark_exit_normal_loop");
		this.emitAudioFromObject.Add("level_pirate_shark_exit_normal_loop");
		base.animator.Play("Exit", 1);
		for (;;)
		{
			if (this.shark.transform.position.x < -950f)
			{
				this.End();
			}
			float speed = ((this.state != PirateLevelShark.State.Exit) ? this.sharkProperties.shotExitSpeed : this.sharkProperties.exitSpeed) * CupheadTime.Delta;
			base.transform.AddPosition(-speed, 0f, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060021BD RID: 8637 RVA: 0x000BB32C File Offset: 0x000B952C
	public IEnumerator fin_cr()
	{
		float t = 0f;
		float time = this.sharkProperties.finTime;
		int startX = 640;
		int endX = -740;
		while (t < time)
		{
			float val = t / time;
			float x = Mathf.Lerp((float)startX, (float)endX, val);
			this.fin.transform.SetPosition(new float?(x), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.fin.transform.SetPosition(new float?((float)endX), null, null);
		yield return CupheadTime.WaitForSeconds(this, this.sharkProperties.attackDelay);
		yield break;
	}

	// Token: 0x060021BE RID: 8638 RVA: 0x000BB348 File Offset: 0x000B9548
	public IEnumerator collider_cr()
	{
		BoxCollider2D collider = base.GetComponent<Collider2D>() as BoxCollider2D;
		BoxCollider2D childCollider = this.shark.GetComponent<Collider2D>() as BoxCollider2D;
		for (;;)
		{
			collider.offset = this.shark.transform.localPosition + childCollider.offset;
			collider.size = childCollider.size;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001BC2 RID: 7106
	public const float SHOT_DELAY = 1f;

	// Token: 0x04001BC3 RID: 7107
	public const float START_X = -950f;

	// Token: 0x04001BC5 RID: 7109
	[SerializeField]
	public GameObject fin;

	// Token: 0x04001BC6 RID: 7110
	[SerializeField]
	public GameObject shark;

	// Token: 0x04001BC7 RID: 7111
	[SerializeField]
	public GameObject splash;

	// Token: 0x04001BC8 RID: 7112
	public LevelProperties.Pirate.Shark sharkProperties;

	// Token: 0x04001BC9 RID: 7113
	public DamageDealer damageDealer;

	// Token: 0x04001BCA RID: 7114
	public IEnumerator shotCoroutine;

	// Token: 0x02000E12 RID: 3602
	public enum State
	{
		// Token: 0x040065E2 RID: 26082
		Init,
		// Token: 0x040065E3 RID: 26083
		Swim,
		// Token: 0x040065E4 RID: 26084
		Attack,
		// Token: 0x040065E5 RID: 26085
		Exit,
		// Token: 0x040065E6 RID: 26086
		Exit_Shot,
		// Token: 0x040065E7 RID: 26087
		Complete
	}
}
