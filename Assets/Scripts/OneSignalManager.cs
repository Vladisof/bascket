using System;
using System.Collections;
using System.Collections.Generic;
using OneSignalSDK;
using UnityEngine;

public class OneSignalManager : MonoBehaviour
{
  private void Awake()
  {
    OneSignal.Initialize("36046332-393b-4c7a-bb4b-9435ce6733d0");
    OneSignal.Notifications.RequestPermissionAsync(true);
  }
}
