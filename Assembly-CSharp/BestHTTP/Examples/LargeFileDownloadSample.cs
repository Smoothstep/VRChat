// Decompiled with JetBrains decompiler
// Type: BestHTTP.Examples.LargeFileDownloadSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

namespace BestHTTP.Examples
{
  public sealed class LargeFileDownloadSample : MonoBehaviour
  {
    private string status = string.Empty;
    private int fragmentSize = 4096;
    private const string URL = "http://ipv4.download.thinkbroadband.com/100MB.zip";
    private HTTPRequest request;
    private float progress;

    private void Awake()
    {
      if (!PlayerPrefs.HasKey("DownloadLength"))
        return;
      this.progress = (float) PlayerPrefs.GetInt("DownloadProgress") / (float) PlayerPrefs.GetInt("DownloadLength");
    }

    private void OnDestroy()
    {
      if (this.request == null || this.request.State >= HTTPRequestStates.Finished)
        return;
      this.request.OnProgress = (OnDownloadProgressDelegate) null;
      this.request.Callback = (OnRequestFinishedDelegate) null;
      this.request.Abort();
    }

    private void OnGUI()
    {
      GUIHelper.DrawArea(GUIHelper.ClientArea, true, (Action) (() =>
      {
        GUILayout.Label("Request status: " + this.status);
        GUILayout.Space(5f);
        GUILayout.Label(string.Format("Progress: {0:P2} of {1:N0}Mb", (object) this.progress, (object) (PlayerPrefs.GetInt("DownloadLength") / 1048576)));
        double num = (double) GUILayout.HorizontalSlider(this.progress, 0.0f, 1f);
        GUILayout.Space(50f);
        if (this.request == null)
        {
          GUILayout.Label(string.Format("Desired Fragment Size: {0:N} KBytes", (object) (float) ((double) this.fragmentSize / 1024.0)));
          this.fragmentSize = (int) GUILayout.HorizontalSlider((float) this.fragmentSize, 4096f, 1.048576E+07f);
          GUILayout.Space(5f);
          if (!GUILayout.Button(!PlayerPrefs.HasKey("DownloadProgress") ? "Start Download" : "Continue Download"))
            return;
          this.StreamLargeFileTest();
        }
        else
        {
          if (this.request.State != HTTPRequestStates.Processing || !GUILayout.Button("Abort Download"))
            return;
          this.request.Abort();
        }
      }));
    }

    private void StreamLargeFileTest()
    {
      this.request = new HTTPRequest(new Uri("http://ipv4.download.thinkbroadband.com/100MB.zip"), (OnRequestFinishedDelegate) ((req, resp) =>
      {
        switch (req.State)
        {
          case HTTPRequestStates.Processing:
            if (!PlayerPrefs.HasKey("DownloadLength"))
            {
              string firstHeaderValue = resp.GetFirstHeaderValue("content-length");
              if (!string.IsNullOrEmpty(firstHeaderValue))
                PlayerPrefs.SetInt("DownloadLength", int.Parse(firstHeaderValue));
            }
            this.ProcessFragments(resp.GetStreamedFragments());
            this.status = "Processing";
            break;
          case HTTPRequestStates.Finished:
            if (resp.IsSuccess)
            {
              this.ProcessFragments(resp.GetStreamedFragments());
              if (resp.IsStreamingFinished)
              {
                this.status = "Streaming finished!";
                PlayerPrefs.DeleteKey("DownloadProgress");
                PlayerPrefs.Save();
                this.request = (HTTPRequest) null;
                break;
              }
              this.status = "Processing";
              break;
            }
            this.status = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", (object) resp.StatusCode, (object) resp.Message, (object) resp.DataAsText);
            Debug.LogWarning((object) this.status);
            this.request = (HTTPRequest) null;
            break;
          case HTTPRequestStates.Error:
            this.status = "Request Finished with Error! " + (req.Exception == null ? "No Exception" : req.Exception.Message + "\n" + req.Exception.StackTrace);
            Debug.LogError((object) this.status);
            this.request = (HTTPRequest) null;
            break;
          case HTTPRequestStates.Aborted:
            this.status = "Request Aborted!";
            Debug.LogWarning((object) this.status);
            this.request = (HTTPRequest) null;
            break;
          case HTTPRequestStates.ConnectionTimedOut:
            this.status = "Connection Timed Out!";
            Debug.LogError((object) this.status);
            this.request = (HTTPRequest) null;
            break;
          case HTTPRequestStates.TimedOut:
            this.status = "Processing the request Timed Out!";
            Debug.LogError((object) this.status);
            this.request = (HTTPRequest) null;
            break;
        }
      }));
      if (PlayerPrefs.HasKey("DownloadProgress"))
        this.request.SetRangeHeader(PlayerPrefs.GetInt("DownloadProgress"));
      else
        PlayerPrefs.SetInt("DownloadProgress", 0);
      this.request.DisableCache = true;
      this.request.UseStreaming = true;
      this.request.StreamFragmentSize = this.fragmentSize;
      this.request.Send();
    }

    private void ProcessFragments(List<byte[]> fragments)
    {
      if (fragments == null || fragments.Count <= 0)
        return;
      for (int index = 0; index < fragments.Count; ++index)
        PlayerPrefs.SetInt("DownloadProgress", PlayerPrefs.GetInt("DownloadProgress") + fragments[index].Length);
      PlayerPrefs.Save();
      this.progress = (float) PlayerPrefs.GetInt("DownloadProgress") / (float) PlayerPrefs.GetInt("DownloadLength");
    }
  }
}
