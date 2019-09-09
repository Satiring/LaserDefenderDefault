using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplayImage : MonoBehaviour
{
    private Image imageLife;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        imageLife = GetComponent<Image>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        imageLife.fillAmount = player.getHealthPercent();
    }
}
