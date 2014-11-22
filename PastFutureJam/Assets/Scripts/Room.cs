using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{
  public Transform up;
  public Transform down;
  public Transform left;
  public Transform right;

  public SpriteRenderer[] tiles;
  private int rows;
  private int cols;

  private const int tileSize = 1;

	void Start ()
	{
	  tiles = new SpriteRenderer[transform.childCount];
    Vector3 min = Vector3.one * 100000, max = Vector3.one * -10000;
    var children = GetComponentsInChildren<Transform>();
    Transform currentMinTransform = null;
    Transform currentMaxTransform = null;
    foreach(Transform t in children)
    {
      Vector3 tPos = t.position;
      if (tPos.x <= min.x && tPos.y <= min.y)
      {
        min = tPos;
        currentMinTransform = t;
      }
      if (tPos.x >= max.x && tPos.y >= max.y)
      {
        max = tPos;
        currentMaxTransform = t;
      }
    }

    tiles[0] = currentMinTransform.GetComponent<SpriteRenderer>();
    tiles[transform.childCount - 1] = currentMaxTransform.GetComponent<SpriteRenderer>();
    Vector3 difference = currentMaxTransform.position - currentMinTransform.position;
    difference /= tileSize;
    cols = (int)difference.x;
    rows = (int)difference.y;

    foreach (Transform t in children)
    {
      Vector3 tDiff = t.position - tiles[0].transform.position;
      tDiff /= tileSize;
      int x = (int)tDiff.x;
      int y = (int)tDiff.y;
      tiles[y * cols + x] = t.GetComponent<SpriteRenderer>();
    }
	}
	
	void Update ()
	{
	  
	}

  public SpriteRenderer GetTileAtPosition(Vector2 pos)
  {
    Vector2 idx = (new Vector3(pos.x, pos.y, 0) - tiles[0].transform.position) / tileSize;
    return tiles[(int)pos.y * cols + (int)pos.x];
  }
}
