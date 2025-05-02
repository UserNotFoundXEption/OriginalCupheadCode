using System;
using UnityEngine;

// Token: 0x02000304 RID: 772
public class RetroArcadeCaterpillarManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x170002ED RID: 749
	// (get) Token: 0x06002246 RID: 8774 RVA: 0x0001D3AF File Offset: 0x0001B5AF
	// (set) Token: 0x06002247 RID: 8775 RVA: 0x0001D3B7 File Offset: 0x0001B5B7
	public float moveSpeed { get; set; }

	// Token: 0x06002248 RID: 8776 RVA: 0x000BCEA4 File Offset: 0x000BB0A4
	public void StartCaterpillar()
	{
		this.p = base.properties.CurrentState.caterpillar;
		this.bodyParts = new RetroArcadeCaterpillarBodyPart[this.p.bodyParts.Length + 1];
		RetroArcadeCaterpillarBodyPart.Direction direction = (!Rand.Bool()) ? RetroArcadeCaterpillarBodyPart.Direction.Right : RetroArcadeCaterpillarBodyPart.Direction.Left;
		this.bodyParts[0] = this.bodyPartPrefabs[0].Create(0, direction, this, this.p);
		for (int i = 0; i < this.p.bodyParts.Length; i++)
		{
			this.bodyParts[i + 1] = this.bodyPartPrefabs[this.p.bodyParts[i]].Create(i + 1, direction, this, this.p);
		}
		this.numDied = 0;
		this.numSpidersSpawned = 0;
		this.moveSpeed = 640f / this.p.moveTime;
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x000BCF84 File Offset: 0x000BB184
	public void OnBodyPartDie(RetroArcadeCaterpillarBodyPart alien)
	{
		this.numDied++;
		this.moveSpeed = 640f / (this.p.moveTime - (float)this.numDied * this.p.moveTimeDecrease);
		if (this.numDied >= this.bodyParts.Length - 1)
		{
			this.StopAllCoroutines();
			this.bodyParts[0].Dead();
			foreach (RetroArcadeCaterpillarBodyPart retroArcadeCaterpillarBodyPart in this.bodyParts)
			{
				retroArcadeCaterpillarBodyPart.OnWaveEnd();
			}
			base.properties.DealDamageToNextNamedState();
		}
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x000BD024 File Offset: 0x000BB224
	public void OnReachBottom()
	{
		if (this.numSpidersSpawned < this.p.spiderCount)
		{
			this.numSpidersSpawned++;
			this.spiderPrefab.Create((!Rand.Bool()) ? RetroArcadeCaterpillarSpider.Direction.Right : RetroArcadeCaterpillarSpider.Direction.Left, this.p);
		}
	}

	// Token: 0x04001C40 RID: 7232
	[SerializeField]
	public RetroArcadeCaterpillarBodyPart[] bodyPartPrefabs;

	// Token: 0x04001C41 RID: 7233
	[SerializeField]
	public RetroArcadeCaterpillarSpider spiderPrefab;

	// Token: 0x04001C42 RID: 7234
	public RetroArcadeCaterpillarBodyPart[] bodyParts;

	// Token: 0x04001C44 RID: 7236
	public LevelProperties.RetroArcade.Caterpillar p;

	// Token: 0x04001C45 RID: 7237
	public int numDied;

	// Token: 0x04001C46 RID: 7238
	public int numSpidersSpawned;
}
