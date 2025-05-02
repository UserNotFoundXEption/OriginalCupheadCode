using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001DA RID: 474
public class DicePalaceCigarLevelCigaretteGhost : AbstractProjectile
{
	// Token: 0x06001618 RID: 5656 RVA: 0x00012BF8 File Offset: 0x00010DF8
	public void InitGhost(LevelProperties.DicePalaceCigar properties)
	{
		this.properties = properties;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001619 RID: 5657 RVA: 0x0009EAE0 File Offset: 0x0009CCE0
	public IEnumerator move_cr()
	{
		base.StartCoroutine(this.spawn_fx_cr());
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y < 560f)
		{
			base.transform.AddPosition(0f, this.properties.CurrentState.cigaretteGhost.verticalSpeed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x0600161A RID: 5658 RVA: 0x00012C0E File Offset: 0x00010E0E
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600161B RID: 5659 RVA: 0x00012C2C File Offset: 0x00010E2C
	public override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x0600161C RID: 5660 RVA: 0x00012C3A File Offset: 0x00010E3A
	public override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x0600161D RID: 5661 RVA: 0x0009EAFC File Offset: 0x0009CCFC
	public IEnumerator spawn_fx_cr()
	{
		bool isVal = Rand.Bool();
		for (;;)
		{
			float value = Random.Range(0.4f, 0.6f);
			float value2 = Random.Range(0.2f, 0.3f);
			float chosenVal = (!isVal) ? value2 : value;
			yield return CupheadTime.WaitForSeconds(this, chosenVal);
			float t = 0f;
			float time = chosenVal;
			while (t < time)
			{
				t += CupheadTime.Delta;
				this.fx.Create(this.root.transform.position);
				yield return CupheadTime.WaitForSeconds(this, 0.1f);
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, Random.Range(0.25f, 0.45f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x04001205 RID: 4613
	[SerializeField]
	public Transform root;

	// Token: 0x04001206 RID: 4614
	[SerializeField]
	public Effect fx;

	// Token: 0x04001207 RID: 4615
	public Vector3 centerPoint;

	// Token: 0x04001208 RID: 4616
	public LevelProperties.DicePalaceCigar properties;
}
