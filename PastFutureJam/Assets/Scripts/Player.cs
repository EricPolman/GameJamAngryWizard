using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour
{

  private Vector2 _startPos;
  private Vector2 _endPos;
  private bool moving = false;
  private float time = 0f;
  public float walkTime = 0.5f;

  public bool fromTheFuture = false;
  private WorldSwitcher _worldSwitcher;

  void Start()
  {
    transform.position = new Vector2(Mathf.Ceil(transform.position.x), Mathf.Ceil(transform.position.y));
    _worldSwitcher = Camera.main.GetComponent<WorldSwitcher>();
  }

  void Update()
  {

    if (moving)
    {
      time += Time.deltaTime;

      transform.position = Vector2.Lerp(_startPos, _endPos, time / walkTime);
      if (time >= walkTime)
      {
        moving = false;
        time = 0;
      }
    }
    else
    {
      if(fromTheFuture && _worldSwitcher.currentWorld == World.FUTURE ||
        !fromTheFuture && _worldSwitcher.currentWorld == World.PAST)
      {
        CheckInput();
      }
    }
  }

  private void CheckInput()
  {
    float moveX = Input.GetAxisRaw("Horizontal");
    if (moveX != 0)
    {
      _startPos = transform.position;
      _endPos = _startPos + Vector2.right * moveX;
      moving = true;
      time = 0;
    }

    float moveY = Input.GetAxisRaw("Vertical");

    if (moveY != 0)
    {
      _startPos = transform.position;
      _endPos = _startPos + Vector2.up * moveY;
      moving = true;
      time = 0;
    }

    if (collides(_endPos))
      moving = false;

  }

  private bool collides(Vector2 tile)
  {
    Vector2[] tileList = new Vector2[] { new Vector2(-1, 0), new Vector2(-1, 1), new Vector2(-1, -1) };
    foreach (Vector2 element in tileList)
    {
      if (tile == element)
        return true;

    }
    return false;
  }
}