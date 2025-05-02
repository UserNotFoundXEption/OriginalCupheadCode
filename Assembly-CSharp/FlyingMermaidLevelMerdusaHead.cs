using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200028A RID: 650
public class FlyingMermaidLevelMerdusaHead : LevelProperties.FlyingMermaid.Entity
{
	// Token: 0x170002B7 RID: 695
	// (get) Token: 0x06001D5D RID: 7517 RVA: 0x00018DE7 File Offset: 0x00016FE7
	// (set) Token: 0x06001D5E RID: 7518 RVA: 0x00018DEF File Offset: 0x00016FEF
	public FlyingMermaidLevelMerdusaHead.State state { get; set; }

	// Token: 0x06001D5F RID: 7519 RVA: 0x00018DF8 File Offset: 0x00016FF8
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001D60 RID: 7520 RVA: 0x00018E2E File Offset: 0x0001702E
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001D61 RID: 7521 RVA: 0x00018E41 File Offset: 0x00017041
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001D62 RID: 7522 RVA: 0x00018E59 File Offset: 0x00017059
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001D63 RID: 7523 RVA: 0x00018E77 File Offset: 0x00017077
	public void StartIntro(Vector2 pos)
	{
		base.transform.position = pos;
		base.properties.OnBossDeath += this.OnBossDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001D64 RID: 7524 RVA: 0x00018EAE File Offset: 0x000170AE
	public void CheckParts(FlyingMermaidLevelMerdusaBodyPart[] parts)
	{
		base.StartCoroutine(this.check_parts_cr(parts));
	}

	// Token: 0x06001D65 RID: 7525 RVA: 0x000B07E4 File Offset: 0x000AE9E4
	public IEnumerator check_parts_cr(FlyingMermaidLevelMerdusaBodyPart[] parts)
	{
		foreach (FlyingMermaidLevelMerdusaBodyPart part in parts)
		{
			while (!part.IsSinking)
			{
				yield return null;
			}
		}
		this.coral.speed = base.properties.CurrentState.coral.coralMoveSpeed;
		foreach (SpriteRenderer spriteRenderer in this.wave1.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.sortingLayerName = SpriteLayer.Background.ToString();
			spriteRenderer.sortingOrder = 100;
		}
		foreach (SpriteRenderer spriteRenderer2 in this.wave2.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer2.sortingLayerName = SpriteLayer.Background.ToString();
			spriteRenderer2.sortingOrder = 101;
		}
		foreach (ScrollingSpriteSpawner scrollingSpriteSpawner in this.scrollingSpritesToEnd)
		{
			scrollingSpriteSpawner.HandlePausing(true);
		}
		foreach (ScrollingSpriteSpawner scrollingSpriteSpawner2 in this.scrollingSprites)
		{
			scrollingSpriteSpawner2.StartLoop(false);
		}
		base.StartCoroutine(this.move_head_cr());
		this.state = FlyingMermaidLevelMerdusaHead.State.Idle;
		base.StartCoroutine(this.spawn_yellow_dots_cr());
		yield break;
	}

	// Token: 0x06001D66 RID: 7526 RVA: 0x00018EBE File Offset: 0x000170BE
	public override void LevelInit(LevelProperties.FlyingMermaid properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001D67 RID: 7527 RVA: 0x00018EC7 File Offset: 0x000170C7
	public void OnBossDeath()
	{
		this.StopAllCoroutines();
		base.animator.Play("Death");
		this.state = FlyingMermaidLevelMerdusaHead.State.Dead;
	}

	// Token: 0x06001D68 RID: 7528 RVA: 0x000B0808 File Offset: 0x000AEA08
	public IEnumerator intro_cr()
	{
		this.state = FlyingMermaidLevelMerdusaHead.State.Intro;
		Level.Current.SetBounds(null, null, null, new int?(300));
		yield return null;
		yield break;
	}

	// Token: 0x06001D69 RID: 7529 RVA: 0x000B0824 File Offset: 0x000AEA24
	public IEnumerator move_head_cr()
	{
		Vector2 pos = base.transform.position;
		YieldInstruction wait = new WaitForFixedUpdate();
		float offset = this.xPosition - pos.x;
		for (;;)
		{
			float targetXDistance = float.MaxValue;
			float targetY = 0f;
			foreach (Transform transform in this.coral.points)
			{
				float num = transform.position.x - pos.x;
				if (num > 0f && num < targetXDistance)
				{
					targetXDistance = num;
					targetY = transform.position.y;
				}
			}
			float t = 0f;
			float time = targetXDistance / this.coral.speed;
			float startY = pos.y;
			while (t < time)
			{
				if (base.transform.position.x < this.xPosition)
				{
					pos.x += offset * (CupheadTime.FixedDelta / this.headBackMoveTime);
				}
				else
				{
					pos.x = this.xPosition;
				}
				t += CupheadTime.FixedDelta;
				pos.y = EaseUtils.EaseInOutSine(startY, targetY, t / time);
				base.transform.position = pos;
				yield return wait;
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06001D6A RID: 7530 RVA: 0x00018EE6 File Offset: 0x000170E6
	public void StartBubble()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.bubble_cr());
	}

	// Token: 0x06001D6B RID: 7531 RVA: 0x00018F11 File Offset: 0x00017111
	public void StartHeadBlast()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.head_blast_cr());
	}

	// Token: 0x06001D6C RID: 7532 RVA: 0x00018F3C File Offset: 0x0001713C
	public void StartHeadBubble()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.head_blast_bubble_cr());
	}

	// Token: 0x06001D6D RID: 7533 RVA: 0x000B0840 File Offset: 0x000AEA40
	public IEnumerator bubble_cr()
	{
		this.state = FlyingMermaidLevelMerdusaHead.State.Bubble;
		base.animator.SetTrigger("OnSnakeATK");
		yield return base.animator.WaitForAnimationToEnd(this, "Snake_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.bubbles.attackDelayRange.RandomFloat());
		this.state = FlyingMermaidLevelMerdusaHead.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001D6E RID: 7534 RVA: 0x000B085C File Offset: 0x000AEA5C
	public IEnumerator head_blast_cr()
	{
		this.state = FlyingMermaidLevelMerdusaHead.State.HeadBlast;
		base.animator.SetTrigger("OnEyewave");
		yield return base.animator.WaitForAnimationToEnd(this, "Eyewave_Attack", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.headBlast.attackDelayRange.RandomFloat());
		this.state = FlyingMermaidLevelMerdusaHead.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001D6F RID: 7535 RVA: 0x000B0878 File Offset: 0x000AEA78
	public IEnumerator head_blast_bubble_cr()
	{
		this.state = FlyingMermaidLevelMerdusaHead.State.Both;
		base.animator.SetTrigger("OnBoth");
		yield return base.animator.WaitForAnimationToEnd(this, "Snake_Eyewave", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.coral.bubbleEyewaveSpawnDelayRange.RandomFloat());
		this.state = FlyingMermaidLevelMerdusaHead.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001D70 RID: 7536 RVA: 0x000B0894 File Offset: 0x000AEA94
	public void SpawnBubble()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 vector = next.transform.position - base.transform.position;
		LevelProperties.FlyingMermaid.Bubbles bubbles = base.properties.CurrentState.bubbles;
		this.bubblePrefab.CreateBubble(this.snakeRoot.transform.position, bubbles.movementSpeed, bubbles.waveSpeed, bubbles.waveAmount, MathUtils.DirectionToAngle(vector));
	}

	// Token: 0x06001D71 RID: 7537 RVA: 0x000B0914 File Offset: 0x000AEB14
	public void SpawnHeadBlast()
	{
		BasicProjectile basicProjectile = this.heatBlastPrefab.Create(this.eyebeamRoot.transform.position, 0f, -base.properties.CurrentState.headBlast.movementSpeed);
		basicProjectile.GetComponent<FlyingMermaidLevelLaser>().SetStoneTime(base.properties.CurrentState.zap.stoneTime);
	}

	// Token: 0x06001D72 RID: 7538 RVA: 0x000B0980 File Offset: 0x000AEB80
	public IEnumerator spawn_yellow_dots_cr()
	{
		float xPos = 690f;
		LevelProperties.FlyingMermaid.Coral p = base.properties.CurrentState.coral;
		int mainIndex = Random.Range(0, p.yellowDotPosString.Length);
		string[] yPosString = p.yellowDotPosString[mainIndex].Split(new char[]
		{
			','
		});
		for (;;)
		{
			yPosString = p.yellowDotPosString[mainIndex].Split(new char[]
			{
				','
			});
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.coral.yellowSpawnDelayRange.RandomFloat());
			float[] yPos = new float[yPosString.Length];
			for (int i = 0; i < yPosString.Length; i++)
			{
				yPos[i] = Parser.FloatParse(yPosString[i]);
			}
			Array.Sort<float>(yPos);
			for (int j = 0; j < yPosString.Length; j++)
			{
				Vector3 vector;
				vector..ctor(xPos, base.transform.position.y - 20f + yPos[j]);
				BasicProjectile basicProjectile = this.yellowDot.Create(vector, 0f, -p.coralMoveSpeed);
				if (yPosString.Length == 1)
				{
					basicProjectile.animator.SetFloat("PillarType", 1f);
				}
				else if (j == 0)
				{
					basicProjectile.animator.SetFloat("PillarType", 0.5f);
				}
				else if (j == yPosString.Length - 1)
				{
					basicProjectile.animator.SetFloat("PillarType", (!Rand.Bool()) ? 0.25f : 0f);
				}
				else
				{
					basicProjectile.animator.SetFloat("PillarType", 0.75f);
				}
				basicProjectile.animator.Play("Pillar", 0, Random.value);
				basicProjectile.GetComponent<SpriteRenderer>().sortingOrder = j;
			}
			mainIndex = (mainIndex + 1) % p.yellowDotPosString.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001D73 RID: 7539 RVA: 0x00018F67 File Offset: 0x00017167
	public void SoundMermaidPhase3GhostShoot()
	{
		AudioManager.Play("level_mermaid_phase3_ghostshoot");
		this.emitAudioFromObject.Add("level_mermaid_phase3_ghostshoot");
	}

	// Token: 0x06001D74 RID: 7540 RVA: 0x00018F83 File Offset: 0x00017183
	public void SoundMermaidPhase3SnakeShoot()
	{
		AudioManager.Play("level_mermaid_phase3_snakeshoot");
		this.emitAudioFromObject.Add("level_mermaid_phase3_snakeshoot");
	}

	// Token: 0x040017F6 RID: 6134
	public const float PillarTopA = 0f;

	// Token: 0x040017F7 RID: 6135
	public const float PillarTopB = 0.25f;

	// Token: 0x040017F8 RID: 6136
	public const float PillarBottom = 0.5f;

	// Token: 0x040017F9 RID: 6137
	public const float PillarPlain = 0.75f;

	// Token: 0x040017FA RID: 6138
	public const float PillarSingle = 1f;

	// Token: 0x040017FB RID: 6139
	public const string PillarParameterName = "PillarType";

	// Token: 0x040017FC RID: 6140
	public const string PillarStateName = "Pillar";

	// Token: 0x040017FD RID: 6141
	[SerializeField]
	public BasicProjectile yellowDot;

	// Token: 0x040017FE RID: 6142
	[SerializeField]
	public SpriteRenderer wave1;

	// Token: 0x040017FF RID: 6143
	[SerializeField]
	public SpriteRenderer wave2;

	// Token: 0x04001800 RID: 6144
	[SerializeField]
	public ScrollingSpriteSpawner[] scrollingSpritesToEnd;

	// Token: 0x04001801 RID: 6145
	[SerializeField]
	public ScrollingSpriteSpawner[] scrollingSprites;

	// Token: 0x04001802 RID: 6146
	[SerializeField]
	public FlyingMermaidLevelBackgroundChange coral;

	// Token: 0x04001803 RID: 6147
	[SerializeField]
	public Transform snakeRoot;

	// Token: 0x04001804 RID: 6148
	[SerializeField]
	public Transform eyebeamRoot;

	// Token: 0x04001805 RID: 6149
	[SerializeField]
	public FlyingMermaidLevelSkullBubble bubblePrefab;

	// Token: 0x04001806 RID: 6150
	[SerializeField]
	public BasicProjectile heatBlastPrefab;

	// Token: 0x04001807 RID: 6151
	[SerializeField]
	public float xPosition;

	// Token: 0x04001808 RID: 6152
	[SerializeField]
	public float headBackMoveTime;

	// Token: 0x0400180A RID: 6154
	public DamageDealer damageDealer;

	// Token: 0x0400180B RID: 6155
	public DamageReceiver damageReceiver;

	// Token: 0x0400180C RID: 6156
	public Coroutine patternCoroutine;

	// Token: 0x02000D39 RID: 3385
	public enum State
	{
		// Token: 0x04005FDF RID: 24543
		Intro,
		// Token: 0x04005FE0 RID: 24544
		Idle,
		// Token: 0x04005FE1 RID: 24545
		HeadBlast,
		// Token: 0x04005FE2 RID: 24546
		Bubble,
		// Token: 0x04005FE3 RID: 24547
		Both,
		// Token: 0x04005FE4 RID: 24548
		Dead
	}
}
