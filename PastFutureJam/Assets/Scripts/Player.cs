using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
    private Vector2 startPos;
    private Vector2 endPos;
    private float speed = 0.25f;
    private bool moving = false;
    private float time = 0f;
    public float walkTime = 0.5f;

	
	void Start () {
	}
	
	void Update () {
		
		//CheckInput();
		
		if(moving) {
			// pos is changed when there's input from the player
            Vector2 curPos = transform.position;
            time += Time.deltaTime;

			transform.position = Vector2.Lerp(startPos, endPos, time/walkTime);
            if (time >= walkTime)
            {
                moving = false;
                time = 0;
            }
        }
        else
        {
            CheckInput();
        }
	}
	
	private void CheckInput() {
		float moveX = Input.GetAxisRaw ("Horizontal");

		if (moveX != 0){
            startPos = transform.position;
			endPos = startPos + Vector2.right * moveX;
			moving = true;
            time = 0;
		}

		float moveY = Input.GetAxisRaw ("Vertical");
		
		if (moveY != 0){
            startPos = transform.position;
            endPos = startPos + Vector2.up * moveY;
			moving = true;
            time = 0;
		}

	}
}