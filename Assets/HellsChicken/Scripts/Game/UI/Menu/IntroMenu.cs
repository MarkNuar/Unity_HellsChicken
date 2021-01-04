using System;
using System.Collections;
using System.Collections.Generic;
using EventManagerNamespace;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

namespace HellsChicken.Scripts.Game.UI.Menu
{
    public class IntroMenu : MonoBehaviour
    {
        public List<TextMeshProUGUI > chapters;
        public float timeBetweenChapters = 2;
        public float dissolvingSpeed = 1;
        public Animator transition;
        public float transitionTime = 1f;

        private bool _loadNextChapter;
        private bool _loadingNextChapter;
        private int _nextChapterToBeLoaded;

        private float _lerpTime;
        // Start is called before the first frame update
        private void Start()
        {
            //var opaque = new Color(1, 1, 1, 1);
            var transparent = new Color(1, 1, 1, 0);
            foreach (var t in chapters)
            {
                t.color = transparent;
            }
            _nextChapterToBeLoaded = 0;
            _loadNextChapter = false;
            _loadingNextChapter = false;
            StartCoroutine(TimerBetweenChapters(1));
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                StartCoroutine(LoadSceneWithFading("Level_1"));
            }
            if (_loadNextChapter)
            {
                _loadNextChapter = false;
                _loadingNextChapter = true;
                _lerpTime = 0f;
            }

            if (_loadingNextChapter)
            {
                var transparency = Mathf.Lerp(0, 1, _lerpTime);
                var color = new Color(1,1,1,transparency);
                chapters[_nextChapterToBeLoaded].color = color;
                _lerpTime += 0.01f * dissolvingSpeed;
                if (_lerpTime > 1f)
                {
                    _lerpTime = 0f;
                    _loadingNextChapter = false;
                    _nextChapterToBeLoaded++;
                    StartCoroutine(TimerBetweenChapters(timeBetweenChapters));
                }
            }
        }

        private IEnumerator TimerBetweenChapters(float time)
        {
            yield return new WaitForSeconds(time);
            if (_nextChapterToBeLoaded < chapters.Count)
            {
                _loadNextChapter = true;
            }
        }
        
        private IEnumerator LoadSceneWithFading(String sceneName)
        {
            transition.SetTrigger("Start");
            EventManager.TriggerEvent("fadeOutMusic");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadScene(sceneName);
        }
    }
}
