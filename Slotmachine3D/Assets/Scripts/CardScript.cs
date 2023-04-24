using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ReelManagement
{
    public class CardScript : MonoBehaviour
    {
        private GameObject prev = null;

        private float cardWidth;
        private float cardHeight;
        private float cardInterval;
        private int maxCard;


        // Start is called before the first frame update
        void Start()
        {
            cardWidth = transform.GetComponentInParent<ReelScript>().getCardWidth();
            cardHeight = transform.GetComponentInParent<ReelScript>().getCardHeight();
            cardInterval = transform.GetComponentInParent<ReelScript>().getCardInterval();
            maxCard = transform.GetComponentInParent<ReelScript>().getMaxCard();
        }

        // Update is called once per frame
        void Update()
        {
            float s = transform.GetComponentInParent<ReelScript>().getSpeed() * Time.deltaTime;
            transform.position -= new Vector3(0, s, 0);

            if(transform.position.y < maxCard * -1)
            {
                transform.GetComponentInParent<ReelScript>().destroyItem();
                Destroy(gameObject);
            }
            if ((prev != null) && 0 < s)
            {
                followPrev();
            }
        }

        // Custom Functions
        public void setPrev(GameObject o)
        {
            prev = o;
        }

        private void followPrev()
        {
            float prevYPos = prev.transform.position.y;
            float aftYPos = transform.position.y;
            if (prevYPos < aftYPos)
            {
                float distance = (aftYPos - cardHeight / 2) - (prevYPos + cardHeight / 2);
                transform.position -= new Vector3(0, distance, 0);
            }
        }
    }
}