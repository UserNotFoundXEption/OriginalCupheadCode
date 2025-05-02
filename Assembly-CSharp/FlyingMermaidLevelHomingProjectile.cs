using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000286 RID: 646
public class FlyingMermaidLevelHomingProjectile : HomingProjectile
{
	// Token: 0x06001D35 RID: 7477 RVA: 0x000B0338 File Offset: 0x000AE538
	public FlyingMermaidLevelHomingProjectile Create(Vector3 pos, float rotation, AbstractPlayerController player, LevelProperties.FlyingMermaid.HomerFish properties)
	{
		FlyingMermaidLevelHomingProjectile flyingMermaidLevelHomingProjectile = base.Create(pos, rotation, properties.initSpeed, properties.bulletSpeed, properties.rotationSpeed, properties.timeBeforeDeath, properties.timeBeforeHoming, player) as FlyingMermaidLevelHomingProjectile;
		flyingMermaidLevelHomingProjectile.properties = properties;
		flyingMermaidLevelHomingProjectile.transform.position = pos;
		return flyingMermaidLevelHomingProjectile;
	}

	// Token: 0x06001D36 RID: 7478 RVA: 0x00018BEC File Offset: 0x00016DEC
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.timer_cr());
	}

	// Token: 0x06001D37 RID: 7479 RVA: 0x000B0394 File Offset: 0x000AE594
	public IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.mainSprite.sortingLayerName = "Foreground";
		this.mainSprite.sortingOrder = 30;
		yield return CupheadTime.WaitForSeconds(this, this.properties.timeBeforeDeath - 0.2f);
		base.HomingEnabled = false;
		base.animator.SetTrigger("StopTracking");
		for (;;)
		{
			base.transform.position += this.velocity.normalized * this.properties.bulletSpeed * 1.4f * CupheadTime.Delta;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(this.velocity) + 180f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x040017D3 RID: 6099
	[SerializeField]
	public SpriteRenderer mainSprite;

	// Token: 0x040017D4 RID: 6100
	public LevelProperties.FlyingMermaid.HomerFish properties;
}
