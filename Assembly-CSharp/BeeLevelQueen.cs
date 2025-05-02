using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000168 RID: 360
public class BeeLevelQueen : LevelProperties.Bee.Entity
{
	// Token: 0x1700023D RID: 573
	// (get) Token: 0x0600112C RID: 4396 RVA: 0x0000E801 File Offset: 0x0000CA01
	// (set) Token: 0x0600112D RID: 4397 RVA: 0x0000E809 File Offset: 0x0000CA09
	public BeeLevelQueen.State state { get; set; }

	// Token: 0x0600112E RID: 4398 RVA: 0x00092040 File Offset: 0x00090240
	public override void Awake()
	{
		base.Awake();
		base.RegisterCollisionChild(this.head.gameObject);
		this.EnableBody(false);
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x0600112F RID: 4399 RVA: 0x0000E812 File Offset: 0x0000CA12
	public void Start()
	{
		AudioManager.Play("bee_queen_intro_vocal");
		this.emitAudioFromObject.Add("bee_queen_intro_vocal");
		Level.Current.OnIntroEvent += this.OnIntro;
	}

	// Token: 0x06001130 RID: 4400 RVA: 0x0000E844 File Offset: 0x0000CA44
	public void OnIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001131 RID: 4401 RVA: 0x0000E853 File Offset: 0x0000CA53
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001132 RID: 4402 RVA: 0x0000E86B File Offset: 0x0000CA6B
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001133 RID: 4403 RVA: 0x0000E894 File Offset: 0x0000CA94
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(this.followerRoot.position, this.followerRadius);
	}

	// Token: 0x06001134 RID: 4404 RVA: 0x0009209C File Offset: 0x0009029C
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != BeeLevelQueen.State.Death)
		{
			this.state = BeeLevelQueen.State.Death;
			this.Death();
		}
	}

	// Token: 0x06001135 RID: 4405 RVA: 0x0000E8C6 File Offset: 0x0000CAC6
	public void EnableBody(bool p)
	{
		this.head.SetActive(p);
		this.body.SetActive(p);
		this.chain.SetActive(p);
	}

	// Token: 0x06001136 RID: 4406 RVA: 0x000920E8 File Offset: 0x000902E8
	public void MagicEffect()
	{
		Transform transform = this.dustEffect.Create(base.transform.position).transform;
		Transform transform2 = this.sparkEffect.Create(base.transform.position).transform;
		transform.SetParent(base.transform);
		transform2.SetParent(base.transform);
		transform.ResetLocalTransforms();
		transform2.ResetLocalTransforms();
	}

	// Token: 0x06001137 RID: 4407 RVA: 0x0000E8EC File Offset: 0x0000CAEC
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.puff = null;
		this.spitPrefab = null;
		this.blackHolePrefab = null;
		this.trianglePrefab = null;
		this.triangleInvinciblePrefab = null;
		this.followerPrefab = null;
		this.dustEffect = null;
		this.sparkEffect = null;
	}

	// Token: 0x06001138 RID: 4408 RVA: 0x00092154 File Offset: 0x00090354
	public IEnumerator intro_cr()
	{
		this.SetTrigger(BeeLevelQueen.Triggers.Continue);
		this.state = BeeLevelQueen.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06001139 RID: 4409 RVA: 0x0000E92C File Offset: 0x0000CB2C
	public void SfxIntroKnife()
	{
		AudioManager.Play("bee_queen_intro_cutlery");
		this.emitAudioFromObject.Add("bee_queen_intro_cutlery");
	}

	// Token: 0x0600113A RID: 4410 RVA: 0x0000E948 File Offset: 0x0000CB48
	public void SfxIntroSnap()
	{
		AudioManager.Play("bee_queen_intro_finger_click");
		this.emitAudioFromObject.Add("bee_queen_intro_finger_click");
	}

	// Token: 0x0600113B RID: 4411 RVA: 0x0000E964 File Offset: 0x0000CB64
	public void StartChain()
	{
		this.state = BeeLevelQueen.State.Chain;
		base.StartCoroutine(this.chain_cr());
	}

	// Token: 0x0600113C RID: 4412 RVA: 0x0000E97A File Offset: 0x0000CB7A
	public void FireChainStartSFX()
	{
		AudioManager.Play("bee_queen_chain_head_spit_start");
		this.emitAudioFromObject.Add("bee_queen_chain_head_spit_start");
	}

	// Token: 0x0600113D RID: 4413 RVA: 0x00092170 File Offset: 0x00090370
	public void FireChainProjectile()
	{
		AudioManager.Play("bee_queen_chain_head_spit_attack");
		this.emitAudioFromObject.Add("bee_queen_chain_head_spit_attack");
		this.spitPrefab.Create(this.spitRoot.position, new Vector2(base.transform.localScale.x, 1f), this.currentChain.speed, new Vector2(this.currentChain.timeX, this.currentChain.timeY));
	}

	// Token: 0x0600113E RID: 4414 RVA: 0x000921F8 File Offset: 0x000903F8
	public void ChainFlip()
	{
		base.transform.SetScale(new float?(base.transform.localScale.x * -1f), new float?(1f), new float?(1f));
	}

	// Token: 0x0600113F RID: 4415 RVA: 0x00092244 File Offset: 0x00090444
	public IEnumerator chain_cr()
	{
		this.currentChain = base.properties.CurrentState.chain;
		base.transform.ResetLocalTransforms();
		base.transform.SetPosition(new float?(-250f), new float?(0f), new float?(0f));
		base.animator.Play("Warning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		base.transform.SetPosition(new float?(0f), new float?(550f), new float?(0f));
		this.EnableBody(true);
		this.SetBool(BeeLevelQueen.Bools.Repeat, true);
		base.animator.Play("Chain_Idle");
		base.animator.Play("Head_Closed_Idle", base.animator.GetLayerIndex("Head"));
		yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, new Vector2(0f, 300f), EaseUtils.EaseType.easeOutQuart, 0.6f));
		AudioManager.Play("bee_queen_chain_ascend_vocal");
		this.emitAudioFromObject.Add("bee_queen_chain_ascend_vocal");
		AudioManager.Play("bee_queen_chain_head_ascend");
		this.emitAudioFromObject.Add("bee_queen_chain_head_ascend");
		base.StartCoroutine(this.tween_cr(this.chain.transform, this.chain.transform.position, new Vector2(0f, -100f), EaseUtils.EaseType.easeInQuart, 0.6f));
		yield return base.StartCoroutine(this.tween_cr(this.head.transform, this.head.transform.position, new Vector2(0f, -100f), EaseUtils.EaseType.easeInQuart, 0.6f));
		CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		base.animator.Play("Spit_Start", base.animator.GetLayerIndex("Head"));
		yield return CupheadTime.WaitForSeconds(this, 1f);
		if (!base.properties.CurrentState.chain.chainForever)
		{
			for (int i = 0; i < this.currentChain.count; i++)
			{
				AudioManager.Play("bee_chain_head_spit_delay");
				this.emitAudioFromObject.Add("bee_chain_head_spit_delay");
				yield return CupheadTime.WaitForSeconds(this, this.currentChain.delay);
				if (i >= this.currentChain.count - 1)
				{
					this.SetBool(BeeLevelQueen.Bools.Repeat, false);
				}
				this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			}
			yield return base.animator.WaitForAnimationToEnd(this, "Spit_Attack_End", base.animator.GetLayerIndex("Head"), false, true);
			AudioManager.Play("bee_queen_chain_head_decend");
			this.emitAudioFromObject.Add("bee_queen_chain_head_decend");
			base.StartCoroutine(this.tween_cr(this.chain.transform, this.chain.transform.position, new Vector2(0f, 300f), EaseUtils.EaseType.easeInQuart, 0.6f));
			yield return base.StartCoroutine(this.tween_cr(this.head.transform, this.head.transform.position, new Vector2(0f, 300f), EaseUtils.EaseType.easeInQuart, 0.6f));
			CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
			yield return CupheadTime.WaitForSeconds(this, 0.7f);
			yield return base.StartCoroutine(this.tween_cr(base.transform, base.transform.position, new Vector2(0f, 550f), EaseUtils.EaseType.easeInQuart, 0.6f));
			this.EnableBody(false);
			yield return CupheadTime.WaitForSeconds(this, this.currentChain.hesitate);
			this.state = BeeLevelQueen.State.Idle;
			yield break;
		}
		for (;;)
		{
			AudioManager.Play("bee_chain_head_spit_delay");
			this.emitAudioFromObject.Add("bee_chain_head_spit_delay");
			yield return CupheadTime.WaitForSeconds(this, this.currentChain.delay);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			yield return null;
		}
	}

	// Token: 0x06001140 RID: 4416 RVA: 0x0000E996 File Offset: 0x0000CB96
	public void StartBlackHole()
	{
		this.state = BeeLevelQueen.State.BlackHole;
		base.StartCoroutine(this.blackHole_cr());
	}

	// Token: 0x06001141 RID: 4417 RVA: 0x00092260 File Offset: 0x00090460
	public IEnumerator blackHole_cr()
	{
		base.transform.ResetLocalTransforms();
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?(1f), new float?(1f));
		base.transform.SetPosition(new float?(290f * base.transform.localScale.x), null, null);
		base.animator.Play("Warning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		this.ClearTrigger(BeeLevelQueen.Triggers.Continue);
		this.SetAttackAnim(BeeLevelQueen.AttackAnimations.BlackHole);
		base.animator.Play("Spell_Start");
		LevelProperties.Bee.BlackHole properties = base.properties.CurrentState.blackHole;
		string[] patternStrings = properties.patterns[Random.Range(0, properties.patterns.Length)].Split(new char[]
		{
			','
		});
		int[] patternArray = new int[patternStrings.Length];
		for (int j = 0; j < patternStrings.Length; j++)
		{
			Parser.IntTryParse(patternStrings[j], out patternArray[j]);
			patternArray[j] = Mathf.Clamp(patternArray[j], 0, 2);
		}
		int i = 0;
		int count = patternArray.Length;
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_Start", false, true);
		AudioManager.PlayLoop("bee_queen_spell_shake_loop");
		this.emitAudioFromObject.Add("bee_queen_spell_shake_loop");
		while (i < count)
		{
			yield return CupheadTime.WaitForSeconds(this, properties.chargeTime);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Charge_End", false, true);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Attack_Start", false, true);
			BeeLevelQueenBlackHole b = this.blackHolePrefab.Create(this.blackHoleRoots[patternArray[i]].position) as BeeLevelQueenBlackHole;
			b.speed = properties.speed;
			b.health = properties.health;
			b.childDelay = properties.childDelay;
			b.childSpeed = (float)properties.childSpeed;
			if (properties.damageable)
			{
				b.gameObject.AddComponent<Rigidbody2D>();
			}
			yield return CupheadTime.WaitForSeconds(this, properties.attackTime);
			i++;
			this.SetBool(BeeLevelQueen.Bools.Repeat, i != count);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_End", false, true);
		AudioManager.Stop("bee_queen_spell_shake_loop");
		base.transform.SetPosition(new float?(0f), null, null);
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = BeeLevelQueen.State.Idle;
		yield break;
	}

	// Token: 0x06001142 RID: 4418 RVA: 0x0000E9AC File Offset: 0x0000CBAC
	public void StartTriangle()
	{
		this.state = BeeLevelQueen.State.Triangle;
		base.StartCoroutine(this.triangle_cr());
	}

	// Token: 0x06001143 RID: 4419 RVA: 0x0009227C File Offset: 0x0009047C
	public IEnumerator triangle_cr()
	{
		base.transform.ResetLocalTransforms();
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?(1f), new float?(1f));
		base.transform.SetPosition(new float?(290f * base.transform.localScale.x), null, null);
		base.animator.Play("Warning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		this.ClearTrigger(BeeLevelQueen.Triggers.Continue);
		this.SetAttackAnim(BeeLevelQueen.AttackAnimations.Triangle);
		base.animator.Play("Spell_Start");
		this.SetBool(BeeLevelQueen.Bools.Repeat, false);
		LevelProperties.Bee.Triangle properties = base.properties.CurrentState.triangle;
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_Start", false, true);
		AudioManager.PlayLoop("bee_queen_spell_shake_loop");
		this.emitAudioFromObject.Add("bee_queen_spell_shake_loop");
		int i = 0;
		while (i < properties.count)
		{
			yield return CupheadTime.WaitForSeconds(this, properties.chargeTime);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Charge_End", false, true);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Attack_Start", false, true);
			BeeLevelQueenTriangle.Properties p = new BeeLevelQueenTriangle.Properties(PlayerManager.GetNext(), properties.introTime, properties.speed, properties.rotationSpeed, properties.health, properties.childSpeed, properties.childDelay, properties.childHealth, properties.childCount, properties.damageable);
			if (properties.damageable)
			{
				this.trianglePrefab.Create(p);
			}
			else
			{
				this.triangleInvinciblePrefab.Create(p);
			}
			yield return CupheadTime.WaitForSeconds(this, properties.attackTime);
			i++;
			this.SetBool(BeeLevelQueen.Bools.Repeat, i != properties.count);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_End", false, true);
		AudioManager.Stop("bee_queen_spell_shake_loop");
		base.transform.SetPosition(new float?(0f), null, null);
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = BeeLevelQueen.State.Idle;
		yield break;
	}

	// Token: 0x06001144 RID: 4420 RVA: 0x0000E9C2 File Offset: 0x0000CBC2
	public void StartMorph()
	{
		base.StartCoroutine(this.morph_cr());
	}

	// Token: 0x06001145 RID: 4421 RVA: 0x00092298 File Offset: 0x00090498
	public IEnumerator morph_cr()
	{
		float t = 0f;
		float time = 2.5f;
		float moveSpeed = 0f;
		base.animator.Play("Warning_Trans");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning_Trans", false, true);
		AudioManager.PlayLoop("bee_queen_spell_antic");
		this.emitAudioFromObject.Add("bee_queen_spell_antic");
		Vector3 endPos = new Vector3(0f, 230f);
		Vector3 startPos = base.transform.position;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(startPos, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = endPos;
		AudioManager.Stop("bee_queen_spell_antic");
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Morph_Morph", false, true);
		yield return CupheadTime.WaitForSeconds(this, 0.54f);
		t = 0f;
		while (t < 0.76f)
		{
			moveSpeed = ((t >= 0.3f) ? 300f : 800f);
			base.transform.position += Vector3.up * moveSpeed * CupheadTime.Delta;
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		time = 0.67f;
		startPos = base.transform.position;
		endPos = new Vector3(0f, -960f);
		base.StartCoroutine(this.spawn_puffs_cr());
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(startPos, endPos, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.airplane.StartIntro();
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001146 RID: 4422 RVA: 0x000922B4 File Offset: 0x000904B4
	public void SnapPosition()
	{
		base.transform.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Player.ToString();
		base.transform.GetComponent<SpriteRenderer>().sortingOrder = 100;
		base.transform.position = new Vector3(0f, 960f);
	}

	// Token: 0x06001147 RID: 4423 RVA: 0x0000E9D1 File Offset: 0x0000CBD1
	public void MoveHoney()
	{
		base.StartCoroutine(this.move_honey_cr());
		base.StartCoroutine(this.move_bee_cr());
	}

	// Token: 0x06001148 RID: 4424 RVA: 0x0009230C File Offset: 0x0009050C
	public IEnumerator move_honey_cr()
	{
		float t = 0f;
		float time = 2.5f;
		Vector3 startPos = this.bottomHoney.transform.position;
		Vector3 endPos = new Vector3(0f, -560f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.bottomHoney.transform.position = Vector2.Lerp(startPos, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.bottomHoney.transform.position = endPos;
		base.StartCoroutine(CupheadLevelCamera.Current.change_zoom_cr(0.97f, 2f));
		yield return null;
		yield break;
	}

	// Token: 0x06001149 RID: 4425 RVA: 0x00092328 File Offset: 0x00090528
	public IEnumerator move_bee_cr()
	{
		float t = 0f;
		float time = 3f;
		while (t < time)
		{
			base.transform.position += Vector3.up * 50f * CupheadTime.Delta;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600114A RID: 4426 RVA: 0x00092344 File Offset: 0x00090544
	public IEnumerator spawn_puffs_cr()
	{
		foreach (Transform root in this.puffRoots)
		{
			this.puff.Create(root.position);
			yield return CupheadTime.WaitForSeconds(this, 0.134f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600114B RID: 4427 RVA: 0x0000E9ED File Offset: 0x0000CBED
	public void StartFollower()
	{
		this.state = BeeLevelQueen.State.Triangle;
		base.StartCoroutine(this.follower_cr());
	}

	// Token: 0x0600114C RID: 4428 RVA: 0x00092360 File Offset: 0x00090560
	public IEnumerator follower_cr()
	{
		base.transform.ResetLocalTransforms();
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?(1f), new float?(1f));
		base.transform.SetPosition(new float?(290f * base.transform.localScale.x), null, null);
		base.animator.Play("Warning");
		yield return base.animator.WaitForAnimationToEnd(this, "Warning", false, true);
		this.ClearTrigger(BeeLevelQueen.Triggers.Continue);
		this.SetAttackAnim(BeeLevelQueen.AttackAnimations.Follower);
		base.animator.Play("Spell_Start");
		this.SetBool(BeeLevelQueen.Bools.Repeat, false);
		LevelProperties.Bee.Follower properties = base.properties.CurrentState.follower;
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_Start", false, true);
		int i = 0;
		while (i < properties.count)
		{
			yield return CupheadTime.WaitForSeconds(this, properties.chargeTime);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Charge_End", false, true);
			yield return base.animator.WaitForAnimationToEnd(this, "Spell_Attack_Start", false, true);
			Vector2 vector = this.followerRoot.position;
			Vector2 vector2;
			vector2..ctor((float)Random.Range(-1, 1), (float)Random.Range(-1, 1));
			Vector2 pos = vector + vector2.normalized * (this.followerRadius * Random.value);
			BeeLevelQueenFollower.Properties p = new BeeLevelQueenFollower.Properties(PlayerManager.GetNext(), properties.introTime, properties.homingSpeed, properties.homingRotation, properties.homingTime, properties.health, properties.childDelay, properties.childHealth, properties.parryable);
			if (properties.damageable)
			{
				this.followerPrefab.Create(pos, p).gameObject.AddComponent<Rigidbody2D>().isKinematic = true;
			}
			else
			{
				this.followerPrefab.Create(pos, p);
			}
			yield return CupheadTime.WaitForSeconds(this, properties.attackTime);
			i++;
			this.SetBool(BeeLevelQueen.Bools.Repeat, i != properties.count);
			this.SetTrigger(BeeLevelQueen.Triggers.Continue);
		}
		yield return base.animator.WaitForAnimationToEnd(this, "Spell_End", false, true);
		base.transform.SetPosition(new float?(0f), null, null);
		yield return CupheadTime.WaitForSeconds(this, properties.hesitate);
		this.state = BeeLevelQueen.State.Idle;
		yield break;
	}

	// Token: 0x0600114D RID: 4429 RVA: 0x0009237C File Offset: 0x0009057C
	public IEnumerator tween_cr(Transform trans, Vector2 start, Vector2 end, EaseUtils.EaseType ease, float time)
	{
		float t = 0f;
		trans.position = start;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
			trans.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		trans.position = end;
		yield return null;
		yield break;
	}

	// Token: 0x0600114E RID: 4430 RVA: 0x0000EA03 File Offset: 0x0000CC03
	public void SetAttackAnim(BeeLevelQueen.AttackAnimations a)
	{
		this.SetInt(BeeLevelQueen.Integers.Attack, (int)a);
	}

	// Token: 0x0600114F RID: 4431 RVA: 0x0000EA0D File Offset: 0x0000CC0D
	public void SetTrigger(BeeLevelQueen.Triggers t)
	{
		base.animator.SetTrigger(t.ToString());
	}

	// Token: 0x06001150 RID: 4432 RVA: 0x0000EA27 File Offset: 0x0000CC27
	public void ClearTrigger(BeeLevelQueen.Triggers t)
	{
		base.animator.ResetTrigger(t.ToString());
	}

	// Token: 0x06001151 RID: 4433 RVA: 0x0000EA41 File Offset: 0x0000CC41
	public void SetInt(BeeLevelQueen.Integers i, int value)
	{
		base.animator.SetInteger(i.ToString(), value);
	}

	// Token: 0x06001152 RID: 4434 RVA: 0x0000EA5C File Offset: 0x0000CC5C
	public void SetBool(BeeLevelQueen.Bools b, bool value)
	{
		base.animator.SetBool(b.ToString(), value);
	}

	// Token: 0x06001153 RID: 4435 RVA: 0x0000EA77 File Offset: 0x0000CC77
	public void Death()
	{
		base.animator.Play("Head_Closed_Idle");
		this.StopAllCoroutines();
	}

	// Token: 0x06001154 RID: 4436 RVA: 0x0000EA8F File Offset: 0x0000CC8F
	public void SpellTossSFX()
	{
		AudioManager.Play("bee_queen_spell_toss");
		this.emitAudioFromObject.Add("bee_queen_spell_toss");
	}

	// Token: 0x06001155 RID: 4437 RVA: 0x0000EAAB File Offset: 0x0000CCAB
	public void SpellCastSFX()
	{
		AudioManager.Play("bee_queen_spell_cast");
		this.emitAudioFromObject.Add("bee_queen_spell_cast");
	}

	// Token: 0x06001156 RID: 4438 RVA: 0x0000EAC7 File Offset: 0x0000CCC7
	public void AttackStartSFX()
	{
		AudioManager.Play("bee_queen_attack_start");
		this.emitAudioFromObject.Add("bee_queen_attack_start");
		AudioManager.PlayLoop("bee_queen_attack_loop");
	}

	// Token: 0x06001157 RID: 4439 RVA: 0x0000EAED File Offset: 0x0000CCED
	public void AttackEndSFX()
	{
		AudioManager.Stop("bee_queen_attack_loop");
		AudioManager.Play("bee_queen_attack_end");
		this.emitAudioFromObject.Add("bee_queen_attack_end");
	}

	// Token: 0x06001158 RID: 4440 RVA: 0x0000EB13 File Offset: 0x0000CD13
	public void WarningSFX()
	{
		AudioManager.Play("bee_queen_warning");
		this.emitAudioFromObject.Add("bee_queen_warning");
	}

	// Token: 0x06001159 RID: 4441 RVA: 0x0000EB2F File Offset: 0x0000CD2F
	public void FlyDownSFX()
	{
		AudioManager.Play("bee_airplane_fly_down");
		this.emitAudioFromObject.Add("bee_airplane_fly_down");
	}

	// Token: 0x04000DEC RID: 3564
	public const float SPELL_X = 290f;

	// Token: 0x04000DEE RID: 3566
	[SerializeField]
	public BeeLevelAirplane airplane;

	// Token: 0x04000DEF RID: 3567
	[SerializeField]
	public Transform bottomHoney;

	// Token: 0x04000DF0 RID: 3568
	[SerializeField]
	public Effect puff;

	// Token: 0x04000DF1 RID: 3569
	[Space(5f)]
	[SerializeField]
	public Transform[] puffRoots;

	// Token: 0x04000DF2 RID: 3570
	[Space(5f)]
	[SerializeField]
	public GameObject head;

	// Token: 0x04000DF3 RID: 3571
	[SerializeField]
	public GameObject body;

	// Token: 0x04000DF4 RID: 3572
	[SerializeField]
	public GameObject chain;

	// Token: 0x04000DF5 RID: 3573
	[Space(10f)]
	[SerializeField]
	public BeeLevelQueenSpitProjectile spitPrefab;

	// Token: 0x04000DF6 RID: 3574
	[SerializeField]
	public Transform spitRoot;

	// Token: 0x04000DF7 RID: 3575
	[Space(10f)]
	[SerializeField]
	public BeeLevelQueenBlackHole blackHolePrefab;

	// Token: 0x04000DF8 RID: 3576
	[SerializeField]
	public Transform[] blackHoleRoots;

	// Token: 0x04000DF9 RID: 3577
	[Space(10f)]
	[SerializeField]
	public BeeLevelQueenTriangle trianglePrefab;

	// Token: 0x04000DFA RID: 3578
	[SerializeField]
	public BeeLevelQueenTriangle triangleInvinciblePrefab;

	// Token: 0x04000DFB RID: 3579
	[Space(10f)]
	[SerializeField]
	public float followerRadius = 200f;

	// Token: 0x04000DFC RID: 3580
	[SerializeField]
	public Transform followerRoot;

	// Token: 0x04000DFD RID: 3581
	[SerializeField]
	public BeeLevelQueenFollower followerPrefab;

	// Token: 0x04000DFE RID: 3582
	[Space(10f)]
	[SerializeField]
	public Effect dustEffect;

	// Token: 0x04000DFF RID: 3583
	[SerializeField]
	public Effect sparkEffect;

	// Token: 0x04000E00 RID: 3584
	public DamageReceiver damageReceiver;

	// Token: 0x04000E01 RID: 3585
	public DamageDealer damageDealer;

	// Token: 0x04000E02 RID: 3586
	public LevelProperties.Bee.Chain currentChain;

	// Token: 0x02000A6A RID: 2666
	public enum State
	{
		// Token: 0x04004C9C RID: 19612
		Intro,
		// Token: 0x04004C9D RID: 19613
		Idle,
		// Token: 0x04004C9E RID: 19614
		BlackHole,
		// Token: 0x04004C9F RID: 19615
		Triangle,
		// Token: 0x04004CA0 RID: 19616
		Follower,
		// Token: 0x04004CA1 RID: 19617
		Chain,
		// Token: 0x04004CA2 RID: 19618
		Death
	}

	// Token: 0x02000A6B RID: 2667
	public enum AttackAnimations
	{
		// Token: 0x04004CA4 RID: 19620
		BlackHole,
		// Token: 0x04004CA5 RID: 19621
		Triangle,
		// Token: 0x04004CA6 RID: 19622
		Follower
	}

	// Token: 0x02000A6C RID: 2668
	public enum Triggers
	{
		// Token: 0x04004CA8 RID: 19624
		Continue
	}

	// Token: 0x02000A6D RID: 2669
	public enum Integers
	{
		// Token: 0x04004CAA RID: 19626
		Attack
	}

	// Token: 0x02000A6E RID: 2670
	public enum Bools
	{
		// Token: 0x04004CAC RID: 19628
		Repeat
	}
}
