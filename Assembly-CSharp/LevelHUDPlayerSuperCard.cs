using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010D RID: 269
public class LevelHUDPlayerSuperCard : AbstractMonoBehaviour
{
	// Token: 0x06000C81 RID: 3201 RVA: 0x0000AEE6 File Offset: 0x000090E6
	public void Start()
	{
		this.end = base.transform.localPosition;
		this.start = this.end + new Vector3(0f, -30f, 0f);
	}

	// Token: 0x06000C82 RID: 3202 RVA: 0x0000AF1E File Offset: 0x0000911E
	public void Update()
	{
		this.UpdatePosition();
	}

	// Token: 0x06000C83 RID: 3203 RVA: 0x00083D14 File Offset: 0x00081F14
	public void UpdatePosition()
	{
		if (!this.initialized)
		{
			return;
		}
		this.target = Vector3.Lerp(this.start, this.end, this.current / this.max);
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, this.target, CupheadTime.Delta * 10f);
		this.image.fillAmount = Mathf.Lerp(this.image.fillAmount, this.current / this.max, CupheadTime.Delta * 10f);
	}

	// Token: 0x06000C84 RID: 3204 RVA: 0x00083DBC File Offset: 0x00081FBC
	public void Init(PlayerId playerId, float exCost)
	{
		this.max = exCost;
		if (playerId != PlayerId.PlayerOne)
		{
			if (playerId != PlayerId.PlayerTwo)
			{
				if (playerId != PlayerId.Any && playerId != PlayerId.None)
				{
				}
			}
			else
			{
				base.animator.SetInteger("Player", (!PlayerManager.player1IsMugman) ? 1 : 0);
			}
		}
		else
		{
			base.animator.SetInteger("Player", (!PlayerManager.player1IsMugman) ? 0 : 1);
		}
		this.initialized = true;
	}

	// Token: 0x06000C85 RID: 3205 RVA: 0x0000AF26 File Offset: 0x00009126
	public void SetAmount(float amount)
	{
		this.current = Mathf.Clamp(amount, 0f, this.max);
	}

	// Token: 0x06000C86 RID: 3206 RVA: 0x0000AF3F File Offset: 0x0000913F
	public void SetSuper(bool super)
	{
		base.animator.SetBool("Super", super);
	}

	// Token: 0x06000C87 RID: 3207 RVA: 0x0000AF52 File Offset: 0x00009152
	public void SetEx(bool ex)
	{
		base.animator.SetBool("Ex", ex);
	}

	// Token: 0x040009F1 RID: 2545
	public const float SPEED = 10f;

	// Token: 0x040009F2 RID: 2546
	public const float Y_DIFF = -30f;

	// Token: 0x040009F3 RID: 2547
	[SerializeField]
	public Image image;

	// Token: 0x040009F4 RID: 2548
	public bool initialized;

	// Token: 0x040009F5 RID: 2549
	public float current;

	// Token: 0x040009F6 RID: 2550
	public float max;

	// Token: 0x040009F7 RID: 2551
	public Vector3 start;

	// Token: 0x040009F8 RID: 2552
	public Vector3 end;

	// Token: 0x040009F9 RID: 2553
	public Vector3 target;
}
