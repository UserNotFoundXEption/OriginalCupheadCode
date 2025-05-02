using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000481 RID: 1153
public class MapLevelDependentObstacle : AbstractMapLevelDependentEntity
{
	// Token: 0x060030C4 RID: 12484 RVA: 0x0002876C File Offset: 0x0002696C
	public override void Start()
	{
		base.Start();
	}

	// Token: 0x060030C5 RID: 12485 RVA: 0x00028774 File Offset: 0x00026974
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

	// Token: 0x060030C6 RID: 12486 RVA: 0x000287B0 File Offset: 0x000269B0
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

	// Token: 0x060030C7 RID: 12487 RVA: 0x000287EC File Offset: 0x000269EC
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

	// Token: 0x060030C8 RID: 12488 RVA: 0x00028828 File Offset: 0x00026A28
	public override void DoTransition()
	{
		base.StartCoroutine(this.transition_cr());
	}

	// Token: 0x060030C9 RID: 12489 RVA: 0x00028837 File Offset: 0x00026A37
	public void OnChange()
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

	// Token: 0x060030CA RID: 12490 RVA: 0x000E7698 File Offset: 0x000E5898
	public IEnumerator transition_cr()
	{
		AudioManager.Play("world_level_bridge_building_poof");
		this.poofPrefab.Create(this.poofRoot.position, new Vector3(0.01f, 0.01f, 1f));
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		SpriteRenderer[] sprites = base.GetComponentsInChildren<SpriteRenderer>(true);
		foreach (SpriteRenderer spriteRenderer in sprites)
		{
			spriteRenderer.material = this.flashMaterial;
		}
		if (!this.seeDisabledOnlyDuringTransition)
		{
			this.ToEnable.SetActive(true);
		}
		if (this.seeEnableOnlyDuringTransition)
		{
			this.ToDisable.SetActive(false);
		}
		yield return CupheadTime.WaitForSeconds(this, 0.04f);
		for (int i = 0; i < 4; i++)
		{
			foreach (SpriteRenderer spriteRenderer2 in sprites)
			{
				spriteRenderer2.color = Color.white;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.04f);
			foreach (SpriteRenderer spriteRenderer3 in sprites)
			{
				spriteRenderer3.color = Color.black;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.04f);
		}
		if (this.seeDisabledOnlyDuringTransition)
		{
			this.ToEnable.SetActive(true);
		}
		if (!this.seeEnableOnlyDuringTransition && this.ToDisable != null)
		{
			this.ToDisable.SetActive(false);
		}
		base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		yield break;
	}

	// Token: 0x04002846 RID: 10310
	[SerializeField]
	public GameObject ToEnable;

	// Token: 0x04002847 RID: 10311
	[SerializeField]
	public bool seeEnableOnlyDuringTransition;

	// Token: 0x04002848 RID: 10312
	[SerializeField]
	public GameObject ToDisable;

	// Token: 0x04002849 RID: 10313
	[SerializeField]
	public bool seeDisabledOnlyDuringTransition;

	// Token: 0x0400284A RID: 10314
	[SerializeField]
	public Effect poofPrefab;

	// Token: 0x0400284B RID: 10315
	[SerializeField]
	public Material flashMaterial;

	// Token: 0x0400284C RID: 10316
	[SerializeField]
	public Transform poofRoot;

	// Token: 0x0400284D RID: 10317
	[SerializeField]
	public bool DontPlayPoofSFX;
}
