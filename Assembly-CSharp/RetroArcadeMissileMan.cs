using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000309 RID: 777
public class RetroArcadeMissileMan : RetroArcadeEnemy
{
	// Token: 0x06002266 RID: 8806 RVA: 0x0001D502 File Offset: 0x0001B702
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
		this.hp = properties.CurrentState.missile.hp;
	}

	// Token: 0x06002267 RID: 8807 RVA: 0x000BD544 File Offset: 0x000BB744
	public void StartMissile()
	{
		base.gameObject.SetActive(true);
		this.missile = Object.Instantiate<RetroArcadeMissile>(this.missilePrefab);
		this.missile.Init(this.shootRootLeft.position, -90f, this.properties.CurrentState.missile, this.pivotPoint.position);
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.fire_missiles_cr());
	}

	// Token: 0x06002268 RID: 8808 RVA: 0x000BD5C4 File Offset: 0x000BB7C4
	public IEnumerator move_cr()
	{
		float time = this.properties.CurrentState.missile.manMoveTime;
		bool movingRight = Rand.Bool();
		for (;;)
		{
			float t = 0f;
			float start = base.transform.position.x;
			float end;
			if (movingRight)
			{
				end = 80f;
			}
			else
			{
				end = -80f;
			}
			while (t < time)
			{
				float val = t / time;
				base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(new float?(end), null, null);
			movingRight = !movingRight;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002269 RID: 8809 RVA: 0x000BD5E0 File Offset: 0x000BB7E0
	public IEnumerator fire_missiles_cr()
	{
		string[] dirString = this.properties.CurrentState.missile.directionString.Split(new char[]
		{
			','
		});
		int dirIndex = Random.Range(0, dirString.Length);
		bool onRight = false;
		for (;;)
		{
			onRight = (dirString[dirIndex] == "R");
			yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.missile.timerRelease.RandomFloat());
			yield return null;
			this.missile.StartCircle(onRight, this.pivotPoint.position);
			dirIndex = (dirIndex + 1) % dirString.Length;
		}
		yield break;
	}

	// Token: 0x0600226A RID: 8810 RVA: 0x0001D521 File Offset: 0x0001B721
	public override void Dead()
	{
		base.Dead();
		this.StopAllCoroutines();
		Object.Destroy(this.missile.gameObject);
		this.properties.DealDamageToNextNamedState();
		Object.Destroy(base.gameObject);
	}

	// Token: 0x04001C64 RID: 7268
	[SerializeField]
	public RetroArcadeMissile missilePrefab;

	// Token: 0x04001C65 RID: 7269
	[SerializeField]
	public Transform shootRootLeft;

	// Token: 0x04001C66 RID: 7270
	[SerializeField]
	public Transform shootRootRight;

	// Token: 0x04001C67 RID: 7271
	[SerializeField]
	public Transform pivotPoint;

	// Token: 0x04001C68 RID: 7272
	public const float MAX_X_POS = 80f;

	// Token: 0x04001C69 RID: 7273
	public LevelProperties.RetroArcade properties;

	// Token: 0x04001C6A RID: 7274
	public RetroArcadeMissile missile;
}
