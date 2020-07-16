using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//          ------------> Y
//         |
//         |
//         |
//         |
//        \/
//         X

//        x - 1, y - 1   |   x - 1 , y   |   x - 1, y + 1
//      ------------------------------------------------
//          x, y - 1    |      x, y     |    x, y + 1
//      -----------------------------------------------
//       x + 1, y - 1  |    x + 1, y   |   x + 1, y + 1

public class OneStep : Figure
{
    public override void Select()
    {
        StepCoordinate.Clear();

        StepCoordinate.Add(new Vector2Int(coordinate.x, coordinate.y + 1));
        StepCoordinate.Add(new Vector2Int(coordinate.x, coordinate.y - 1));
        StepCoordinate.Add(new Vector2Int(coordinate.x + 1, coordinate.y));
        StepCoordinate.Add(new Vector2Int(coordinate.x + 1, coordinate.y + 1));
        StepCoordinate.Add(new Vector2Int(coordinate.x + 1, coordinate.y - 1));
        StepCoordinate.Add(new Vector2Int(coordinate.x - 1, coordinate.y));
        StepCoordinate.Add(new Vector2Int(coordinate.x - 1, coordinate.y + 1));
        StepCoordinate.Add(new Vector2Int(coordinate.x - 1, coordinate.y - 1));
    }
}
