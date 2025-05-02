using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000387 RID: 903
public class SlimeLevelTinySlime : AbstractCollidableObject
{
	// Token: 0x060027EE RID: 10222 RVA: 0x000CD018 File Offset: 0x000CB218
	public void Init(Vector3 pos, LevelProperties.Slime.Tombstone properties, bool goingRight, SlimeLevelTombstone parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.goingRight = goingRight;
		this.parent = parent;
		SlimeLevelTombstone slimeLevelTombstone = this.parent;
		slimeLevelTombstone.onDeath = (Action)Delegate.Combine(slimeLevelTombstone.onDeath, new Action(this.Death));
		if (goingRight)
		{
			base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		}
		else
		{
			base.transform.SetScale(new float?(base.transform.localScale.x), new float?(1f), new float?(1f));
		}
	}

	// Token: 0x060027EF RID: 10223 RVA: 0x000CD0E8 File Offset: 0x000CB2E8
	public void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.Health = this.properties.tinyHealth;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060027F0 RID: 10224 RVA: 0x000217AC File Offset: 0x0001F9AC
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060027F1 RID: 10225 RVA: 0x000217C4 File Offset: 0x0001F9C4
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060027F2 RID: 10226 RVA: 0x000217E2 File Offset: 0x0001F9E2
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f && !this.melted)
		{
			this.Melt();
		}
	}

	// Token: 0x060027F3 RID: 10227 RVA: 0x00021818 File Offset: 0x0001FA18
	public void Melt()
	{
		this.melted = true;
		base.StartCoroutine(this.melt_cr());
	}

	// Token: 0x060027F4 RID: 10228 RVA: 0x000CD144 File Offset: 0x000CB344
	public IEnumerator melt_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		AudioManager.Stop("level_blobrunner");
		AudioManager.Play("level_frogs_tall_firefly_death");
		base.animator.Play("Melt");
		yield return base.animator.WaitForAnimationToEnd(this, "Melt", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.tinyMeltDelay);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("level_blobrunner_reform");
		this.emitAudioFromObject.Add("level_blobrunner_reform");
		yield return CupheadTime.WaitForSeconds(this, this.properties.tinyTimeUntilUnmelt);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Unmelt", false, true);
		this.melted = false;
		this.Health = this.properties.tinyHealth;
		base.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x060027F5 RID: 10229 RVA: 0x000CD160 File Offset: 0x000CB360
	public IEnumerator move_cr()
	{
		float sizeX = this.sprite.bounds.size.x;
		float sizeY = this.sprite.bounds.size.y;
		float left = -640f + sizeX / 2f;
		float right = 640f - sizeX / 2f;
		float down = (float)Level.Current.Ground + sizeY / 3f;
		float t = 0f;
		float time = this.properties.tinyRunTime;
		EaseUtils.EaseType ease = EaseUtils.EaseType.linear;
		float speed = 600f;
		float acceleration = 10f;
		Vector3 endPos = Vector3.zero;
		endPos = ((!this.goingRight) ? new Vector3(left, down) : new Vector3(right, down));
		while (base.transform.position != endPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, endPos, speed * CupheadTime.Delta);
			speed += acceleration;
			yield return null;
		}
		float start = 0f;
		float end = 0f;
		this.sprite.sortingLayerName = SpriteLayer.Projectiles.ToString();
		if (this.goingRight)
		{
			start = base.transform.position.x;
			end = right;
		}
		else
		{
			start = base.transform.position.x;
			end = left;
		}
		time = 0f;
		for (;;)
		{
			t = 0f;
			while (t < time)
			{
				if (!this.melted)
				{
					float value = t / time;
					base.transform.SetPosition(new float?(EaseUtils.Ease(ease, start, end, value)), null, null);
					t += CupheadTime.Delta;
				}
				yield return null;
			}
			base.animator.Play("Turn");
			yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
			base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
			this.goingRight = !this.goingRight;
			if (this.goingRight)
			{
				start = left;
				end = right;
			}
			else
			{
				start = right;
				end = left;
			}
			time = this.properties.tinyRunTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060027F6 RID: 10230 RVA: 0x0002182E File Offset: 0x0001FA2E
	public void Death()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Melt");
	}

	// Token: 0x060027F7 RID: 10231 RVA: 0x00021852 File Offset: 0x0001FA52
	public override void OnDestroy()
	{
		SlimeLevelTombstone slimeLevelTombstone = this.parent;
		slimeLevelTombstone.onDeath = (Action)Delegate.Remove(slimeLevelTombstone.onDeath, new Action(this.Death));
		base.OnDestroy();
	}

	// Token: 0x04002110 RID: 8464
	[SerializeField]
	public SpriteRenderer sprite;

	// Token: 0x04002111 RID: 8465
	public LevelProperties.Slime.Tombstone properties;

	// Token: 0x04002112 RID: 8466
	public SlimeLevelTombstone parent;

	// Token: 0x04002113 RID: 8467
	public DamageDealer damageDealer;

	// Token: 0x04002114 RID: 8468
	public DamageReceiver damageReceiver;

	// Token: 0x04002115 RID: 8469
	public bool goingRight;

	// Token: 0x04002116 RID: 8470
	public bool melted;

	// Token: 0x04002117 RID: 8471
	public float Health;
}
