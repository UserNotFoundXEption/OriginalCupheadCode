using System;
using System.Collections;
using UnityEngine;

// Token: 0x020002DD RID: 733
public class OldManLevelGoose : AbstractProjectile
{
	// Token: 0x0600207C RID: 8316 RVA: 0x000B85A4 File Offset: 0x000B67A4
	public virtual OldManLevelGoose Init(Vector2 pos, float speed, LevelProperties.OldMan.GooseAttack properties, bool hasCollision, string sortingLayer, int sortingOrder, float whiten)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.properties = properties;
		this.speed = speed;
		this.coll.enabled = hasCollision;
		this.rend.sortingLayerName = sortingLayer;
		this.rend.color = new Color(whiten, whiten, whiten);
		if (sortingLayer == "Foreground")
		{
			this.rend.material = this.altMaterial;
			base.gameObject.layer = 31;
		}
		this.rend.sortingOrder = sortingOrder;
		this.anim.Play((Random.Range(0, 8) % 6).ToString());
		this.Move();
		return this;
	}

	// Token: 0x0600207D RID: 8317 RVA: 0x0001B9C0 File Offset: 0x00019BC0
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600207E RID: 8318 RVA: 0x0001B9DE File Offset: 0x00019BDE
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600207F RID: 8319 RVA: 0x0001B9FC File Offset: 0x00019BFC
	public void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002080 RID: 8320 RVA: 0x000B8674 File Offset: 0x000B6874
	public IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > (float)Level.Current.Left - 1000f)
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.Recycle<OldManLevelGoose>();
		yield return null;
		yield break;
	}

	// Token: 0x04001AAB RID: 6827
	public const float OFFSET = 1000f;

	// Token: 0x04001AAC RID: 6828
	public LevelProperties.OldMan.GooseAttack properties;

	// Token: 0x04001AAD RID: 6829
	public float speed;

	// Token: 0x04001AAE RID: 6830
	[SerializeField]
	public BoxCollider2D coll;

	// Token: 0x04001AAF RID: 6831
	[SerializeField]
	public Animator anim;

	// Token: 0x04001AB0 RID: 6832
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x04001AB1 RID: 6833
	[SerializeField]
	public Material altMaterial;
}
