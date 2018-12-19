using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
#region singleton
public static GameManager instance;
private void Awake() {instance = this;}
#endregion
    public Grid grid;

    public ArcController arcPrefab;

    public Transform player;

    public float playerSpeed;
    public Vector3 destination;

	public List<ArcController> arcss;

    bool moving;

    private void Start()
    {
        destination = player.position;

    }

    void Update()
    {
        if (Input.anyKeyDown && !moving)
        {
            float XAxis = Input.GetAxisRaw("Horizontal");
            float YAxis = Input.GetAxisRaw("Vertical");

            if (XAxis != 0)
            {
                destination.x += XAxis;
                StartCoroutine(MovePlayer(destination));
		//		ClearArray();
                moveGrid(Vector3.right * XAxis);
            }
            else if (YAxis != 0)
            {
                destination.y += YAxis;
                StartCoroutine(MovePlayer(destination));
			//	ClearArray();
                moveGrid(Vector3.up * YAxis);
            }

        }

		rotato();
		
    }

	void rotato()
	{
		foreach (ArcController arc in arcss)
		{
			arc.Rotate();
		}

		
	}


    public IEnumerator MovePlayer(Vector3 dest)
    {
        if (moving)
            StopCoroutine("MovePlayer");
        else
        {
            moving = true;
            while (player.position != dest)
            {
                player.position = Vector3.MoveTowards(player.position, dest, Time.deltaTime * playerSpeed);
                yield return 0;
            }
            moving = false;
        }

    }

	// void ClearArray()
	// {
	// 	for (int i = 0; i< arcss.Count;i++)
	// 	{
			
	// 		if(Vector3.Distance(arcss[i].transform.localPosition,player.localPosition)>2)
	// 			{
	// 				ArcController tmp = arcss[i];
	// 				arcss.Remove(arcss[i]);
	// 				Destroy(tmp.gameObject);

	// 			}
	// 	}

	// }

	public ArcController SpawnArc(Vector3 pos, float spinSpeed,Direction spinDirection)
	{
		ArcController arc = Instantiate(arcPrefab, pos, Quaternion.identity, grid.transform);
		arc.rotationSpeed = spinSpeed;
		arc.rotationDirection = spinDirection;
		// arc.color = color;
		return  arc;
	}

	


    public void moveGrid(Vector3 direction)
    {
        if (direction.y == 0)
        {
      	for (int i = -2; i < 3; i++)
            {
                ArcController arc = SpawnArc(new Vector3(destination.x + direction.x*2, destination.y + i, 0),Random.Range(90,190),(Direction)Random.Range(0,2));
				arcss.Add(arc);
            }
        }
        else

		{
			for (int i = -2; i < 3; i++)
            {
                ArcController arc = SpawnArc(new Vector3(destination.x+i, destination.y + direction.y*2, 0),Random.Range(90,190),(Direction)Random.Range(0,2));
				arcss.Add(arc);
            }
		}
    }



}
