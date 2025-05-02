using System;
using UnityEngine;

// Token: 0x020003C0 RID: 960
public class TutorialLevelDamagableBox : AbstractCollidableObject
{
	// Token: 0x06002A63 RID: 10851 RVA: 0x00023AF1 File Offset: 0x00021CF1
	public void Start()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002A64 RID: 10852 RVA: 0x00023B0A File Offset: 0x00021D0A
	public override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.explosionPosition + base.transform.position, 10f);
	}

	// Token: 0x06002A65 RID: 10853 RVA: 0x000D3C94 File Offset: 0x000D1E94
	public void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.boxHealth -= info.damage;
		if (this.boxHealth <= 0f)
		{
			this.toEnable.SetActive(true);
			this.toDisable.SetActive(false);
			this.explosionPrefab.Create(this.explosionPosition + base.transform.position);
			AudioManager.Play("sfx_object_explode");
			this.emitAudioFromObject.Add("sfx_object_explode");
			Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400235A RID: 9050
	[SerializeField]
	public float boxHealth = 20f;

	// Token: 0x0400235B RID: 9051
	[SerializeField]
	public GameObject toDisable;

	// Token: 0x0400235C RID: 9052
	[SerializeField]
	public GameObject toEnable;

	// Token: 0x0400235D RID: 9053
	[SerializeField]
	public PlatformingLevelGenericExplosion explosionPrefab;

	// Token: 0x0400235E RID: 9054
	[SerializeField]
	public Vector3 explosionPosition;
}
