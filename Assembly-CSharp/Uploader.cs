using System;
using Amazon.S3.Model;
using UnityEngine;

// Token: 0x02000CA1 RID: 3233
public class Uploader
{
	// Token: 0x0600642E RID: 25646 RVA: 0x0023D190 File Offset: 0x0023B590
	public static PostObjectRequest UploadFile(string fileName, string s3FolderName = "other", Action<string> onSuccess = null)
	{
		if (Uploader.uploadManager == null)
		{
			Uploader.uploadManager = new GameObject();
			Uploader.uploadManager.name = "UploadManager";
		}
		S3Manager orAddComponent = Uploader.uploadManager.GetOrAddComponent<S3Manager>();
		return orAddComponent.PostObject(fileName, s3FolderName, onSuccess);
	}

	// Token: 0x04004976 RID: 18806
	public static GameObject uploadManager;
}
