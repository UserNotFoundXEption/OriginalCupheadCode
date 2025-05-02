using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000273 RID: 627
public class FlyingGenieLevelPuppetProjectile : BasicProjectile
{
	// Token: 0x170002AF RID: 687
	// (get) Token: 0x06001CBA RID: 7354 RVA: 0x000185F8 File Offset: 0x000167F8
	public float minRadius
	{
		get
		{
			return this._minRadius;
		}
	}

	// Token: 0x170002B0 RID: 688
	// (get) Token: 0x06001CBB RID: 7355 RVA: 0x00018600 File Offset: 0x00016800
	public float maxRadius
	{
		get
		{
			return this._maxRadius;
		}
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x00018608 File Offset: 0x00016808
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_spark_cr());
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x000AF018 File Offset: 0x000AD218
	public IEnumerator spawn_spark_cr()
	{
		string[] pattern = new string[]
		{
			"B",
			"P",
			"B",
			"P",
			"P",
			"B",
			"P",
			"B",
			"B",
			"P",
			"B",
			"P"
		};
		int patternIndex = Random.Range(0, pattern.Length);
		for (;;)
		{
			Vector2 vector = base.baseTransform.position;
			Vector2 vector2;
			vector2..ctor(Random.value * (float)((!Rand.Bool()) ? -1 : 1), Random.value * (float)((!Rand.Bool()) ? -1 : 1));
			Vector2 target = vector + vector2.normalized * Random.Range(this.minRadius, this.maxRadius);
			if (pattern[patternIndex] == "B")
			{
				this.sparksBlue[Random.Range(0, this.sparksBlue.Length)].Create(target);
			}
			else
			{
				this.sparksPink[Random.Range(0, this.sparksPink.Length)].Create(target);
			}
			patternIndex = (patternIndex + 1) % pattern.Length;
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.08f, 0.2f));
		}
		yield break;
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x000AF034 File Offset: 0x000AD234
	public override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(1f, 0f, 0f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position, this.minRadius);
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawWireSphere(base.baseTransform.position, this.maxRadius);
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x0001861D File Offset: 0x0001681D
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x04001758 RID: 5976
	[SerializeField]
	public float _minRadius = 100f;

	// Token: 0x04001759 RID: 5977
	[SerializeField]
	public float _maxRadius = 200f;

	// Token: 0x0400175A RID: 5978
	[SerializeField]
	public Effect[] sparksBlue;

	// Token: 0x0400175B RID: 5979
	[SerializeField]
	public Effect[] sparksPink;
}
