using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000486 RID: 1158
public class MapLevelMausoleumEntity : AbstractMapLevelDependentEntity
{
	// Token: 0x060030D9 RID: 12505 RVA: 0x00028911 File Offset: 0x00026B11
	public override void OnConditionNotMet()
	{
		if (this.ToEnable != null)
		{
			this.ToEnable.SetActive(false);
		}
		if (this.ToDisable != null)
		{
			this.ToDisable.SetActive(true);
		}
	}

	// Token: 0x060030DA RID: 12506 RVA: 0x0002894D File Offset: 0x00026B4D
	public override void OnConditionMet()
	{
		if (this.ToEnable != null)
		{
			this.ToEnable.SetActive(false);
		}
		if (this.ToDisable != null)
		{
			this.ToDisable.SetActive(true);
		}
	}

	// Token: 0x060030DB RID: 12507 RVA: 0x00028989 File Offset: 0x00026B89
	public override void OnConditionAlreadyMet()
	{
		if (this.ToEnable != null)
		{
			this.ToEnable.SetActive(true);
		}
		if (this.ToDisable != null)
		{
			this.ToDisable.SetActive(false);
		}
	}

	// Token: 0x060030DC RID: 12508 RVA: 0x000289C5 File Offset: 0x00026BC5
	public override bool ValidateSucess()
	{
		return PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, this.superUnlock) && PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, this.superUnlock);
	}

	// Token: 0x060030DD RID: 12509 RVA: 0x000289F1 File Offset: 0x00026BF1
	public override bool ValidateCondition(Levels level)
	{
		return Level.Won && Level.PreviousLevel == level && Level.SuperUnlocked;
	}

	// Token: 0x060030DE RID: 12510 RVA: 0x00028A19 File Offset: 0x00026C19
	public override void DoTransition()
	{
		base.StartCoroutine(this.transition_cr());
	}

	// Token: 0x060030DF RID: 12511 RVA: 0x000E7DC8 File Offset: 0x000E5FC8
	public IEnumerator transition_cr()
	{
		this.poofPrefab.Create(this.poofRoot.position, new Vector3(0.01f, 0.01f, 1f));
		AudioManager.Play("world_map_mausoleum_destruction");
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		this.ToEnable.SetActive(true);
		this.ToDisable.SetActive(false);
		yield return CupheadTime.WaitForSeconds(this, 0.36f);
		base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		yield break;
	}

	// Token: 0x04002856 RID: 10326
	[SerializeField]
	public GameObject ToEnable;

	// Token: 0x04002857 RID: 10327
	[SerializeField]
	public GameObject ToDisable;

	// Token: 0x04002858 RID: 10328
	[SerializeField]
	public Effect poofPrefab;

	// Token: 0x04002859 RID: 10329
	[SerializeField]
	public Transform poofRoot;

	// Token: 0x0400285A RID: 10330
	[SerializeField]
	public Super superUnlock;
}
