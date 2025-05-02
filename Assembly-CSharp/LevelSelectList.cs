using System;
using System.Collections.Generic;
using RektTransform;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020004D7 RID: 1239
public class LevelSelectList : AbstractMonoBehaviour
{
	// Token: 0x06003356 RID: 13142 RVA: 0x0002A867 File Offset: 0x00028A67
	public override void Awake()
	{
		base.Awake();
		this.SetupList();
	}

	// Token: 0x06003357 RID: 13143 RVA: 0x000F3DA4 File Offset: 0x000F1FA4
	public void SetupList()
	{
		List<Scenes> list = new List<Scenes>();
		foreach (Scenes scenes in EnumUtils.GetValues<Scenes>())
		{
			if (this.GetSceneGroup(scenes).included)
			{
				list.Add(scenes);
			}
		}
		int num = 0;
		foreach (Scenes scenes2 in list)
		{
			GameObject gameObject = Object.Instantiate<GameObject>(this.button.gameObject);
			Button b = gameObject.GetComponent<Button>();
			string text = scenes2.ToString().Replace("scene_", string.Empty).Replace("level_", string.Empty).Replace("dice_palace_", string.Empty).Replace("platforming_", string.Empty);
			b.name = scenes2.ToString();
			gameObject.GetComponentInChildren<Text>().text = text;
			b.onClick.AddListener(delegate
			{
				SceneLoader.LoadScene(b.name, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
			});
			b.transform.SetParent(this.button.transform.parent);
			b.transform.ResetLocalTransforms();
			num++;
		}
		this.button.gameObject.SetActive(false);
		this.contentPanel.SetHeight(30f * (float)num);
	}

	// Token: 0x06003358 RID: 13144 RVA: 0x000F3F58 File Offset: 0x000F2158
	public LevelSelectList.SceneGroup GetSceneGroup(Scenes s)
	{
		foreach (LevelSelectList.SceneGroup sceneGroup in this.scenes)
		{
			if (sceneGroup.scene == s)
			{
				return sceneGroup;
			}
		}
		return new LevelSelectList.SceneGroup();
	}

	// Token: 0x06003359 RID: 13145 RVA: 0x000F3F98 File Offset: 0x000F2198
	public bool ContainsScene(Scenes s)
	{
		foreach (LevelSelectList.SceneGroup sceneGroup in this.scenes)
		{
			if (sceneGroup.scene == s)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x04002A67 RID: 10855
	[HideInInspector]
	public LevelSelectList.SceneGroup[] scenes;

	// Token: 0x04002A68 RID: 10856
	public Button button;

	// Token: 0x04002A69 RID: 10857
	public RectTransform contentPanel;

	// Token: 0x02001137 RID: 4407
	[Serializable]
	public class SceneGroup
	{
		// Token: 0x0400795F RID: 31071
		public bool included;

		// Token: 0x04007960 RID: 31072
		public Scenes scene;
	}
}
