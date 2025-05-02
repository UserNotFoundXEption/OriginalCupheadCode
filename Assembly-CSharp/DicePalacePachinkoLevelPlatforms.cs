using System;
using UnityEngine;

// Token: 0x02000200 RID: 512
public class DicePalacePachinkoLevelPlatforms : AbstractCollidableObject
{
	// Token: 0x06001795 RID: 6037 RVA: 0x000A1EB0 File Offset: 0x000A00B0
	public void InitPlatforms(LevelProperties.DicePalacePachinko properties)
	{
		Vector3 vector;
		vector.y = (float)Level.Current.Ground;
		vector.x = (float)Level.Current.Left;
		vector.z = 0f;
		for (int i = 0; i < 3; i++)
		{
			int num;
			if (i != 0)
			{
				num = ((i % 2 != 0) ? 4 : 3);
			}
			else
			{
				num = 3;
			}
			for (int j = 0; j < num; j++)
			{
				GameObject gameObject = this.platformSprite.gameObject;
				Vector2 vector2;
				if (num == 4)
				{
					vector2.x = properties.CurrentState.pachinko.platformWidthFour;
				}
				else
				{
					vector2.x = properties.CurrentState.pachinko.platformWidthThree;
				}
				vector2.y = 1f;
				gameObject.transform.localScale = vector2;
				Vector3 vector3 = vector;
				if (num == 3)
				{
					float num2 = (float)Level.Current.Width - gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 3.6f;
					vector3.x = vector3.x + num2 / (float)(num - 1) * (float)j + gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 1.8f;
				}
				else
				{
					vector3.x += (float)(Level.Current.Width / (num - 1) * j);
				}
				vector3.y = (float)Level.Current.Ground + Parser.FloatParse(properties.CurrentState.pachinko.platformHeights.Split(new char[]
				{
					','
				})[i]) + gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y / 2f;
				if (j == 0)
				{
					vector3.x += gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f;
				}
				else if (j == num - 1)
				{
					vector3.x -= gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x / 2f;
				}
				GameObject gameObject2 = new GameObject();
				gameObject2.AddComponent<LevelPlatform>();
				gameObject2.GetComponent<BoxCollider2D>().size = new Vector2(gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.x * vector2.x, gameObject.GetComponent<SpriteRenderer>().sprite.bounds.size.y);
				gameObject2.transform.position = vector3;
				Object.Instantiate<GameObject>(gameObject, vector3, Quaternion.identity);
			}
		}
	}

	// Token: 0x04001327 RID: 4903
	public const int NUMBER_OF_ROWS = 3;

	// Token: 0x04001328 RID: 4904
	public const int MAX_NUMBER_OF_COLUMNS = 4;

	// Token: 0x04001329 RID: 4905
	[SerializeField]
	public SpriteRenderer platformSprite;
}
