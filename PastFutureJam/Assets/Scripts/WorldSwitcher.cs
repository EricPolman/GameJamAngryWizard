using UnityEngine;
using System.Collections;

public enum World { PAST, FUTURE };

public class WorldSwitcher : MonoBehaviour
{
  public Transform futureStartPoint;
  public Transform pastStartPoint;

  public World currentWorld;

	void Start ()
	{
    transform.position = 
      currentWorld == World.PAST ? pastStartPoint.position : futureStartPoint.position;
    transform.position -= Vector3.forward * 10;
	}
	
	void Update ()
	{
	  if(Input.GetKeyDown(KeyCode.LeftControl))
    {
      SwitchWorld();
    }
	}

  void SwitchWorld()
  {
    currentWorld = currentWorld == World.PAST ? World.FUTURE : World.PAST;

    transform.position =
      currentWorld == World.PAST ? pastStartPoint.position : futureStartPoint.position;
    transform.position -= Vector3.forward * 10;
  }
}
