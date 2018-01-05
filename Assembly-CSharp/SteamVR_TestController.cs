// Decompiled with JetBrains decompiler
// Type: SteamVR_TestController
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 11D96FA5-73D5-49AE-8538-9A130950C0D8
// Assembly location: C:\Program Files (x86)\Steam\SteamApps\common\VRChat\VRChat_Data\Managed\Assembly-CSharp.dll

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR;

public class SteamVR_TestController : MonoBehaviour
{
    private List<int> controllerIndices = new List<int>();
    private EVRButtonId[] buttonIds = new EVRButtonId[4] { EVRButtonId.k_EButton_ApplicationMenu, EVRButtonId.k_EButton_Grip, EVRButtonId.k_EButton_Axis0, EVRButtonId.k_EButton_Axis1 };
    private EVRButtonId[] axisIds = new EVRButtonId[2] { EVRButtonId.k_EButton_Axis0, EVRButtonId.k_EButton_Axis1 };
    public Transform point;
    public Transform pointer;

    private void OnDeviceConnected(int index, bool connected)
    {
        CVRSystem system = OpenVR.System;
        if (system == null || system.GetTrackedDeviceClass((uint)index) != ETrackedDeviceClass.Controller)
            return;
        if (connected)
        {
            Debug.Log((object)string.Format("Controller {0} connected.", (object)index));
            this.PrintControllerStatus(index);
            this.controllerIndices.Add(index);
        }
        else
        {
            Debug.Log((object)string.Format("Controller {0} disconnected.", (object)index));
            this.PrintControllerStatus(index);
            this.controllerIndices.Remove(index);
        }
    }

    private void OnEnable()
    {
        SteamVR_Events.DeviceConnected.Listen(new UnityAction<int, bool>(this.OnDeviceConnected));
    }

    private void OnDisable()
    {
        SteamVR_Events.DeviceConnected.Remove(new UnityAction<int, bool>(this.OnDeviceConnected));
    }

    private void PrintControllerStatus(int index)
    {
        SteamVR_Controller.Device device = SteamVR_Controller.Input(index);
        Debug.Log((object)("index: " + (object)device.index));
        Debug.Log((object)("connected: " + (object)device.connected));
        Debug.Log((object)("hasTracking: " + (object)device.hasTracking));
        Debug.Log((object)("outOfRange: " + (object)device.outOfRange));
        Debug.Log((object)("calibrating: " + (object)device.calibrating));
        Debug.Log((object)("uninitialized: " + (object)device.uninitialized));
        Debug.Log((object)("pos: " + (object)device.transform.pos));
        Debug.Log((object)("rot: " + (object)device.transform.rot.eulerAngles));
        Debug.Log((object)("velocity: " + (object)device.velocity));
        Debug.Log((object)("angularVelocity: " + (object)device.angularVelocity));
        int deviceIndex1 = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Leftmost, ETrackedDeviceClass.Controller, 0);
        int deviceIndex2 = SteamVR_Controller.GetDeviceIndex(SteamVR_Controller.DeviceRelation.Rightmost, ETrackedDeviceClass.Controller, 0);
        Debug.Log(deviceIndex1 != deviceIndex2 ? (deviceIndex1 != index ? (object)"right" : (object)"left") : (object)"first");
    }

    private void Update()
    {
        foreach (int controllerIndex in this.controllerIndices)
        {
            SteamVR_Overlay instance = SteamVR_Overlay.instance;
            if ((bool)((Object)instance) && (bool)((Object)this.point) && (bool)((Object)this.pointer))
            {
                SteamVR_Utils.RigidTransform transform = SteamVR_Controller.Input(controllerIndex).transform;
                this.pointer.transform.localPosition = transform.pos;
                this.pointer.transform.localRotation = transform.rot;
                SteamVR_Overlay.IntersectionResults results = new SteamVR_Overlay.IntersectionResults();
                if (instance.ComputeIntersection(transform.pos, transform.rot * Vector3.forward, ref results))
                {
                    this.point.transform.localPosition = results.point;
                    this.point.transform.localRotation = Quaternion.LookRotation(results.normal);
                }
            }
            else
            {
                foreach (EVRButtonId buttonId in this.buttonIds)
                {
                    if (SteamVR_Controller.Input(controllerIndex).GetPressDown(buttonId))
                        Debug.Log((object)(((int)buttonId).ToString() + " press down"));
                    if (SteamVR_Controller.Input(controllerIndex).GetPressUp(buttonId))
                    {
                        Debug.Log((object)(((int)buttonId).ToString() + " press up"));
                        if (buttonId == EVRButtonId.k_EButton_Axis1)
                        {
                            SteamVR_Controller.Input(controllerIndex).TriggerHapticPulse((ushort)500, EVRButtonId.k_EButton_Axis0);
                            this.PrintControllerStatus(controllerIndex);
                        }
                    }
                    if (SteamVR_Controller.Input(controllerIndex).GetPress(buttonId))
                        Debug.Log((object)buttonId);
                }
                foreach (EVRButtonId axisId in this.axisIds)
                {
                    if (SteamVR_Controller.Input(controllerIndex).GetTouchDown(axisId))
                        Debug.Log((object)(((int)axisId).ToString() + " touch down"));
                    if (SteamVR_Controller.Input(controllerIndex).GetTouchUp(axisId))
                        Debug.Log((object)(((int)axisId).ToString() + " touch up"));
                    if (SteamVR_Controller.Input(controllerIndex).GetTouch(axisId))
                        Debug.Log((object)("axis: " + (object)SteamVR_Controller.Input(controllerIndex).GetAxis(axisId)));
                }
            }
        }
    }
}
