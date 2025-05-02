using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000346 RID: 838
public class RumRunnersLevelLaser : AbstractCollidableObject
{
	// Token: 0x060024B3 RID: 9395 RVA: 0x000C4890 File Offset: 0x000C2A90
	public override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		foreach (CollisionChild collisionChild in this.childColliders)
		{
			collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		}
	}

	// Token: 0x060024B4 RID: 9396 RVA: 0x0001F086 File Offset: 0x0001D286
	public void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060024B5 RID: 9397 RVA: 0x000C48E0 File Offset: 0x000C2AE0
	public override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		float num = this.damageDealer.DealDamage(hit);
		if (num > 0f)
		{
			this.SFX_RUMRUN_Grammobeam_DamagePlayer();
		}
	}

	// Token: 0x060024B6 RID: 9398 RVA: 0x000C4914 File Offset: 0x000C2B14
	public IEnumerator moveMask_cr(GameObject laserMask, float startX, float endX, float duration, bool destroyMask)
	{
		float elapsedTime = 0f;
		while (elapsedTime < duration)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			laserMask.transform.SetLocalPosition(new float?(EaseUtils.Linear(startX, endX, elapsedTime / duration)), null, null);
		}
		if (destroyMask)
		{
			Object.Destroy(laserMask);
		}
		yield break;
	}

	// Token: 0x060024B7 RID: 9399 RVA: 0x0001F09E File Offset: 0x0001D29E
	public void Begin()
	{
		base.StartCoroutine(this.begin_cr(this.mainRenderers, 1f));
	}

	// Token: 0x060024B8 RID: 9400 RVA: 0x000C4950 File Offset: 0x000C2B50
	public IEnumerator begin_cr(SpriteRenderer[] renderers, float durationMultiplier = 1f)
	{
		MinMax DurationRange = new MinMax(1.2f, 1.5f);
		foreach (SpriteRenderer renderer in renderers)
		{
			renderer.enabled = true;
			GameObject laserMask = Object.Instantiate<GameObject>(this.laserMaskPrefab);
			laserMask.transform.parent = renderer.transform;
			laserMask.transform.ResetLocalRotation();
			laserMask.transform.localPosition = new Vector3(-400f, 0f);
			laserMask.GetComponent<RumRunnersLevelLaserMask>().Setup(renderer.sortingLayerID, renderer.sortingOrder);
			base.StartCoroutine(this.moveMask_cr(laserMask, -400f, 800f, DurationRange.RandomFloat() * durationMultiplier, true));
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x060024B9 RID: 9401 RVA: 0x0001F0B8 File Offset: 0x0001D2B8
	public void End()
	{
		base.StartCoroutine(this.end_cr());
	}

	// Token: 0x060024BA RID: 9402 RVA: 0x000C497C File Offset: 0x000C2B7C
	public IEnumerator end_cr()
	{
		MinMax DurationRange = new MinMax(1.2f, 1.5f);
		Coroutine[] coroutines = new Coroutine[this.mainRenderers.Length];
		for (int i = 0; i < this.mainRenderers.Length; i++)
		{
			SpriteRenderer renderer = this.mainRenderers[i];
			GameObject laserMask = Object.Instantiate<GameObject>(this.laserMaskPrefab);
			laserMask.transform.parent = renderer.transform;
			laserMask.transform.ResetLocalRotation();
			laserMask.transform.localRotation = Quaternion.Euler(0f, 0f, 180f);
			laserMask.transform.localPosition = new Vector3(220f, 0f);
			laserMask.GetComponent<RumRunnersLevelLaserMask>().Setup(renderer.sortingLayerID, renderer.sortingOrder);
			coroutines[i] = base.StartCoroutine(this.moveMask_cr(laserMask, 220f, 1600f, DurationRange.RandomFloat(), false));
			base.StartCoroutine(this.endSparkles_cr(laserMask.transform));
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		foreach (Coroutine coroutine in coroutines)
		{
			yield return coroutine;
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060024BB RID: 9403 RVA: 0x000C4998 File Offset: 0x000C2B98
	public IEnumerator endSparkles_cr(Transform maskTransform)
	{
		MinMax SpawnRandomizationRange = new MinMax(-15f, 15f);
		float elapsedTime = 0f;
		for (;;)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			if (elapsedTime >= 0.02f)
			{
				elapsedTime -= 0.02f;
				for (int i = 0; i < 1; i++)
				{
					this.sparklesEffect.Create(maskTransform.position + maskTransform.right * 280f + new Vector3(SpawnRandomizationRange.RandomFloat(), SpawnRandomizationRange.RandomFloat()));
				}
			}
		}
		yield break;
	}

	// Token: 0x060024BC RID: 9404 RVA: 0x0001F0C7 File Offset: 0x0001D2C7
	public void Warning()
	{
		base.StartCoroutine(this.warning_cr());
	}

	// Token: 0x060024BD RID: 9405 RVA: 0x000C49BC File Offset: 0x000C2BBC
	public IEnumerator warning_cr()
	{
		float elapsedTime = 0f;
		while (elapsedTime < 0.3f)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			float alpha = Mathf.Lerp(1f, 0f, elapsedTime / 0.3f);
			for (int i = 0; i < this.mainRenderers.Length; i++)
			{
				Color color = this.mainRenderers[i].color;
				color.a = alpha;
				this.mainRenderers[i].color = color;
				color = this.warningRenderers[i].color;
				color.a = 1f - alpha;
				this.warningRenderers[i].color = color;
			}
		}
		yield break;
	}

	// Token: 0x060024BE RID: 9406 RVA: 0x000C49D8 File Offset: 0x000C2BD8
	public void CancelWarning()
	{
		this.StopAllCoroutines();
		foreach (SpriteRenderer spriteRenderer in this.mainRenderers)
		{
			Color color = spriteRenderer.color;
			color.a = 1f;
			spriteRenderer.color = color;
		}
		foreach (SpriteRenderer spriteRenderer2 in this.warningRenderers)
		{
			Color color2 = spriteRenderer2.color;
			color2.a = 0f;
			spriteRenderer2.color = color2;
		}
	}

	// Token: 0x060024BF RID: 9407 RVA: 0x000C4A6C File Offset: 0x000C2C6C
	public void Attack()
	{
		base.animator.SetBool("On", true);
		foreach (SpriteRenderer spriteRenderer in this.notesRenderers)
		{
			spriteRenderer.enabled = false;
		}
		base.StartCoroutine(this.begin_cr(this.notesRenderers, 0.5f));
	}

	// Token: 0x060024C0 RID: 9408 RVA: 0x0001F0D6 File Offset: 0x0001D2D6
	public void EndAttack()
	{
		base.animator.SetBool("On", false);
	}

	// Token: 0x060024C1 RID: 9409 RVA: 0x000C4AC8 File Offset: 0x000C2CC8
	public void animationEvent_WarningToOnStarted()
	{
		foreach (SpriteRenderer spriteRenderer in this.mainRenderers)
		{
			Color color = spriteRenderer.color;
			color.a = 1f;
			spriteRenderer.color = color;
		}
		foreach (SpriteRenderer spriteRenderer2 in this.warningRenderers)
		{
			Color color2 = spriteRenderer2.color;
			color2.a = 0f;
			spriteRenderer2.color = color2;
		}
	}

	// Token: 0x060024C2 RID: 9410 RVA: 0x0001F0E9 File Offset: 0x0001D2E9
	public void SFX_RUMRUN_Grammobeam_DamagePlayer()
	{
		AudioManager.Play("sfx_dlc_rumrun_p2_grammobeam_damageplayer");
	}

	// Token: 0x04001E64 RID: 7780
	[SerializeField]
	public SpriteRenderer[] mainRenderers;

	// Token: 0x04001E65 RID: 7781
	[SerializeField]
	public SpriteRenderer[] warningRenderers;

	// Token: 0x04001E66 RID: 7782
	[SerializeField]
	public SpriteRenderer[] notesRenderers;

	// Token: 0x04001E67 RID: 7783
	[SerializeField]
	public CollisionChild[] childColliders;

	// Token: 0x04001E68 RID: 7784
	[SerializeField]
	public GameObject laserMaskPrefab;

	// Token: 0x04001E69 RID: 7785
	[SerializeField]
	public Effect sparklesEffect;

	// Token: 0x04001E6A RID: 7786
	public DamageDealer damageDealer;
}
