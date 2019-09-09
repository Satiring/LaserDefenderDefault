using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeDisplay : MonoBehaviour
{
    private Text lifeText;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        lifeText = GetComponent<Text>();
        player = FindObjectOfType<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        lifeText.text = player.getHealth().ToString();
    }
}
