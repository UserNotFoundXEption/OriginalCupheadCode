using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200027C RID: 636
public class FlyingMermaidLevelBackgroundChange : AbstractPausableComponent
{
	// Token: 0x06001D08 RID: 7432 RVA: 0x000AFABC File Offset: 0x000ADCBC
	public void Start()
	{
		this.points = new List<Transform>();
		this.size = this.toCopy.GetComponent<Collider2D>().bounds.size.x;
		this.getOffset.x = base.transform.position.x;
		this.copy1 = Object.Instantiate<FlyingMermaidLevelCoralCluster>(this.toCopy);
		this.copy1.transform.parent = base.transform;
		FlyingMermaidLevelCoralCluster flyingMermaidLevelCoralCluster = Object.Instantiate<FlyingMermaidLevelCoralCluster>(this.toCopy);
		flyingMermaidLevelCoralCluster.transform.parent = base.transform;
		this.copy1.transform.SetPosition(new float?(this.getOffset.x + this.size), new float?(this.toCopy.transform.position.y), new float?(0f));
		flyingMermaidLevelCoralCluster.transform.SetPosition(new float?(this.getOffset.x + this.size * 2f), new float?(this.toCopy.transform.position.y), new float?(0f));
		this.points.AddRange(this.toCopy.points);
		this.points.AddRange(this.copy1.points);
		this.points.AddRange(flyingMermaidLevelCoralCluster.points);
		this.copies = new List<FlyingMermaidLevelCoralCluster>();
		this.copies.Add(this.toCopy);
		this.copies.Add(this.copy1);
		this.copies.Add(flyingMermaidLevelCoralCluster);
	}

	// Token: 0x06001D09 RID: 7433 RVA: 0x000AFC74 File Offset: 0x000ADE74
	public void FixedUpdate()
	{
		if (base.GetComponent<ParallaxLayer>() != null)
		{
			base.GetComponent<ParallaxLayer>().enabled = false;
		}
		Vector3 localPosition = base.transform.localPosition;
		if (this.copies[this.index].transform.position.x <= -this.size)
		{
			this.copies[this.index].transform.position = new Vector2(this.size * 2f, this.copies[this.index].transform.position.y);
			this.index = (this.index + 1) % this.copies.Count;
		}
		localPosition.x -= this.speed * CupheadTime.FixedDelta * this.b_playbackSpeed;
		base.transform.localPosition = localPosition;
	}

	// Token: 0x040017A4 RID: 6052
	public List<Transform> points;

	// Token: 0x040017A5 RID: 6053
	public float size;

	// Token: 0x040017A6 RID: 6054
	public const float X_OUT = -1280f;

	// Token: 0x040017A7 RID: 6055
	[Range(0f, 2000f)]
	public float speed;

	// Token: 0x040017A8 RID: 6056
	[NonSerialized]
	public float b_playbackSpeed = 1f;

	// Token: 0x040017A9 RID: 6057
	[SerializeField]
	public FlyingMermaidLevelCoralCluster toCopy;

	// Token: 0x040017AA RID: 6058
	public FlyingMermaidLevelCoralCluster copy1;

	// Token: 0x040017AB RID: 6059
	public List<FlyingMermaidLevelCoralCluster> copies;

	// Token: 0x040017AC RID: 6060
	public Vector3 getOffset;

	// Token: 0x040017AD RID: 6061
	public Vector3 _offset;

	// Token: 0x040017AE RID: 6062
	public int index;
}
