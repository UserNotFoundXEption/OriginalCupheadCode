using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200010B RID: 267
public class LevelHUDPlayerHealth : AbstractLevelHUDComponent
{
	// Token: 0x06000C78 RID: 3192 RVA: 0x0000AE67 File Offset: 0x00009067
	public override void Awake()
	{
		base.Awake();
		this.image = base.GetComponent<Image>();
	}

	// Token: 0x06000C79 RID: 3193 RVA: 0x0000AE7B File Offset: 0x0000907B
	public override void Init(LevelHUDPlayer hud)
	{
		base.Init(hud);
		this.lastHealth = base._player.stats.Health;
		this.OnHealthChanged(base._player.stats.Health);
	}

	// Token: 0x06000C7A RID: 3194 RVA: 0x0008384C File Offset: 0x00081A4C
	public void OnHealthChanged(int health)
	{
		base.animator.SetInteger("Health", Mathf.Clamp(health, 0, base._player.stats.HealthMax));
		base.animator.Play("Entry");
		if (this.lastHealth != health)
		{
			this.OnChangedHealth();
		}
		this.lastHealth = health;
	}

	// Token: 0x06000C7B RID: 3195 RVA: 0x0000AEB0 File Offset: 0x000090B0
	public void OnChangedHealth()
	{
		base.TweenValue(0f, 1f, 0.3f, EaseUtils.EaseType.easeOutSine, new AbstractMonoBehaviour.TweenUpdateHandler(this.ChangedHealthTween));
	}

	// Token: 0x06000C7C RID: 3196 RVA: 0x000838AC File Offset: 0x00081AAC
	public void ChangedHealthTween(float value)
	{
		Color white = Color.white;
		Color gray = Color.gray;
		base.transform.localScale = Vector3.one * Mathf.Lerp(2f, 1f, value);
		this.image.color = Color.Lerp(white, gray, value);
	}

	// Token: 0x040009E9 RID: 2537
	public const string HealthParameter = "Health";

	// Token: 0x040009EA RID: 2538
	public Image image;

	// Token: 0x040009EB RID: 2539
	public int lastHealth;
}
