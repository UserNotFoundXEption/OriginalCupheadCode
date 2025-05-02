using System;

// Token: 0x02000213 RID: 531
public class DragonLevelPeashot : BasicProjectile
{
	// Token: 0x1700028C RID: 652
	// (get) Token: 0x06001859 RID: 6233 RVA: 0x00014CD5 File Offset: 0x00012ED5
	// (set) Token: 0x0600185A RID: 6234 RVA: 0x00014CDD File Offset: 0x00012EDD
	public int color
	{
		get
		{
			return this._color;
		}
		set
		{
			this._color = value;
			this.SetColor();
		}
	}

	// Token: 0x0600185B RID: 6235 RVA: 0x00014CEC File Offset: 0x00012EEC
	public void SetColor()
	{
		base.animator.SetInteger("Color", this._color);
		if (this.color == 2)
		{
			this.SetParryable(true);
		}
	}

	// Token: 0x040013C6 RID: 5062
	public int _color;
}
