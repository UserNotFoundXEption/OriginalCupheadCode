using System;
using UnityEngine;

// Token: 0x020003D0 RID: 976
public class VeggiesLevelOnionTearsStream : AbstractMonoBehaviour
{
	// Token: 0x06002B0B RID: 11019 RVA: 0x000D556C File Offset: 0x000D376C
	public VeggiesLevelOnionTearsStream Create(Vector2 pos, int scale)
	{
		VeggiesLevelOnionTearsStream veggiesLevelOnionTearsStream = this.InstantiatePrefab<VeggiesLevelOnionTearsStream>();
		veggiesLevelOnionTearsStream.transform.SetScale(new float?((float)scale), new float?(1f), new float?(1f));
		veggiesLevelOnionTearsStream.transform.position = pos;
		return veggiesLevelOnionTearsStream;
	}

	// Token: 0x06002B0C RID: 11020 RVA: 0x00024251 File Offset: 0x00022451
	public void End()
	{
		if (this.ending)
		{
			return;
		}
		this.ending = true;
		base.animator.Play("Out");
	}

	// Token: 0x06002B0D RID: 11021 RVA: 0x00024276 File Offset: 0x00022476
	public void OnAnimEnd()
	{
		Object.Destroy(base.gameObject);
	}

	// Token: 0x040023CC RID: 9164
	public bool ending;
}
