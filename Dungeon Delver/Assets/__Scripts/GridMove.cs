using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMove : MonoBehaviour
{
    private IFacingMover mover;
    private void Awake()
    {
        mover = GetComponent<IFacingMover>();
    }
    private void FixedUpdate()
    {
        if (!mover.moving) return;//Если обьект не перемещается - выйти
        int facing = mover.GetFacing();
        //Если обьект перемещается, принимать выравнивание по сетке
        //Сначала получить координаты, ближайшего узла сетки
        Vector2 rPos = mover.roomPos;
        Vector2 rPosGrid = mover.GetRoomPosOnGrid();
        //Этот код пологается на интерфейс IFacingMover (который испоьзует InRoom) для определения шага сетки

        //Затем подвинуть обьект в сторону линии сетки
        float delta = facing == 0 || facing == 2 ? rPosGrid.y - rPos.y : rPosGrid.x - rPos.x;
        if (delta == 0) return;// обьект уже выровнен по сетке
        float move = mover.GetSpeed() * Time.fixedDeltaTime;
        move = Mathf.Min(move, Mathf.Abs(delta));
        if (delta < 0)
            _ = -move;

        if (facing == 0 || facing == 2)
            //Движение по горизонтали, выравнивание по оси y
            rPos.y += move;
        else
            //Движение по вертикали, выравнивание по оси x
            rPos.x += move;
        mover.roomPos = rPos;
    }
}
