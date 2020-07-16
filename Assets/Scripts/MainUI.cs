using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum TypeGame
{
    Null,
    OneStep,
    Diagonal,
    Axis
}

public class MainUI : MonoBehaviour
{
    #region  Events

    public delegate bool GameEvent(int size, Vector2Int house, TypeGame type, bool animationOn, float speed_animation);
    public static event GameEvent StartEvent;

    #endregion

    #region Public Variables

    public TextMeshProUGUI Text_sizeBattlefield = null;
    public TextMeshProUGUI Text_sizeHouse = null;

    #endregion

    #region Private Variables

    private int sizeBattlefield = 8;
    private Vector2Int sizeHouse = new Vector2Int(3, 3);
    private TypeGame typeGame = TypeGame.Null;
    private bool animation_on = true;
    private float speed_animation = 1;

    #endregion

    public void ShowMenu()
    {
        gameObject.SetActive(true);
    }

    public void StartGame()
    {
        if (StartEvent != null && typeGame != TypeGame.Null)
        {
            if (StartEvent.Invoke(sizeBattlefield, sizeHouse, typeGame, animation_on, speed_animation))
                gameObject.SetActive(false);
        }
    }

    public void ChangeSize(float size)
    {
        if (Text_sizeBattlefield == null)
        {
            Debug.LogError("Text_sizeBattlefield is null");
            return;
        }

        sizeBattlefield = (int)size;
        Text_sizeBattlefield.text = sizeBattlefield + "*" + sizeBattlefield;

        int half = sizeBattlefield/2;

        if(sizeHouse.y > half)
        {
            sizeHouse.y = half;
        }

        if(sizeHouse.x > half - 1)
        {
            sizeHouse.x = half - 1;
        }

        Text_sizeHouse.text = sizeHouse.x + "*" + sizeHouse.y;
    }

    public void ChangeHouse(bool right)
    {
        if (Text_sizeHouse == null)
        {
            Debug.LogError("Text_sizeHouse is null");
            return;
        }

        int half = sizeBattlefield / 2;

        if (right)
        {
            if (sizeHouse.y + 1 > half)
            {
                if (sizeHouse.x + 1 > half - 1)
                    return;
                else
                {
                    sizeHouse.x++;
                    sizeHouse.y = 3;
                    Text_sizeHouse.text = sizeHouse.x + "*" + sizeHouse.y;
                }
            }
            else
            {
                sizeHouse.y++;
                Text_sizeHouse.text = sizeHouse.x + "*" + sizeHouse.y;
            }
        }
        else
        {
            if (sizeHouse.y - 1 < 3)
            {
                if (sizeHouse.x - 1 < 3)
                    return;
                else
                {
                    sizeHouse.x--;
                    sizeHouse.y = half;
                    Text_sizeHouse.text = sizeHouse.x + "*" + sizeHouse.y;
                }
            }
            else
            {
                sizeHouse.y--;
                Text_sizeHouse.text = sizeHouse.x + "*" + sizeHouse.y;
            }
        }
    }

    public void ChangeTypeGame(int idType)
    {
        idType = Mathf.Clamp(idType, 0, 3);

        typeGame = (TypeGame)idType;
    }

    public void OnOffAnimation(bool on)
    {
        animation_on = on;
    }

    public void SpeedAnimation(float speed)
    {
        speed_animation = speed;
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
