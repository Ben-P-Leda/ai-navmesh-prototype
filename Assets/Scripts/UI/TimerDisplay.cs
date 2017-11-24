using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TimerDisplay : MonoBehaviour
    {
        private Text _timerText;

        private void Start()
        {
            _timerText = GetComponent<Text>();
        }

        private void Update()
        {
            string time = (Mathf.Round(Time.timeSinceLevelLoad * 100.0f) / 100.0f).ToString() + ".00";
            string[] segments = time.Split('.');

            if (segments[0].Length < 2)
            {
                segments[0] = "0" + segments[0];
            }

            if (segments[1].Length < 2)
            {
                segments[1] = segments[1] + "0";
            }
            _timerText.text = segments[0] + ":" + segments[1];
        }
    }
}