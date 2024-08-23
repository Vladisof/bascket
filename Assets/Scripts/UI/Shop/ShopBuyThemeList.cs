using UnityEngine;
using System.Collections.Generic;
using Themes;
using TMPro;
using UI.Shop;
using UnityEngine.AddressableAssets;


public class ShopBuyThemeList : ShopBuyList
{
	protected override void Populate()
    {
		MRefreshCallback = null;
        foreach (Transform t in listRoot)
        {
            Destroy(t.gameObject);
        }

        foreach (KeyValuePair<string, ThemesData> pair in ThemesDatabases.dictionary)
        {
            ThemesData themes = pair.Value;
            if (themes != null)
            {
                prefabItem.InstantiateAsync().Completed += (op) =>
                {
                    if (op.Result == null || !(op.Result is GameObject))
                    {
                        Debug.LogWarning(string.Format("Unable to load theme shop list {0}.", prefabItem.Asset.name));
                        return;
                    }

                    if (themes.isDeepLink&& !PlayerSaveData.instance.themes.Contains("Secret"))
                    {
	                    GameObject newEntry = op.Result;
	                    newEntry.transform.SetParent(listRoot, false);

	                    ShopItemListBuy itm = newEntry.GetComponent<ShopItemListBuy>();
	                    itm.PriceParent.SetActive(false);
	                    itm.deepLinkText.gameObject.SetActive(true);
	                    itm.nameText.gameObject.SetActive(false);
	                    itm.priceText.gameObject.SetActive(false);
	                    itm.icon.sprite = themes.themeIcon;

	                    if (themes.premiumCost > 0)
	                    {
		                    itm.premiumText.transform.parent.gameObject.SetActive(true);
		                    itm.premiumText.text = themes.premiumCost.ToString();
	                    }
	                    else
	                    {
		                    itm.premiumText.transform.parent.gameObject.SetActive(false);
	                    }
	                    

	                    itm.buyButton.onClick.AddListener(delegate() { Buy(themes); });

	                    itm.buyButton.gameObject.SetActive(false);

	                    RefreshButton(itm, themes);
	                    MRefreshCallback += delegate() { RefreshButton(itm, themes); };
                    } else
                    {
	                    GameObject newEntry = op.Result;
	                    newEntry.transform.SetParent(listRoot, false);

	                    ShopItemListBuy itm = newEntry.GetComponent<ShopItemListBuy>();

	                    itm.nameText.text = themes.themeName;
	                    itm.priceText.text = themes.cost.ToString();
	                    itm.icon.sprite = themes.themeIcon;

	                    if (themes.premiumCost > 0)
	                    {
		                    itm.premiumText.transform.parent.gameObject.SetActive(true);
		                    itm.premiumText.text = themes.premiumCost.ToString();
	                    }
	                    else
	                    {
		                    itm.premiumText.transform.parent.gameObject.SetActive(false);
	                    }

	                    itm.buyButton.onClick.AddListener(delegate() { Buy(themes); });

	                    itm.buyButton.image.sprite = itm.buyButtonSprite;

	                    RefreshButton(itm, themes);
	                    MRefreshCallback += delegate() { RefreshButton(itm, themes); };
                    }
                   
                };
            }
        }
    }

	protected void RefreshButton(ShopItemListBuy itm, ThemesData themes)
	{
		if (themes.cost > PlayerSaveData.instance.coins)
		{
			itm.buyButton.interactable = false;
		}

		if (themes.premiumCost > PlayerSaveData.instance.premium)
		{
			itm.buyButton.interactable = false;
		}

		if (PlayerSaveData.instance.themes.Contains(themes.themeName))
		{
			itm.PriceParent.SetActive(false);
			itm.buyButton.gameObject.SetActive(true);
			itm.deepLinkText.gameObject.SetActive(false);
			itm.buyButton.interactable = false;
			itm.buyButton.image.sprite = itm.disabledButtonSprite;
			itm.buyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Owned";
		}
	}


	public void Buy(ThemesData t)
    {
        PlayerSaveData.instance.coins -= t.cost;
		    PlayerSaveData.instance.premium -= t.premiumCost;
        PlayerSaveData.instance.AddTheme(t.themeName);
        PlayerSaveData.instance.Save();
        
        Populate();
    }
}
