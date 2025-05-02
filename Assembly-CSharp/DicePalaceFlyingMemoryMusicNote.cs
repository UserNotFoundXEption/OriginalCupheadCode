using System;
using System.Collections;
using UnityEngine;

// Token: 0x020001F4 RID: 500
public class DicePalaceFlyingMemoryMusicNote : BasicProjectile
{
	// Token: 0x0600171F RID: 5919 RVA: 0x000A0F88 File Offset: 0x0009F188
	public DicePalaceFlyingMemoryMusicNote Create(Vector3 pos, float rotation, float speed, float deathTimer)
	{
		DicePalaceFlyingMemoryMusicNote dicePalaceFlyingMemoryMusicNote = base.Create(pos, rotation, speed) as DicePalaceFlyingMemoryMusicNote;
		dicePalaceFlyingMemoryMusicNote.deathTimer = deathTimer;
		return dicePalaceFlyingMemoryMusicNote;
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x00013AC4 File Offset: 0x00011CC4
	public override void Start()
	{
		base.Start();
		base.StartCoroutine(this.death_timer_cr());
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x000A0FB4 File Offset: 0x0009F1B4
	public override void FixedUpdate()
	{
		base.FixedUpdate();
		this.sprite.transform.SetEulerAngles(null, null, new float?(0f));
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x000A0FF4 File Offset: 0x0009F1F4
	public IEnumerator death_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.deathTimer);
		base.animator.SetTrigger("OnDeath");
		yield return base.animator.WaitForAnimationToEnd(this, "Note_Death", false, true);
		this.move = false;
		yield return null;
		yield break;
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x00013AD9 File Offset: 0x00011CD9
	public void Kill()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040012D3 RID: 4819
	[SerializeField]
	public Transform sprite;

	// Token: 0x040012D4 RID: 4820
	public float deathTimer;
}
