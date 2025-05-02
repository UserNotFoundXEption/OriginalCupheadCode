using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003CA RID: 970
public class VeggiesLevelCarrotBgCarrot : AbstractMonoBehaviour
{
	// Token: 0x06002AB5 RID: 10933 RVA: 0x000D4A9C File Offset: 0x000D2C9C
	public VeggiesLevelCarrotBgCarrot Create(int side, float speed, VeggiesLevelCarrot parentCarrot)
	{
		VeggiesLevelCarrotBgCarrot veggiesLevelCarrotBgCarrot = this.InstantiatePrefab<VeggiesLevelCarrotBgCarrot>();
		veggiesLevelCarrotBgCarrot.Init(side, speed, parentCarrot);
		return veggiesLevelCarrotBgCarrot;
	}

	// Token: 0x06002AB6 RID: 10934 RVA: 0x000D4ABC File Offset: 0x000D2CBC
	public void Init(int side, float speed, VeggiesLevelCarrot parentCarrot)
	{
		base.transform.SetPosition(new float?(Random.Range(150f, 600f) * (float)side), new float?(-360f), new float?(0f));
		this.parentCarrot = parentCarrot;
		base.StartCoroutine(this.float_cr(speed));
	}

	// Token: 0x06002AB7 RID: 10935 RVA: 0x00023E6E File Offset: 0x0002206E
	public void End()
	{
		this.StopAllCoroutines();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x06002AB8 RID: 10936 RVA: 0x000D4B14 File Offset: 0x000D2D14
	public IEnumerator float_cr(float speed)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (base.transform.position.y > 720f)
			{
				this.parentCarrot.ShootHoming();
				this.End();
			}
			base.transform.AddPosition(0f, speed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0400239C RID: 9116
	public const float RANGE_MIN = 150f;

	// Token: 0x0400239D RID: 9117
	public const float RANGE_MAX = 600f;

	// Token: 0x0400239E RID: 9118
	public VeggiesLevelCarrot parentCarrot;
}
