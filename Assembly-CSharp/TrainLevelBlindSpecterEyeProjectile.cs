using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003A8 RID: 936
public class TrainLevelBlindSpecterEyeProjectile : AbstractProjectile
{
	// Token: 0x06002980 RID: 10624 RVA: 0x000D22CC File Offset: 0x000D04CC
	public TrainLevelBlindSpecterEyeProjectile Create(Vector2 pos, Vector2 time, float y, bool flipped, float health)
	{
		TrainLevelBlindSpecterEyeProjectile trainLevelBlindSpecterEyeProjectile = base.Create() as TrainLevelBlindSpecterEyeProjectile;
		trainLevelBlindSpecterEyeProjectile.transform.position = pos;
		trainLevelBlindSpecterEyeProjectile.time = time;
		trainLevelBlindSpecterEyeProjectile.end = y;
		trainLevelBlindSpecterEyeProjectile.health = health;
		if (flipped)
		{
			trainLevelBlindSpecterEyeProjectile.sprite.transform.SetScale(new float?(-1f), null, null);
		}
		return trainLevelBlindSpecterEyeProjectile;
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x000D2340 File Offset: 0x000D0540
	public override void Start()
	{
		base.Start();
		this.startPos = base.transform.position.y;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.x_cr());
		base.StartCoroutine(this.y_cr());
		TrainLevel trainLevel = Level.Current as TrainLevel;
		if (trainLevel != null)
		{
			this.handCarCollider = trainLevel.handCarCollider;
		}
	}

	// Token: 0x06002982 RID: 10626 RVA: 0x00022E09 File Offset: 0x00021009
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002983 RID: 10627 RVA: 0x00022E32 File Offset: 0x00021032
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002984 RID: 10628 RVA: 0x00022E5D File Offset: 0x0002105D
	public void End()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002985 RID: 10629 RVA: 0x00022E70 File Offset: 0x00021070
	public override void Die()
	{
		if (!base.GetComponent<Collider2D>().enabled)
		{
			return;
		}
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x06002986 RID: 10630 RVA: 0x000D23CC File Offset: 0x000D05CC
	public override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (phase == CollisionPhase.Enter && hit.GetComponent<TrainLevelPlatform>() != null)
		{
			this.start = hit.transform.position.y + 20f;
			this.t = 1000f;
		}
	}

	// Token: 0x06002987 RID: 10631 RVA: 0x000D2424 File Offset: 0x000D0624
	public IEnumerator x_cr()
	{
		float start = base.transform.position.x;
		float t = 0f;
		while (t < this.time.x)
		{
			float val = t / this.time.x;
			float x = Mathf.Lerp(start, -740f, val);
			base.transform.SetPosition(new float?(x), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.End();
		yield break;
	}

	// Token: 0x06002988 RID: 10632 RVA: 0x000D2440 File Offset: 0x000D0640
	public IEnumerator y_cr()
	{
		int counter = 0;
		int maxCounter = 2;
		float frameTime = 0f;
		YieldInstruction wait = new WaitForFixedUpdate();
		this.start = base.transform.position.y;
		for (;;)
		{
			AudioManager.Play("train_blindspector_eye_bounce");
			this.emitAudioFromObject.Add("train_blindspector_eye_bounce");
			this.t = 0f;
			float newY = this.start;
			if (this.handCarCollider != null)
			{
				Physics2D.IgnoreCollision(this.handCarCollider, this.eyeCollider, true);
			}
			while (this.t < this.time.y)
			{
				float val = this.t / this.time.y;
				newY = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, this.start, this.end, val);
				base.transform.SetPosition(null, new float?(newY), null);
				this.t += CupheadTime.FixedDelta;
				yield return wait;
			}
			base.transform.SetPosition(null, new float?(this.end), null);
			this.start = this.startPos;
			yield return null;
			if (this.handCarCollider != null)
			{
				Physics2D.IgnoreCollision(this.handCarCollider, this.eyeCollider, false);
			}
			this.t = 0f;
			while (this.t < this.time.y)
			{
				float val2 = this.t / this.time.y;
				newY = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, this.end, this.start, val2);
				base.transform.SetPosition(null, new float?(newY), null);
				this.t += CupheadTime.FixedDelta;
				yield return wait;
			}
			base.transform.SetPosition(null, new float?(this.start), null);
			this.effectPrefab.Create(base.transform.position);
			while (counter < maxCounter)
			{
				frameTime += CupheadTime.FixedDelta;
				if (frameTime > 0.0416666679f)
				{
					counter++;
					frameTime -= 0.0416666679f;
					if (counter >= 2)
					{
						base.transform.SetScale(null, new float?(0.3f), null);
						break;
					}
					base.transform.SetScale(null, new float?(0.5f), null);
				}
				yield return wait;
			}
			counter = 0;
			base.transform.SetScale(null, new float?(1f), null);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002989 RID: 10633 RVA: 0x00022E8F File Offset: 0x0002108F
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x040022BD RID: 8893
	public const float FRAME_TIME = 0.0416666679f;

	// Token: 0x040022BE RID: 8894
	[SerializeField]
	public Effect effectPrefab;

	// Token: 0x040022BF RID: 8895
	[SerializeField]
	public Transform sprite;

	// Token: 0x040022C0 RID: 8896
	[SerializeField]
	public Collider2D eyeCollider;

	// Token: 0x040022C1 RID: 8897
	public DamageReceiver damageReceiver;

	// Token: 0x040022C2 RID: 8898
	public float health;

	// Token: 0x040022C3 RID: 8899
	public float startPos;

	// Token: 0x040022C4 RID: 8900
	public float t;

	// Token: 0x040022C5 RID: 8901
	public float start;

	// Token: 0x040022C6 RID: 8902
	public float end;

	// Token: 0x040022C7 RID: 8903
	public Vector2 time;

	// Token: 0x040022C8 RID: 8904
	public Collider2D handCarCollider;
}
