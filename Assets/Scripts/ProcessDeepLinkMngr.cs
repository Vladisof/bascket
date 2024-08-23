using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProcessDeepLinkMngr : MonoBehaviour
{
  private static ProcessDeepLinkMngr Instance
  {
    get;
    set;
  }
  public string deeplinkURL;
  public GameObject deepLinkPopup;


  private void Awake()
  {
    
    if (Instance == null)
    {
      Instance = this;
      Application.deepLinkActivated += onDeepLinkActivated;

      if (!string.IsNullOrEmpty(Application.absoluteURL))
      {
        onDeepLinkActivated(Application.absoluteURL);
      } else
        deeplinkURL = "[none]";

      DontDestroyOnLoad(gameObject);
    } else
    {
      Destroy(gameObject);
    }
  }

  private void onDeepLinkActivated (string url)
  {
    deeplinkURL = url;
    PlayerSaveData.Create();

    if (!PlayerSaveData.instance.isDeepLinkClaimed)
    {
      deepLinkPopup.SetActive(true);
      PlayerSaveData.instance.DeepLinkActive();
    }
  }
}