using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001FB RID: 507
public class DicePalaceMainLevelKingDice : LevelProperties.DicePalaceMain.Entity
{
	// Token: 0x06001760 RID: 5984 RVA: 0x000A17D0 File Offset: 0x0009F9D0
	public void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiver.enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		Level.Current.OnWinEvent += this.OnDeath;
		AudioManager.Play("king_dice_intro");
		this.emitAudioFromObject.Add("king_dice_intro");
	}

	// Token: 0x06001761 RID: 5985 RVA: 0x00013E77 File Offset: 0x00012077
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001762 RID: 5986 RVA: 0x000A1848 File Offset: 0x0009FA48
	public void StartKingDiceBattle()
	{
		AudioManager.FadeBGMVolume(0f, 0.5f, true);
		AudioManager.Play("king_dice_trans");
		AudioManager.PlayBGMPlaylistManually(false);
		base.animator.SetBool("IsAttacking", true);
		base.animator.SetBool("IsBattling", true);
		LevelIntroAnimation levelIntroAnimation = LevelIntroAnimation.Create(null);
		levelIntroAnimation.Play();
		base.StartCoroutine(this.cards_cr());
	}

	// Token: 0x06001763 RID: 5987 RVA: 0x00013E8A File Offset: 0x0001208A
	public void RevealSFX()
	{
		AudioManager.Play("king_dice_reveal");
		this.emitAudioFromObject.Add("king_dice_reveal");
	}

	// Token: 0x06001764 RID: 5988 RVA: 0x000A18B4 File Offset: 0x0009FAB4
	public void RevealDice()
	{
		DicePalaceMainLevel dicePalaceMainLevel = Level.Current as DicePalaceMainLevel;
		dicePalaceMainLevel.GameManager.RevealDice();
	}

	// Token: 0x06001765 RID: 5989 RVA: 0x000A18D8 File Offset: 0x0009FAD8
	public IEnumerator cards_cr()
	{
		LevelProperties.DicePalaceMain.Cards p = base.properties.CurrentState.cards;
		int cardIndex = Random.Range(0, p.cardString.Length);
		string[] sideString = p.cardSideOrder.GetRandom<string>().Split(new char[]
		{
			','
		});
		int suitIndex = Random.Range(0, 3);
		int sideIndex = Random.Range(0, sideString.Length);
		bool onLeft = false;
		Vector3 rootPos = Vector3.zero;
		this.damageReceiver.enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		for (;;)
		{
			string[] cardString = p.cardString[cardIndex].Split(new char[]
			{
				','
			});
			if (sideString[sideIndex][0] == 'L')
			{
				onLeft = true;
				rootPos = this.leftRoot.transform.position;
			}
			else if (sideString[sideIndex][0] == 'R')
			{
				onLeft = false;
				rootPos = this.rightRoot.transform.position;
			}
			else
			{
				Debug.LogError("Invalid pattern string", null);
			}
			base.animator.SetBool("OnLeftAttack", onLeft);
			yield return base.animator.WaitForAnimationToEnd(this, (!onLeft) ? "Attack_Right" : "Attack_Left", false, true);
			AudioManager.PlayLoop("king_dice_march_loop");
			this.emitAudioFromObject.Add("king_dice_march_loop");
			base.StartCoroutine(this.kd_laugh_cr());
			for (int i = 0; i < cardString.Length; i++)
			{
				if (cardString[i][0] == 'R')
				{
					DicePalaceMainLevelCard dicePalaceMainLevelCard = this.cardRegular.Create(rootPos, p, onLeft);
					dicePalaceMainLevelCard.transform.SetScale(new float?((float)((!onLeft) ? -1 : 1)), null, null);
					dicePalaceMainLevelCard.GetComponent<SpriteRenderer>().sortingOrder = i;
					suitIndex = (suitIndex + 1) % 3;
				}
				else if (cardString[i][0] == 'P')
				{
					DicePalaceMainLevelCard dicePalaceMainLevelCard2 = this.cardPink.Create(rootPos, p, onLeft);
					dicePalaceMainLevelCard2.transform.SetScale(new float?((float)((!onLeft) ? -1 : 1)), null, null);
					dicePalaceMainLevelCard2.GetComponent<SpriteRenderer>().sortingOrder = i;
				}
				else
				{
					Debug.LogError("Invalid pattern string", null);
				}
				yield return CupheadTime.WaitForSeconds(this, p.cardDelay);
			}
			AudioManager.Stop("king_dice_march_loop");
			base.animator.SetBool("IsAttacking", false);
			yield return CupheadTime.WaitForSeconds(this, p.hesitate);
			base.animator.SetBool("IsAttacking", true);
			sideIndex = (sideIndex + 1) % sideString.Length;
			cardIndex = (cardIndex + 1) % p.cardString.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001766 RID: 5990 RVA: 0x00013EA6 File Offset: 0x000120A6
	public void AttackSFX()
	{
		AudioManager.PlayLoop("king_dice_attack");
		this.emitAudioFromObject.Add("king_dice_attack");
	}

	// Token: 0x06001767 RID: 5991 RVA: 0x00013EC2 File Offset: 0x000120C2
	public void IntroSFX()
	{
		AudioManager.Play("king_dice_intro");
		this.emitAudioFromObject.Add("king_dice_intro");
	}

	// Token: 0x06001768 RID: 5992 RVA: 0x00013EDE File Offset: 0x000120DE
	public void VoxCurious()
	{
		AudioManager.Play("vox_curious");
		this.emitAudioFromObject.Add("vox_curious");
	}

	// Token: 0x06001769 RID: 5993 RVA: 0x00013EFA File Offset: 0x000120FA
	public void AttackSFXStop()
	{
		AudioManager.Stop("king_dice_attack");
	}

	// Token: 0x0600176A RID: 5994 RVA: 0x000A18F4 File Offset: 0x0009FAF4
	public void OnDeath()
	{
		AudioManager.PlayLoop("king_dice_death");
		AudioManager.Play("vox_death");
		this.emitAudioFromObject.Add("vox_death");
		base.animator.SetTrigger("OnDeath");
		this.StopAllCoroutines();
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		component.sortingLayerName = "Background";
		component.sortingOrder = 100;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x0600176B RID: 5995 RVA: 0x000A1964 File Offset: 0x0009FB64
	public IEnumerator kd_laugh_cr()
	{
		MinMax delay = new MinMax(1f, 3.5f);
		while (base.animator.GetBool("IsAttacking"))
		{
			AudioManager.Play("king_dice_attack_vox");
			this.emitAudioFromObject.Add("king_dice_attack_vox");
			while (AudioManager.CheckIfPlaying("king_dice_attack_vox"))
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, delay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x0600176C RID: 5996 RVA: 0x00013F06 File Offset: 0x00012106
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.cardPink = null;
		this.cardRegular = null;
	}

	// Token: 0x04001308 RID: 4872
	[SerializeField]
	public Transform rightRoot;

	// Token: 0x04001309 RID: 4873
	[SerializeField]
	public Transform leftRoot;

	// Token: 0x0400130A RID: 4874
	[SerializeField]
	public DicePalaceMainLevelCard cardRegular;

	// Token: 0x0400130B RID: 4875
	[SerializeField]
	public DicePalaceMainLevelCard cardPink;

	// Token: 0x0400130C RID: 4876
	public DamageReceiver damageReceiver;
}
