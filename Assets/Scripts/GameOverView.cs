using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI text = null;

    public void GameOver()
    {
        gameObject.SetActive(true);

        if (text != null)
        {
            if (Playground.FirstPlayer)
                text.text = "Win first player!!!";
            else
                text.text = "Win second player!!!";
        }
    }
}
