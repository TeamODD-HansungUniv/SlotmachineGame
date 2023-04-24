using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using static EventType.EventType;

namespace EventManagement
{ 
    public class EventManagerScript : MonoBehaviour
    {
        [SerializeField] GameObject fadeNormal;
        [SerializeField] GameObject fadeWin;
        [SerializeField] GameObject fadeLose;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Custom Functions
        public void runEvent(SlotmachineEvent e)
        {
            switch(e)
            {
            case SlotmachineEvent.Win:
                StartCoroutine(winEvent());
                break;
            case SlotmachineEvent.Lose:
                StartCoroutine(loseEvent());
                break;
            }
        }

        private IEnumerator winEvent()
        {
            Color c = fadeWin.GetComponent<Image>().color;
            c.a = 0;

            while (c.a <= 0.5f)
            {
                c.a += 0.2f;
                fadeWin.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }

            while (0 <= c.a)
            {
                c.a -= 0.2f;
                fadeWin.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }

            c.a = 0;
            fadeWin.GetComponent<Image>().color = c;
            yield break;
        }

        private IEnumerator loseEvent()
        {
            Color c = fadeLose.GetComponent<Image>().color;
            c.a = 0;

            while (c.a <= 0.5f)
            {
                c.a += 0.2f;
                fadeLose.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }

            while (0 <= c.a)
            {
                c.a -= 0.2f;
                fadeLose.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }

            c.a = 0;
            fadeLose.GetComponent<Image>().color = c;
            yield break;
        }

        private IEnumerator runFadeIn()
        {
            Color c = fadeNormal.GetComponent<Image>().color;
            c.a = 0;

            while (c.a <= 1.0f)
            {
                c.a += 0.2f;
                fadeNormal.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }
            yield break;
        }

        private IEnumerator runFadeOut()
        {
            Color c = fadeNormal.GetComponent<Image>().color;
            c.a = 1.0f;

            while (0 <= c.a)
            {
                c.a -= 0.2f;
                fadeNormal.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }

            c.a = 0;
            fadeNormal.GetComponent<Image>().color = c;
            yield break;
        }

        public void fadeIn()
        {
            StartCoroutine(runFadeIn());
        }

        public void fadeOut()
        {
            StartCoroutine(runFadeOut());
        }
    }
}