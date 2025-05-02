using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000357 RID: 855
public class SallyStagePlayLevelBackgroundHandler : AbstractPausableComponent
{
	// Token: 0x17000319 RID: 793
	// (get) Token: 0x060025C2 RID: 9666 RVA: 0x0001FBD4 File Offset: 0x0001DDD4
	// (set) Token: 0x060025C3 RID: 9667 RVA: 0x0001FBDB File Offset: 0x0001DDDB
	public static bool HUSBAND_GONE { get; set; }

	// Token: 0x1700031A RID: 794
	// (get) Token: 0x060025C4 RID: 9668 RVA: 0x0001FBE3 File Offset: 0x0001DDE3
	// (set) Token: 0x060025C5 RID: 9669 RVA: 0x0001FBEA File Offset: 0x0001DDEA
	public static bool CURTAIN_OPEN { get; set; }

	// Token: 0x060025C6 RID: 9670 RVA: 0x0001FBF2 File Offset: 0x0001DDF2
	public override void Awake()
	{
		base.Awake();
		this.curtain.gameObject.SetActive(true);
	}

	// Token: 0x060025C7 RID: 9671 RVA: 0x000C78A4 File Offset: 0x000C5AA4
	public void Start()
	{
		Level.Current.OnLevelStartEvent += this.StartPriestLoop;
		Level.Current.OnLevelStartEvent += this.StartHusbandLoop;
		this.curtainStartPos = this.curtainSprite.position;
		this.curtainShadowStartPos = this.curtainShadow.position;
		this.chandelierStartPosX = this.chandelier.transform.position.x;
		SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE = false;
		foreach (SallyStagePlayLevelBackgroundHandler.Cupid cupid in this.cupids)
		{
			cupid.startPosition = cupid.cupidTransform.position;
		}
		this.applauseHandler.SlideApplause(true);
	}

	// Token: 0x060025C8 RID: 9672 RVA: 0x000C7960 File Offset: 0x000C5B60
	public void GetProperties(LevelProperties.SallyStagePlay properties, SallyStagePlayLevel parent)
	{
		this.properties = properties;
		this.parent = parent;
		foreach (SpriteRenderer flicker in this.flickeringLights)
		{
			base.StartCoroutine(this.flicker_cr(flicker));
		}
		this.phaseDependentCoroutines = new List<Coroutine>();
		this.phaseDependentCoroutines.Add(base.StartCoroutine(this.check_bools_cr()));
		for (int j = 0; j < this.cupids.Length; j++)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.cupid_check_falling(this.cupids[j])));
		}
		foreach (Transform swing in this.churchSwingies)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.swing_cr(swing)));
		}
		AbstractPlayerController next = PlayerManager.GetNext();
		LevelPlayerController levelPlayerController = (LevelPlayerController)next;
		levelPlayerController.motor.OnHitEvent += this.PlayYay;
		parent.OnPhase2 += this.OnPhase2;
		parent.OnPhase3 += this.OnPhase3;
		parent.OnPhase4 += this.OnPhase4;
	}

	// Token: 0x060025C9 RID: 9673 RVA: 0x0001FC0B File Offset: 0x0001DE0B
	public void OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundSelected)
	{
		if (backgroundSelected != SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale)
		{
			this.applauseHandler.SlideApplause(false);
		}
		base.StartCoroutine(this.open_curtain_cr(backgroundSelected));
	}

	// Token: 0x060025CA RID: 9674 RVA: 0x0001FC2E File Offset: 0x0001DE2E
	public void CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundSelected)
	{
		this.applauseHandler.SlideApplause(true);
		base.StartCoroutine(this.close_curtain_cr(backgroundSelected));
	}

	// Token: 0x060025CB RID: 9675 RVA: 0x000C7AA4 File Offset: 0x000C5CA4
	public IEnumerator open_curtain_cr(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundsSelected)
	{
		this.SelectBackground(backgroundsSelected);
		float t = 0f;
		float frameTime = 0f;
		float openTime = 1.58f;
		Vector3 shadowRoot = new Vector3(this.curtainUpRoot.position.x, this.curtainUpRoot.position.y - 100f);
		AudioManager.Play("sally_bg_stage_curtain_raise");
		this.emitAudioFromObject.Add("sally_bg_stage_curtain_raise");
		while (t < openTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				float num = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / openTime);
				this.curtainSprite.transform.position = Vector2.Lerp(this.curtainSprite.transform.position, this.curtainUpRoot.position, num);
				this.curtainShadow.transform.position = Vector2.Lerp(this.curtainShadow.transform.position, shadowRoot, num);
				frameTime -= 0.0416666679f;
			}
			yield return null;
		}
		SallyStagePlayLevelBackgroundHandler.CURTAIN_OPEN = true;
		yield return null;
		yield break;
	}

	// Token: 0x060025CC RID: 9676 RVA: 0x000C7AC8 File Offset: 0x000C5CC8
	public IEnumerator close_curtain_cr(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundSelected)
	{
		float t = 0f;
		float frameTime = 0f;
		float closeTime = 1.58f;
		AudioManager.Play("sally_bg_stage_curtain_lower");
		this.emitAudioFromObject.Add("sally_bg_stage_curtain_lower");
		switch (backgroundSelected)
		{
		case SallyStagePlayLevelBackgroundHandler.Backgrounds.House:
			AudioManager.Play("sally_bg_stage_reset_phase1");
			this.emitAudioFromObject.Add("sally_bg_stage_reset_phase1");
			break;
		case SallyStagePlayLevelBackgroundHandler.Backgrounds.Nunnery:
			AudioManager.Play("sally_bg_stage_reset_phase1");
			this.emitAudioFromObject.Add("sally_bg_stage_reset_phase1");
			break;
		case SallyStagePlayLevelBackgroundHandler.Backgrounds.Purgatory:
			AudioManager.Play("sally_bg_stage_reset_phase2");
			this.emitAudioFromObject.Add("sally_bg_stage_reset_phase2");
			break;
		case SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale:
			AudioManager.Play("sally_bg_stage_reset_phase3");
			this.emitAudioFromObject.Add("sally_bg_stage_reset_phase3");
			break;
		}
		while (t < closeTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				float num = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / closeTime);
				this.curtainSprite.transform.position = Vector2.Lerp(this.curtainSprite.transform.position, this.curtainStartPos, num);
				this.curtainShadow.transform.position = Vector2.Lerp(this.curtainShadow.transform.position, this.curtainShadowStartPos, num);
				frameTime -= 0.0416666679f;
			}
			yield return null;
		}
		SallyStagePlayLevelBackgroundHandler.CURTAIN_OPEN = false;
		yield return null;
		yield break;
	}

	// Token: 0x060025CD RID: 9677 RVA: 0x000C7AEC File Offset: 0x000C5CEC
	public void SelectBackground(SallyStagePlayLevelBackgroundHandler.Backgrounds backgroundSelected)
	{
		for (int i = 0; i < this.backgrounds.Length; i++)
		{
			if (i == (int)backgroundSelected)
			{
				this.backgrounds[i].SetActive(true);
			}
			else
			{
				this.backgrounds[i].SetActive(false);
			}
		}
	}

	// Token: 0x060025CE RID: 9678 RVA: 0x000C7B3C File Offset: 0x000C5D3C
	public IEnumerator flicker_cr(SpriteRenderer flicker)
	{
		float flickerTime = 0.3f;
		for (;;)
		{
			int counter = 0;
			float waitTime = Random.Range(this.fadeWaitMinSecond, this.fadeWaitMaxSecond);
			float t = 0f;
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			while (counter < 2)
			{
				while (t < flickerTime)
				{
					flicker.color = new Color(1f, 1f, 1f, 1f - t / flickerTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				t = 0f;
				flicker.color = new Color(1f, 1f, 1f, 0f);
				while (t < flickerTime)
				{
					flicker.color = new Color(1f, 1f, 1f, t / flickerTime);
					t += CupheadTime.Delta;
					yield return null;
				}
				flicker.color = new Color(1f, 1f, 1f, 1f);
				counter++;
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025CF RID: 9679 RVA: 0x000C7B60 File Offset: 0x000C5D60
	public IEnumerator swing_cr(Transform swing)
	{
		float t = 0f;
		float speed = 0f;
		float maxSpeed = 8f;
		float minSpeed = 3f;
		bool movingRight = Rand.Bool();
		speed = minSpeed;
		for (;;)
		{
			t = ((!movingRight) ? (t - CupheadTime.Delta) : (t + CupheadTime.Delta));
			float phase = Mathf.Sin(t);
			swing.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * speed));
			if (CupheadLevelCamera.Current.isShaking)
			{
				if (speed < maxSpeed)
				{
					speed += 0.15f;
				}
			}
			else if (speed > minSpeed)
			{
				speed -= 0.05f;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025D0 RID: 9680 RVA: 0x0001FC4A File Offset: 0x0001DE4A
	public void StartPriestLoop()
	{
		this.phaseDependentCoroutines.Add(base.StartCoroutine(this.priest_animations_cr()));
	}

	// Token: 0x060025D1 RID: 9681 RVA: 0x000C7B7C File Offset: 0x000C5D7C
	public IEnumerator priest_animations_cr()
	{
		while (this.properties.CurrentState.stateName == LevelProperties.SallyStagePlay.States.Generic)
		{
			bool tuckDown = false;
			int counter = 0;
			int maxCounter = Random.Range(2, 6);
			while (!tuckDown)
			{
				yield return CupheadTime.WaitForSeconds(this, Random.Range(2f, 5f));
				this.priest.SetTrigger("Continue");
				if (counter < maxCounter)
				{
					counter++;
				}
				else
				{
					tuckDown = true;
				}
				yield return null;
			}
			bool isLookingRight = true;
			maxCounter = Random.Range(4, 8);
			counter = 0;
			this.priest.Play("Tuck_Down");
			while (tuckDown)
			{
				yield return CupheadTime.WaitForSeconds(this, Random.Range(2f, 5f));
				this.priest.SetBool("isLookingRight", isLookingRight);
				if (counter < maxCounter)
				{
					counter++;
				}
				else
				{
					tuckDown = false;
				}
				isLookingRight = !isLookingRight;
				yield return null;
			}
			this.priest.Play("Stand_Up");
			yield return null;
		}
		this.priest.Play("Look_Around");
		yield return null;
		yield break;
	}

	// Token: 0x060025D2 RID: 9682 RVA: 0x0001FC63 File Offset: 0x0001DE63
	public void StartHusbandLoop()
	{
		this.husband.SetTrigger("Continue");
		this.phaseDependentCoroutines.Add(base.StartCoroutine(this.husband_move_cr()));
	}

	// Token: 0x060025D3 RID: 9683 RVA: 0x000C7B98 File Offset: 0x000C5D98
	public IEnumerator husband_move_cr()
	{
		bool movingRight = true;
		float start = 0f;
		float t = 0f;
		float time = 2f;
		float end = 0f;
		float moveOffset = 400f;
		yield return this.husband.WaitForAnimationToStart(this, "Move", false);
		while (this.husbandMoving)
		{
			yield return null;
			t = 0f;
			start = this.husband.transform.position.x;
			if (movingRight)
			{
				end = 640f - moveOffset;
			}
			else
			{
				end = -640f + moveOffset;
			}
			while (t < time && this.husbandMoving)
			{
				while (this.husband.GetCurrentAnimatorStateInfo(0).IsName("OhNo") || this.husband.GetCurrentAnimatorStateInfo(0).IsName("Yay"))
				{
					yield return null;
				}
				float val = t / time;
				this.husband.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			if (this.husbandMoving)
			{
				this.husband.transform.SetPosition(new float?(end), null, null);
			}
			movingRight = !movingRight;
			yield return null;
		}
		if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			t = 0f;
			time = 0.3f;
			start = this.husband.transform.position.x;
			while (t < time)
			{
				t += CupheadTime.Delta;
				this.husband.transform.SetPosition(new float?(Mathf.Lerp(start, 0f, t / time)), null, null);
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x060025D4 RID: 9684 RVA: 0x000C7BB4 File Offset: 0x000C5DB4
	public void PlayYay()
	{
		if (this.husbandMoving && !AudioManager.CheckIfPlaying("sally_bg_church_fiance_yay"))
		{
			AudioManager.Play("sally_bg_church_fiance_yay");
			this.emitAudioFromObject.Add("sally_bg_church_fiance_yay");
			this.husband.Play("Yay");
		}
	}

	// Token: 0x060025D5 RID: 9685 RVA: 0x000C7C08 File Offset: 0x000C5E08
	public IEnumerator cupid_check_falling(SallyStagePlayLevelBackgroundHandler.Cupid cupid)
	{
		LevelProperties.SallyStagePlay.General p = this.properties.CurrentState.general;
		for (;;)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (player2 != null && !player2.IsDead)
			{
				if (player.IsDead)
				{
					if (player2.transform.parent != cupid.cupidTransform && cupid.cupidTransform.position.y >= p.cupidDropMaxY)
					{
						if (cupid.cupidTransform.position.y < cupid.startPosition.y)
						{
							cupid.cupidTransform.position += Vector3.up * p.cupidMoveSpeed * CupheadTime.Delta;
						}
						yield return null;
					}
					else if (cupid.cupidTransform.position.y > p.cupidDropMaxY)
					{
						cupid.cupidTransform.position += Vector3.down * p.cupidMoveSpeed * CupheadTime.Delta;
					}
				}
				else if (player.transform.parent != cupid.cupidTransform && player2.transform.parent != cupid.cupidTransform && cupid.cupidTransform.position.y >= p.cupidDropMaxY)
				{
					if (cupid.cupidTransform.position.y < cupid.startPosition.y)
					{
						cupid.cupidTransform.position += Vector3.up * p.cupidMoveSpeed * CupheadTime.Delta;
					}
					yield return null;
				}
				else if (cupid.cupidTransform.position.y > p.cupidDropMaxY)
				{
					cupid.cupidTransform.position += Vector3.down * p.cupidMoveSpeed * CupheadTime.Delta;
				}
			}
			else if (player.transform.parent != cupid.cupidTransform && cupid.cupidTransform.position.y >= p.cupidDropMaxY)
			{
				if (cupid.cupidTransform.position.y < cupid.startPosition.y)
				{
					cupid.cupidTransform.position += Vector3.up * p.cupidMoveSpeed * CupheadTime.Delta;
				}
				yield return null;
			}
			else if (cupid.cupidTransform.position.y > p.cupidDropMaxY)
			{
				cupid.cupidTransform.position += Vector3.down * p.cupidMoveSpeed * CupheadTime.Delta;
			}
			if (cupid.cupidTransform.position.y <= p.cupidDropMaxY)
			{
				if (!cupid.playSound)
				{
					AudioManager.Play("sally_platform_cherub_full_travel");
					this.emitAudioFromObject.Add("sally_platform_cherub_full_travel");
					cupid.playSound = true;
				}
				cupid.acceptableLevel = true;
			}
			else
			{
				cupid.acceptableLevel = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025D6 RID: 9686 RVA: 0x000C7C2C File Offset: 0x000C5E2C
	public IEnumerator check_bools_cr()
	{
		bool chandelierWarning = false;
		while (!this.cupids[0].acceptableLevel || !this.cupids[1].acceptableLevel)
		{
			if ((this.cupids[0].acceptableLevel || this.cupids[1].acceptableLevel) && !chandelierWarning)
			{
				chandelierWarning = true;
				base.StartCoroutine(this.chandelier_cr(true));
			}
			yield return null;
		}
		base.StartCoroutine(this.chandelier_cr(false));
		SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE = true;
		float clamp = 20f;
		while (this.husband.transform.position.x > clamp || this.husband.transform.position.x < -clamp)
		{
			yield return null;
		}
		this.husbandMoving = false;
		this.husband.SetTrigger("Dead");
		this.properties.DealDamageToNextNamedState();
		yield return CupheadTime.WaitForSeconds(this, 0.75f);
		this.dropChandelier = true;
		yield return null;
		float t = 0f;
		float frameTime = 0f;
		float moveTime = 0.3f;
		Vector3 start = new Vector3(this.chandelierStartPosX, this.chandelier.transform.position.y);
		Vector3 end = new Vector3(this.chandelierStartPosX, this.sallyBackground.transform.position.y - 70f);
		while (t < moveTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				float num = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / moveTime);
				this.chandelier.transform.position = Vector2.Lerp(start, end, num);
				frameTime -= 0.0416666679f;
			}
			yield return null;
		}
		yield return null;
		CupheadLevelCamera.Current.Shake(10f, 0.4f, false);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x060025D7 RID: 9687 RVA: 0x000C7C48 File Offset: 0x000C5E48
	public IEnumerator chandelier_cr(bool isWarning)
	{
		float t = 0f;
		this.chandelier.GetComponent<Animator>().Play("Shake");
		while (!this.dropChandelier)
		{
			t += CupheadTime.Delta;
			if (t > 0.8f && isWarning)
			{
				break;
			}
			yield return null;
		}
		if (isWarning)
		{
			this.chandelier.GetComponent<Animator>().SetTrigger("OnSlump");
			AudioManager.Play("sally_chandelier_warning");
		}
		else
		{
			this.chandelier.GetComponent<Animator>().Play("Off");
			AudioManager.Play("sally_chandelier_impact");
		}
		yield return null;
		yield break;
	}

	// Token: 0x060025D8 RID: 9688 RVA: 0x0001FC8C File Offset: 0x0001DE8C
	public void OnPhase2()
	{
		if (!SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			base.StartCoroutine(this.just_married_cr());
		}
		else
		{
			base.StartCoroutine(this.crying_cr());
		}
	}

	// Token: 0x060025D9 RID: 9689 RVA: 0x000C7C6C File Offset: 0x000C5E6C
	public void RollUpCupids()
	{
		foreach (SallyStagePlayLevelBackgroundHandler.Cupid cupid in this.cupids)
		{
			base.StartCoroutine(this.roll_up_cupids_cr(cupid));
		}
	}

	// Token: 0x060025DA RID: 9690 RVA: 0x000C7CA8 File Offset: 0x000C5EA8
	public IEnumerator roll_up_cupids_cr(SallyStagePlayLevelBackgroundHandler.Cupid cupid)
	{
		float t = 0f;
		float frameTime = 0f;
		float moveTime = 3.5f;
		Vector3 end = new Vector3(cupid.cupidTransform.position.x, 800f);
		cupid.cupidTransform.GetComponent<Collider2D>().enabled = false;
		while (t < moveTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				float num = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / moveTime);
				cupid.cupidTransform.position = Vector2.Lerp(cupid.cupidTransform.position, end, num);
				frameTime -= 0.0416666679f;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x060025DB RID: 9691 RVA: 0x000C7CC4 File Offset: 0x000C5EC4
	public IEnumerator just_married_cr()
	{
		float t = 0f;
		float frameTime = 0f;
		float moveTime = 1.5f;
		Vector3 end = new Vector3(this.husband.transform.position.x, this.car.position.y);
		while (this.sally.state != SallyStagePlayLevelSally.State.Transition)
		{
			yield return null;
		}
		this.sally.animator.SetTrigger("OnPhase2");
		this.priest.SetTrigger("CarAppeared");
		this.husbandMoving = false;
		this.husband.SetTrigger("Married");
		yield return this.husband.WaitForAnimationToEnd(this, "Tada_Start", false, true);
		this.sallyBackground.Play("Wave");
		AudioManager.Play("sally_ph1_bg_car_enter");
		this.sallyBackground.transform.parent = this.car.transform;
		this.sallyBackground.transform.position = this.carRoot.transform.position;
		this.sallyBackground.transform.position = this.carRoot.transform.position;
		while (t < moveTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				float num = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / moveTime);
				this.car.transform.position = Vector2.Lerp(this.car.transform.position, end, num);
				frameTime -= 0.0416666679f;
			}
			yield return null;
		}
		AudioManager.PlayLoop("sally_ph1_bg_car_loop");
		this.husband.SetTrigger("Drive");
		this.husband.transform.parent = this.car.transform;
		AudioManager.Play("sally_ph1_bg_car_exit");
		AudioManager.Stop("sally_ph1_bg_car_loop");
		t = 0f;
		frameTime = 0f;
		moveTime = 2f;
		end.x = 1140f;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		while (t < moveTime)
		{
			frameTime += CupheadTime.Delta;
			t += CupheadTime.Delta;
			if (frameTime > 0.0416666679f)
			{
				float num2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / moveTime);
				this.car.transform.position = Vector2.Lerp(this.car.transform.position, end, num2);
				frameTime -= 0.0416666679f;
			}
			yield return null;
		}
		this.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.House);
		yield return CupheadTime.WaitForSeconds(this, this.curtainWaitTime);
		this.HaltCoroutines();
		this.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.House);
		this.residence.StartPhase2(this.parent, this.properties);
		this.sally.StartPhase2();
		this.StartPeeking();
		yield return null;
		yield break;
	}

	// Token: 0x060025DC RID: 9692 RVA: 0x000C7CE0 File Offset: 0x000C5EE0
	public IEnumerator cry_sound_cr()
	{
		yield return this.sallyBackground.WaitForAnimationToEnd(this, "Run", false, true);
		AudioManager.Play("sally_cry");
		this.emitAudioFromObject.Add("sally_cry");
		yield return null;
		yield break;
	}

	// Token: 0x060025DD RID: 9693 RVA: 0x000C7CFC File Offset: 0x000C5EFC
	public IEnumerator crying_cr()
	{
		while (this.sally.state != SallyStagePlayLevelSally.State.Transition)
		{
			yield return null;
		}
		this.sally.animator.SetTrigger("OnPhase2");
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.priest.Play("Tuck_Down_Disappear");
		base.StartCoroutine(this.cry_sound_cr());
		this.sallyBackground.Play("Run");
		yield return this.sallyBackground.WaitForAnimationToEnd(this, "Run_End", false, true);
		this.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Nunnery);
		SallyStagePlayHusbandExplosion ex = this.husband.GetComponent<SallyStagePlayHusbandExplosion>();
		if (ex != null)
		{
			ex.StopExplosions();
		}
		yield return CupheadTime.WaitForSeconds(this, this.curtainWaitTime);
		this.HaltCoroutines();
		this.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Nunnery);
		this.residence.StartPhase2(this.parent, this.properties);
		this.sally.StartPhase2();
		this.StartPeeking();
		yield return null;
		yield break;
	}

	// Token: 0x060025DE RID: 9694 RVA: 0x000C7D18 File Offset: 0x000C5F18
	public void HaltCoroutines()
	{
		foreach (Coroutine coroutine in this.phaseDependentCoroutines)
		{
			base.StopCoroutine(coroutine);
		}
		this.phaseDependentCoroutines.Clear();
	}

	// Token: 0x060025DF RID: 9695 RVA: 0x000C7D80 File Offset: 0x000C5F80
	public void StartPeeking()
	{
		if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.peek_cr(this.priestPhase2, this.priestRoots)));
		}
		else
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.peek_cr(this.husbandPhase2, this.husbandRoots)));
		}
	}

	// Token: 0x060025E0 RID: 9696 RVA: 0x000C7DE4 File Offset: 0x000C5FE4
	public IEnumerator peek_cr(Animator animator, Transform[] roots)
	{
		float waitTime = Random.Range(8f, 20f);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			yield return null;
			int rootChosen = Random.Range(0, roots.Length);
			animator.GetComponent<Transform>().position = roots[rootChosen].position;
			animator.GetComponent<Transform>().SetScale(new float?(roots[rootChosen].localScale.x), null, null);
			if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE && rootChosen == 1)
			{
				animator.SetBool("isDiag", true);
			}
			else
			{
				animator.SetBool("isDiag", Rand.Bool());
			}
			animator.SetTrigger("Peek");
			waitTime = Random.Range(8f, 20f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060025E1 RID: 9697 RVA: 0x000C7E10 File Offset: 0x000C6010
	public void OnPhase3()
	{
		this.HaltCoroutines();
		foreach (Transform swing in this.purgSwingies)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.swing_cr(swing)));
		}
		if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			this.priestPhase2.Play("Shake");
		}
		else
		{
			this.husbandPhase2.Play("Cry");
		}
		base.StartCoroutine(this.phase3_background_cr());
	}

	// Token: 0x060025E2 RID: 9698 RVA: 0x000C7E98 File Offset: 0x000C6098
	public IEnumerator phase3_background_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Purgatory);
		yield return CupheadTime.WaitForSeconds(this, this.curtainWaitTime);
		this.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Purgatory);
		yield return null;
		yield break;
	}

	// Token: 0x060025E3 RID: 9699 RVA: 0x000C7EB4 File Offset: 0x000C60B4
	public void OnPhase4()
	{
		this.HaltCoroutines();
		foreach (Transform swing in this.finaleSwingies)
		{
			this.phaseDependentCoroutines.Add(base.StartCoroutine(this.swing_cr(swing)));
		}
		base.StartCoroutine(this.phase4_background_cr());
		if (SallyStagePlayLevelBackgroundHandler.HUSBAND_GONE)
		{
			this.husbandDeadObject.SetActive(true);
		}
		else
		{
			this.husbandAliveObject.SetActive(true);
		}
	}

	// Token: 0x060025E4 RID: 9700 RVA: 0x000C7F34 File Offset: 0x000C6134
	public IEnumerator phase4_background_cr()
	{
		this.CloseCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale);
		yield return CupheadTime.WaitForSeconds(this, this.curtainWaitTime);
		this.OpenCurtain(SallyStagePlayLevelBackgroundHandler.Backgrounds.Finale);
		yield return null;
		yield break;
	}

	// Token: 0x04001F45 RID: 8005
	public const float FRAME_TIME = 0.0416666679f;

	// Token: 0x04001F46 RID: 8006
	[Header("Main")]
	[SerializeField]
	public SallyStagePlayLevelSally sally;

	// Token: 0x04001F47 RID: 8007
	[SerializeField]
	public Transform curtain;

	// Token: 0x04001F48 RID: 8008
	[SerializeField]
	public Transform curtainSprite;

	// Token: 0x04001F49 RID: 8009
	[SerializeField]
	public Transform curtainShadow;

	// Token: 0x04001F4A RID: 8010
	[SerializeField]
	public Transform curtainUpRoot;

	// Token: 0x04001F4B RID: 8011
	[SerializeField]
	public SpriteRenderer[] flickeringLights;

	// Token: 0x04001F4C RID: 8012
	[SerializeField]
	public SallyStagePlayApplauseHandler applauseHandler;

	// Token: 0x04001F4D RID: 8013
	[Header("Church")]
	[SerializeField]
	public Transform[] churchSwingies;

	// Token: 0x04001F4E RID: 8014
	[SerializeField]
	public SallyStagePlayLevelBackgroundHandler.Cupid[] cupids;

	// Token: 0x04001F4F RID: 8015
	[SerializeField]
	public Animator priest;

	// Token: 0x04001F50 RID: 8016
	[SerializeField]
	public Animator husband;

	// Token: 0x04001F51 RID: 8017
	[SerializeField]
	public Animator sallyBackground;

	// Token: 0x04001F52 RID: 8018
	[SerializeField]
	public Transform car;

	// Token: 0x04001F53 RID: 8019
	[SerializeField]
	public Transform carRoot;

	// Token: 0x04001F54 RID: 8020
	[SerializeField]
	public Transform chandelier;

	// Token: 0x04001F55 RID: 8021
	[SerializeField]
	public Transform sallyRoot;

	// Token: 0x04001F56 RID: 8022
	[Header("Residence")]
	[SerializeField]
	public SallyStagePlayLevelHouse residence;

	// Token: 0x04001F57 RID: 8023
	[SerializeField]
	public Animator husbandPhase2;

	// Token: 0x04001F58 RID: 8024
	[SerializeField]
	public Transform[] husbandRoots;

	// Token: 0x04001F59 RID: 8025
	[SerializeField]
	public Animator priestPhase2;

	// Token: 0x04001F5A RID: 8026
	[SerializeField]
	public Transform[] priestRoots;

	// Token: 0x04001F5B RID: 8027
	[Header("Purgatory")]
	[SerializeField]
	public Transform[] purgSwingies;

	// Token: 0x04001F5C RID: 8028
	[Header("Finale")]
	[SerializeField]
	public Transform[] finaleSwingies;

	// Token: 0x04001F5D RID: 8029
	[SerializeField]
	public GameObject husbandAliveObject;

	// Token: 0x04001F5E RID: 8030
	[SerializeField]
	public GameObject husbandDeadObject;

	// Token: 0x04001F5F RID: 8031
	[Header("Backgrounds")]
	[SerializeField]
	public GameObject[] backgrounds;

	// Token: 0x04001F60 RID: 8032
	public float fadeWaitMinSecond = 8f;

	// Token: 0x04001F61 RID: 8033
	public float fadeWaitMaxSecond = 25f;

	// Token: 0x04001F62 RID: 8034
	public float curtainWaitTime = 2.5f;

	// Token: 0x04001F63 RID: 8035
	public float chandelierStartPosX;

	// Token: 0x04001F64 RID: 8036
	public bool husbandMoving = true;

	// Token: 0x04001F65 RID: 8037
	public bool dropChandelier;

	// Token: 0x04001F66 RID: 8038
	public Vector3 curtainStartPos;

	// Token: 0x04001F67 RID: 8039
	public Vector3 curtainShadowStartPos;

	// Token: 0x04001F68 RID: 8040
	public LevelProperties.SallyStagePlay properties;

	// Token: 0x04001F69 RID: 8041
	public SallyStagePlayLevel parent;

	// Token: 0x04001F6A RID: 8042
	public List<Coroutine> phaseDependentCoroutines;

	// Token: 0x02000EE9 RID: 3817
	public enum Backgrounds
	{
		// Token: 0x04006B80 RID: 27520
		Church,
		// Token: 0x04006B81 RID: 27521
		House,
		// Token: 0x04006B82 RID: 27522
		Nunnery,
		// Token: 0x04006B83 RID: 27523
		Purgatory,
		// Token: 0x04006B84 RID: 27524
		Finale
	}

	// Token: 0x02000EEA RID: 3818
	[Serializable]
	public class Cupid
	{
		// Token: 0x04006B85 RID: 27525
		public Transform cupidTransform;

		// Token: 0x04006B86 RID: 27526
		public Vector3 startPosition;

		// Token: 0x04006B87 RID: 27527
		public bool acceptableLevel;

		// Token: 0x04006B88 RID: 27528
		public bool playSound;
	}
}
