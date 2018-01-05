using System;
using System.Collections;
using UnityEngine;

namespace BestHTTP.Examples
{
    // Token: 0x020003EA RID: 1002
    public sealed class AssetBundleSample : MonoBehaviour
    {
        // Token: 0x060023E1 RID: 9185 RVA: 0x000B2BF0 File Offset: 0x000B0FF0
        private void OnGUI()
        {
            GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
            {
                GUILayout.Label("Status: " + this.status, new GUILayoutOption[0]);
                if (this.texture != null)
                {
                    GUILayout.Box(this.texture, new GUILayoutOption[]
                    {
                        GUILayout.MaxHeight(256f)
                    });
                }
                if (!this.downloading && GUILayout.Button("Start Download", new GUILayoutOption[0]))
                {
                    this.UnloadBundle();
                    base.StartCoroutine(this.DownloadAssetBundle());
                }
            });
        }

        // Token: 0x060023E2 RID: 9186 RVA: 0x000B2C09 File Offset: 0x000B1009
        private void OnDestroy()
        {
            this.UnloadBundle();
        }

        // Token: 0x060023E3 RID: 9187 RVA: 0x000B2C14 File Offset: 0x000B1014
        private IEnumerator DownloadAssetBundle()
        {
            this.downloading = true;
            HTTPRequest request = new HTTPRequest(new Uri(" https://goo.gl/ZLycrQ")).Send();
            this.status = "Download started";
            while (request.State < HTTPRequestStates.Finished)
            {
                yield return new WaitForSeconds(0.1f);
                this.status += ".";
            }
            switch (request.State)
            {
                case HTTPRequestStates.Finished:
                    if (request.Response.IsSuccess)
                    {
                        this.status = string.Format("AssetBundle downloaded! Loaded from local cache: {0}", request.Response.IsFromCache.ToString());
                        AssetBundleCreateRequest async = AssetBundle.LoadFromMemoryAsync(request.Response.Data);
                        yield return async;
                        yield return base.StartCoroutine(this.ProcessAssetBundle(async.assetBundle));
                    }
                    else
                    {
                        this.status = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", request.Response.StatusCode, request.Response.Message, request.Response.DataAsText);
                        Debug.LogWarning(this.status);
                    }
                    break;
                case HTTPRequestStates.Error:
                    this.status = "Request Finished with Error! " + ((request.Exception == null) ? "No Exception" : (request.Exception.Message + "\n" + request.Exception.StackTrace));
                    Debug.LogError(this.status);
                    break;
                case HTTPRequestStates.Aborted:
                    this.status = "Request Aborted!";
                    Debug.LogWarning(this.status);
                    break;
                case HTTPRequestStates.ConnectionTimedOut:
                    this.status = "Connection Timed Out!";
                    Debug.LogError(this.status);
                    break;
                case HTTPRequestStates.TimedOut:
                    this.status = "Processing the request Timed Out!";
                    Debug.LogError(this.status);
                    break;
            }
            this.downloading = false;
            yield break;
        }

        // Token: 0x060023E4 RID: 9188 RVA: 0x000B2C30 File Offset: 0x000B1030
        private IEnumerator ProcessAssetBundle(AssetBundle bundle)
        {
            if (bundle == null)
            {
                yield break;
            }
            this.cachedBundle = bundle;
            AssetBundleRequest asyncAsset = this.cachedBundle.LoadAssetAsync("9443182_orig", typeof(Texture2D));
            yield return asyncAsset;
            this.texture = (asyncAsset.asset as Texture2D);
            yield break;
        }

        // Token: 0x060023E5 RID: 9189 RVA: 0x000B2C52 File Offset: 0x000B1052
        private void UnloadBundle()
        {
            if (this.cachedBundle != null)
            {
                this.cachedBundle.Unload(true);
                this.cachedBundle = null;
            }
        }

        // Token: 0x040011E5 RID: 4581
        private const string URL = " https://goo.gl/ZLycrQ";

        // Token: 0x040011E6 RID: 4582
        private string status = "Waiting for user interaction";

        // Token: 0x040011E7 RID: 4583
        private AssetBundle cachedBundle;

        // Token: 0x040011E8 RID: 4584
        private Texture2D texture;

        // Token: 0x040011E9 RID: 4585
        private bool downloading;
    }
}
