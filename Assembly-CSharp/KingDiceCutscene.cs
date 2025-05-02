using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020000C4 RID: 196
public class KingDiceCutscene : Cutscene
{
	// Token: 0x060008F1 RID: 2289 RVA: 0x00076E48 File Offset: 0x00075048
	public override void Awake()
	{
		base.Awake();
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Normal) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Normal) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Normal))
		{
			base.StartCoroutine(this.have_all_contracts_cr());
		}
		else
		{
			base.StartCoroutine(this.missing_contracts_cr());
		}
	}

	// Token: 0x060008F2 RID: 2290 RVA: 0x00076ED8 File Offset: 0x000750D8
	public IEnumerator have_all_contracts_cr()
	{
		base.animator.Play("All_Contracts");
		int numScreens = 2;
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		for (int i = 0; i < numScreens; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.25f);
			this.arrowVisible = true;
			while (!this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			base.animator.SetTrigger("Continue");
		}
		SceneLoader.LoadScene(Scenes.scene_level_dice_palace_main, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x060008F3 RID: 2291 RVA: 0x00076EF4 File Offset: 0x000750F4
	public IEnumerator missing_contracts_cr()
	{
		base.animator.Play("Missing_Contracts");
		int numScreens = 3;
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		for (int i = 0; i < numScreens; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.25f);
			this.arrowVisible = true;
			while (!this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			base.animator.SetTrigger("Continue");
		}
		SceneLoader.LoadLastMap();
		yield return null;
		yield break;
	}

	// Token: 0x060008F4 RID: 2292 RVA: 0x00076F10 File Offset: 0x00075110
	public void Update()
	{
		if (this.arrowVisible)
		{
			this.arrowTransparency = Mathf.Clamp01(this.arrowTransparency + Time.deltaTime / 0.25f);
		}
		else
		{
			this.arrowTransparency = 0f;
		}
		this.arrow.color = new Color(1f, 1f, 1f, this.arrowTransparency);
	}

	// Token: 0x060008F5 RID: 2293 RVA: 0x000087E3 File Offset: 0x000069E3
	public override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x040006CA RID: 1738
	public CupheadInput.AnyPlayerInput input;

	// Token: 0x040006CB RID: 1739
	[SerializeField]
	public Image arrow;

	// Token: 0x040006CC RID: 1740
	public float arrowTransparency;

	// Token: 0x040006CD RID: 1741
	public bool arrowVisible;
}
