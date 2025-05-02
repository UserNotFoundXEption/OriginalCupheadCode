using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200027F RID: 639
public class FlyingMermaidLevelEel : AbstractCollidableObject
{
	// Token: 0x170002B4 RID: 692
	// (get) Token: 0x06001D0E RID: 7438 RVA: 0x000189FF File Offset: 0x00016BFF
	// (set) Token: 0x06001D0F RID: 7439 RVA: 0x00018A07 File Offset: 0x00016C07
	public FlyingMermaidLevelEel.State state { get; set; }

	// Token: 0x06001D10 RID: 7440 RVA: 0x00018A10 File Offset: 0x00016C10
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001D11 RID: 7441 RVA: 0x00018A46 File Offset: 0x00016C46
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001D12 RID: 7442 RVA: 0x00018A5E File Offset: 0x00016C5E
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state == FlyingMermaidLevelEel.State.Spawned)
		{
			this.Die(true, false);
		}
	}

	// Token: 0x06001D13 RID: 7443 RVA: 0x000AFDA8 File Offset: 0x000ADFA8
	public void Die(bool explode, bool permanent)
	{
		Collider2D component = base.GetComponent<Collider2D>();
		if (!component.enabled)
		{
			return;
		}
		this.StopAllCoroutines();
		component.enabled = false;
		base.animator.SetTrigger("Despawn");
		base.animator.ResetTrigger("Attack");
		base.animator.ResetTrigger("Continue");
		base.animator.ResetTrigger("Leave");
		float num = (float)((!(base.GetComponent<SpriteRenderer>().sortingLayerName == "Foreground")) ? -270 : -380);
		if (explode && this.state == FlyingMermaidLevelEel.State.Spawned)
		{
			SpriteRenderer component2 = base.GetComponent<SpriteRenderer>();
			for (int i = 0; i < this.numSegments; i++)
			{
				float floatAt = this.segmentY.GetFloatAt((float)i / ((float)this.numSegments - 1f));
				Vector2 position = base.transform.position;
				position.y += floatAt;
				if (position.y >= num + 30f)
				{
					FlyingMermaidLevelEelSegment flyingMermaidLevelEelSegment;
					if (i == this.numSegments - 1)
					{
						flyingMermaidLevelEelSegment = this.headSegmentPrefab;
					}
					else
					{
						flyingMermaidLevelEelSegment = this.bodySegmentPrefabs.RandomChoice<FlyingMermaidLevelEelSegment>();
					}
					string text = component2.sortingLayerName;
					int sortingOrder = component2.sortingOrder;
					int num2 = Random.Range(-1, 2);
					if (text == "Foreground")
					{
						if (num2 == -1)
						{
							text = "Enemies";
							sortingOrder = 1000;
						}
						else if (num2 == 1)
						{
							sortingOrder = 21;
						}
					}
					else if (num2 == -1)
					{
						text = "Background";
						sortingOrder = 75;
					}
					else if (num2 == 1)
					{
						text = "Foreground";
						sortingOrder = 1;
					}
					flyingMermaidLevelEelSegment.Create(position, text, sortingOrder);
				}
			}
		}
		if (!permanent)
		{
			base.StartCoroutine(this.main_cr());
		}
		this.state = FlyingMermaidLevelEel.State.Unspawned;
	}

	// Token: 0x06001D14 RID: 7444 RVA: 0x00018A97 File Offset: 0x00016C97
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001D15 RID: 7445 RVA: 0x000AFF9C File Offset: 0x000AE19C
	public void Init(LevelProperties.FlyingMermaid.Eel properties)
	{
		this.properties = properties;
		this.initialY = base.transform.localPosition.y;
		Collider2D component = base.GetComponent<Collider2D>();
		component.enabled = false;
		this.bulletPinkPattern = properties.bulletPinkString.Split(new char[]
		{
			','
		});
		this.bulletPinkIndex = Random.Range(0, this.bulletPinkPattern.Length);
	}

	// Token: 0x06001D16 RID: 7446 RVA: 0x00018AB5 File Offset: 0x00016CB5
	public void StartPattern()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06001D17 RID: 7447 RVA: 0x000B0008 File Offset: 0x000AE208
	public IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.appearDelay.RandomFloat());
		this.state = FlyingMermaidLevelEel.State.Spawned;
		base.animator.SetTrigger("Spawn");
		AudioManager.Play("level_mermaid_eel_intro");
		float t = 0f;
		this.hp = this.properties.hp;
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = true;
		while (t < this.riseTime - 0.25f)
		{
			t += CupheadTime.Delta;
			base.transform.SetLocalPosition(null, new float?(Mathf.Lerp(this.initialY - this.riseDistance, this.initialY, t / this.riseTime)), null);
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		while (t < this.riseTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetLocalPosition(null, new float?(Mathf.Lerp(this.initialY - this.riseDistance, this.initialY, t / this.riseTime)), null);
			yield return null;
		}
		base.transform.SetLocalPosition(null, new float?(this.initialY), null);
		yield return base.animator.WaitForAnimationToStart(this, "Idle", false);
		for (int numAttacks = this.properties.attackAmount.RandomInt(); numAttacks >= 0; numAttacks--)
		{
			yield return CupheadTime.WaitForSeconds(this, this.properties.idleTime.RandomFloat());
			AudioManager.Play("level_mermaid_eel_attack_start");
			base.animator.SetTrigger("Attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_Start", false, true);
			this.FireProjectiles();
			yield return base.animator.WaitForAnimationToEnd(this, "Attack_End", false, true);
			AudioManager.Play("level_mermaid_eel_attack_end");
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.idleTime.RandomFloat());
		base.animator.SetTrigger("Leave");
		yield return base.animator.WaitForAnimationToEnd(this, "Leave_Start", false, true);
		AudioManager.Play("level_mermaid_eel_attack_leave");
		t = 0f;
		bool spawnedSplash = false;
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		float waterY = (float)((!(sprite.sortingLayerName == "Foreground")) ? -270 : -380);
		while (t < this.leaveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetLocalPosition(null, new float?(Mathf.Lerp(this.initialY, this.initialY - this.riseDistance, t / this.leaveTime)), null);
			if (!spawnedSplash && base.transform.position.y < waterY - 80f)
			{
				FlyingMermaidLevelSplashManager.Instance.SpawnSplashMedium(base.gameObject, 35f, true, waterY + 80f);
				spawnedSplash = true;
			}
			yield return null;
		}
		this.Die(false, false);
		yield break;
	}

	// Token: 0x06001D18 RID: 7448 RVA: 0x000B0024 File Offset: 0x000AE224
	public void FireProjectiles()
	{
		int num = 0;
		while ((float)num < this.properties.numBullets)
		{
			float floatAt = this.properties.spreadAngle.GetFloatAt((float)num / (this.properties.numBullets - 1f));
			BasicProjectile basicProjectile = this.projectilePrefab.Create(this.projectileRoot.position, floatAt, this.properties.bulletSpeed);
			basicProjectile.SetParryable(this.bulletPinkPattern[this.bulletPinkIndex][0] == 'P');
			this.bulletPinkIndex = (this.bulletPinkIndex + 1) % this.bulletPinkPattern.Length;
			num++;
		}
	}

	// Token: 0x040017B2 RID: 6066
	[SerializeField]
	public float riseTime;

	// Token: 0x040017B3 RID: 6067
	[SerializeField]
	public float riseDistance;

	// Token: 0x040017B4 RID: 6068
	[SerializeField]
	public float leaveTime;

	// Token: 0x040017B5 RID: 6069
	[SerializeField]
	public MinMax segmentY;

	// Token: 0x040017B6 RID: 6070
	[SerializeField]
	public int numSegments;

	// Token: 0x040017B7 RID: 6071
	[SerializeField]
	public BasicProjectile projectilePrefab;

	// Token: 0x040017B8 RID: 6072
	[SerializeField]
	public Transform projectileRoot;

	// Token: 0x040017B9 RID: 6073
	[SerializeField]
	public FlyingMermaidLevelEelSegment headSegmentPrefab;

	// Token: 0x040017BA RID: 6074
	[SerializeField]
	public FlyingMermaidLevelEelSegment[] bodySegmentPrefabs;

	// Token: 0x040017BB RID: 6075
	public LevelProperties.FlyingMermaid.Eel properties;

	// Token: 0x040017BC RID: 6076
	public DamageDealer damageDealer;

	// Token: 0x040017BD RID: 6077
	public DamageReceiver damageReceiver;

	// Token: 0x040017BE RID: 6078
	public string[] bulletPinkPattern;

	// Token: 0x040017BF RID: 6079
	public int bulletPinkIndex;

	// Token: 0x040017C0 RID: 6080
	public float initialY;

	// Token: 0x040017C1 RID: 6081
	public float hp;

	// Token: 0x02000D28 RID: 3368
	public enum State
	{
		// Token: 0x04005F81 RID: 24449
		Unspawned,
		// Token: 0x04005F82 RID: 24450
		Spawned
	}
}
