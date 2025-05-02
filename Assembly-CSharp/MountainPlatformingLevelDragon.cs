using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200043E RID: 1086
public class MountainPlatformingLevelDragon : PlatformingLevelShootingEnemy
{
	// Token: 0x06002EAE RID: 11950 RVA: 0x00026EAD File Offset: 0x000250AD
	public override void Start()
	{
		base.Start();
		AudioManager.Play("castle_dragon_spawn");
		this.emitAudioFromObject.Add("castle_dragon_spawn");
	}

	// Token: 0x06002EAF RID: 11951 RVA: 0x00026ECF File Offset: 0x000250CF
	public void Init(Vector3 startPos, Vector3 endPos)
	{
		this.startPos = startPos;
		base.transform.position = startPos;
		this.endPos = endPos;
		base.StartCoroutine(this.move_to_pos_cr());
		base.StartCoroutine(this.check_cr());
	}

	// Token: 0x06002EB0 RID: 11952 RVA: 0x000E0074 File Offset: 0x000DE274
	public IEnumerator move_to_pos_cr()
	{
		float t = 0f;
		float time = base.Properties.dragonTimeIn;
		Vector2 start = base.transform.position;
		this._target = PlayerManager.GetNext();
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.StartShoot();
		yield return null;
		yield break;
	}

	// Token: 0x06002EB1 RID: 11953 RVA: 0x000E0090 File Offset: 0x000DE290
	public IEnumerator check_cr()
	{
		while (MountainPlatformingLevelElevatorHandler.elevatorIsMoving)
		{
			yield return null;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06002EB2 RID: 11954 RVA: 0x00026F05 File Offset: 0x00025105
	public override void Shoot()
	{
		base.Shoot();
		AudioManager.Play("castle_dragon_attack");
		this.emitAudioFromObject.Add("castle_dragon_attack");
		base.StartCoroutine(this.leave_cr());
	}

	// Token: 0x06002EB3 RID: 11955 RVA: 0x000E00AC File Offset: 0x000DE2AC
	public override void SpawnShootEffect()
	{
		if (base.transform.localScale.x < 0f)
		{
			this._effectRoot.localEulerAngles = new Vector3(0f, 0f, this._effectRoot.localEulerAngles.z - 180f);
		}
		if (this._shootEffect != null)
		{
			Effect effect = this._shootEffect.Create(this._effectRoot.position);
			effect.transform.rotation = this._effectRoot.rotation;
		}
	}

	// Token: 0x06002EB4 RID: 11956 RVA: 0x000E0148 File Offset: 0x000DE348
	public IEnumerator leave_cr()
	{
		float t = 0f;
		float time = base.Properties.dragonTimeOut;
		base.transform.position = this.endPos;
		Vector2 start = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, base.Properties.dragonLeaveDelay);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.startPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06002EB5 RID: 11957 RVA: 0x00026F34 File Offset: 0x00025134
	public override void Die()
	{
		AudioManager.Play("castle_dragon_death");
		this.emitAudioFromObject.Add("castle_dragon_death");
		base.Die();
	}

	// Token: 0x06002EB6 RID: 11958 RVA: 0x00026F56 File Offset: 0x00025156
	public void ActivateTail()
	{
		base.animator.Play("Tail", 1);
	}

	// Token: 0x040026BC RID: 9916
	public Vector3 endPos;

	// Token: 0x040026BD RID: 9917
	public Vector3 startPos;
}
