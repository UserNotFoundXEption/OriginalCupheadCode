using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Token: 0x020004DC RID: 1244
public class UserProfileDisplay : AbstractMonoBehaviour
{
	// Token: 0x0600338F RID: 13199 RVA: 0x0002AA51 File Offset: 0x00028C51
	public override void Awake()
	{
		this.root.SetActive(false);
		this.gamerPic.gameObject.SetActive(false);
	}

	// Token: 0x06003390 RID: 13200 RVA: 0x000F56FC File Offset: 0x000F38FC
	public void Start()
	{
		this.isSlotSelect = (SceneManager.GetActiveScene().name == Scenes.scene_slot_select.ToString());
	}

	// Token: 0x06003391 RID: 13201 RVA: 0x000F5730 File Offset: 0x000F3930
	public void Update()
	{
		if (OnlineManager.Instance.Interface.SupportsMultipleUsers || (this.showForMultipleUsersUnsupported && OnlineManager.Instance.Interface.SupportsUserSignIn))
		{
			OnlineUser user = OnlineManager.Instance.Interface.GetUser(this.player);
			if (user != null)
			{
				this.root.SetActive(true);
				string name = user.Name;
				if (this.gamerTag.text != name)
				{
					this.gamerTag.text = name;
				}
				Texture2D profilePic = OnlineManager.Instance.Interface.GetProfilePic(this.player);
				if (profilePic != null && this.currentPicUser != user)
				{
					this.currentPicUser = user;
					Sprite sprite = Sprite.Create(profilePic, new Rect(0f, 0f, (float)profilePic.width, (float)profilePic.height), new Vector2(0.5f, 0.5f));
					this.gamerPic.sprite = sprite;
					this.gamerPic.gameObject.SetActive(true);
				}
				else if (profilePic == null)
				{
					this.gamerPic.gameObject.SetActive(false);
				}
			}
			else
			{
				this.root.SetActive(false);
			}
		}
		else
		{
			this.root.SetActive(false);
		}
	}

	// Token: 0x04002ACD RID: 10957
	[SerializeField]
	public Image gamerPic;

	// Token: 0x04002ACE RID: 10958
	[SerializeField]
	public Text gamerTag;

	// Token: 0x04002ACF RID: 10959
	[SerializeField]
	public PlayerId player;

	// Token: 0x04002AD0 RID: 10960
	[SerializeField]
	public GameObject root;

	// Token: 0x04002AD1 RID: 10961
	[SerializeField]
	public GameObject switchPromptRoot;

	// Token: 0x04002AD2 RID: 10962
	[SerializeField]
	public bool showForMultipleUsersUnsupported;

	// Token: 0x04002AD3 RID: 10963
	[SerializeField]
	public Sprite defaultAvatarCuphead;

	// Token: 0x04002AD4 RID: 10964
	[SerializeField]
	public Sprite defaultAvatarMugman;

	// Token: 0x04002AD5 RID: 10965
	public OnlineUser currentPicUser;

	// Token: 0x04002AD6 RID: 10966
	public bool isSlotSelect;
}
