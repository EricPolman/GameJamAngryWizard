using UnityEngine;
using System.Collections;

public enum World { PAST, FUTURE };
public enum Direction { UP, DOWN, LEFT, RIGHT };
public enum State { IDLE, SWITCHING_WORLD, SWITCHING_ROOM };

public class WorldSwitcher : MonoBehaviour
{
  public Transform futureStartPoint;
  public Transform pastStartPoint;

  private Transform _previousFutureRoom;
  private Transform _previousPastRoom;
  public Transform currentFutureRoom;
  public Transform currentPastRoom;

  public State state = State.IDLE;
  private float switchTimer = 0;
  private float secondSwitchTimer = 0;

  public World currentWorld;
  private Vector3 _offset = Vector3.forward * 10;

  public GUIText worldGuiText;

	void Start ()
  {
    _previousFutureRoom = currentFutureRoom;
    _previousPastRoom = currentPastRoom;
    transform.position =
      currentWorld == World.PAST ? currentPastRoom.position : currentFutureRoom.position;
    transform.position -= _offset;
	}
	
	void Update ()
  {
    switch(state)
    {
      case State.IDLE:
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
          SwitchWorld();
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
          SwitchRoom(Direction.UP);
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
          SwitchRoom(Direction.DOWN);
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
          SwitchRoom(Direction.LEFT);
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
          SwitchRoom(Direction.RIGHT);
        }
        break;
      case State.SWITCHING_ROOM:
        switchTimer += Time.deltaTime;
        if(currentWorld == World.PAST)
        {
          transform.position = 
            Vector3.Lerp(_previousPastRoom.position, currentPastRoom.position, switchTimer);
        }
        else
        {
          transform.position = 
            Vector3.Lerp(_previousFutureRoom.position, currentFutureRoom.position, switchTimer);        
        }
        transform.position -= _offset;  

        if(switchTimer > 1)
        {
          if(currentWorld == World.PAST)
          {
            transform.position = currentPastRoom.position - _offset;
          }
          else
          {
            transform.position = currentFutureRoom.position - _offset;
          }
          state = State.IDLE;
          switchTimer = 0;
        }
        break;
      case State.SWITCHING_WORLD:
        Transform curr = null;
        if (currentWorld == World.PAST)
        {
          curr = currentPastRoom;
        }
        else
        {
          curr = currentFutureRoom;
        }

        switchTimer += Time.deltaTime;
        curr.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(90, 0, 0), switchTimer);
        if(switchTimer > 1)
        {
          curr.rotation = Quaternion.identity;
          worldGuiText.text = currentWorld == World.FUTURE ? "pAst" : "futuRE";
          
          transform.position =
            (currentWorld == World.FUTURE ? currentPastRoom.position : currentFutureRoom.position) - _offset;

          Transform newTransform = currentWorld == World.FUTURE ? currentPastRoom : currentFutureRoom;
          newTransform.rotation = Quaternion.Lerp(Quaternion.Euler(90, 0, 0), Quaternion.identity, switchTimer - 1.0f);
          if (switchTimer >= 2.0f)
          {
            newTransform.rotation = Quaternion.identity;
            currentWorld = currentWorld == World.PAST ? World.FUTURE : World.PAST;
            state = State.IDLE;
            switchTimer = 0;
          }
        }
        break;
    }
	}

  void SwitchWorld()
  {
    state = State.SWITCHING_WORLD;
  }

  void SwitchRoom(Direction dir)
  {
    Transform curr =
      currentWorld == World.PAST ? currentPastRoom : currentFutureRoom;

    Transform newRoom = null;
    switch(dir)
    {
      case Direction.UP:
        newRoom = curr.GetComponent<Room>().up;
        break;
      case Direction.DOWN:
        newRoom = curr.GetComponent<Room>().down;
        break;
      case Direction.LEFT:
        newRoom = curr.GetComponent<Room>().left;
        break;
      case Direction.RIGHT:
        newRoom = curr.GetComponent<Room>().right;
        break;
    }
    if (newRoom != null)
    {
      if (currentWorld == World.PAST)
      {
        _previousPastRoom = currentPastRoom;
        currentPastRoom = newRoom;
      }
      else
      {
        _previousFutureRoom = currentFutureRoom;
        currentFutureRoom = newRoom;
      }
      state = State.SWITCHING_ROOM;
    }
  }
}
