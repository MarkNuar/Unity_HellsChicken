using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace HellsChicken.Scripts.Game.UI
{
    public class LevelTimerDisplay : MonoBehaviour
    {

        [SerializeField] private List<TextMeshProUGUI> bestTimes;
        
        private void OnEnable()
        {
            for (var i = 0; i < bestTimes.Count; i++)
            {
                if (GameManager.Instance.IsBestTimeNonZero(i+1))
                {
                    bestTimes[i].text = "Best time<br>" + GetFormattedTime(GameManager.Instance.GetBestTime(i+1));
                }
                else
                {
                    bestTimes[i].text = "Not played yet";
                }
                
            }
        }

        private string GetFormattedTime(float timer)
        {
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer % 60F);
            int milliseconds = Mathf.FloorToInt((timer * 100F) % 100F);
            return minutes.ToString ("00") + ":" + seconds.ToString ("00") + ":" + milliseconds.ToString("00");
        }
    }
}
