using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using HellsChicken.Scripts.Game.Audio;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace HellsChicken.Scripts.Game.UI.Menu
{
    public class IntroMenu : MonoBehaviour
    {
        public List<TextMeshProUGUI> chapters1;
        public List<float> durations1;
        public List<TextMeshProUGUI> chapters2;
        public List<float> durations2;
        public float voiceOverDelay = 0.5f;
        
        
        public float dissolvingSpeed = 1;
        
        public Animator transition;
        public float transitionTime = 1f;

        private bool _loadNextChapter;
        private bool _loadingNextChapter;
        private int _nextChapterToBeLoaded;
        private bool _clearScreen;
        private bool _clearingScreen;

        private float _lerpTime;

        private List<TextMeshProUGUI> _currentChapters;
        private List<float> _currentDurations;
        private string _currentChaptersName;

        private int _voiceOverCounter;
        
        // Start is called before the first frame update
        private void Start()
        {
            _currentChapters = chapters1;
            _currentDurations = durations1;
            _currentChaptersName = "chapters1";
            
            var transparent = new Color(1, 1, 1, 0);
            foreach (var t in chapters1)
            {
                t.color = transparent;
            }
            foreach (var t in chapters2)
            {
                t.color = transparent;
            }
            var i = 0;
            var j = 0;
            for (i = 0; i < durations1.Count; i++)
            {
                durations1[i] = IntroAudioManager.Instance.GetVoiceOverDuration(i);
            }
            for (j = 0; j < durations2.Count; j++)
            {
                durations2[j] = IntroAudioManager.Instance.GetVoiceOverDuration(j + i);
            }

            _voiceOverCounter = 0;
            _nextChapterToBeLoaded = 0;
            _loadNextChapter = false;
            _loadingNextChapter = false;
            _clearScreen = false;
            _clearingScreen = false;
            StartCoroutine(TimerBetweenChapters(1.75f));
            EventManager.TriggerEvent("startIntroSoundtrack");
        }

        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                EventManager.TriggerEvent("stopVoiceOver");
                EventManager.TriggerEvent("stopIntroSoundtrack"); //stop the music and destroy the audio manager
                StartCoroutine(LoadSceneWithFading("Level_1"));
            }
            
            if (_loadNextChapter)
            {
                _loadNextChapter = false;
                StartCoroutine(DelayedStartVoiceOver(voiceOverDelay));
                _loadingNextChapter = true;
                _lerpTime = 0f;
            }
            if (_loadingNextChapter)
            {
                var transparency = Mathf.Lerp(0, 1, _lerpTime);
                var color = new Color(1,1,1,transparency);
                _currentChapters[_nextChapterToBeLoaded].color = color;
                _lerpTime += 0.01f * dissolvingSpeed;
                if (_lerpTime > 1f)
                {
                    _lerpTime = 0f;
                    _loadingNextChapter = false;
                    _nextChapterToBeLoaded++;
                    StartCoroutine(TimerBetweenChapters(_currentDurations[_nextChapterToBeLoaded-1]));
                }
            }
            if (_clearScreen)
            {
                _loadNextChapter = false; //useless
                _loadingNextChapter = false; //useless
                _clearScreen = false;
                _clearingScreen = true;
            }
            if (_clearingScreen)
            {
                var transparency = Mathf.Lerp(1, 0, _lerpTime);
                var color = new Color(1,1,1,transparency);
                foreach (var t in _currentChapters)
                {
                    t.color = color;
                }
                _lerpTime += 0.01f * dissolvingSpeed;
                if (_lerpTime > 1f)
                {
                    _lerpTime = 0f;
                    _clearingScreen = false;
                    _nextChapterToBeLoaded = 0;
                    _currentChapters = chapters2;
                    _currentDurations = durations2;
                    _currentChaptersName = "chapters2";
                    StartCoroutine(TimerBetweenChapters(1));
                }
            }
        }

        private IEnumerator TimerBetweenChapters(float time)
        {
            yield return new WaitForSeconds(time);
            if (_nextChapterToBeLoaded == _currentChapters.Count && _currentChaptersName == "chapters1")
            {
                _clearScreen = true;
            }
            else if (_nextChapterToBeLoaded == _currentChapters.Count && _currentChaptersName == "chapters2")
            {
                EventManager.TriggerEvent("stopIntroSoundtrack");
                StartCoroutine(LoadSceneWithFading("Level_1"));
            }
            else
            {
                _loadNextChapter = true;
            }
        }

        private IEnumerator DelayedStartVoiceOver(float time)
        {
            yield return new WaitForSeconds(time);
            EventManager.TriggerEvent("startVoiceOver",_voiceOverCounter);
            _voiceOverCounter++;
        }
        
        private IEnumerator LoadSceneWithFading(String sceneName)
        {
            transition.SetTrigger("Start");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(sceneName);
        }
    }
}
