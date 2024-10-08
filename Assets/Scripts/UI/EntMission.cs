﻿using Missions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
namespace UI
{
	public class EntMission : MonoBehaviour
	{
		public TextMeshProUGUI descText;
		public TextMeshProUGUI rewardText;
		public Button claimButton;
		public TextMeshProUGUI progressText;
		public Image background;

		public Color notCompletedColor;
		public Color completedColor;

		public void FillWithMission(MissionsBase m, Missions owner)
		{
			descText.text = m.GetMissionDesc();
			rewardText.text = m.reward.ToString();

			if (m.isComplete)
			{
				claimButton.gameObject.SetActive(true);
				progressText.gameObject.SetActive(false);

				background.color = completedColor;
				

				claimButton.onClick.AddListener(delegate { owner.Claim(m); } );
			} else
			{
				claimButton.gameObject.SetActive(false);
				progressText.gameObject.SetActive(true);

				background.color = notCompletedColor;

				progressText.color = Color.black;
				//descText.color = completedColor;

				progressText.text = ((int)m.progress) + " / " + ((int)m.max);
			}
		}
	}
}
