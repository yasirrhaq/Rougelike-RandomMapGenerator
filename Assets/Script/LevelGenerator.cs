using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour {

    public Transform[] startingPosition;
    public GameObject[] rooms; // index 0 -> LR, 1 -> LRB, 2-> LRT, 3 -> LRBT

    private int direction;

    public float moveAmount;
    private float timeBtwRoom; //delay spawn
    public float startTimeBtwRoom = 0.25f; //the value

    public float minX;
    public float maxX;
    public float minY;
    public bool stopGeneration;

    public LayerMask room;

    public int downCounter;

	// Use this for initialization
	void Start () {
        int randStartingPos = Random.Range(0, startingPosition.Length); 
        transform.position = startingPosition[randStartingPos].position;
        Instantiate(rooms[0], transform.position,Quaternion.identity);

        direction = Random.Range(1, 6);
	}

    // Update is called once per frame
    void Update()
    {
        if (timeBtwRoom <= 0 && stopGeneration == false)
        {
            Move();
            timeBtwRoom = startTimeBtwRoom;
        }
        else
        {
            timeBtwRoom -= Time.deltaTime;
        }
    }

    void Move()
    {
        if (direction == 1 || direction == 2) //Move Right
        {
            if (transform.position.x < maxX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x + moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6);

                if (direction == 3) //if move left this will change it to move right
                {
                    direction = 1;
                }
                else if (direction == 4) //this will change to move down
                {
                    direction = 5;
                } 
            }
            else //if generation > the maxX
            {
                direction = 5;
            }
        }
        else if (direction == 3 || direction == 4) //Move Left
        {
            if (transform.position.x > minX)
            {
                downCounter = 0;
                Vector2 newPos = new Vector2(transform.position.x - moveAmount, transform.position.y);
                transform.position = newPos;

                int rand = Random.Range(0, rooms.Length);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(3, 6); //Will only move left/down
            }
            else
            {
                direction = 5;
            } 
        }
        else if (direction == 5) { //Move Down

            downCounter++;

            if (transform.position.y > minY)
            {
                Collider2D roomDetection = Physics2D.OverlapCircle(transform.position, 1, room);

                ////if room doesn't  have bottom opening
                if (roomDetection.GetComponent<RoomType>().type != 1 && roomDetection.GetComponent<RoomType>().type != 3)
                {

                    if (downCounter>=2)
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();
                        Instantiate(rooms[3], transform.position, Quaternion.identity);
                    }
                    else
                    {
                        roomDetection.GetComponent<RoomType>().RoomDestruction();

                        int randBottomRoom = Random.Range(1, 4);
                        if (randBottomRoom == 2)
                        {
                            randBottomRoom = 1;
                        }
                        Instantiate(rooms[randBottomRoom], transform.position, Quaternion.identity);
                    }
                }

                Vector2 newPos = new Vector2(transform.position.x, transform.position.y - moveAmount);
                transform.position = newPos;

                int rand = Random.Range(2, 4);
                Instantiate(rooms[rand], transform.position, Quaternion.identity);

                direction = Random.Range(1, 6); //if we just move to bottom, make a random move left/right
            }
            else
            {
                //Stop level generation
                stopGeneration = true;
            }
        }
    }
}
