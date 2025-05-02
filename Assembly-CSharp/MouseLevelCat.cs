using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002C7 RID: 711
public class MouseLevelCat : LevelProperties.Mouse.Entity
{
	// Token: 0x170002D7 RID: 727
	// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x0001ABE6 File Offset: 0x00018DE6
	// (set) Token: 0x06001FA2 RID: 8098 RVA: 0x0001ABEE File Offset: 0x00018DEE
	public MouseLevelCat.State state { get; set; }

	// Token: 0x06001FA3 RID: 8099 RVA: 0x000B65DC File Offset: 0x000B47DC
	public override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.headStartPos = this.head.localPosition;
		this.head.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06001FA4 RID: 8100 RVA: 0x0001ABF7 File Offset: 0x00018DF7
	public override void LevelInit(LevelProperties.Mouse properties)
	{
		base.LevelInit(properties);
		this.fallingObjectsIndex = Random.Range(0, properties.CurrentState.claw.fallingObjectStrings.Length);
	}

	// Token: 0x06001FA5 RID: 8101 RVA: 0x0001AC1E File Offset: 0x00018E1E
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001FA6 RID: 8102 RVA: 0x0001AC31 File Offset: 0x00018E31
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.fallingObjectPrefabs = null;
	}

	// Token: 0x06001FA7 RID: 8103 RVA: 0x0001AC40 File Offset: 0x00018E40
	public void StartIntro()
	{
		base.properties.OnBossDeath += this.OnBossDeath;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001FA8 RID: 8104 RVA: 0x000B663C File Offset: 0x000B483C
	public IEnumerator intro_cr()
	{
		this.head.GetComponent<Collider2D>().enabled = true;
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		base.transform.position = this.startPosition;
		base.animator.SetTrigger("StartIntro");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 2, false, true);
		this.state = MouseLevelCat.State.Idle;
		yield break;
	}

	// Token: 0x06001FA9 RID: 8105 RVA: 0x0001AC66 File Offset: 0x00018E66
	public void EatMouse()
	{
		this.mouse.BeEaten();
		base.StartCoroutine(this.tail_cr());
	}

	// Token: 0x06001FAA RID: 8106 RVA: 0x000B6658 File Offset: 0x000B4858
	public void StartWallBreak()
	{
		this.wallAnimator.SetTrigger("OnContinue");
		foreach (GameObject gameObject in this.toDestroyOnWallBreakStart)
		{
			Object.Destroy(gameObject);
		}
	}

	// Token: 0x06001FAB RID: 8107 RVA: 0x000B669C File Offset: 0x000B489C
	public void EndWallBreak()
	{
		this.wallAnimator.SetTrigger("OnContinue");
		foreach (GameObject gameObject in this.toDestroyOnWallBreakEnd)
		{
			Object.Destroy(gameObject);
		}
		Object.Destroy(this.foreground);
		this.alternateForeground.SetActive(true);
	}

	// Token: 0x06001FAC RID: 8108 RVA: 0x000B66F8 File Offset: 0x000B48F8
	public IEnumerator tail_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, 0.75f));
			base.animator.SetTrigger("SwitchTailDirection");
			yield return base.animator.WaitForAnimationToStart(this, "Idle_Left", 1, false);
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0f, 0.75f));
			base.animator.SetTrigger("SwitchTailDirection");
			yield return base.animator.WaitForAnimationToStart(this, "Idle_Right", 1, false);
		}
		yield break;
	}

	// Token: 0x06001FAD RID: 8109 RVA: 0x000B6714 File Offset: 0x000B4914
	public void BlinkMaybe()
	{
		this.blinks++;
		if (this.blinks >= this.maxBlinks)
		{
			this.blinks = 0;
			this.maxBlinks = Random.Range(8, 16);
			this.blinkOverlaySprite.enabled = true;
			base.animator.SetBool("Blinking", true);
		}
		else
		{
			this.blinkOverlaySprite.enabled = false;
			base.animator.SetBool("Blinking", false);
		}
	}

	// Token: 0x06001FAE RID: 8110 RVA: 0x0001AC80 File Offset: 0x00018E80
	public void StartClaw(bool left)
	{
		this.state = MouseLevelCat.State.Claw;
		base.StartCoroutine(this.claw_cr(left));
	}

	// Token: 0x06001FAF RID: 8111 RVA: 0x000B6794 File Offset: 0x000B4994
	public IEnumerator claw_cr(bool left)
	{
		LevelProperties.Mouse.Claw p = base.properties.CurrentState.claw;
		MouseLevelCatPaw paw = (!left) ? this.rightPaw : this.leftPaw;
		float totalPawAttackTime = 0.584f + 2f * p.holdGroundTime;
		float totalPawLeaveTime = 0.584f * p.moveSpeed / p.leaveSpeed;
		float headMoveBackTime = p.holdGroundTime + totalPawLeaveTime + 0.417f;
		base.animator.SetTrigger((!left) ? "StartClawRight" : "StartClawLeft");
		yield return base.animator.WaitForAnimationToStart(this, (!left) ? "Claw_Right_Start" : "Claw_Left_Start", 2, false);
		yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
		paw.Attack(p);
		base.StartCoroutine(this.spawnFallingObjects_cr(left));
		yield return base.StartCoroutine(this.tween_cr(this.head.transform, this.headStartPos, this.headMoveTransform.localPosition, Quaternion.identity, this.headMoveTransform.rotation, EaseUtils.EaseType.easeOutQuad, totalPawAttackTime));
		base.StartCoroutine(this.tween_cr(this.head.transform, this.headMoveTransform.localPosition, this.headStartPos, this.headMoveTransform.rotation, Quaternion.identity, EaseUtils.EaseType.easeInOutSine, headMoveBackTime));
		yield return CupheadTime.WaitForSeconds(this, headMoveBackTime - 0.417f);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, (!left) ? "Claw_Right_End" : "Claw_Left_End", 2, false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitateAfterAttack);
		this.state = MouseLevelCat.State.Idle;
		yield break;
	}

	// Token: 0x06001FB0 RID: 8112 RVA: 0x000B67B8 File Offset: 0x000B49B8
	public IEnumerator spawnFallingObjects_cr(bool left)
	{
		LevelProperties.Mouse.Claw p = base.properties.CurrentState.claw;
		MouseLevelCatPaw paw = (!left) ? this.rightPaw : this.leftPaw;
		yield return paw.animator.WaitForAnimationToStart(this, "Attack_Hit", false);
		this.fallingObjectsIndex = (this.fallingObjectsIndex + 1) % p.fallingObjectStrings.Length;
		string[] pattern = p.fallingObjectStrings[this.fallingObjectsIndex].Split(new char[]
		{
			','
		});
		float waitTime = 0f;
		foreach (string instruction in pattern)
		{
			if (instruction[0] == 'D')
			{
				Parser.FloatTryParse(instruction.Substring(1), out waitTime);
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, waitTime);
				string[] positions = instruction.Split(new char[]
				{
					'-'
				});
				foreach (string s in positions)
				{
					float xPos = 0f;
					Parser.FloatTryParse(s, out xPos);
					this.fallingObjectPrefabs.RandomChoice<MouseLevelFallingObject>().Create(xPos, p);
				}
				waitTime = p.objectSpawnDelay;
			}
		}
		yield break;
	}

	// Token: 0x06001FB1 RID: 8113 RVA: 0x0001AC97 File Offset: 0x00018E97
	public void StartGhostMouse()
	{
		this.state = MouseLevelCat.State.GhostMouse;
		base.StartCoroutine(this.jailHead_cr());
	}

	// Token: 0x06001FB2 RID: 8114 RVA: 0x000B67DC File Offset: 0x000B49DC
	public IEnumerator jailHead_cr()
	{
		MouseLevelGhostMouse[] ghostMice = (!base.properties.CurrentState.ghostMouse.fourMice) ? this.twoGhostMice : this.fourGhostMice;
		bool unspawnedGhosts = false;
		foreach (MouseLevelGhostMouse mouseLevelGhostMouse in ghostMice)
		{
			if (mouseLevelGhostMouse.state == MouseLevelGhostMouse.State.Unspawned)
			{
				unspawnedGhosts = true;
				break;
			}
		}
		if (unspawnedGhosts)
		{
			base.animator.SetTrigger("StartGhostMouse");
			yield return base.animator.WaitForAnimationToStart(this, "Jail_Loop", 2, false);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.ghostMouse.jailDuration);
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToEnd(this, "Jail_End", 2, false, true);
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.ghostMouse.hesitateAfterAttack);
		}
		this.state = MouseLevelCat.State.Idle;
		yield break;
	}

	// Token: 0x06001FB3 RID: 8115 RVA: 0x0001ACAD File Offset: 0x00018EAD
	public void SpawnGhostMice()
	{
		base.StartCoroutine(this.spawnGhostMice_cr());
	}

	// Token: 0x06001FB4 RID: 8116 RVA: 0x000B67F8 File Offset: 0x000B49F8
	public IEnumerator spawnGhostMice_cr()
	{
		MouseLevelGhostMouse[] ghostMice = (!base.properties.CurrentState.ghostMouse.fourMice) ? this.twoGhostMice : this.fourGhostMice;
		ghostMice.Shuffle<MouseLevelGhostMouse>();
		foreach (MouseLevelGhostMouse mouse in ghostMice)
		{
			if (mouse.state == MouseLevelGhostMouse.State.Unspawned)
			{
				mouse.Spawn(base.properties);
				yield return CupheadTime.WaitForSeconds(this, 0.1f);
			}
		}
		while (ghostMice[ghostMice.Length - 1].state == MouseLevelGhostMouse.State.Intro)
		{
			yield return null;
		}
		if (!this.alreadyManagingGhostMice)
		{
			base.StartCoroutine(this.manageGhostMice_cr());
		}
		yield break;
	}

	// Token: 0x06001FB5 RID: 8117 RVA: 0x000B6814 File Offset: 0x000B4A14
	public IEnumerator manageGhostMice_cr()
	{
		this.alreadyManagingGhostMice = true;
		MouseLevelGhostMouse[] ghostMice = (!base.properties.CurrentState.ghostMouse.fourMice) ? this.twoGhostMice : this.fourGhostMice;
		int shotsTillPinkAttack = base.properties.CurrentState.ghostMouse.pinkBallRange.RandomInt();
		bool anyMiceSpawned = true;
		while (anyMiceSpawned)
		{
			ghostMice.Shuffle<MouseLevelGhostMouse>();
			anyMiceSpawned = false;
			foreach (MouseLevelGhostMouse mouse in ghostMice)
			{
				if (mouse.state != MouseLevelGhostMouse.State.Unspawned && mouse.state != MouseLevelGhostMouse.State.Dying)
				{
					anyMiceSpawned = true;
				}
				if (mouse.state == MouseLevelGhostMouse.State.Idle)
				{
					shotsTillPinkAttack--;
					bool pinkAttack = false;
					if (shotsTillPinkAttack == 0)
					{
						pinkAttack = true;
						shotsTillPinkAttack = base.properties.CurrentState.ghostMouse.pinkBallRange.RandomInt();
					}
					mouse.Attack(pinkAttack);
					while (mouse.state == MouseLevelGhostMouse.State.Attack)
					{
						yield return null;
					}
					yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.ghostMouse.attackDelayRange.RandomFloat());
				}
			}
			yield return null;
		}
		this.alreadyManagingGhostMice = false;
		yield break;
	}

	// Token: 0x06001FB6 RID: 8118 RVA: 0x000B6830 File Offset: 0x000B4A30
	public void OnBossDeath()
	{
		this.state = MouseLevelCat.State.Dying;
		this.StopAllCoroutines();
		Object.Destroy(this.leftPaw.gameObject);
		Object.Destroy(this.rightPaw.gameObject);
		this.head.transform.localPosition = this.headStartPos;
		this.head.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		base.animator.SetTrigger("Die");
		this.headFrontRenderer.sortingLayerName = "Enemies";
		MouseLevelGhostMouse[] array = (!base.properties.CurrentState.ghostMouse.fourMice) ? this.twoGhostMice : this.fourGhostMice;
		foreach (MouseLevelGhostMouse mouseLevelGhostMouse in array)
		{
			mouseLevelGhostMouse.Die();
		}
	}

	// Token: 0x06001FB7 RID: 8119 RVA: 0x000B6920 File Offset: 0x000B4B20
	public IEnumerator tween_cr(Transform trans, Vector2 startPos, Vector2 endPos, Quaternion startRotation, Quaternion endRotation, EaseUtils.EaseType ease, float time)
	{
		float t = 0f;
		trans.localPosition = startPos;
		trans.localRotation = startRotation;
		float accumulator = 0f;
		while (t < time)
		{
			accumulator += CupheadTime.Delta;
			while (accumulator > 0.0416666679f)
			{
				accumulator -= 0.0416666679f;
				float num = EaseUtils.Ease(ease, 0f, 1f, t / time);
				trans.localPosition = Vector2.Lerp(startPos, endPos, num);
				trans.localRotation = Quaternion.Slerp(startRotation, endRotation, num);
				t += 0.0416666679f;
			}
			yield return null;
		}
		trans.localPosition = endPos;
		trans.localRotation = endRotation;
		yield return null;
		yield break;
	}

	// Token: 0x06001FB8 RID: 8120 RVA: 0x0001ACBC File Offset: 0x00018EBC
	public void SoundCatIntro()
	{
		AudioManager.Play("level_mouse_cat_intro");
	}

	// Token: 0x06001FB9 RID: 8121 RVA: 0x0001ACC8 File Offset: 0x00018EC8
	public void SoundCatJailEnd()
	{
		AudioManager.Play("level_mouse_cat_jail_end");
		this.emitAudioFromObject.Add("level_mouse_cat_jail_end");
	}

	// Token: 0x040019C0 RID: 6592
	public const string EnemiesLayerName = "Enemies";

	// Token: 0x040019C1 RID: 6593
	public const int BODY_LAYER = 0;

	// Token: 0x040019C2 RID: 6594
	public const int TAIL_LAYER = 1;

	// Token: 0x040019C3 RID: 6595
	public const int HEAD_LAYER = 2;

	// Token: 0x040019C5 RID: 6597
	[SerializeField]
	public Vector2 startPosition;

	// Token: 0x040019C6 RID: 6598
	[SerializeField]
	public MouseLevelBrokenCanMouse mouse;

	// Token: 0x040019C7 RID: 6599
	[SerializeField]
	public Animator wallAnimator;

	// Token: 0x040019C8 RID: 6600
	[SerializeField]
	public LevelPlatform wallPlatform;

	// Token: 0x040019C9 RID: 6601
	[SerializeField]
	public GameObject foreground;

	// Token: 0x040019CA RID: 6602
	[SerializeField]
	public GameObject alternateForeground;

	// Token: 0x040019CB RID: 6603
	[SerializeField]
	public GameObject[] toDestroyOnWallBreakStart;

	// Token: 0x040019CC RID: 6604
	[SerializeField]
	public GameObject[] toDestroyOnWallBreakEnd;

	// Token: 0x040019CD RID: 6605
	[SerializeField]
	public SpriteRenderer blinkOverlaySprite;

	// Token: 0x040019CE RID: 6606
	[SerializeField]
	public Transform head;

	// Token: 0x040019CF RID: 6607
	[SerializeField]
	public MouseLevelCatPaw leftPaw;

	// Token: 0x040019D0 RID: 6608
	[SerializeField]
	public MouseLevelCatPaw rightPaw;

	// Token: 0x040019D1 RID: 6609
	[SerializeField]
	public Transform headMoveTransform;

	// Token: 0x040019D2 RID: 6610
	[SerializeField]
	public MouseLevelFallingObject[] fallingObjectPrefabs;

	// Token: 0x040019D3 RID: 6611
	[SerializeField]
	public MouseLevelGhostMouse[] twoGhostMice;

	// Token: 0x040019D4 RID: 6612
	[SerializeField]
	public MouseLevelGhostMouse[] fourGhostMice;

	// Token: 0x040019D5 RID: 6613
	[SerializeField]
	public SpriteRenderer headFrontRenderer;

	// Token: 0x040019D6 RID: 6614
	public DamageReceiver damageReceiver;

	// Token: 0x040019D7 RID: 6615
	public Vector2 headStartPos;

	// Token: 0x040019D8 RID: 6616
	public int fallingObjectsIndex;

	// Token: 0x040019D9 RID: 6617
	public int blinks;

	// Token: 0x040019DA RID: 6618
	public int maxBlinks = 8;

	// Token: 0x040019DB RID: 6619
	public bool alreadyManagingGhostMice;

	// Token: 0x02000DB8 RID: 3512
	public enum State
	{
		// Token: 0x0400632B RID: 25387
		Init,
		// Token: 0x0400632C RID: 25388
		Idle,
		// Token: 0x0400632D RID: 25389
		Claw,
		// Token: 0x0400632E RID: 25390
		GhostMouse,
		// Token: 0x0400632F RID: 25391
		Dying
	}
}
