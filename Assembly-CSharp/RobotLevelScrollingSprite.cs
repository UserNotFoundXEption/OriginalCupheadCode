using System;
using UnityEngine;

// Token: 0x02000338 RID: 824
public class RobotLevelScrollingSprite : ScrollingSpriteSpawner
{
	// Token: 0x0600240A RID: 9226 RVA: 0x000C261C File Offset: 0x000C081C
	public override void OnSpawn(GameObject obj)
	{
		base.OnSpawn(obj);
		SpriteRenderer component = obj.GetComponent<SpriteRenderer>();
		component.sortingLayerName = this.layer.ToString();
		component.sprite = this.sprites[Random.Range(0, this.sprites.Length)];
		Vector3 vector = Vector3.up * Random.Range(this.yOffset.min, this.yOffset.max);
		obj.transform.position += vector;
		obj.transform.localScale = new Vector3(base.transform.localScale.x * (float)MathUtils.PlusOrMinus(), base.transform.localScale.y, base.transform.localScale.z);
	}

	// Token: 0x04001DDD RID: 7645
	[SerializeField]
	public SpriteLayer layer = SpriteLayer.Default;

	// Token: 0x04001DDE RID: 7646
	[SerializeField]
	public MinMax yOffset;

	// Token: 0x04001DDF RID: 7647
	[SerializeField]
	public Sprite[] sprites;
}
