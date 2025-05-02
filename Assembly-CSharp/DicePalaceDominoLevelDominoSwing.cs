using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001E0 RID: 480
public class DicePalaceDominoLevelDominoSwing : AbstractCollidableObject
{
	// Token: 0x0600165D RID: 5725 RVA: 0x000130A0 File Offset: 0x000112A0
	public override void Awake()
	{
		base.Awake();
	}

	// Token: 0x0600165E RID: 5726 RVA: 0x0009EEC0 File Offset: 0x0009D0C0
	public void InitSwing(LevelProperties.DicePalaceDomino properties)
	{
		this.speed = properties.CurrentState.domino.swingSpeed;
		this.strength = properties.CurrentState.domino.swingDistance;
		this.origin = new Vector3(base.transform.position.x, properties.CurrentState.domino.swingPosY);
		base.transform.position = this.origin;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600165F RID: 5727 RVA: 0x000130A8 File Offset: 0x000112A8
	public virtual float hitPauseCoefficient()
	{
		return (!base.GetComponentInChildren<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06001660 RID: 5728 RVA: 0x0009EF48 File Offset: 0x0009D148
	public IEnumerator move_cr()
	{
		yield return this.domino.GetComponent<Animator>().WaitForAnimationToEnd(this, "Intro", false, true);
		float angle = 0f;
		for (;;)
		{
			angle += this.speed * CupheadTime.Delta * this.hitPauseCoefficient();
			base.transform.position = this.origin + Vector3.up * (Mathf.Sin(angle) * this.strength);
			yield return null;
		}
		yield break;
	}

	// Token: 0x0400122C RID: 4652
	[SerializeField]
	public DicePalaceDominoLevelDomino domino;

	// Token: 0x0400122D RID: 4653
	public float speed;

	// Token: 0x0400122E RID: 4654
	public float strength;

	// Token: 0x0400122F RID: 4655
	public Vector3 origin;
}
