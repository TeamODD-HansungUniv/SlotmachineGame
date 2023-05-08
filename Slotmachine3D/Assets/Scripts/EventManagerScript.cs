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
            public GameObject frame;
        };

        [SerializeField] GameObject reelManager;
        [SerializeField] GameObject resultCanvas;
        [SerializeField] GameObject continueText;
        [SerializeField] GameObject resultCardPrefab;
        [SerializeField] GameObject frameNormalPrefab;
        [SerializeField] GameObject frameBombPrefab;
        [SerializeField] GameObject settingCanvas;
        [SerializeField] GameObject gameInfoCanvas;
        [SerializeField] GameObject fadeNormal;
        [SerializeField] GameObject fadeWin;
        [SerializeField] GameObject fadeLose;
        [SerializeField] GameObject fadeBomb;

        [SerializeField] AudioClip beginSound;
        [SerializeField] AudioClip winSound;
        [SerializeField] AudioClip normalSound;
        [SerializeField] AudioClip bombSound;
        [SerializeField] AudioClip resultSound;


        private float cardWidth;
        private float cardHeight;
        private float cardInterval;
        private int maxCard;
        private int reelNum;
        private float maxSpeed;
        private AudioSource audioSource;

        private List<Item> itemList;

        // Start is called before the first frame update
        void Start()
        {
            cardWidth = reelManager.GetComponent<ReelManagerScript>().getCardWidth();
            cardHeight = reelManager.GetComponent<ReelManagerScript>().getCardHeight();
            cardInterval = reelManager.GetComponent<ReelManagerScript>().getCardInterval();
            maxCard = reelManager.GetComponent<ReelManagerScript>().getMaxCard();
            maxSpeed = reelManager.GetComponent<ReelManagerScript>().getMaxSpeed();
            reelNum = reelManager.GetComponent<ReelManagerScript>().reelNum;

            itemList = new List<Item>();
            audioSource = GetComponent<AudioSource>();
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
                    /*StartCoroutine(loseEvent());*/
                    StartCoroutine(normalEvent());
                    break;
                case SlotmachineEvent.Normal:
                    StartCoroutine(normalEvent());
                    break;
                case SlotmachineEvent.Bomb:
                StartCoroutine(bombEvent());
                break;
            }
        }

        private IEnumerator winEvent()
        {
            ReelManagerScript reelmgr = reelManager.GetComponent<ReelManagerScript>();
            Image img = fadeWin.GetComponent<Image>();

            float cardArea = (cardInterval * 2) + (cardWidth * 1.3f);
            reelNum = reelmgr.reelNum;
            Color c = img.color;
            c.a = 0;

            while (c.a <= 0.3f)
            {
                c.a += 0.2f;
                img.color = c;
                yield return new WaitForSeconds(0.05f);
            }

            StartCoroutine(showResultCards(reelNum, cardArea, 0.5f));
            yield return new WaitForSeconds(3f);

            audioSource.clip = winSound;
            audioSource.Play();
            resultCanvas.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            resultCanvas.SetActive(true);
            yield return new WaitForSeconds(1f);
            resultCanvas.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            resultCanvas.SetActive(true);
            yield return new WaitForSeconds(1f);

            yield return new WaitForSeconds(1f);
            continueText.SetActive(true);
            while (!Input.GetKey(KeyCode.Return))
            {
                yield return new WaitForEndOfFrame();
            }

            continueText.SetActive(false);
            clearResultCards();

            while (0 <= c.a)
            {
                c.a -= 0.2f;
                img.color = c;
                yield return new WaitForSeconds(0.05f);
            }
            c.a = 0;
            reelmgr.setActive(true);
            reelmgr.clearResultList();
            yield break;
        }

        private IEnumerator loseEvent()
        {

            float cardArea = (cardInterval * 2) + (cardWidth * 1.3f);
            reelNum = reelManager.GetComponent<ReelManagerScript>().reelNum;
            Color c = fadeNormal.GetComponent<Image>().color;
            c.a = 0;

            while (c.a <= 0.5f)
            {
                c.a += 0.2f;
                fadeWin.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }
            showResultCards(reelNum, cardArea);

            while (!Input.GetKey(KeyCode.Return)) 
            {
                yield return new WaitForEndOfFrame();
            }

            clearResultCards();

            while (0 <= c.a)
            {
                c.a -= 0.2f;
                fadeWin.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }
            c.a = 0;
            reelManager.GetComponent<ReelManagerScript>().setActive(true);
            reelManager.GetComponent<ReelManagerScript>().clearResultList();
            yield break;
        }

        private IEnumerator normalEvent()
        {
            ReelManagerScript reelmgr = reelManager.GetComponent<ReelManagerScript>();
            Image img = fadeNormal.GetComponent<Image>();

            float cardArea = (cardInterval * 2) + (cardWidth * 1.3f);
            reelNum = reelmgr.reelNum;
            Color c = img.color;
            c.a = 0;

            while (c.a <= 0.3f)
            {
                c.a += 0.2f;
                img.color = c;
                yield return new WaitForSeconds(0.05f);
            }

            StartCoroutine(showResultCards(reelNum, cardArea, 0.5f));
            yield return new WaitForSeconds(3f);

            audioSource.clip = resultSound;
            audioSource.Play();
            resultCanvas.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            resultCanvas.SetActive(true);
            yield return new WaitForSeconds(1f);
            resultCanvas.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            resultCanvas.SetActive(true);
            yield return new WaitForSeconds(1f);

            yield return new WaitForSeconds(1f);
            continueText.SetActive(true);
            while (!Input.GetKey(KeyCode.Return)) 
            {
                yield return new WaitForEndOfFrame();
            }

            continueText.SetActive(false);
            clearResultCards();

            while (0 <= c.a)
            {
                c.a -= 0.2f;
                img.color = c;
                yield return new WaitForSeconds(0.05f);
            }
            c.a = 0;
            reelmgr.setActive(true);
            reelmgr.clearResultList();
            yield break;
        }

        private IEnumerator bombEvent()
        {
            ReelManagerScript reelmgr = reelManager.GetComponent<ReelManagerScript>();
            Image img = fadeBomb.GetComponent<Image>();

            float cardArea = (cardInterval * 2) + (cardWidth * 1.3f);
            reelNum = reelmgr.reelNum;
            Color c = img.color;
            c.a = 0;

            while (c.a <= 0.3f)
            {
                c.a += 0.2f;
                img.color = c;
                yield return new WaitForSeconds(0.05f);
            }

            StartCoroutine(showResultCards(reelNum, cardArea, 0.5f));
            yield return new WaitForSeconds(3f);

            /*audioSource.clip = resultSound;
            audioSource.Play();
            resultCanvas.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            resultCanvas.SetActive(true);
            yield return new WaitForSeconds(1f);
            resultCanvas.SetActive(false);
            yield return new WaitForSeconds(0.2f);
            resultCanvas.SetActive(true);
            yield return new WaitForSeconds(1f);*/

            yield return new WaitForSeconds(1f);
            continueText.SetActive(true);
            while (!Input.GetKey(KeyCode.Return))
            {
                yield return new WaitForEndOfFrame();
            }

            continueText.SetActive(false);
            clearResultCards();

            while (0 <= c.a)
            {
                c.a -= 0.2f;
                img.color = c;
                yield return new WaitForSeconds(0.05f);
            }
            c.a = 0;
            reelmgr.setActive(true);
            reelmgr.clearResultList();
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
            gameInfoCanvas.SetActive(false);
            yield break;
        }

        private IEnumerator runFadeOut()
        {
            Color c = fadeNormal.GetComponent<Image>().color;
            c.a = 1.0f;
            gameInfoCanvas.SetActive(true);
            while (0 <= c.a)
            {
                c.a -= 0.2f;
                fadeNormal.GetComponent<Image>().color = c;
                yield return new WaitForSeconds(0.05f);
            }

            c.a = 0;
            fadeNormal.GetComponent<Image>().color = c;

            audioSource.clip = beginSound;
            audioSource.Play();
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

        public void showResultCards(int reelNum, float cardArea)
        {

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
        }

        public IEnumerator showResultCards(int reelNum, float cardArea, float delay)
        {

            float beginXPos = reelNum % 2 == 0 ? (int)(reelNum / 2) * cardArea - (cardArea / 2) : (reelNum / 2) * cardArea;
            beginXPos *= -1;
            for (int i = 0; i < reelNum; i++)
            {
                ReelManagerScript script = reelManager.GetComponent<ReelManagerScript>();
                float xPos = beginXPos + (cardArea * i);
                int symbolIndex = script.resultList[i];

                GameObject s = script.symbolList[symbolIndex];

                GameObject card = Instantiate(resultCardPrefab) as GameObject;
                card.transform.SetParent(resultCanvas.transform);
                card.transform.position = new Vector3(xPos, 0, 0);
                card.transform.localScale = new Vector3(13f, 13f, 1f);
                card.GetComponent<SpriteRenderer>().sortingLayerName = "Result";
                card.GetComponent<SpriteRenderer>().sortingOrder = 0;

                GameObject so = Instantiate(s) as GameObject;
                so.transform.SetParent(card.transform);
                so.transform.localScale = new Vector3(1.2f, 1.2f, 1f);
                so.GetComponent<SpriteRenderer>().sortingLayerName = "Result";
                so.GetComponent<SpriteRenderer>().sortingOrder = 1;
                so.transform.position = so.transform.parent.position;

                GameObject f;
                if(symbolIndex == script.symbolList.Count - 1)
                    f = Instantiate(frameBombPrefab) as GameObject;
                else 
                    f = Instantiate(frameNormalPrefab) as GameObject;
                f.transform.SetParent(card.transform);
                f.transform.localScale = new Vector3(1.05f, 1.05f, 1f);
                f.GetComponent<SpriteRenderer>().sortingLayerName = "Result";
                f.GetComponent<SpriteRenderer>().sortingOrder = 2;
                f.transform.position = so.transform.parent.position;

                Item item = new Item
                {
                    symbolIndex = symbolIndex,
                    symbol = so,
                    card = card,
                    frame = f,
                };
                itemList.Add(item);
                if (symbolIndex == script.symbolList.Count - 1)
                    audioSource.clip = bombSound;
                else
                    audioSource.clip = normalSound;
                audioSource.Play();
                yield return new WaitForSeconds(delay);
            }
            yield break;
        }



        public void clearResultCards()
        {
            for (int i = 0; i < itemList.Count; i++)
            {
                Destroy(itemList[i].symbol);
                Destroy(itemList[i].card);
                Destroy(itemList[i].frame);
            }
            itemList.Clear();
        }
    }
}