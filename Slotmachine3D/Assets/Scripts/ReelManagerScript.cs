using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EventManagement;

using static EventType.EventType;

namespace ReelManagement
{
    public class ReelManagerScript : MonoBehaviour
    {
        [SerializeField] public List<GameObject> symbolList;
        [SerializeField] public GameObject eventManager;
        [SerializeField] public GameObject reelPrefab;
        [SerializeField] public GameObject reelFramePrefab;
        [SerializeField] public GameObject reelContainer;
        [SerializeField] public GameObject cardPrefab;
        [SerializeField] public int reelNum;

        struct Reel
        {
            public GameObject reel;
            public GameObject reelFrame;
        };

        // readonly data
        private float cardWidth;                
        private float cardHeight;               
        private float cardInterval;   
        
        private int maxCard = 5;
        private float maxSpeed = 30.0f;
        private const int minReel = 2, maxReel = 5;

        private List<Reel> reelList;
        private List<int> resultList;
        private bool isActive, isBegin;

        // Start is called before the first frame update
        void Start()
        {
            cardWidth = cardPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;     // 3f
            cardHeight = cardPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y;    // 2f
            cardInterval = cardWidth / 10.0f;                                               // 0.3f

            reelList = new List<Reel>();
            resultList = new List<int>();
            isActive = false;
            isBegin = false;

            StartCoroutine(refreshSlotmachine());
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (isActive) 
                {
                    setActive(false);
                    if (!isBegin)
                    {
                        StartCoroutine(beginRotation());
                    }
                    else
                    {
                        StartCoroutine(stopRotation());
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            { 
                if(!isBegin && isActive)
                {
                    if (minReel < reelNum)
                    {
                        setActive(false);
                        reelNum--;
                        StartCoroutine(refreshSlotmachine());
                    }
                }
            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            { 
                if(!isBegin && isActive)
                {
                    if (reelNum < maxReel)
                    {
                        setActive(false);
                        reelNum++;
                        StartCoroutine(refreshSlotmachine());
                    }
                }
            }
        }

        // Custom Functions
        private IEnumerator refreshSlotmachine()
        {

            eventManager.GetComponent<EventManagerScript>().fadeIn();
            yield return new WaitForSeconds(1.0f);

            for (int i = 0; i < reelList.Count; i++)
            {
                Destroy(reelList[i].reelFrame);
                Destroy(reelList[i].reel);
            }
            reelList.Clear();
            StartCoroutine(generateReels());
            yield return new WaitForSeconds(0.5f);

            eventManager.GetComponent<EventManagerScript>().fadeOut();
            setActive(true);

            yield break;
        }

        private IEnumerator generateReels()
        {
            float cardArea = (cardInterval * 2) + (cardWidth);
            float beginXPos = reelNum % 2 == 0 ? (int)(reelNum / 2) * cardArea - (cardArea / 2) : (reelNum / 2) * cardArea;
            beginXPos *= -1;

            for (int i = 0; i < reelNum; i++)
            {
                float xPos = beginXPos + (cardArea * i);

                GameObject reel = Instantiate(reelPrefab) as GameObject;
                reel.transform.SetParent(reelContainer.transform);
                reel.transform.position = new Vector3(xPos, 0, 0);
                /*reel.tag = "Reel";*/
                reel.GetComponent<ReelScript>().reelManager = gameObject;

                GameObject frame = Instantiate(reelFramePrefab) as GameObject;
                frame.transform.position = reel.transform.position;
                frame.transform.parent = reel.transform;

                Reel newReel = new Reel
                {
                    reelFrame = frame,
                    reel = reel,
                };
                reelList.Add(newReel);
            }

            setActive(true);
            yield break;
        }

        private IEnumerator beginRotation()
        {
            for (int i = 0; i < reelNum; i++)
            {
                reelList[i].reel.GetComponent<ReelScript>().rotateReel();
            }
            isBegin = true;
            while (true)
            {
                if (maxSpeed <= reelList[reelNum - 1].reel.GetComponent<ReelScript>().getSpeed())
                {
                    setActive(true);
                    break;
                }
                yield return new WaitForEndOfFrame();
            }
            yield break;
        }

        private IEnumerator stopRotation()
        {
            initResultList();
            for (int i = 0; i < reelNum; i++)
            {
                reelList[i].reel.GetComponent<ReelScript>().stopReel(resultList[i]);
            }

            while(true)
            {
                int i = 0;
                for (; i < reelNum; i++) 
                {
                    if (reelList[i].reel.GetComponent<ReelScript>().getSpeed() != 0)
                        break;
                }
                if (i == reelNum)
                    break;
                yield return new WaitForEndOfFrame();
            }
            yield return new WaitForSeconds(0.5f);

            StartCoroutine(runEvent());

            isBegin = false;
            yield break;
        }

        private IEnumerator runEvent()
        {
            int i = 1;
            for(; i<resultList.Count; i++)
            {
                if (resultList[i - 1] != resultList[i])
                    break;
            }

            if(i == resultList.Count)
                eventManager.GetComponent<EventManagerScript>().runEvent(SlotmachineEvent.Win);
            else
                eventManager.GetComponent<EventManagerScript>().runEvent(SlotmachineEvent.Lose);

            yield return new WaitForSeconds(1.0f);

            setActive(true);
            clearResultList();
            yield break;
        }

        public float getCardWidth()
        {
            return cardWidth;
        }

        public float getCardHeight()
        {
            return cardHeight;
        }

        public float getCardInterval()
        {
            return cardInterval;
        }

        public int getMaxCard()
        {
            return maxCard;
        }

        public float getMaxSpeed()
        {
            return maxSpeed;
        }

        public void setActive(bool b)
        {
            isActive = b;
        }

        public void initResultList()
        {
            for (int i = 0; i < reelNum; i++)
            {
                int r = getResult();
                resultList.Add(r);
            }
        }

        public void clearResultList()
        {
            resultList.Clear();
        }

        private int getResult()
        {
            float r = Random.Range(0, 100);

            if (r <= 25)
                return 0;
            else if (r <= 50)
                return 1;
            else if (r <= 75)
                return 2;
            else
                return 3;
        }
    }
}
