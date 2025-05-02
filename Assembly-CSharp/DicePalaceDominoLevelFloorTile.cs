using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001E4 RID: 484
public class DicePalaceDominoLevelFloorTile : DicePalaceDominoLevelBaseTile
{
	// Token: 0x17000277 RID: 631
	// (get) Token: 0x0600166D RID: 5741 RVA: 0x00013180 File Offset: 0x00011380
	// (set) Token: 0x0600166E RID: 5742 RVA: 0x00013188 File Offset: 0x00011388
	public bool spikesActive { get; set; }

	// Token: 0x0600166F RID: 5743 RVA: 0x00013191 File Offset: 0x00011391
	public override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.boxCollider = base.GetComponent<BoxCollider2D>();
		base.Awake();
	}

	// Token: 0x06001670 RID: 5744 RVA: 0x000131B0 File Offset: 0x000113B0
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001671 RID: 5745 RVA: 0x000131C8 File Offset: 0x000113C8
	public override void InitTile()
	{
		base.InitTile();
		this.OnMoveStart();
	}

	// Token: 0x06001672 RID: 5746 RVA: 0x000131D6 File Offset: 0x000113D6
	public void SetColour(int colourIndex, LevelProperties.DicePalaceDomino properties)
	{
		this.properties = properties;
		base.currentColourIndex = colourIndex;
		this.spriteRenderer = base.GetComponent<SpriteRenderer>();
		this.spriteRenderer.sprite = this.colours[base.currentColourIndex];
	}

	// Token: 0x06001673 RID: 5747 RVA: 0x0001320A File Offset: 0x0001140A
	public void OnMoveStart()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001674 RID: 5748 RVA: 0x0009F0C8 File Offset: 0x0009D2C8
	public IEnumerator move_cr()
	{
		yield return null;
		while (base.isActivated)
		{
			base.transform.position += Vector3.left * this.properties.CurrentState.domino.floorSpeed * CupheadTime.Delta;
			if (base.transform.position.x + 200f < (float)Level.Current.Left)
			{
				this.DeactivateTile();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001675 RID: 5749 RVA: 0x00013219 File Offset: 0x00011419
	public void TriggerSpikes(bool spikesActive)
	{
		base.StartCoroutine(this.toggleSpikes_cr(spikesActive));
	}

	// Token: 0x06001676 RID: 5750 RVA: 0x00013229 File Offset: 0x00011429
	public override void DeactivateTile()
	{
		base.DeactivateTile();
		this.toggleSpikes_cr(false);
	}

	// Token: 0x06001677 RID: 5751 RVA: 0x0009F0E4 File Offset: 0x0009D2E4
	public IEnumerator toggleSpikes_cr(bool spikesActive)
	{
		if (spikesActive)
		{
			base.animator.Play("Spikes_Up");
			this.spikesActive = true;
			this.boxCollider.enabled = true;
		}
		else
		{
			if (this.spikesActive)
			{
				base.animator.Play("Spikes_Down");
				base.StartCoroutine(this.disableCollider_cr());
			}
			else
			{
				base.animator.Play("Off");
				this.boxCollider.enabled = false;
			}
			this.spikesActive = false;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001678 RID: 5752 RVA: 0x0009F108 File Offset: 0x0009D308
	public IEnumerator disableCollider_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Spikes_Down", true, true);
		this.boxCollider.enabled = false;
		yield break;
	}

	// Token: 0x06001679 RID: 5753 RVA: 0x00013239 File Offset: 0x00011439
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600167A RID: 5754 RVA: 0x00013257 File Offset: 0x00011457
	public override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400123B RID: 4667
	public SpriteRenderer spriteRenderer;

	// Token: 0x0400123C RID: 4668
	public DamageDealer damageDealer;

	// Token: 0x0400123E RID: 4670
	public BoxCollider2D boxCollider;
}
