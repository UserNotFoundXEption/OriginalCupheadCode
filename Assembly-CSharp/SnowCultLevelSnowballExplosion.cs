using System;
using UnityEngine;

// Token: 0x02000399 RID: 921
public class SnowCultLevelSnowballExplosion : MonoBehaviour
{
	// Token: 0x060028A5 RID: 10405 RVA: 0x000CF350 File Offset: 0x000CD550
	public void Init(Vector3 pos, SnowCultLevelSnowball.Size size, SnowCultLevelYeti main)
	{
		base.transform.position = pos;
		if (size != SnowCultLevelSnowball.Size.Large)
		{
			if (size != SnowCultLevelSnowball.Size.Medium)
			{
				if (size == SnowCultLevelSnowball.Size.Small)
				{
					int smallExplosion = main.GetSmallExplosion();
					if (smallExplosion != 0)
					{
						if (smallExplosion != 1)
						{
							if (smallExplosion == 2)
							{
								this.animator.Play("SmallC");
							}
						}
						else
						{
							this.animator.Play("SmallB");
						}
					}
					else
					{
						this.animator.Play("SmallA");
					}
				}
			}
			else
			{
				this.animator.Play((main.GetMediumExplosion() != 0) ? "MediumB" : "MediumA");
			}
		}
		else
		{
			this.animator.Play("Large");
		}
	}

	// Token: 0x060028A6 RID: 10406 RVA: 0x00022241 File Offset: 0x00020441
	public void Update()
	{
		if (!this.rend.enabled)
		{
			this.Recycle<SnowCultLevelSnowballExplosion>();
		}
	}

	// Token: 0x040021E9 RID: 8681
	[SerializeField]
	public SpriteRenderer rend;

	// Token: 0x040021EA RID: 8682
	[SerializeField]
	public Animator animator;
}
