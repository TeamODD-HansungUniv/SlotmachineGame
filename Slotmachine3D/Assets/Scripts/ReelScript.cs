using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReelManagement
{
    public class ReelScript : MonoBehaviour
    {
        struct Item
        {
            public int symbolIndex;
            public GameObject symbol;
            public GameObject card;
        };

        private GameObject cardPrefab;
        private List<Item> itemList;
        private float speed;


        // Start is called before the first frame update
        void Start()
        {
            cardPrefab = transform.GetComponentInParent<ReelManagerScript>().cardPrefab;
            itemList = new List<Item>();
            speed = 0;

            /*StartCoroutine(generateItems());*/
        }

        // Update is called once per frame
        void Update()
        {

        }

        // Custom Functions
        /*private IEnumerator generateItems()
        {

        }*/

        public float getSpeed()
        {
            return speed;
        }
    }
}
