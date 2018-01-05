// Decompiled with JetBrains decompiler
// Type: BestHTTP.Examples.CacheMaintenanceSample
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using BestHTTP.Caching;
using System;
using UnityEngine;

namespace BestHTTP.Examples
{
  public sealed class CacheMaintenanceSample : MonoBehaviour
  {
    private CacheMaintenanceSample.DeleteOlderTypes deleteOlderType = CacheMaintenanceSample.DeleteOlderTypes.Secs;
    private int value = 10;
    private int maxCacheSize = 5242880;

    private void OnGUI()
    {
      GUIHelper.DrawArea(GUIHelper.ClientArea, true, (Action) (() =>
      {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Delete cached entities older then");
        GUILayout.Label(this.value.ToString(), new GUILayoutOption[1]
        {
          GUILayout.MinWidth(50f)
        });
        this.value = (int) GUILayout.HorizontalSlider((float) this.value, 1f, 60f, GUILayout.MinWidth(100f));
        GUILayout.Space(10f);
        this.deleteOlderType = (CacheMaintenanceSample.DeleteOlderTypes) GUILayout.SelectionGrid((int) this.deleteOlderType, new string[4]
        {
          "Days",
          "Hours",
          "Mins",
          "Secs"
        }, 4);
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        GUILayout.BeginHorizontal();
        GUILayout.Label("Max Cache Size (bytes): ", new GUILayoutOption[1]
        {
          GUILayout.Width(150f)
        });
        GUILayout.Label(this.maxCacheSize.ToString("N0"), new GUILayoutOption[1]
        {
          GUILayout.Width(70f)
        });
        this.maxCacheSize = (int) GUILayout.HorizontalSlider((float) this.maxCacheSize, 1024f, 1.048576E+07f);
        GUILayout.EndHorizontal();
        GUILayout.Space(10f);
        if (!GUILayout.Button("Maintenance"))
          return;
        TimeSpan deleteOlder = TimeSpan.FromDays(14.0);
        switch (this.deleteOlderType)
        {
          case CacheMaintenanceSample.DeleteOlderTypes.Days:
            deleteOlder = TimeSpan.FromDays((double) this.value);
            break;
          case CacheMaintenanceSample.DeleteOlderTypes.Hours:
            deleteOlder = TimeSpan.FromHours((double) this.value);
            break;
          case CacheMaintenanceSample.DeleteOlderTypes.Mins:
            deleteOlder = TimeSpan.FromMinutes((double) this.value);
            break;
          case CacheMaintenanceSample.DeleteOlderTypes.Secs:
            deleteOlder = TimeSpan.FromSeconds((double) this.value);
            break;
        }
        HTTPCacheService.BeginMaintainence(new HTTPCacheMaintananceParams(deleteOlder, (ulong) this.maxCacheSize));
      }));
    }

    private enum DeleteOlderTypes
    {
      Days,
      Hours,
      Mins,
      Secs,
    }
  }
}
