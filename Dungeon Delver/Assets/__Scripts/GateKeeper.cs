using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateKeeper : MonoBehaviour
{
    //-------Индексы плиток с запертыми дверями
    const int lockerR = 95;
    const int lockerUR = 81;
    const int lockerUL = 80;
    const int lockerL = 100;
    const int lockerDL = 101;
    const int lockerDR = 102;
    //-------Индексы плиток с откртыми дверями
    const int openR = 48;
    const int openUR = 93;
    const int openUL = 92;
    const int openL = 51;
    const int openDL = 26;
    const int openDR = 27;

    private IKeyMaster keys;
    private void Awake()
    {
        keys = GetComponent<IKeyMaster>();
    }
    private void OnCollisionStay(Collision collision)
    {
        //Если ключей нет, можно не продолжать
        if (keys.keyCount < 1) return;

        //Интерес представлют только плитки
        Tile ti = collision.gameObject.GetComponent<Tile>();
        if (ti == null) return;

        //Открывать, только если Дрей обращен лицом к двери (предотвратить случайное использование ключа)
        int facing = keys.GetFacing();
        //Проверить является ли плитка закрытой дверью
        Tile ti2;
        switch (ti.tileNum)
        {
            case lockerR:
                if (facing != 0) return;
                ti.SetTile(ti.x, ti.y, openR);
                break;
            case lockerUR:
                if (facing != 1) return;
                ti.SetTile(ti.x, ti.y, openUR);
                ti2 = TileCamera.TILES[ti.x - 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openUL);
                break;
            case lockerUL:
                if (facing != 1) return;
                ti.SetTile(ti.x, ti.y, openUL);
                ti2 = TileCamera.TILES[ti.x + 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openUR);
                break;
            case lockerL:
                if (facing != 2) return;
                ti.SetTile(ti.x, ti.y, openL);
                break;
            case lockerDL:
                if (facing != 3) return;
                ti.SetTile(ti.x, ti.y, openDL);
                ti2 = TileCamera.TILES[ti.x + 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openDR);
                break;
            case lockerDR:
                if (facing != 3) return;
                ti.SetTile(ti.x, ti.y, openDR);
                ti2 = TileCamera.TILES[ti.x - 1, ti.y];
                ti2.SetTile(ti2.x, ti2.y, openDL);
                break;
            default:
                return;//Выйтиб что бы исключить уменьшение стетчика ключей
        }
        keys.keyCount--;
    }
}
