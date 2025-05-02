using System;

// Token: 0x02000355 RID: 853
public class SallyStagePlayHusbandExplosion : LevelBossDeathExploder
{
	// Token: 0x060025A0 RID: 9632 RVA: 0x0001FA95 File Offset: 0x0001DC95
	public override void Start()
	{
		this.effectPrefab = Level.Current.LevelResources.levelBossDeathExplosion;
	}
}
