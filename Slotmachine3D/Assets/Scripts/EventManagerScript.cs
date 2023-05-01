using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ReelManagement;
using static EventType.EventType;

namespace EventManagement
{ 
    public class EventManagerScript : MonoBehaviour
    {
        struct Item
        {
            public int symbolIndex;
            public GameObject symbol;
            public GameObject card;
        };

        [SerializeField] GameObject reelManager;
        [SerializeField] GameObject resultCanvas;
        [SerializeField] GameObject resultCardPrefab;
        [SerializeField] GameObject settingCanvas;
        [SerializeField] GameObject fadeNormal;
        [SerializeField] GameObject fadeWin;
        [SerializeField] GameObject fadeLose;


        private float cardWidth;
        private float cardHeight;
        private float cardInterval;
        private int maxCard;
        private float maxSpeed;

        private List<Item> itemList;

        // Start is called before the first frame update
        void Start()
        {
            cardWidth = reelManager.GetComponent<ReelManagerScript>().getCardWidth();
            cardHeight = reelManager.GetComponent<ReelManagerScript>().getCardHeight();
            cardInterval = reelManager.GetComponent<ReelManagerScript>().getCardInterval();
            maxCard = reelManager.GetComponent<ReelManagerScript>().getMaxCard();
            maxSpeed = reelManager.GetComponent<ReelManagerScript>().getMaxSpeed();

            itemList = new List<Item>();
        }

        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                if(!settingCanvas.activeSelf)
                    settingCanvas.SetActive(true);
            }
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

            reelManager.GetComponent<ReelManagerScript>().setActive(true);
            reelManager.GetComponent<ReelManagerScript>().clearResultList();
            yield break;
        }

        private IEnumerator loseEvent()
        {

            float cardArea = (cardInterval * 2) + (cardWidth * 1.3f);
            int reelNum = reelManager.GetComponent<ReelManagerScript>().reelNum;
            Color c = fadeNormal.GetComponent<Image>().color;
            c.a = 0;

            while (c.a <= 0.5f)
            {
                c.a += 0.2f;
                fadeWin.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }


            float beginXPos = reelNum % 2 == 0 ? (int)(reelNum / 2) * cardArea - (cardArea / 2) : (reelNum / 2) * cardArea;
            beginXPos *= -1;
            for (int i = 0; i < reelNum; i++)
            {
                float xPos = beginXPos + (cardArea * i);
                int symbolIndex = reelManager.GetComponent<ReelManagerScript>().resultList[i];

                GameObject s = reelManager.GetComponent<ReelManagerScript>().symbolList[symbolIndex];

                GameObject card = Instantiate(resultCardPrefab) as GameObject;
                card.transform.SetParent(resultCanvas.transform);
                card.transform.position = new Vector3(xPos, 0, 0);
                card.transform.localScale = new Vector3(13f, 13f, 1f);
                card.GetComponent<SpriteRenderer>().sortingLayerName = "Result";
                card.GetComponent<SpriteRenderer>().sortingOrder = 0;

                GameObject so = Instantiate(s) as GameObject;
                so.transform.SetParent(card.transform);
                so.transform.localScale = new Vector3(1.4f, 1.4f, 1f);
                so.GetComponent<SpriteRenderer>().sortingLayerName = "Result";
                so.GetComponent<SpriteRenderer>().sortingOrder = 1;
                so.transform.position = so.transform.parent.position;

                Item item = new Item
                {
                    symbolIndex = symbolIndex,
                    symbol = so,
                    card = card,
                };

                itemList.Add(item);
            }

            while (true) 
            {
                if(Input.GetKeyDown(KeyCode.Return))
                {
                    break;
                }
                yield return new WaitForEndOfFrame();
            }

            /*while (0 <= c.a)
            {
                c.a -= 0.2f;
                fadeWin.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }

            c.a = 0;
            fadeWin.GetComponent<Image>().color = c;*/

            for(int i = 0; i < itemList.Count; i++)
            {
                Destroy(itemList[i].symbol);
                Destroy(itemList[i].card);
            }
            itemList.Clear();
            reelManager.GetComponent<ReelManagerScript>().setActive(true);
            reelManager.GetComponent<ReelManagerScript>().clearResultList();
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