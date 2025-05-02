using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000288 RID: 648
public class FlyingMermaidLevelMerdusa : LevelProperties.FlyingMermaid.Entity
{
	// Token: 0x170002B5 RID: 693
	// (get) Token: 0x06001D3E RID: 7486 RVA: 0x00018C84 File Offset: 0x00016E84
	// (set) Token: 0x06001D3F RID: 7487 RVA: 0x00018C8C File Offset: 0x00016E8C
	public FlyingMermaidLevelMerdusa.State state { get; set; }

	// Token: 0x06001D40 RID: 7488 RVA: 0x000B03B0 File Offset: 0x000AE5B0
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		CollisionChild collisionChild = this.blockingColliders.gameObject.AddComponent<CollisionChild>();
		collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06001D41 RID: 7489 RVA: 0x00018C95 File Offset: 0x00016E95
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001D42 RID: 7490 RVA: 0x00018CA8 File Offset: 0x00016EA8
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001D43 RID: 7491 RVA: 0x00018CC0 File Offset: 0x00016EC0
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001D44 RID: 7492 RVA: 0x000B0418 File Offset: 0x000AE618
	public override void LevelInit(LevelProperties.FlyingMermaid properties)
	{
		base.LevelInit(properties);
		foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
		{
			flyingMermaidLevelEel.Init(properties.CurrentState.eel);
		}
		properties.OnBossDeath += this.OnBossDeath;
	}

	// Token: 0x06001D45 RID: 7493 RVA: 0x000B0470 File Offset: 0x000AE670
	public void StartIntro(Vector2 pos)
	{
		AudioManager.Play("level_mermaid_merdusa_cackle");
		base.transform.position = pos;
		base.animator.SetTrigger("Continue");
		base.StartCoroutine(this.intro_cr());
		base.StartCoroutine(this.moveBack_cr());
	}

	// Token: 0x06001D46 RID: 7494 RVA: 0x000B04C4 File Offset: 0x000AE6C4
	public IEnumerator intro_cr()
	{
		this.StartEels();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.state = FlyingMermaidLevelMerdusa.State.Idle;
		yield break;
	}

	// Token: 0x06001D47 RID: 7495 RVA: 0x000B04E0 File Offset: 0x000AE6E0
	public IEnumerator moveBack_cr()
	{
		float startX = base.transform.position.x;
		float t = 0f;
		while (t < this.introMoveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetPosition(new float?(Mathf.Lerp(startX, startX + this.transformMoveX, t / this.introMoveTime)), null, null);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001D48 RID: 7496 RVA: 0x000B04FC File Offset: 0x000AE6FC
	public void BlinkMaybe()
	{
		this.blinks++;
		if (this.blinks >= this.maxBlinks)
		{
			this.blinks = 0;
			this.maxBlinks = Random.Range(2, 4);
			this.blinkOverlaySprite.enabled = true;
		}
		else
		{
			this.blinkOverlaySprite.enabled = false;
		}
	}

	// Token: 0x06001D49 RID: 7497 RVA: 0x000B055C File Offset: 0x000AE75C
	public void StartEels()
	{
		foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
		{
			flyingMermaidLevelEel.StartPattern();
		}
	}

	// Token: 0x06001D4A RID: 7498 RVA: 0x000B0590 File Offset: 0x000AE790
	public IEnumerator eels_cr()
	{
		foreach (FlyingMermaidLevelEel prefab in this.eels)
		{
			prefab.Spawn<FlyingMermaidLevelEel>();
		}
		bool allEelsGone = false;
		while (!allEelsGone)
		{
			allEelsGone = true;
			foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
			{
				if (flyingMermaidLevelEel.state == FlyingMermaidLevelEel.State.Spawned)
				{
					allEelsGone = false;
				}
			}
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.eel.hesitateAfterAttack);
		this.state = FlyingMermaidLevelMerdusa.State.Idle;
		yield break;
	}

	// Token: 0x06001D4B RID: 7499 RVA: 0x00018CDE File Offset: 0x00016EDE
	public void StartZap()
	{
		this.state = FlyingMermaidLevelMerdusa.State.Zap;
		base.StartCoroutine(this.zap_cr());
	}

	// Token: 0x06001D4C RID: 7500 RVA: 0x000B05AC File Offset: 0x000AE7AC
	public IEnumerator zap_cr()
	{
		AudioManager.Play("level_mermaid_merdusa_zap_loop_start");
		base.animator.SetTrigger("Zap");
		yield return base.animator.WaitForAnimationToEnd(this, "Zap_Start", false, true);
		this.laser.SetStoneTime(base.properties.CurrentState.zap.stoneTime);
		this.laser.animator.SetTrigger("Start");
		this.laser.transform.SetParent(null);
		AudioManager.PlayLoop("level_mermaid_merdusa_zap_loop");
		this.laser.StartLaser();
		yield return this.laser.animator.WaitForAnimationToEnd(this, "Lightning_Start", false, true);
		this.laser.animator.SetTrigger("End");
		AudioManager.Stop("level_mermaid_merdusa_zap_loop");
		AudioManager.Play("level_mermaid_merdusa_zap_loop_end");
		this.laser.StopLaser();
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Zap_End", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.zap.hesitateAfterAttack.RandomFloat());
		this.state = FlyingMermaidLevelMerdusa.State.Idle;
		yield break;
	}

	// Token: 0x06001D4D RID: 7501 RVA: 0x000B05C8 File Offset: 0x000AE7C8
	public void StartTransform()
	{
		if (this.state == FlyingMermaidLevelMerdusa.State.Zap)
		{
			AudioManager.Play("level_mermaid_merdusa_zap_loop_end");
			this.laser.StopLaser();
		}
		AudioManager.Stop("level_mermaid_merdusa_zap_loop");
		this.head.StartIntro(this.headRoot.position);
		base.properties.OnBossDeath -= this.OnBossDeath;
		this.Die();
	}

	// Token: 0x06001D4E RID: 7502 RVA: 0x000B0638 File Offset: 0x000AE838
	public void Die()
	{
		List<FlyingMermaidLevelMerdusaBodyPart> list = new List<FlyingMermaidLevelMerdusaBodyPart>();
		this.StopAllCoroutines();
		AudioManager.Play("level_mermaid_merdusa_fallapart_turnstone");
		list.Add(this.bodyPrefab.Create(this.bodyRoot.position));
		list.Add(this.leftArmPrefab.Create(this.leftArmRoot.position));
		list.Add(this.rightArmPrefab.Create(this.rightArmRoot.position));
		this.head.CheckParts(list.ToArray());
		this.StopAllCoroutines();
		CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
		foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
		{
			flyingMermaidLevelEel.Die(true, true);
		}
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06001D4F RID: 7503 RVA: 0x000B071C File Offset: 0x000AE91C
	public void DieEasyMode()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("Die");
		foreach (FlyingMermaidLevelEel flyingMermaidLevelEel in this.eels)
		{
			flyingMermaidLevelEel.Die(true, true);
		}
	}

	// Token: 0x06001D50 RID: 7504 RVA: 0x00018CF4 File Offset: 0x00016EF4
	public void OnBossDeath()
	{
		if (Level.CurrentMode == Level.Mode.Easy)
		{
			this.DieEasyMode();
		}
		else
		{
			this.Die();
		}
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x00018D11 File Offset: 0x00016F11
	public void RightSplash()
	{
		this.splashRight.Create(this.splashRoot.transform.position);
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x00018D2F File Offset: 0x00016F2F
	public void LeftSplash()
	{
		this.splashLeft.Create(this.splashRoot.transform.position);
	}

	// Token: 0x040017D8 RID: 6104
	[SerializeField]
	public float introMoveTime;

	// Token: 0x040017D9 RID: 6105
	[SerializeField]
	public float transformMoveX;

	// Token: 0x040017DA RID: 6106
	[SerializeField]
	public SpriteRenderer blinkOverlaySprite;

	// Token: 0x040017DB RID: 6107
	[SerializeField]
	public Transform blockingColliders;

	// Token: 0x040017DC RID: 6108
	[SerializeField]
	public FlyingMermaidLevelLaser laser;

	// Token: 0x040017DD RID: 6109
	[SerializeField]
	public FlyingMermaidLevelEel[] eels;

	// Token: 0x040017DE RID: 6110
	[SerializeField]
	public FlyingMermaidLevelMerdusaHead head;

	// Token: 0x040017DF RID: 6111
	[SerializeField]
	public FlyingMermaidLevelMerdusaBodyPart bodyPrefab;

	// Token: 0x040017E0 RID: 6112
	[SerializeField]
	public FlyingMermaidLevelMerdusaBodyPart leftArmPrefab;

	// Token: 0x040017E1 RID: 6113
	[SerializeField]
	public FlyingMermaidLevelMerdusaBodyPart rightArmPrefab;

	// Token: 0x040017E2 RID: 6114
	[SerializeField]
	public Transform headRoot;

	// Token: 0x040017E3 RID: 6115
	[SerializeField]
	public Transform bodyRoot;

	// Token: 0x040017E4 RID: 6116
	[SerializeField]
	public Transform leftArmRoot;

	// Token: 0x040017E5 RID: 6117
	[SerializeField]
	public Transform rightArmRoot;

	// Token: 0x040017E6 RID: 6118
	[SerializeField]
	public Effect splashLeft;

	// Token: 0x040017E7 RID: 6119
	[SerializeField]
	public Effect splashRight;

	// Token: 0x040017E8 RID: 6120
	[SerializeField]
	public Transform splashRoot;

	// Token: 0x040017E9 RID: 6121
	public DamageDealer damageDealer;

	// Token: 0x040017EA RID: 6122
	public DamageReceiver damageReceiver;

	// Token: 0x040017EB RID: 6123
	public Vector2 startPos;

	// Token: 0x040017EC RID: 6124
	public int blinks;

	// Token: 0x040017ED RID: 6125
	public int maxBlinks = 3;

	// Token: 0x02000D32 RID: 3378
	public enum State
	{
		// Token: 0x04005FBB RID: 24507
		Intro,
		// Token: 0x04005FBC RID: 24508
		Idle,
		// Token: 0x04005FBD RID: 24509
		Zap
	}
}
