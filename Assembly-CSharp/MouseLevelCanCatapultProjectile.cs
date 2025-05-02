using System;
using UnityEngine;

// Token: 0x020002C4 RID: 708
public class MouseLevelCanCatapultProjectile : BasicProjectile
{
	// Token: 0x06001F69 RID: 8041 RVA: 0x000B5EFC File Offset: 0x000B40FC
	public MouseLevelCanCatapultProjectile CreateFromPrefab(Vector2 pos, float rotation, float speed, char c)
	{
		MouseLevelCanCatapultProjectile mouseLevelCanCatapultProjectile = base.Create(pos, rotation, speed) as MouseLevelCanCatapultProjectile;
		mouseLevelCanCatapultProjectile.Set(c);
		return mouseLevelCanCatapultProjectile;
	}

	// Token: 0x06001F6A RID: 8042 RVA: 0x0001A7F0 File Offset: 0x000189F0
	public override void RandomizeVariant()
	{
	}

	// Token: 0x06001F6B RID: 8043 RVA: 0x000B5F24 File Offset: 0x000B4124
	public void Set(char c)
	{
		int i;
		switch (c)
		{
		case 'b':
			break;
		case 'c':
			i = 4;
			goto IL_67;
		default:
			switch (c)
			{
			case 'n':
				i = 1;
				goto IL_67;
			case 'p':
				i = 3;
				goto IL_67;
			}
			break;
		case 'g':
			i = 2;
			this.SetParryable(true);
			goto IL_67;
		}
		i = 0;
		IL_67:
		this.SetInt(AbstractProjectile.Variant, i);
	}
}
