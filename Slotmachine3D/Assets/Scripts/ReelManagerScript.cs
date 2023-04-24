using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReelManagement
{
    public class ReelManagerScript : MonoBehaviour
    {
        [SerializeField] public List<GameObject> symbolList;
        [SerializeField] public GameObject reelPrefab;
        [SerializeField] public GameObject reelContainer;
        [SerializeField] public GameObject cardPrefab;
        [SerializeField] public int reelNum;

        // readonly data
        private float cardWidth;                
        private float cardHeight;               
        private float cardInterval;              
        private const int minReel = 2, maxReel = 5;

        private List<GameObject> reelList;
        private List<int> resultList;
        private bool isActive, isBegin;

        // Start is called before the first frame update
        void Start()
        {
            cardWidth = cardPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x;     // 3f
            cardHeight = cardPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y;    // 2f
            cardInterval = cardWidth / 10.0f;                                                                 // 0.3f

            reelList = new List<GameObject>();
            resultList = new List<int>();
            isActive = false;
            isBegin = false;

            StartCoroutine(generateReels());
        }

        void Update()
        {
            if (isActive)
            {
                if (Input.GetKeyDown(KeyCode.Space)) 
                {
                    if (!isBegin)
                    {
                        clearResultList();
                        for (int i = 0; i < reelNum; i++)
                        {
                            reelList[i].GetComponent<ReelScript>().rotateReel();
                        }
                        isBegin = true;
                        setActive(false);
                    }
                    else
                    {
                        initResultList();
                        for (int i = 0; i < reelNum; i++)
                        {
                            reelList[i].GetComponent<ReelScript>().stopReel(resultList[i]);
                        }
                    }
                }
            }
        }

        // Custom Functions
        private IEnumerator generateReels()
        {
            yield return new WaitForSeconds(0.3f);

            float cardArea = (cardInterval * 2) + (cardWidth);
            float beginXPos = reelNum % 2 == 0 ? (int)(reelNum / 2) * cardArea + (cardArea / 2) : (reelNum / 2) * cardArea;
            beginXPos *= -1;

            for (int i = 0; i < reelNum; i++)
            {
                float xPos = beginXPos + (cardArea * i);

                GameObject reel = Instantiate(reelPrefab) as GameObject;
                reel.transform.SetParent(reelContainer.transform);
                reel.transform.position = new Vector3(xPos, 0, 0);
                reel.tag = "Reel";
                reel.GetComponent<ReelScript>().reelManager = gameObject;
                reelList.Add(reel);
            }

            setActive(true);
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

        public void setActive(bool b)
        {
            isActive = b;
        }

        public void initResultList()
        {
            resultList = new List<int>(new int[reelNum]);
            for (int i = 0; i < reelNum; i++)
            {
                int r = (int)(Random.Range(0, symbolList.Count - 1));
                resultList.Add(r);
            }
        }

        public void clearResultList()
        {
            resultList.Clear();
        }
    }
}
