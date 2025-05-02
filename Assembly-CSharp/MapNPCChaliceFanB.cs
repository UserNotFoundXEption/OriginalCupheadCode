using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class MapNPCChaliceFanB : AbstractMonoBehaviour
{
	// Token: 0x06003110 RID: 12560 RVA: 0x000E8858 File Offset: 0x000E6A58
	public void Start()
	{
		if (PlayerData.Data.hasTalkedToChaliceFan)
		{
			Dialoguer.SetGlobalFloat(25, 1f);
		}
		this.AddDialoguerEvents();
		int num = 0;
		for (int i = 0; i < Level.chaliceLevels.Length; i++)
		{
			if (PlayerData.Data.GetLevelData(Level.chaliceLevels[i]).completedAsChaliceP1)
			{
				this.lineSprites[i].enabled = true;
			}
		}
		for (int j = 0; j < Level.chaliceLevels.Length; j++)
		{
			if (PlayerData.Data.GetLevelData(Level.chaliceLevels[j]).completedAsChaliceP1)
			{
				num++;
			}
			else if (this.undefeatedBoss == Levels.Test)
			{
				this.undefeatedBoss = Level.chaliceLevels[j];
			}
		}
		if (num == Level.chaliceLevels.Length)
		{
			Dialoguer.SetGlobalFloat(25, 2f);
			this.campfire.gameObject.SetActive(true);
			base.StartCoroutine(this.campfire_smoke_cr());
			this.questComplete = true;
		}
		else
		{
			this.SetBossRefText(this.undefeatedBoss);
		}
		base.StartCoroutine(this.blink_cr());
	}

	// Token: 0x06003111 RID: 12561 RVA: 0x000E897C File Offset: 0x000E6B7C
	public void SetBossRefText(Levels level)
	{
		TranslationElement translationElement = Localization.Find(level.ToString() + "Reference");
		SpeechBubble.Instance.setBossRefText = translationElement.translation.text;
	}

	// Token: 0x06003112 RID: 12562 RVA: 0x00028D36 File Offset: 0x00026F36
	public void UpdateBossRef()
	{
		this.SetBossRefText(this.undefeatedBoss);
	}

	// Token: 0x06003113 RID: 12563 RVA: 0x00028D44 File Offset: 0x00026F44
	public void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x06003114 RID: 12564 RVA: 0x00028D4C File Offset: 0x00026F4C
	public void AddDialoguerEvents()
	{
		Localization.OnLanguageChangedEvent += this.UpdateBossRef;
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded += this.OnDialogueEnded;
	}

	// Token: 0x06003115 RID: 12565 RVA: 0x00028D8B File Offset: 0x00026F8B
	public void RemoveDialoguerEvents()
	{
		Localization.OnLanguageChangedEvent -= this.UpdateBossRef;
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded -= this.OnDialogueEnded;
	}

	// Token: 0x06003116 RID: 12566 RVA: 0x00028DCA File Offset: 0x00026FCA
	public void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "MetChaliceFan")
		{
			PlayerData.Data.hasTalkedToChaliceFan = true;
			PlayerData.SaveCurrentFile();
			Dialoguer.SetGlobalFloat(25, 1f);
		}
	}

	// Token: 0x06003117 RID: 12567 RVA: 0x000E89C0 File Offset: 0x000E6BC0
	public IEnumerator blink_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.blinkRange.RandomFloat());
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f > 0.1f)
			{
				yield return null;
			}
			this.blinkRend.enabled = true;
			while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < 0.9f)
			{
				yield return null;
			}
			this.blinkRend.enabled = false;
		}
		yield break;
	}

	// Token: 0x06003118 RID: 12568 RVA: 0x000E89DC File Offset: 0x000E6BDC
	public IEnumerator campfire_smoke_cr()
	{
		this.campfire.SetBool("SmokeL", true);
		yield return CupheadTime.WaitForSeconds(this, Random.Range(0.5f, 1f));
		this.campfire.SetBool("SmokeR", true);
		for (;;)
		{
			if (!this.campfire.GetCurrentAnimatorStateInfo(1).IsName("None") || !this.campfire.GetCurrentAnimatorStateInfo(2).IsName("None"))
			{
				yield return CupheadTime.WaitForSeconds(this, Random.Range(1.5f, 3f));
			}
			this.campfire.SetBool("SmokeL", Rand.Bool());
			this.campfire.SetBool("SmokeR", Rand.Bool());
			if (this.campfire.GetCurrentAnimatorStateInfo(1).IsName("None"))
			{
				this.campfire.SetBool("SmokeL", true);
			}
			if (this.campfire.GetCurrentAnimatorStateInfo(2).IsName("None"))
			{
				this.campfire.SetBool("SmokeR", true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003119 RID: 12569 RVA: 0x000E89F8 File Offset: 0x000E6BF8
	public void OnDialogueEnded()
	{
		if (this.questComplete && !PlayerData.Data.unlockedChaliceRecolor)
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.ChaliceFan);
			PlayerData.Data.unlockedChaliceRecolor = true;
			PlayerData.SaveCurrentFile();
			MapUI.Current.Refresh();
		}
	}

	// Token: 0x0400287D RID: 10365
	public const int CHALICEFANBSTATE_INDEX = 25;

	// Token: 0x0400287E RID: 10366
	[SerializeField]
	public SpriteRenderer[] lineSprites;

	// Token: 0x0400287F RID: 10367
	[SerializeField]
	public SpriteRenderer blinkRend;

	// Token: 0x04002880 RID: 10368
	[SerializeField]
	public MinMax blinkRange = new MinMax(3f, 5f);

	// Token: 0x04002881 RID: 10369
	[SerializeField]
	public Animator campfire;

	// Token: 0x04002882 RID: 10370
	public Levels undefeatedBoss = Levels.Test;

	// Token: 0x04002883 RID: 10371
	public bool questComplete;
}
