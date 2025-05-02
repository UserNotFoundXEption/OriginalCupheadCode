using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000561 RID: 1377
public class PlanePlayerDeathPart : AbstractMonoBehaviour
{
	// Token: 0x060039B3 RID: 14771 RVA: 0x0010CF34 File Offset: 0x0010B134
	public PlanePlayerDeathPart CreatePart(PlayerId player, Vector3 position)
	{
		PlanePlayerDeathPart planePlayerDeathPart = this.InstantiatePrefab<PlanePlayerDeathPart>();
		planePlayerDeathPart.transform.position = position;
		planePlayerDeathPart.animator.SetInteger("Player", (int)player);
		return planePlayerDeathPart;
	}

	// Token: 0x060039B4 RID: 14772 RVA: 0x0002F015 File Offset: 0x0002D215
	public override void Awake()
	{
		base.Awake();
		this.velocity = new Vector2(Random.Range(-500f, 500f), Random.Range(500f, 1000f));
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060039B5 RID: 14773 RVA: 0x0010CF68 File Offset: 0x0010B168
	public IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += (this.velocity + new Vector2(-300f, this.accumulatedGravity)) * base.LocalDeltaTime;
			this.accumulatedGravity += -6000f * base.LocalDeltaTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060039B6 RID: 14774 RVA: 0x0010CF84 File Offset: 0x0010B184
	public void GameOverUnpause()
	{
		base.animator.enabled = true;
		AnimationHelper component = base.GetComponent<AnimationHelper>();
		component.IgnoreGlobal = true;
		this.ignoreGlobalTime = true;
		base.enabled = true;
	}

	// Token: 0x04002E4D RID: 11853
	public const float VELOCITY_X_MIN = -500f;

	// Token: 0x04002E4E RID: 11854
	public const float VELOCITY_X_MAX = 500f;

	// Token: 0x04002E4F RID: 11855
	public const float VELOCITY_Y_MIN = 500f;

	// Token: 0x04002E50 RID: 11856
	public const float VELOCITY_Y_MAX = 1000f;

	// Token: 0x04002E51 RID: 11857
	public const float GRAVITY = -6000f;

	// Token: 0x04002E52 RID: 11858
	public Vector2 velocity;

	// Token: 0x04002E53 RID: 11859
	public float accumulatedGravity;
}
