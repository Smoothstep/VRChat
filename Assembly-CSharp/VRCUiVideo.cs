using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000C8E RID: 3214
public class VRCUiVideo : MonoBehaviour
{
	// Token: 0x060063CC RID: 25548 RVA: 0x0023711C File Offset: 0x0023551C
	private void OnEnable()
	{
		this.image = base.GetComponent<RawImage>();
		this.movie = Resources.Load<MovieTexture>(this.resourceName);
		if (this.movie != null)
		{
			this.image.texture = this.movie;
			this.movie.loop = true;
			this.movie.Play();
			return;
		}
		this.texture = Resources.Load<Texture2D>(this.resourceName);
		if (this.texture != null)
		{
			this.image.texture = this.texture;
			return;
		}
	}

	// Token: 0x060063CD RID: 25549 RVA: 0x002371B4 File Offset: 0x002355B4
	private void OnDisable()
	{
		if (this.movie != null)
		{
			this.movie.Stop();
			this.image.texture = null;
			Resources.UnloadAsset(this.movie);
			this.movie = null;
		}
		else if (this.texture != null)
		{
			this.image.texture = null;
			Resources.UnloadAsset(this.texture);
			this.texture = null;
		}
	}

	// Token: 0x04004919 RID: 18713
	public string resourceName;

	// Token: 0x0400491A RID: 18714
	private Texture2D texture;

	// Token: 0x0400491B RID: 18715
	private MovieTexture movie;

	// Token: 0x0400491C RID: 18716
	private RawImage image;
}
