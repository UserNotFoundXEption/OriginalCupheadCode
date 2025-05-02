using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001D7 RID: 471
public class DicePalaceChipsLevelChips : LevelProperties.DicePalaceChips.Entity
{
	// Token: 0x060015E2 RID: 5602 RVA: 0x000128E4 File Offset: 0x00010AE4
	public override void Awake()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x060015E3 RID: 5603 RVA: 0x0001290F File Offset: 0x00010B0F
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x060015E4 RID: 5604 RVA: 0x00012922 File Offset: 0x00010B22
	public virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060015E5 RID: 5605 RVA: 0x0009E120 File Offset: 0x0009C320
	public override void LevelInit(LevelProperties.DicePalaceChips properties)
	{
		Level.Current.OnLevelStartEvent += this.StartAttacking;
		Level.Current.OnWinEvent += this.Death;
		this.leftScreenXPos = (float)Level.Current.Left + 100f;
		this.rightScreenXPos = (float)Level.Current.Right - 100f;
		this.rightScreenXPosStart = this.chips[0].chipTransform.position.x;
		for (int i = 0; i < this.chips.Length; i++)
		{
			this.chips[i].startPosition = this.chips[i].chipTransform.position;
		}
		this.currentAttackCount = 0;
		base.LevelInit(properties);
	}

	// Token: 0x060015E6 RID: 5606 RVA: 0x00012943 File Offset: 0x00010B43
	public void StartAttacking()
	{
		base.StartCoroutine(this.chipAttack_cr());
	}

	// Token: 0x060015E7 RID: 5607 RVA: 0x0009E1F0 File Offset: 0x0009C3F0
	public IEnumerator chipAttack_cr()
	{
		LevelProperties.DicePalaceChips.Chips p = base.properties.CurrentState.chips;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.chips.initialAttackDelay);
		int mainStringIndex = Random.Range(0, p.chipAttackString.Length);
		int dir = -1;
		int attackIndex = Random.Range(0, this.maxAttacksPerCycle);
		for (;;)
		{
			string[] currentAttackChips = p.chipAttackString[mainStringIndex].Split(new char[]
			{
				','
			});
			this.maxAttacksPerCycle = currentAttackChips.Length;
			for (int j = 0; j < this.chips.Length; j++)
			{
				float rotationSpeed = (!Rand.Bool()) ? -5f : 5f;
				this.chips[j].rotationSpeed = rotationSpeed;
			}
			base.animator.SetBool("IsSpread", true);
			yield return base.animator.WaitForAnimationToStart(this, "Spread_Open", false);
			float startPos = this.chips[this.chips.Length - 1].chipTransform.position.y;
			float frameTime = 0f;
			float time = 1.5f;
			float t = 0f;
			int counter = 0;
			while (t < time)
			{
				float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
				frameTime += CupheadTime.Delta * base.animator.speed;
				t += CupheadTime.Delta * base.animator.speed;
				if (frameTime > 0.0416666679f)
				{
					frameTime -= 0.0416666679f;
					for (int k = this.chips.Length - 1; k >= 0; k--)
					{
						float num = (k != 0) ? (this.chips[k].chipTransform.GetComponent<Renderer>().bounds.size.y / 1.7f) : (this.chips[k].chipTransform.GetComponent<Renderer>().bounds.size.y / 5.5f);
						float num2 = startPos + (float)counter * num;
						Vector3 position = this.chips[k].chipTransform.position;
						position.y = Mathf.Lerp(position.y, num2, val);
						this.chips[k].chipTransform.position = position;
						counter = (counter + 1) % this.chips.Length;
						float num3 = Mathf.Sin(t / 0.7f);
						this.chips[k].chipTransform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, num3 * this.chips[k].rotationSpeed));
					}
				}
				yield return null;
			}
			this.currentlyFloating = true;
			foreach (DicePalaceChipsLevelChips.ChipPieces chipPieces in this.chips)
			{
				base.StartCoroutine(this.rotate_chips_cr(chipPieces.chipTransform, chipPieces.rotationSpeed, 0.7f, t));
			}
			yield return null;
			for (int i = attackIndex; i < currentAttackChips.Length; i++)
			{
				string[] currentAttackChipsMultiple = currentAttackChips[i].Split(new char[]
				{
					'-'
				});
				this.SFX_DicePalaceChipsShoot();
				foreach (string chip in currentAttackChipsMultiple)
				{
					if (chip[0] == 'D')
					{
						yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(chip.Substring(1)));
					}
					else if (this.currentAttackCount < this.maxAttacksPerCycle - 1)
					{
						base.StartCoroutine(this.moveChip_cr(base.transform.GetChild(Parser.IntParse(chip) - 1).transform, dir, false));
					}
					else
					{
						base.StartCoroutine(this.moveChip_cr(base.transform.GetChild(Parser.IntParse(chip) - 1).transform, dir, true));
					}
				}
				this.currentAttackCount++;
				if (this.currentAttackCount >= this.maxAttacksPerCycle)
				{
					this.currentAttackCount = 0;
					attackIndex = Random.Range(0, this.maxAttacksPerCycle);
				}
				else
				{
					yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.chips.chipAttackDelay);
				}
				attackIndex = 0;
			}
			while (this.chipInFlight)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, 1f);
			time = 0.3f;
			t = 0f;
			counter = 0;
			base.animator.SetBool("IsSpread", false);
			yield return base.animator.WaitForAnimationToStart(this, "Spread_Close", false);
			yield return CupheadTime.WaitForSeconds(this, 0.4f);
			this.currentlyFloating = false;
			while (t < time)
			{
				float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
				for (int n = this.chips.Length - 1; n >= 0; n--)
				{
					float num4 = 0f;
					if (n != this.chips.Length - 1)
					{
						num4 = 10f;
					}
					float num5 = this.chips[n].startPosition.y - (float)counter * num4;
					Vector3 position2 = this.chips[n].chipTransform.position;
					position2.y = Mathf.Lerp(position2.y, num5, val2);
					this.chips[n].chipTransform.position = position2;
					counter = (counter + 1) % this.chips.Length;
				}
				t += CupheadTime.Delta;
				yield return null;
			}
			dir *= -1;
			yield return null;
			mainStringIndex = (mainStringIndex + 1) % p.chipAttackString.Length;
			float tt = 0f;
			while (tt < base.properties.CurrentState.chips.attackCycleDelay)
			{
				tt += CupheadTime.Delta * base.animator.speed;
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x060015E8 RID: 5608 RVA: 0x0009E20C File Offset: 0x0009C40C
	public void FlipSprite()
	{
		this.mainLayer.transform.SetScale(new float?(-this.mainLayer.transform.localScale.x), new float?(1f), new float?(1f));
		foreach (DicePalaceChipsLevelChips.ChipPieces chipPieces in this.chips)
		{
			Vector3 position = chipPieces.chipTransform.position;
			position.y = chipPieces.startPosition.y;
			chipPieces.chipTransform.position = position;
		}
	}

	// Token: 0x060015E9 RID: 5609 RVA: 0x0009E2A8 File Offset: 0x0009C4A8
	public IEnumerator moveChip_cr(Transform chip, int dir, bool lastChipOfCycle)
	{
		this.chipInFlight = lastChipOfCycle;
		float start = (dir != 1) ? this.rightScreenXPos : this.leftScreenXPos;
		float end = (dir != 1) ? this.leftScreenXPos : this.rightScreenXPos;
		Vector3 pos = chip.position;
		if (this.firstTimeMoving)
		{
			start = this.rightScreenXPosStart;
		}
		float pct = 0f;
		while (pct < 1f)
		{
			pos.x = start + (end - start) * pct;
			chip.position = pos;
			pct += CupheadTime.Delta * base.properties.CurrentState.chips.chipSpeedMultiplier * this.hitPauseCoefficient() * base.animator.speed;
			yield return null;
		}
		pos.x = end;
		chip.position = pos;
		this.chipInFlight = false;
		if (lastChipOfCycle)
		{
			this.firstTimeMoving = false;
		}
		yield break;
	}

	// Token: 0x060015EA RID: 5610 RVA: 0x0009E2D8 File Offset: 0x0009C4D8
	public IEnumerator rotate_chips_cr(Transform chip, float speed, float time, float t)
	{
		while (this.currentlyFloating)
		{
			t += CupheadTime.Delta;
			float phase = Mathf.Sin(t / time);
			chip.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * speed));
			yield return null;
		}
		chip.localRotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
		yield return null;
		yield break;
	}

	// Token: 0x060015EB RID: 5611 RVA: 0x0009E310 File Offset: 0x0009C510
	public void Death()
	{
		this.StopAllCoroutines();
		base.animator.SetBool("IsSpread", true);
		base.animator.SetTrigger("OnDeath");
		this.chips[0].chipTransform.SetScale(new float?(this.mainLayer.transform.localScale.x), new float?(1f), new float?(1f));
		base.StartCoroutine(this.head_fall_cr());
		for (int i = 1; i < this.chips.Length; i++)
		{
			base.StartCoroutine(this.chips_die(this.chips[i].chipTransform));
		}
	}

	// Token: 0x060015EC RID: 5612 RVA: 0x0009E3C8 File Offset: 0x0009C5C8
	public IEnumerator chips_die(Transform chip)
	{
		float speed = 2500f;
		float angle = (float)Random.Range(0, 360);
		Vector3 dir = MathUtils.AngleToDirection(-angle);
		chip.GetComponent<Collider2D>().enabled = false;
		for (;;)
		{
			chip.position += dir * speed * CupheadTime.FixedDelta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060015ED RID: 5613 RVA: 0x00012952 File Offset: 0x00010B52
	public void SpawnHat()
	{
		this.hat.SetActive(true);
		base.StartCoroutine(this.hat_fall_cr());
	}

	// Token: 0x060015EE RID: 5614 RVA: 0x0009E3E4 File Offset: 0x0009C5E4
	public IEnumerator head_fall_cr()
	{
		float velocity = 800f;
		float posY = (float)Level.Current.Ground + this.chips[0].chipTransform.GetComponent<Collider2D>().bounds.size.y / 1.2f;
		while (this.chips[0].chipTransform.position.y > posY)
		{
			this.chips[0].chipTransform.position += Vector3.down * velocity * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		yield return null;
		yield break;
	}

	// Token: 0x060015EF RID: 5615 RVA: 0x0009E400 File Offset: 0x0009C600
	public IEnumerator hat_fall_cr()
	{
		float velocity = 30f;
		while (this.hat.transform.position.y > -250f)
		{
			this.hat.transform.position += Vector3.down * velocity * CupheadTime.Delta;
			yield return null;
		}
		this.hat.GetComponent<Animator>().SetTrigger("Continue");
		yield return null;
		yield break;
	}

	// Token: 0x060015F0 RID: 5616 RVA: 0x0001296D File Offset: 0x00010B6D
	public void SFX_DicePalaceChipsIntro()
	{
		AudioManager.Play("chips_intro");
		this.emitAudioFromObject.Add("chips_intro");
		AudioManager.Play("vox_intro");
		this.emitAudioFromObject.Add("vox_intro");
	}

	// Token: 0x060015F1 RID: 5617 RVA: 0x0009E41C File Offset: 0x0009C61C
	public void SFX_DicePalaceChipsDeath()
	{
		if (!this.DeathSoundPlaying)
		{
			AudioManager.PlayLoop("chips_death");
			this.emitAudioFromObject.Add("chips_death");
			AudioManager.Play("vox_die");
			this.emitAudioFromObject.Add("vox_die");
			this.DeathSoundPlaying = true;
		}
	}

	// Token: 0x060015F2 RID: 5618 RVA: 0x0009E470 File Offset: 0x0009C670
	public void SFX_DicePalaceChipsExpand()
	{
		if (!this.ExpandSoundPlaying)
		{
			AudioManager.Play("chips_expand");
			this.emitAudioFromObject.Add("chips_expand");
			AudioManager.Play("vox_idle");
			this.emitAudioFromObject.Add("vox_idle");
			this.ExpandSoundPlaying = true;
		}
	}

	// Token: 0x060015F3 RID: 5619 RVA: 0x000129A3 File Offset: 0x00010BA3
	public void SFX_DicePalaceChipsRetract()
	{
		AudioManager.Play("chips_retract");
		AudioManager.Play("vox_idle");
		this.ExpandSoundPlaying = false;
	}

	// Token: 0x060015F4 RID: 5620 RVA: 0x000129C0 File Offset: 0x00010BC0
	public void SFX_DicePalaceChipsShoot()
	{
		AudioManager.Play("chips_shoot");
		this.emitAudioFromObject.Add("chips_shoot");
	}

	// Token: 0x060015F5 RID: 5621 RVA: 0x000129DC File Offset: 0x00010BDC
	public void SFX_DicePalaceChipsSpinLoop()
	{
		if (!this.SpinSoundPlaying)
		{
			AudioManager.PlayLoop("chips_spin_loop");
			this.emitAudioFromObject.Add("chips_spin_loop");
			this.SpinSoundPlaying = true;
		}
	}

	// Token: 0x060015F6 RID: 5622 RVA: 0x00012A0A File Offset: 0x00010C0A
	public void SFX_DicePalaceChipsSpinLoopStop()
	{
		AudioManager.Stop("chips_spin_loop");
		this.SpinSoundPlaying = false;
	}

	// Token: 0x060015F7 RID: 5623 RVA: 0x00012A1D File Offset: 0x00010C1D
	public void SFX_DicePalaceChipsBounce()
	{
		AudioManager.Play("chips_bounce");
	}

	// Token: 0x040011D6 RID: 4566
	public const float FRAME_TIME = 0.0416666679f;

	// Token: 0x040011D7 RID: 4567
	[SerializeField]
	public DicePalaceChipsLevelChips.ChipPieces[] chips;

	// Token: 0x040011D8 RID: 4568
	[SerializeField]
	public Transform mainLayer;

	// Token: 0x040011D9 RID: 4569
	[SerializeField]
	public GameObject hat;

	// Token: 0x040011DA RID: 4570
	public float leftScreenXPos;

	// Token: 0x040011DB RID: 4571
	public float rightScreenXPos;

	// Token: 0x040011DC RID: 4572
	public float rightScreenXPosStart;

	// Token: 0x040011DD RID: 4573
	public int currentAttackCount;

	// Token: 0x040011DE RID: 4574
	public int maxAttacksPerCycle;

	// Token: 0x040011DF RID: 4575
	public bool chipInFlight;

	// Token: 0x040011E0 RID: 4576
	public bool currentlyFloating;

	// Token: 0x040011E1 RID: 4577
	public bool firstTimeMoving = true;

	// Token: 0x040011E2 RID: 4578
	public DamageReceiver damageReceiver;

	// Token: 0x040011E3 RID: 4579
	public bool DeathSoundPlaying;

	// Token: 0x040011E4 RID: 4580
	public bool SpinSoundPlaying;

	// Token: 0x040011E5 RID: 4581
	public bool ExpandSoundPlaying;

	// Token: 0x02000B74 RID: 2932
	[Serializable]
	public class ChipPieces
	{
		// Token: 0x040053BF RID: 21439
		public Transform chipTransform;

		// Token: 0x040053C0 RID: 21440
		public Vector3 startPosition;

		// Token: 0x040053C1 RID: 21441
		public float rotationSpeed;
	}
}
