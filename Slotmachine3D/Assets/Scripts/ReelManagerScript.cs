using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ReelManagement
{
    public class ReelManagerScript : MonoBehaviour
    {
        [SerializeField] public List<Texture2D> symbolList;
        [SerializeField] public GameObject reelPrefab;
        [SerializeField] public GameObject cardPrefab;
        [SerializeField] public int reelNum;

        private const float cardWidth = 3f;
        private const float cardHeight = 2f;
        private const float cardInterval = 0.3f;
        private const int minReel = 2, maxReel = 5;

        private List<GameObject> reelList;
        private List<int> resultList;

        // Start is called before the first frame update
        void Start()
        {
            reelList = new List<GameObject>();
            resultList = new List<int>();

            StartCoroutine(generateReels());
        }

        void Update()
        {
            
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
                reel.transform.SetParent(gameObject.transform);
                reel.transform.position = new Vector3(xPos, 0, 0);
                reelList.Add(reel);
            }
        }
    }
}
