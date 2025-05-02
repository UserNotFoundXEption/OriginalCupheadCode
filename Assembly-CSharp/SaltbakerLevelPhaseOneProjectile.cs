using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200037B RID: 891
public class SaltbakerLevelPhaseOneProjectile : BasicProjectile
{
	// Token: 0x06002740 RID: 10048 RVA: 0x00020FFB File Offset: 0x0001F1FB
	public override void Start()
	{
		base.Start();
		this.createSparks = true;
		base.StartCoroutine(this.spawn_sparkles_cr());
	}

	// Token: 0x06002741 RID: 10049 RVA: 0x00021017 File Offset: 0x0001F217
	public virtual bool SparksFollow()
	{
		return false;
	}

	// Token: 0x06002742 RID: 10050 RVA: 0x000CB25C File Offset: 0x000C945C
	public IEnumerator spawn_sparkles_cr()
	{
		this.sparkAngle = (float)Random.Range(0, 360);
		while (this.createSparks)
		{
			yield return CupheadTime.WaitForSeconds(this, this.sparkSpawnDelay);
			int count = 1;
			if (this.sparkSpawnDelay < CupheadTime.Delta)
			{
				count = (int)(CupheadTime.Delta / this.sparkSpawnDelay);
			}
			for (int i = 0; i < count; i++)
			{
				Effect effect = this.sparkEffect.Create(base.transform.position + MathUtils.AngleToDirection(this.sparkAngle) * this.sparkDistanceRange.RandomFloat());
				if (this.SparksFollow())
				{
					effect.transform.parent = base.transform;
				}
				this.sparkAngle = (this.sparkAngle + this.sparkAngleShiftRange.RandomFloat()) % 360f;
			}
		}
		yield break;
	}

	// Token: 0x06002743 RID: 10051 RVA: 0x000CB278 File Offset: 0x000C9478
	public void HandleShadow(float heightOffset, float shadowPosOffset)
	{
		this.shadow.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground + shadowPosOffset);
		float num = Mathf.InverseLerp(this.shadowScaleHeightRange.max, this.shadowScaleHeightRange.min, base.transform.position.y - heightOffset - (float)Level.Current.Ground);
		this.shadow.transform.eulerAngles = Vector3.zero;
		this.shadow.transform.localScale = Vector3.Lerp(new Vector3(0.25f, 0.25f), new Vector3(1f, 1f), num);
		this.shadow.color = new Color(1f, 1f, 1f, Mathf.Lerp(0.25f, 1f, num));
	}

	// Token: 0x04002079 RID: 8313
	[SerializeField]
	public SpriteRenderer shadow;

	// Token: 0x0400207A RID: 8314
	[SerializeField]
	public MinMax shadowScaleHeightRange = new MinMax(100f, 500f);

	// Token: 0x0400207B RID: 8315
	[SerializeField]
	public Effect sparkEffect;

	// Token: 0x0400207C RID: 8316
	[SerializeField]
	public float sparkSpawnDelay = 0.15f;

	// Token: 0x0400207D RID: 8317
	[SerializeField]
	public MinMax sparkAngleShiftRange = new MinMax(60f, 300f);

	// Token: 0x0400207E RID: 8318
	[SerializeField]
	public MinMax sparkDistanceRange = new MinMax(0f, 20f);

	// Token: 0x0400207F RID: 8319
	public float sparkAngle;

	// Token: 0x04002080 RID: 8320
	public bool createSparks = true;
}
