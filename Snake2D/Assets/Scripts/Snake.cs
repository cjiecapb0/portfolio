using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Snake : MonoBehaviour
{
    public Tilemap Level;
    public Tilemap Items;
    private Vector3 _direction = Vector3.up;
    private float _speed = 1;
    private Vector3 _position;
    private Transform _selfTransform;
    private SpriteRenderer _render;
    public GameObject[] _tail = new GameObject[3];
    private Vector3 _oldPosition;
    public Sprite UpSprite, RigthSprite, DownSprite, LeftSprite;
    public Sprite TailEndUpSprite, TailEndRigthSprite, TailEndDownSprite, TailEndLeftSprite;
    public Sprite Horizontal, Vertical;
    public Sprite TailBendinUpRigth, TailBendinUpLeft, TailBendinDownRigth, TailBendinDownLeft;
    public TileBase Ground;
    private void Start()
    {
        _selfTransform = GetComponent<Transform>();
        _render = GetComponent<SpriteRenderer>();
        _position = _selfTransform.position;
    }
    private void Update()
    {
        _position += _direction * _speed * Time.deltaTime;
        Vector3 newPosition = Level.WorldToCell(_position);
        _selfTransform.position = newPosition;

        if (_oldPosition != newPosition)
        {
            TileBase tile = Level.GetTile(Level.WorldToCell(newPosition));
            if(tile == Ground)
            {

            }
            if(tile is FooTile)
            {
                var food = (FooTile)tile;
                //food.FoodValue;
            }
            MoveTail(_oldPosition);
        }
        _oldPosition = newPosition;
    }
    public void MoveUp()
    {
        _direction = Vector3.up;
    }
    public void MoveRight()
    {
        _direction = Vector3.right;
    }
    public void MoveDown()
    {
        _direction = Vector3.down;
    }
    public void MoveLeft()
    {
        _direction = Vector3.left;
    }
    public void AddBoneTail()
    {

    }
    public void MoveTail(Vector3 target)
    {
        for (int i = _tail.Length-1; i > 0; i--)
        {
            _tail[i].transform.position = _tail[i - 1].transform.position;
        }
        _tail[0].transform.position = target;

        for (int i = _tail.Length - 2; i >= 1; i--)
        {
            var prev = _tail[i + 1];
            var next = _tail[i - 1];
            var current = _tail[i];

            current.GetComponent<SpriteRenderer>().sprite = GetSprite(current.transform.position, next.transform.position,
                                                                        prev.transform.position);
        }
        _tail[0].GetComponent<SpriteRenderer>().sprite = GetSprite(_tail[0].transform.position, _selfTransform.position,
                                                                    _tail[1].transform.position);
        _tail[_tail.Length - 1].GetComponent<SpriteRenderer>().sprite = 
                                          GetTailEnd(_tail[_tail.Length - 2].transform.position);
        _render.sprite = GetTailHead();
    }
    private Sprite GetTailEnd( Vector3 next)
    {
        var direction = (_tail[_tail.Length - 1].transform.position - next).normalized;
        if (direction.x == 1)
            return TailEndLeftSprite;
        else if (direction.x == -1)
            return TailEndRigthSprite;
        else if (direction.y == 1)
            return TailEndDownSprite;
        else if (direction.y == -1)
            return TailEndUpSprite;
        return null;
    }
    private Sprite GetTailHead()
    {
        if (_direction.x == 1)
            return RigthSprite;
        else if (_direction.x == -1)
            return LeftSprite;
        else if (_direction.y == 1)
            return UpSprite;
        else if (_direction.y == -1)
            return DownSprite;
        return null;
    }
    private Sprite GetSprite(Vector3 current, Vector3 next, Vector3 prev)
    {   
        if (prev.y == next.y)
        {
            return Horizontal;
        }
        if (prev.x == next.x)
        {
            return Vertical;
        }
        var vert = next.x == current.x ? next : prev;
        var hor = next.y == current.y ? next : prev;
        //сверху
        if (vert.y > current.y)
        {
            if (hor.x > current.x)
                return TailBendinUpRigth;
            else
                return TailBendinUpLeft;
        }
        //снизу
        else
        {
            if (hor.x > current.x)
                return TailBendinDownRigth;
            else
                return TailBendinDownLeft;
        }
    }
}
