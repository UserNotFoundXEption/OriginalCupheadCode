using System;
using UnityEngine;

// Token: 0x02000600 RID: 1536
[Serializable]
public class NextGenRingPieces
{
	// Token: 0x06003FEE RID: 16366 RVA: 0x0012A35C File Offset: 0x0012855C
	public Texture[] getPieces()
	{
		if (this._pieces == null)
		{
			this._pieces = new Texture[6];
			this._pieces[0] = this.topRight;
			this._pieces[1] = this.middleRight;
			this._pieces[2] = this.bottomRight;
			this._pieces[3] = this.topLeft;
			this._pieces[4] = this.middleLeft;
			this._pieces[5] = this.bottomLeft;
		}
		return this._pieces;
	}

	// Token: 0x040032E8 RID: 13032
	public Texture topLeft;

	// Token: 0x040032E9 RID: 13033
	public Texture topRight;

	// Token: 0x040032EA RID: 13034
	public Texture middleLeft;

	// Token: 0x040032EB RID: 13035
	public Texture middleRight;

	// Token: 0x040032EC RID: 13036
	public Texture bottomLeft;

	// Token: 0x040032ED RID: 13037
	public Texture bottomRight;

	// Token: 0x040032EE RID: 13038
	public Texture[] _pieces;
}
