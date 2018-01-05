// Decompiled with JetBrains decompiler
// Type: Analytics
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using BestHTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC.Core;
using VRCSDK2;

public class Analytics : MonoBehaviour
{
    public float MaxExitWait = 2f;
    private bool ApplicationExiting;
    private float QuitCancelTime;
    private int pendingClosingMessages;

    public static void Send(IEnumerable<ApiAnalyticEvent.EventInfo> events)
    {
        ApiAnalyticEvent.Send(events.Select<ApiAnalyticEvent.EventInfo, ApiAnalyticEvent.EventInfo>((Func<ApiAnalyticEvent.EventInfo, ApiAnalyticEvent.EventInfo>)(e =>
        {
            e.userId = APIUser.CurrentUser == null ? string.Empty : APIUser.CurrentUser.id;
            e.worldId = RoomManager.currentRoom == null ? string.Empty : RoomManager.currentRoom.id;
            if (!e.location.HasValue)
                e.location = new Vector3?(!((UnityEngine.Object)VRCPlayer.Instance != (UnityEngine.Object)null) ? Vector3.zero : VRCPlayer.Instance.transform.position);
            if (e.eventType == ApiAnalyticEvent.EventType.error)
                e.parameters.Add("buildVersion", (object)VRCApplicationSetup.GetBuildVersionString());
            return e;
        })));
    }

    public static void Send(ApiAnalyticEvent.EventType type)
    {
        Analytics.Send(type, (string)null, new Vector3?(), (System.Action<bool>)null);
    }

    public static void Send(ApiAnalyticEvent.EventType eventType, string detail, Vector3? location = null, System.Action<bool> completeCallback = null)
    {
        ApiAnalyticEvent.EventInfo[] eventInfoArray = new ApiAnalyticEvent.EventInfo[1];
        int index = 0;
        ApiAnalyticEvent.EventInfo eventInfo1 = new ApiAnalyticEvent.EventInfo();
        eventInfo1.eventType = eventType;
        eventInfo1.location = location;
        ApiAnalyticEvent.EventInfo eventInfo2 = eventInfo1;
        Dictionary<string, object> dictionary;
        if (!string.IsNullOrEmpty(detail))
            dictionary = new Dictionary<string, object>()
      {
        {
          "parameter",
          (object) detail
        }
      };
        else
            dictionary = new Dictionary<string, object>();
        eventInfo2.parameters = dictionary;
        eventInfo1.completeCallback = completeCallback;
        ApiAnalyticEvent.EventInfo eventInfo3 = eventInfo1;
        eventInfoArray[index] = eventInfo3;
        Analytics.Send((IEnumerable<ApiAnalyticEvent.EventInfo>)eventInfoArray);
    }

    public static void Send(ApiAnalyticEvent.EventType eventType, Dictionary<string, object> details, Vector3? location = null, System.Action<bool> completeCallback = null)
    {
        Analytics.Send((IEnumerable<ApiAnalyticEvent.EventInfo>)new ApiAnalyticEvent.EventInfo[1]
        {
      new ApiAnalyticEvent.EventInfo()
      {
        eventType = eventType,
        location = location,
        parameters = details,
        completeCallback = completeCallback
      }
        });
    }

    public static void VRC_Analytics_Send(VRC_Analytics.EventType eventType, Dictionary<string, object> details, Vector3? location)
    {
        try
        {
            Analytics.Send((ApiAnalyticEvent.EventType)Enum.Parse(typeof(ApiAnalyticEvent.EventType), eventType.ToString()), details, location, (System.Action<bool>)null);
        }
        catch (ArgumentException ex)
        {
            Debug.LogError((object)("Analytics.Send: invalid event type: " + eventType.ToString()));
        }
    }

    private void Start()
    {
        Dictionary<string, object> details = new Dictionary<string, object>();
        details["deviceId"] = (object)ApiModel.DeviceID;
        details["mode"] = !VRCApplicationSetup.LaunchedInDesktopMode ? (object)"VR" : (object)"Desktop";
        Analytics.Send(ApiAnalyticEvent.EventType.startsVrChat, details, new Vector3?(), (System.Action<bool>)null);
    }

    private void ResolveClosingMessage(bool res)
    {
        --this.pendingClosingMessages;
        if (this.pendingClosingMessages > 0)
            return;
        this.ActualQuit();
    }

    private void Update()
    {
        if (!this.ApplicationExiting || (double)Time.time - (double)this.QuitCancelTime <= (double)this.MaxExitWait)
            return;
        this.pendingClosingMessages = 0;
        this.ActualQuit();
    }

    private void ActualQuit()
    {
        HTTPManager.OnQuit();
        Application.Quit();
    }

    private void OnApplicationQuit()
    {
        if (this.ApplicationExiting)
            return;
        this.ApplicationExiting = true;
        this.QuitCancelTime = Time.time;
        Application.CancelQuit();
        List<ApiAnalyticEvent.EventInfo> eventInfoList = new List<ApiAnalyticEvent.EventInfo>();
        if (RoomManager.currentRoom != null)
            eventInfoList.Add(new ApiAnalyticEvent.EventInfo()
            {
                eventType = ApiAnalyticEvent.EventType.leavesWorld,
                parameters = new Dictionary<string, object>()
        {
          {
            "parameter",
            (object) RoomManager.currentRoom.id
          }
        },
                completeCallback = new System.Action<bool>(this.ResolveClosingMessage)
            });
        if (APIUser.CurrentUser != null)
            eventInfoList.Add(new ApiAnalyticEvent.EventInfo()
            {
                eventType = ApiAnalyticEvent.EventType.logout,
                completeCallback = new System.Action<bool>(this.ResolveClosingMessage)
            });
        eventInfoList.Add(new ApiAnalyticEvent.EventInfo()
        {
            eventType = ApiAnalyticEvent.EventType.leavesVrChat,
            completeCallback = new System.Action<bool>(this.ResolveClosingMessage),
            parameters = new Dictionary<string, object>()
      {
        {
          "deviceId",
          (object) ApiModel.DeviceID
        }
      }
        });
        this.pendingClosingMessages = eventInfoList.Count;
        Analytics.Send((IEnumerable<ApiAnalyticEvent.EventInfo>)eventInfoList);
    }
}
