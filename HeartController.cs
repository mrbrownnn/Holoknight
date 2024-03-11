using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartController : MonoBehaviour
{
    PlayerControler player;
    public GameObject[] HeartContainers;
    public Image[] heartFills;
    public Transform heartsParent;
    public GameObject heartContainersPrefab;
    void Start()
    {
        player = PlayerControler.Instance;
        HeartContainers = new GameObject[PlayerControler.Instance.maxHealth];
        heartFills = new Image[PlayerControler.Instance.maxHealth];

        InstantiateHeartContainers();
        UpdateHeart();

        PlayerControler.Instance.onHealthChangedCallback += UpdateHeart;
    }

    // Update is called once per frame
    void Update()
    {

    }
    void SetHeartContainers()
    {
        for(int i =0; i < HeartContainers.Length; i++)
        {
            if(i< PlayerControler.Instance.maxHealth)
            {
                HeartContainers[i].SetActive(false);
            }
            else
            {
                HeartContainers[i].SetActive(false);
            }
        }
    }
    void SetFillsHeart()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < PlayerControler.Instance.maxHealth)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }
    }
    void InstantiateHeartContainers ()
    {
        for(int i = 0;i < PlayerControler.Instance.maxHealth; i++)
        {
            GameObject temp = Instantiate(heartContainersPrefab);
            temp.transform.SetParent(heartsParent, false);
            HeartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
    void UpdateHeart()
    {
        SetHeartContainers();

        SetFillsHeart();
    }
}
