using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class FloatSquare : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] float speed = 7f;
    FloatSquareType type;
    Vector3 direction;
    Vector3 position;
    System.Action<int> onCollect;
    public void Init(FloatSquareType type, Vector3 position, Vector3 direction, System.Action<int> onCollect)
    {
        this.type = type;
        this.direction = direction;
        this.position = position;
        this.onCollect = onCollect;
        GetComponent<SpriteRenderer>().color = colors[(int)type];
    }

    void Update()
    {
        position += direction * Time.deltaTime * speed;
        transform.position = position + Vector3.up * Mathf.Sin(Time.time + position.x);
    }

    private void OnMouseUpAsButton()
    {
        switch (type)
        {
            case FloatSquareType.green:
                onCollect(100);
                Destroy(gameObject);
                break;
            case FloatSquareType.orange:
                onCollect(200);
                Destroy(gameObject);
                break;
            case FloatSquareType.red:
                onCollect(-300);
                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}
public enum FloatSquareType
{
    green,
    orange,
    red
}
