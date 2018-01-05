using System;
using System.Collections;
using System.IO;
using Amazon;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using UnityEngine;
using VRC.Core;

// Token: 0x02000C93 RID: 3219
public class S3Manager : MonoBehaviour
{
	// Token: 0x17000DC5 RID: 3525
	// (get) Token: 0x060063F0 RID: 25584 RVA: 0x00237797 File Offset: 0x00235B97
	private RegionEndpoint _CognitoIdentityRegion
	{
		get
		{
			return RegionEndpoint.GetBySystemName(this.CognitoIdentityRegion);
		}
	}

	// Token: 0x17000DC6 RID: 3526
	// (get) Token: 0x060063F1 RID: 25585 RVA: 0x002377A4 File Offset: 0x00235BA4
	private RegionEndpoint _S3Region
	{
		get
		{
			return RegionEndpoint.GetBySystemName(this.S3Region);
		}
	}

	// Token: 0x17000DC7 RID: 3527
	// (get) Token: 0x060063F2 RID: 25586 RVA: 0x002377B1 File Offset: 0x00235BB1
	private AWSCredentials Credentials
	{
		get
		{
			if (this._credentials == null)
			{
				this._credentials = new CognitoAWSCredentials(this.IdentityPoolId, this._CognitoIdentityRegion);
			}
			return this._credentials;
		}
	}

	// Token: 0x17000DC8 RID: 3528
	// (get) Token: 0x060063F3 RID: 25587 RVA: 0x002377DB File Offset: 0x00235BDB
	private IAmazonS3 Client
	{
		get
		{
			if (this._s3Client == null)
			{
				this._s3Client = new AmazonS3Client(this.Credentials, this._S3Region);
			}
			return this._s3Client;
		}
	}

	// Token: 0x060063F4 RID: 25588 RVA: 0x00237805 File Offset: 0x00235C05
	private void Start()
	{
		UnityInitializer.AttachToGameObject(base.gameObject);
	}

	// Token: 0x060063F5 RID: 25589 RVA: 0x00237814 File Offset: 0x00235C14
	public PostObjectRequest PostObject(string filePath, string s3FolderName, Action<string> onSuccess = null)
	{
		string text = s3FolderName + "/" + Path.GetFileName(filePath);
		VRC.Core.Logger.Log("uploading " + text, DebugLevel.All);
		AWSConfigs.LoggingConfig.LogTo = LoggingOptions.None;
		FileStream inputStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
		VRC.Core.Logger.Log("Creating request object", DebugLevel.All);
		PostObjectRequest postObjectRequest = new PostObjectRequest
		{
			Bucket = this.S3BucketName,
			Key = text,
			InputStream = inputStream,
			CannedACL = S3CannedACL.Private
		};
		VRC.Core.Logger.Log("Making HTTP post call", DebugLevel.All);
		base.StartCoroutine(this.PostObjectRoutine(postObjectRequest, onSuccess));
		return postObjectRequest;
	}

	// Token: 0x060063F6 RID: 25590 RVA: 0x002378AC File Offset: 0x00235CAC
	private IEnumerator PostObjectRoutine(PostObjectRequest request, Action<string> onSuccess)
	{
		yield return null;
		yield return null;
		this.Client.PostObjectAsync(request, delegate(AmazonServiceResult<PostObjectRequest, PostObjectResponse> responseObj)
		{
			if (responseObj.Exception == null)
			{
				VRC.Core.Logger.Log("object " + responseObj.Request.Key + " posted to bucket " + responseObj.Request.Bucket, DebugLevel.All);
				string obj = string.Format("https://s3-us-west-2.amazonaws.com/{0}/{1}", responseObj.Request.Bucket, responseObj.Request.Key);
				if (onSuccess != null)
				{
					onSuccess(obj);
				}
			}
			else
			{
				VRC.Core.Logger.Log("Exception while posting the result object", DebugLevel.Always);
				VRC.Core.Logger.Log("receieved error " + responseObj.Response.HttpStatusCode.ToString(), DebugLevel.Always);
			}
		}, null);
		yield break;
	}

	// Token: 0x04004928 RID: 18728
	public string IdentityPoolId = "us-east-1:066cd25b-b249-4394-a267-7a49247aa8f9";

	// Token: 0x04004929 RID: 18729
	public string CognitoIdentityRegion = RegionEndpoint.USEast1.SystemName;

	// Token: 0x0400492A RID: 18730
	public string S3Region = RegionEndpoint.USEast1.SystemName;

	// Token: 0x0400492B RID: 18731
	public string S3BucketName = "vrc-uploads";

	// Token: 0x0400492C RID: 18732
	private IAmazonS3 _s3Client;

	// Token: 0x0400492D RID: 18733
	private AWSCredentials _credentials;
}
