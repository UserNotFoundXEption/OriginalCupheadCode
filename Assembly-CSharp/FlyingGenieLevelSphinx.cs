using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000278 RID: 632
public class FlyingGenieLevelSphinx : AbstractProjectile
{
	// Token: 0x06001CDF RID: 7391 RVA: 0x000186FB File Offset: 0x000168FB
	public void Init(Vector3 startPos, LevelProperties.FlyingGenie.Sphinx properties, AbstractPlayerController player, string[] pinkPattern, int pinkIndex)
	{
		base.transform.position = startPos;
		this.properties = properties;
		this.player = player;
		this.pinkPattern = pinkPattern;
		this.pinkIndex = pinkIndex;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001CE0 RID: 7392 RVA: 0x00018734 File Offset: 0x00016934
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001CE1 RID: 7393 RVA: 0x0001875D File Offset: 0x0001695D
	public override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CE2 RID: 7394 RVA: 0x000AF648 File Offset: 0x000AD848
	public IEnumerator move_cr()
	{
		float startPos = (base.transform.position + Vector3.up * this.outOfChestY).y;
		while (base.transform.position.y < startPos)
		{
			base.transform.AddPosition(0f, this.outOfChestY * this.outOfChestSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		this.sphinxRenderer.sortingLayerName = "Projectiles";
		this.sphinxRenderer.sortingOrder = 2;
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 targetPos = this.player.transform.position;
		while (base.transform.position != targetPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, targetPos, this.properties.sphinxSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.splitDelay);
		base.animator.SetTrigger("Split");
		yield break;
	}

	// Token: 0x06001CE3 RID: 7395 RVA: 0x0001877B File Offset: 0x0001697B
	public void Split()
	{
		base.StartCoroutine(this.split_cr());
		AudioManager.Play("genie_scarab_release");
		this.emitAudioFromObject.Add("genie_scarab_release");
	}

	// Token: 0x06001CE4 RID: 7396 RVA: 0x000AF664 File Offset: 0x000AD864
	public IEnumerator split_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		int counter = (int)Mathf.Round(this.properties.sphinxSpawnNum / 2f);
		bool moveRight = false;
		for (int i = 0; i < this.sphinxPieces.Length; i++)
		{
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			int pink = (this.pinkIndex + i * 2) % this.pinkPattern.Length;
			this.sphinxPieces[i].StartMoving(this.properties, this.player, counter, moveRight, this.pinkPattern, pink);
			moveRight = !moveRight;
			yield return null;
		}
		this.Die();
		while (base.transform.childCount > 1)
		{
			yield return null;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001CE5 RID: 7397 RVA: 0x000187A4 File Offset: 0x000169A4
	public override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x04001775 RID: 6005
	public const string SplitParameterName = "Split";

	// Token: 0x04001776 RID: 6006
	public const string ProjectilesLayer = "Projectiles";

	// Token: 0x04001777 RID: 6007
	[SerializeField]
	public SpriteRenderer sphinxRenderer;

	// Token: 0x04001778 RID: 6008
	[SerializeField]
	public float outOfChestY;

	// Token: 0x04001779 RID: 6009
	[SerializeField]
	public float outOfChestSpeed;

	// Token: 0x0400177A RID: 6010
	public FlyingGenieLevelSphinxPiece[] sphinxPieces;

	// Token: 0x0400177B RID: 6011
	public AbstractPlayerController player;

	// Token: 0x0400177C RID: 6012
	public LevelProperties.FlyingGenie.Sphinx properties;

	// Token: 0x0400177D RID: 6013
	public bool moving;

	// Token: 0x0400177E RID: 6014
	public string[] pinkPattern;

	// Token: 0x0400177F RID: 6015
	public int pinkIndex;
}
