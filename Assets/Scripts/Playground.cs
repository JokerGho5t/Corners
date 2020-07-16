using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

[RequireComponent(typeof(Animator))]
public class Playground : MonoBehaviour
{
    #region Events

    public UnityEvent GameOver;

    #endregion

    #region Public Variables

    public static bool FirstPlayer { get { return moveFirstPlayer; } private set { } }
    private static bool moveFirstPlayer = true;

    public static int GetSizeBattlefield { get { return sizeBattlefield; } private set { } }
    private static int sizeBattlefield;

    [Header("Настройки минимальной границы")]

    [Range(8, 100), Tooltip("Минимальный размер поля")]
    public int minSizeBattlefield = 8;
    [Range(3, 100), Tooltip("Минимальная высота начального уголка")]
    public int minHeightHouse = 3;
    [Range(3, 100), Tooltip("Минимальная ширина начального уголка")]
    public int minWidthHouse = 3;

    [Header("Имя ресурсов")]

    [Tooltip("Имя префаба для клетки")]
    public string NamePrefabCell = "PrefabCell";
    [Tooltip("Имя префаба для фигуры")]
    public string NamePrefabFigure = "PrefabFigure";

    public Vector2 GridSpacing = new Vector2(5, 5);

    #endregion

    #region  Private Variables

    private GridLayoutGroup grid = null;
    private Animator anim = null;
    private List<Figure> FirstPlayerFigures = new List<Figure>();
    private List<Figure> SecondPlayerFigures = new List<Figure>();
    private Vector2Int FirstFigureBorde;
    private Vector2Int SecondFigureBorde;
    private int numberStep_first = 0;
    private int numberStep_second = 0;

    #endregion

    private void DebugMessage(TypeMessage type, string message)
    {
        Log.LogMe(this.name, message, type);
    }

    void OnEnable()
    {
        MainUI.StartEvent += Init;
        Figure.Step += NextStep;
    }

    void OnDisable()
    {
        MainUI.StartEvent -= Init;
        Figure.Step -= NextStep;
    }

    public bool Init(int SizeBattlefield, Vector2Int House, TypeGame type, bool animationOn, float speed_animation)
    {
        if (SizeBattlefield < minSizeBattlefield || SizeBattlefield < 3)
        {
            DebugMessage(TypeMessage.Error, "Нельзя играть с игровым полем меньше чем " + minSizeBattlefield + "*" + minSizeBattlefield);
            return false;
        }
        int halfSizeBattlefield = SizeBattlefield / 2;

        if (House.x < minHeightHouse || House.y < minWidthHouse)
        {
            DebugMessage(TypeMessage.Error, "Нельзя играть с расстановкой дома меньше чем " + minHeightHouse + "*" + minWidthHouse);
            return false;
        }
        else if (House.x > halfSizeBattlefield - 1 || House.y > halfSizeBattlefield)
        {
            DebugMessage(TypeMessage.Error, "Нельзя играть с расстановкой дома больше чем " + (halfSizeBattlefield - 1) + "*" + halfSizeBattlefield);
            return false;
        }

        moveFirstPlayer = true;
        sizeBattlefield = SizeBattlefield;
        anim = (animationOn)? GetComponent<Animator>() : null;
        anim?.SetFloat("Speed", speed_animation);

        numberStep_first = 0;
        numberFirstText.text = "0";
        numberFirstText.color = Color.yellow;
        numberStep_second = 0;
        numberSecondText.text = "0";
        numberSecondText.color = Color.white;

        GameObject PrefabCell = Resources.Load<GameObject>("Prefabs/" + NamePrefabCell);
        GameObject PrefabFigure = Resources.Load<GameObject>("Prefabs/" + NamePrefabFigure);

        SetupGridLayout(SizeBattlefield);


        //Create battlefield

        FirstPlayerFigures.Clear();
        SecondPlayerFigures.Clear();

        int curColumn = 0;
        int curRow = 0;

        FirstFigureBorde = new Vector2Int(SizeBattlefield - House.x, House.y);
        SecondFigureBorde = new Vector2Int(House.x, SizeBattlefield - House.y);

        for (int i = 0; i < SizeBattlefield * SizeBattlefield; i++)
        {
            var obj = Instantiate(PrefabCell, grid.transform);
            var cell = obj.GetComponent<Cell>();
            cell.Init(new Vector2Int(curRow, curColumn));


            Figure figure = null;


            if (curRow >= FirstFigureBorde.x && curColumn < FirstFigureBorde.y)
            {
                //FirstPlayerFigure
                figure = CreateFigure(PrefabFigure, cell.transform, type, true, new Vector2Int(curRow, curColumn));
                FirstPlayerFigures.Add(figure);
                figure.name = "FirstPlayerFigure_" + FirstPlayerFigures.Count;
            }
            else if (curRow < SecondFigureBorde.x && curColumn >= SecondFigureBorde.y)
            {
                //SecondPlayerFigure
                figure = CreateFigure(PrefabFigure, cell.transform, type, false, new Vector2Int(curRow, curColumn));
                SecondPlayerFigures.Add(figure);
                figure.name = "SecondPlayerFigure_" + SecondPlayerFigures.Count;
            }

            if (figure != null)
            {
                cell.SetFigure(figure.gameObject);
            }


            if ((i + 1) % SizeBattlefield == 0)
            {
                curRow++;
                curColumn = 0;
            }
            else
            {
                curColumn++;
            }
        }

        //------------------------------------

        return true;
    }

    private Figure CreateFigure(GameObject prefab, Transform parent, TypeGame type, bool firstPlayer, Vector2Int coordinate)
    {
        var obj = Instantiate(prefab, parent);
        Figure figure = null;

        switch (type)
        {
            case TypeGame.OneStep:
                figure = obj.AddComponent<OneStep>();
                break;
            case TypeGame.Diagonal:
                figure = obj.AddComponent<Diagonal>();
                break;
            case TypeGame.Axis:
                figure = obj.AddComponent<Axis>();
                break;
        }

        figure.Init(firstPlayer, coordinate);

        return figure;
    }

    private void SetupGridLayout(int Size)
    {
        if (grid != null)
            Destroy(grid.gameObject);

        var temp = new GameObject("Grid");
        temp.transform.SetParent(transform);
        grid = temp.AddComponent<GridLayoutGroup>();

        RectTransform RectThis = GetComponent<RectTransform>();
        RectTransform RectGrid = temp.GetComponent<RectTransform>();
        RectGrid.position = RectThis.position;
        RectGrid.sizeDelta = new Vector2(RectThis.rect.width, RectThis.rect.height);

        float widthCell = (RectThis.rect.width / Size) - GridSpacing.x;
        float heightCell = (RectThis.rect.height / Size) - GridSpacing.y;

        grid.spacing = GridSpacing;
        grid.cellSize = new Vector2(widthCell, heightCell);
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = Size;
    }

    public void ExitGame()
    {
        if (grid != null)
            Destroy(grid.gameObject);

        FirstPlayerFigures.Clear();
        SecondPlayerFigures.Clear();
    }

    //Проверка на выйгрыш. Если все фишки игрока, который последний делал ход 
    //находяться в доме противника, то игра закончена и он выиграл
    private bool CheckGameOver()
    {
        bool result = true;

        if (moveFirstPlayer)
        {
            foreach (var item in FirstPlayerFigures)
            {
                if (item.GetCoordinate.x >= SecondFigureBorde.x || item.GetCoordinate.y < SecondFigureBorde.y)
                {
                    result = false;
                    break;
                }
            }
        }
        else
        {
            foreach (var item in SecondPlayerFigures)
            {
                if (item.GetCoordinate.x < FirstFigureBorde.x || item.GetCoordinate.y >= FirstFigureBorde.y)
                {
                    result = false;
                    break;
                }
            }
        }

        return result;
    }

    public TextMeshProUGUI numberFirstText = null;
    public TextMeshProUGUI numberSecondText = null;

    private void NextStep()
    {
        if (moveFirstPlayer)
        {
            numberStep_first++;
            numberFirstText.text = numberStep_first.ToString();
        }
        else
        {
            numberStep_second++;
            numberSecondText.text = numberStep_second.ToString();
        }



        if (CheckGameOver())
        {
            GameOver?.Invoke();
        }
        else
        {
            moveFirstPlayer = !moveFirstPlayer;
        }

        anim?.SetTrigger((moveFirstPlayer)? "First" : "Second");


        numberFirstText.color = (moveFirstPlayer) ? Color.yellow : Color.white;
        numberSecondText.color = (!moveFirstPlayer) ? Color.yellow : Color.white;
    }
}
