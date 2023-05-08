using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReelManagement
{
    public class ReelScript : MonoBehaviour
    {
        [SerializeField] AudioClip spinSound;

        struct Item
        {
            public int symbolIndex;
            public GameObject symbol;
            public GameObject card;
        };

        private float cardWidth;
        private float cardHeight;
        private float cardInterval;
        private int maxCard;
        private float maxSpeed;

        public GameObject reelManager;
        private GameObject cardPrefab;
        private List<Item> itemList;
        private float speed;
        private AudioSource audioSource;


        // Start is called before the first frame update
        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.clip = spinSound;
        }

        // Custom Functions
        public void setup()
        {
            cardWidth = reelManager.GetComponent<ReelManagerScript>().getCardWidth();
            cardHeight = reelManager.GetComponent<ReelManagerScript>().getCardHeight();
            cardInterval = reelManager.GetComponent<ReelManagerScript>().getCardInterval();
            maxCard = reelManager.GetComponent<ReelManagerScript>().getMaxCard();
            maxSpeed = reelManager.GetComponent<ReelManagerScript>().getMaxSpeed();

            cardPrefab = reelManager.GetComponent<ReelManagerScript>().cardPrefab;
            itemList = new List<Item>();
            speed = 0;

            StartCoroutine(generateStartItems());
        }

        private IEnumerator generateStartItems()
        {
            int symbolNum = reelManager.GetComponent<ReelManagerScript>().symbolList.Count;
            float cardHeight = reelManager.GetComponent<ReelManagerScript>().getCardHeight();
            float xPos = transform.position.x;
            float yPos = transform.position.y;
            float zPos = transform.position.z;

            for (int i = 0; i < maxCard; i++)
            {
                int r = (int)(Random.Range(0, symbolNum));
                GameObject s = reelManager.GetComponent<ReelManagerScript>().symbolList[r];

                GameObject c = Instantiate(cardPrefab) as GameObject;
                c.transform.position = new Vector3(xPos, yPos + (cardHeight * ((int)(maxCard / 2) - i) * (-1)), zPos);
                c.GetComponent<SpriteRenderer>().sortingLayerName = "ReelCard";
                c.GetComponent<SpriteRenderer>().sortingOrder = 2;
                if (i == 0) c.GetComponent<CardScript>().setPrev(null);
                else c.GetComponent<CardScript>().setPrev(itemList[i - 1].card);
                /*c.GetComponent<SpriteRenderer>().sortingOrder = 3;*/
                c.transform.parent = gameObject.transform;

                GameObject so = Instantiate(s) as GameObject;
                /*so.GetComponent<SpriteRenderer>().sprite = Sprite.Create(s, new Rect(0, 0, s.width, s.height), new Vector2(0.5f, 0.5f));*/
                so.GetComponent<SpriteRenderer>().sortingLayerName = "ReelSymbol";
                so.GetComponent<SpriteRenderer>().sortingOrder = 1;
                so.transform.parent = c.transform;

                Item item = new Item
                {
                    symbolIndex = r,
                    symbol = so,
                    card = c,
                };

                itemList.Add(item);
            }

            yield break;
        }

        private IEnumerator generateItem()
        {
            int symbolNum = reelManager.GetComponent<ReelManagerScript>().symbolList.Count;
            float cardHeight = reelManager.GetComponent<ReelManagerScript>().getCardHeight();
            float xPos = transform.position.x;
            float yPos = transform.position.y;
            float zPos = transform.position.z;

            int i = itemList.Count;
            int r = (int)(Random.Range(0, symbolNum));
            GameObject s = reelManager.GetComponent<ReelManagerScript>().symbolList[r];

            GameObject c = Instantiate(cardPrefab) as GameObject;
            c.transform.position = new Vector3(xPos, yPos + cardHeight * ((int)(maxCard / 2) - i) * (-1), zPos);
            c.GetComponent<SpriteRenderer>().sortingLayerName = "ReelCard";
            if (i == 0) c.GetComponent<CardScript>().setPrev(null);
            else c.GetComponent<CardScript>().setPrev(itemList[i - 1].card);
            /*c.GetComponent<SpriteRenderer>().sortingOrder = 3;*/
            c.transform.parent = gameObject.transform;

            GameObject so = Instantiate(s) as GameObject;
            /*so.GetComponent<SpriteRenderer>().sprite = Sprite.Create(s, new Rect(0, 0, s.width, s.height), new Vector2(0.5f, 0.5f));*/
            so.transform.position = new Vector3(xPos, yPos + cardHeight * ((int)(maxCard / 2) - i) * (-1), zPos);
            so.GetComponent<SpriteRenderer>().sortingLayerName = "ReelSymbol";
            so.transform.parent = c.transform;

            Item item = new Item
            {
                symbolIndex = r,
                symbol = so,
                card = c,
            };

            itemList.Add(item);

            audioSource.Play();

            yield break;
        }

        private IEnumerator rotateItems()
        {
            while (speed < maxSpeed)  
            {
                float accel = 60.0f * Time.deltaTime;
                speed += accel;
                yield return new WaitForEndOfFrame();
            }

            speed = maxSpeed;
            yield break;
        }

        private IEnumerator stopItems(int result)
        {
            float minSpeed = 10.0f;
            float yPos = transform.position.y;
            /*yield return new WaitForSeconds(Random.Range(0.5f, 2.0f));*/

            while (minSpeed < speed)
            {
                float accel = -80.0f * Time.deltaTime;
                speed += accel;
                yield return new WaitForEndOfFrame();
            }
            speed = minSpeed;

            Item item;
            while(true)
            {
                item = itemList[maxCard - 1];
                if(item.symbolIndex == result)
                    break;

                yield return new WaitForEndOfFrame();
            }

            while (yPos + 0.2f < item.card.transform.position.y)
            {
                if (yPos + 1.0f < speed)
                {
                    float accel = -10.0f * Time.deltaTime;
                    speed += accel;
                }
                yield return new WaitForEndOfFrame();
            }

            speed = 0f;

            float distance = item.card.transform.position.y - transform.position.y;
            for (int i = 0; i < maxCard; i++)
            {
                itemList[i].card.transform.position -= new Vector3(0, distance, 0);
            }

            yield break;
        }

        public void rotateReel()
        {
            StartCoroutine(rotateItems());
        }

        public void stopReel(int result)
        {
            StartCoroutine(stopItems(result));
        }

        public void destroyItem()
        {
            if (itemList[0].symbol != null) Destroy(itemList[0].symbol);
            if (itemList[0].card != null) Destroy(itemList[0].card);
            itemList.RemoveAt(0);
            StartCoroutine(generateItem());
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

        public float getSpeed()
        {
            return speed;
        }

        /*public int getRandomSymbolIndex()
        {
            return 
        }*/
    }
}
