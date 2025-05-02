using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000184 RID: 388
public class ChessKingLevelParryPoint : ParrySwitch
{
	// Token: 0x1700024B RID: 587
	// (get) Token: 0x06001264 RID: 4708 RVA: 0x0000F7E9 File Offset: 0x0000D9E9
	// (set) Token: 0x06001265 RID: 4709 RVA: 0x0000F7F1 File Offset: 0x0000D9F1
	public bool GOT_PARRIED { get; set; }

	// Token: 0x1700024C RID: 588
	// (get) Token: 0x06001266 RID: 4710 RVA: 0x0000F7FA File Offset: 0x0000D9FA
	// (set) Token: 0x06001267 RID: 4711 RVA: 0x0000F802 File Offset: 0x0000DA02
	public bool IS_BLUE { get; set; }

	// Token: 0x06001268 RID: 4712 RVA: 0x0000F80B File Offset: 0x0000DA0B
	public override void Awake()
	{
		this.GOT_PARRIED = false;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().color = Color.grey;
		base.Awake();
	}

	// Token: 0x06001269 RID: 4713 RVA: 0x0000F836 File Offset: 0x0000DA36
	public void Init(Vector3 pos)
	{
		base.transform.position = pos;
		this.IS_BLUE = false;
		this.GOT_PARRIED = false;
	}

	// Token: 0x0600126A RID: 4714 RVA: 0x00094FC0 File Offset: 0x000931C0
	public void Init(Vector3 pos, Vector3 dir, float speed, float amount)
	{
		base.GetComponent<SpriteRenderer>().color = Color.blue;
		base.transform.position = pos;
		this.dir = dir;
		this.amount = amount;
		this.speed = speed;
		this.IS_BLUE = true;
		this.GOT_PARRIED = false;
	}

	// Token: 0x0600126B RID: 4715 RVA: 0x0000F852 File Offset: 0x0000DA52
	public void Activate()
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().color = Color.magenta;
	}

	// Token: 0x0600126C RID: 4716 RVA: 0x0000F870 File Offset: 0x0000DA70
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		this.GOT_PARRIED = true;
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().color = Color.grey;
	}

	// Token: 0x0600126D RID: 4717 RVA: 0x0000F89C File Offset: 0x0000DA9C
	public void MovePoint()
	{
		if (this.IS_BLUE)
		{
			base.StartCoroutine(this.move_cr());
		}
	}

	// Token: 0x0600126E RID: 4718 RVA: 0x00095010 File Offset: 0x00093210
	public IEnumerator move_cr()
	{
		Vector3 startPos = base.transform.position;
		Vector3 endPos = this.GetEndPos();
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		float time = Vector3.Distance(startPos, endPos) / this.speed;
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.position = Vector3.Lerp(startPos, endPos, t / time);
			yield return wait;
		}
		base.transform.position = endPos;
		yield return null;
		yield break;
	}

	// Token: 0x0600126F RID: 4719 RVA: 0x0009502C File Offset: 0x0009322C
	public Vector3 GetEndPos()
	{
		if (this.dir == Vector3.right)
		{
			return new Vector3(base.transform.position.x + this.amount, base.transform.position.y);
		}
		if (this.dir == Vector3.left)
		{
			return new Vector3(base.transform.position.x - this.amount, base.transform.position.y);
		}
		if (this.dir == Vector3.up)
		{
			return new Vector3(base.transform.position.x, base.transform.position.y + this.amount);
		}
		return new Vector3(base.transform.position.x, base.transform.position.y - this.amount);
	}

	// Token: 0x04000EE3 RID: 3811
	public Vector3 dir;

	// Token: 0x04000EE4 RID: 3812
	public float amount;

	// Token: 0x04000EE5 RID: 3813
	public float speed;
}
