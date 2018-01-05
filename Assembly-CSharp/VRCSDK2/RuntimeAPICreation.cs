using System;
using System.Collections.Generic;
using UnityEngine;
using VRC.Core;

namespace VRCSDK2
{
	// Token: 0x02000C9D RID: 3229
	public class RuntimeAPICreation : MonoBehaviour
	{
		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06006428 RID: 25640 RVA: 0x0023D158 File Offset: 0x0023B558
		protected bool isUpdate
		{
			get
			{
				return !string.IsNullOrEmpty(this.pipelineManager.blueprintId);
			}
		}

		// Token: 0x0400493D RID: 18749
		public PipelineManager pipelineManager;

		// Token: 0x0400493E RID: 18750
		protected bool forceNewFileCreation;

		// Token: 0x0400493F RID: 18751
		protected bool useFileApi = true;

		// Token: 0x04004940 RID: 18752
		protected bool isUploading;

		// Token: 0x04004941 RID: 18753
		protected float uploadProgress;

		// Token: 0x04004942 RID: 18754
		protected string uploadMessage;

		// Token: 0x04004943 RID: 18755
		protected string uploadTitle;

		// Token: 0x04004944 RID: 18756
		protected string uploadVrcPath;

		// Token: 0x04004945 RID: 18757
		protected string uploadPluginPath;

		// Token: 0x04004946 RID: 18758
		protected string uploadUnityPackagePath;

		// Token: 0x04004947 RID: 18759
		protected string cloudFrontAssetUrl;

		// Token: 0x04004948 RID: 18760
		protected string cloudFrontImageUrl;

		// Token: 0x04004949 RID: 18761
		protected string cloudFrontPluginUrl;

		// Token: 0x0400494A RID: 18762
		protected string cloudFrontUnityPackageUrl;

		// Token: 0x0400494B RID: 18763
		protected CameraImageCapture imageCapture;

		// Token: 0x0400494C RID: 18764
		private bool cancelRequested;

		// Token: 0x0400494D RID: 18765
		private Dictionary<string, string> mRetryState = new Dictionary<string, string>();
	}
}
