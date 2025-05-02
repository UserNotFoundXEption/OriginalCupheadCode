using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003A2 RID: 930
public class SnowCultLevelYetiLegs : BasicDamageDealingObject
{
	// Token: 0x0600292F RID: 10543 RVA: 0x00022ADB File Offset: 0x00020CDB
	public void Start()
	{
		base.StartCoroutine(this.run_away_cr());
	}

	// Token: 0x06002930 RID: 10544 RVA: 0x000D0994 File Offset: 0x000CEB94
	public IEnumerator run_away_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.rend.sortingLayerName = "Player";
		this.rend.sortingOrder = -19;
		base.animator.Play("Run");
		this.SFX_SNOWCULT_YetiLegsWalkOff();
		for (int i = 0; i < 1000; i++)
		{
			base.transform.position += Vector3.left * Mathf.Sign(base.transform.localScale.x) * 1000f * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002931 RID: 10545 RVA: 0x00022AEA File Offset: 0x00020CEA
	public void SFX_SNOWCULT_YetiLegsWalkOff()
	{
		AudioManager.Play("sfx_dlc_snowcult_p2_snowmonster_death_stompoffscreen");
		this.emitAudioFromObject.Add("sfx_dlc_snowcult_p2_snowmonster_death_stompoffscreen");
	}

	// Token: 0x04002283 RID: 8835
	public const float RUN_SPEED = 1000f;

	// Token: 0x04002284 RID: 8836
	public const float RUN_DELAY = 1f;

	// Token: 0x04002285 RID: 8837
	[SerializeField]
	public SpriteRenderer rend;
}
