// Decompiled with JetBrains decompiler
// Type: BestHTTP.Examples.GUIHelper
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using UnityEngine;

namespace BestHTTP.Examples
{
  public static class GUIHelper
  {
    private static GUIStyle centerAlignedLabel;
    private static GUIStyle rightAlignedLabel;
    public static Rect ClientArea;

    private static void Setup()
    {
      if (GUIHelper.centerAlignedLabel != null)
        return;
      GUIHelper.centerAlignedLabel = new GUIStyle(GUI.skin.label);
      GUIHelper.centerAlignedLabel.alignment = TextAnchor.MiddleCenter;
      GUIHelper.rightAlignedLabel = new GUIStyle(GUI.skin.label);
      GUIHelper.rightAlignedLabel.alignment = TextAnchor.MiddleRight;
    }

    public static void DrawArea(Rect area, bool drawHeader, System.Action action)
    {
      GUIHelper.Setup();
      GUI.Box(area, string.Empty);
      GUILayout.BeginArea(area);
      if (drawHeader)
      {
        GUIHelper.DrawCenteredText(SampleSelector.SelectedSample.DisplayName);
        GUILayout.Space(5f);
      }
      if (action != null)
        action();
      GUILayout.EndArea();
    }

    public static void DrawCenteredText(string msg)
    {
      GUIHelper.Setup();
      GUILayout.BeginHorizontal();
      GUILayout.FlexibleSpace();
      GUILayout.Label(msg, GUIHelper.centerAlignedLabel, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
    }

    public static void DrawRow(string key, string value)
    {
      GUIHelper.Setup();
      GUILayout.BeginHorizontal();
      GUILayout.Label(key);
      GUILayout.FlexibleSpace();
      GUILayout.Label(value, GUIHelper.rightAlignedLabel, new GUILayoutOption[0]);
      GUILayout.FlexibleSpace();
      GUILayout.EndHorizontal();
    }
  }
}
