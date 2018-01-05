// Decompiled with JetBrains decompiler
// Type: BestHTTP.Examples.GUIMessageList
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;

namespace BestHTTP.Examples
{
  public class GUIMessageList
  {
    private List<string> messages = new List<string>();
    private Vector2 scrollPos;

    public void Draw()
    {
      this.Draw((float) Screen.width, 0.0f);
    }

    public void Draw(float minWidth, float minHeight)
    {
      this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, false, false, new GUILayoutOption[1]
      {
        GUILayout.MinHeight(minHeight)
      });
      for (int index = 0; index < this.messages.Count; ++index)
        GUILayout.Label(this.messages[index], new GUILayoutOption[1]
        {
          GUILayout.MinWidth(minWidth)
        });
      GUILayout.EndScrollView();
    }

    public void Add(string msg)
    {
      this.messages.Add(msg);
      this.scrollPos = new Vector2(this.scrollPos.x, float.MaxValue);
    }

    public void Clear()
    {
      this.messages.Clear();
    }
  }
}
