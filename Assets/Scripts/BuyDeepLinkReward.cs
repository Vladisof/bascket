using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyDeepLinkReward : MonoBehaviour
{
  public Button buyButton;
  public GameObject poPup;
  
  private void Start()
  {
    buyButton.onClick.AddListener(Buy);
  }

  private void Buy()
  {
    PlayerSaveData.instance.DeepLinkClaim();
    PlayerSaveData.instance.coins -= 2000;
    poPup.SetActive(false);
  }

}
