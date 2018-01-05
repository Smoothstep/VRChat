// Decompiled with JetBrains decompiler
// Type: BestHTTP.Examples.TextureDownloadSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using System;
using UnityEngine;

namespace BestHTTP.Examples
{
  public sealed class TextureDownloadSample : MonoBehaviour
  {
    private string[] Images = new string[9]{ "One.png", "Two.png", "Three.png", "Four.png", "Five.png", "Six.png", "Seven.png", "Eight.png", "Nine.png" };
    private Texture2D[] Textures = new Texture2D[9];
    private const string BaseURL = "https://besthttp.azurewebsites.net/Content/";
    private bool allDownloadedFromLocalCache;
    private int finishedCount;
    private Vector2 scrollPos;

    private void Awake()
    {
      HTTPManager.MaxConnectionPerServer = (byte) 1;
      for (int index = 0; index < this.Images.Length; ++index)
        this.Textures[index] = new Texture2D(100, 150);
    }

    private void OnDestroy()
    {
      HTTPManager.MaxConnectionPerServer = (byte) 4;
    }

    private void OnGUI()
    {
      GUIHelper.DrawArea(GUIHelper.ClientArea, true, (Action) (() =>
      {
        this.scrollPos = GUILayout.BeginScrollView(this.scrollPos);
        GUILayout.SelectionGrid(0, (Texture[]) this.Textures, 3);
        if (this.finishedCount == this.Images.Length && this.allDownloadedFromLocalCache)
          GUIHelper.DrawCenteredText("All images loaded from the local cache!");
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max Connection/Server: ", new GUILayoutOption[1]
        {
          GUILayout.Width(150f)
        });
        GUILayout.Label(HTTPManager.MaxConnectionPerServer.ToString(), new GUILayoutOption[1]
        {
          GUILayout.Width(20f)
        });
        HTTPManager.MaxConnectionPerServer = (byte) GUILayout.HorizontalSlider((float) HTTPManager.MaxConnectionPerServer, 1f, 10f);
        GUILayout.EndHorizontal();
        if (GUILayout.Button("Start Download"))
          this.DownloadImages();
        GUILayout.EndScrollView();
      }));
    }

    private void DownloadImages()
    {
      this.allDownloadedFromLocalCache = true;
      this.finishedCount = 0;
      for (int index = 0; index < this.Images.Length; ++index)
      {
        this.Textures[index] = new Texture2D(100, 150);
        new HTTPRequest(new Uri("https://besthttp.azurewebsites.net/Content/" + this.Images[index]), new OnRequestFinishedDelegate(this.ImageDownloaded))
        {
          UseAlternateSSL = true,
          Tag = ((object) this.Textures[index])
        }.Send();
      }
    }

    private void ImageDownloaded(HTTPRequest req, HTTPResponse resp)
    {
      ++this.finishedCount;
      switch (req.State)
      {
        case HTTPRequestStates.Finished:
          if (resp.IsSuccess)
          {
            (req.Tag as Texture2D).LoadImage(resp.Data);
            this.allDownloadedFromLocalCache = this.allDownloadedFromLocalCache && resp.IsFromCache;
            break;
          }
          Debug.LogWarning((object) string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", (object) resp.StatusCode, (object) resp.Message, (object) resp.DataAsText));
          break;
        case HTTPRequestStates.Error:
          Debug.LogError((object) ("Request Finished with Error! " + (req.Exception == null ? "No Exception" : req.Exception.Message + "\n" + req.Exception.StackTrace)));
          break;
        case HTTPRequestStates.Aborted:
          Debug.LogWarning((object) "Request Aborted!");
          break;
        case HTTPRequestStates.ConnectionTimedOut:
          Debug.LogError((object) "Connection Timed Out!");
          break;
        case HTTPRequestStates.TimedOut:
          Debug.LogError((object) "Processing the request Timed Out!");
          break;
      }
    }
  }
}
