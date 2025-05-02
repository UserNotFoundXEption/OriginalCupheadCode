using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003BE RID: 958
public class TrainLevelTrain : LevelProperties.Train.Entity
{
	// Token: 0x17000334 RID: 820
	// (get) Token: 0x06002A55 RID: 10837 RVA: 0x00023A64 File Offset: 0x00021C64
	// (set) Token: 0x06002A56 RID: 10838 RVA: 0x00023A6C File Offset: 0x00021C6C
	public TrainLevelTrain.State state { get; set; }

	// Token: 0x06002A57 RID: 10839 RVA: 0x000D3BE8 File Offset: 0x000D1DE8
	public override void Awake()
	{
		base.Awake();
		base.transform.SetPosition(new float?(455f), null, null);
	}

	// Token: 0x06002A58 RID: 10840 RVA: 0x00023A75 File Offset: 0x00021C75
	public void OnBlindSpectreDeath()
	{
		base.StartCoroutine(this.blindSpectreDeath_cr());
	}

	// Token: 0x06002A59 RID: 10841 RVA: 0x00023A84 File Offset: 0x00021C84
	public void OnSkeletonDeath()
	{
		base.StartCoroutine(this.skeletonDeath_cr());
	}

	// Token: 0x06002A5A RID: 10842 RVA: 0x00023A93 File Offset: 0x00021C93
	public void OnLollipopsDeath()
	{
		base.StartCoroutine(this.lollipopsDeath_cr());
	}

	// Token: 0x06002A5B RID: 10843 RVA: 0x000D3C24 File Offset: 0x000D1E24
	public IEnumerator blindSpectreDeath_cr()
	{
		this.state = TrainLevelTrain.State.Skeleton;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield return base.TweenPositionX(base.transform.position.x, -960f, 2.5f, EaseUtils.EaseType.easeInOutSine);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (int i = 0; i < this.skeletonCars.Length; i++)
		{
			int i2 = (i != 1) ? 0 : 1;
			this.skeletonCars[i].Explode(i2);
		}
		AudioManager.Play("level_train_top_explode");
		this.skeleton.StartSkeleton();
		yield break;
	}

	// Token: 0x06002A5C RID: 10844 RVA: 0x000D3C40 File Offset: 0x000D1E40
	public IEnumerator skeletonDeath_cr()
	{
		this.state = TrainLevelTrain.State.LollipopGhouls;
		this.ghouls.Setup();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield return base.TweenPositionX(base.transform.position.x, -2358f, 2.5f, EaseUtils.EaseType.easeInOutSine);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.ghouls.StartGhouls();
		yield break;
	}

	// Token: 0x06002A5D RID: 10845 RVA: 0x000D3C5C File Offset: 0x000D1E5C
	public IEnumerator lollipopsDeath_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		yield return base.TweenPositionX(base.transform.position.x, -4816f, 2.5f, EaseUtils.EaseType.easeInSine);
		this.engineCar.PlayRage();
		yield return base.TweenPositionX(base.transform.position.x, -6016f, 2.5f, EaseUtils.EaseType.linear);
		this.engineCar.End();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.engineBoss.StartBoss();
		base.gameObject.SetActive(false);
		yield break;
	}

	// Token: 0x0400234D RID: 9037
	public const float TRAIN_MOVE_TIME = 2.5f;

	// Token: 0x0400234E RID: 9038
	public const float START_X = 455f;

	// Token: 0x0400234F RID: 9039
	public const float SKELETON_X = -960f;

	// Token: 0x04002350 RID: 9040
	public const float GHOUL_X = -2358f;

	// Token: 0x04002351 RID: 9041
	public const float ENGINE_MID_X = -4816f;

	// Token: 0x04002352 RID: 9042
	public const float ENGINE_X = -6016f;

	// Token: 0x04002354 RID: 9044
	[SerializeField]
	public TrainLevelSkeleton skeleton;

	// Token: 0x04002355 RID: 9045
	[SerializeField]
	public TrainLevelPassengerCar[] skeletonCars;

	// Token: 0x04002356 RID: 9046
	[Space(10f)]
	[SerializeField]
	public TrainLevelLollipopGhoulsManager ghouls;

	// Token: 0x04002357 RID: 9047
	[Space(10f)]
	[SerializeField]
	public TrainLevelEngineCar engineCar;

	// Token: 0x04002358 RID: 9048
	[SerializeField]
	public TrainLevelEngineBoss engineBoss;

	// Token: 0x02000FCE RID: 4046
	public enum State
	{
		// Token: 0x040071A8 RID: 29096
		BlindSpecter,
		// Token: 0x040071A9 RID: 29097
		Skeleton,
		// Token: 0x040071AA RID: 29098
		LollipopGhouls,
		// Token: 0x040071AB RID: 29099
		Engine
	}
}
