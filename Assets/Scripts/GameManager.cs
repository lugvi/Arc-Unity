using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance;
    private void Awake() { instance = this; }
    #endregion
    public Grid grid;

    public Color[] colors;
    [Range(0, 1)]
    public float CoinChance;
    public ArcController arcPrefab;
    public GameObject coinPrefab;

    public Transform player;

    public float playerSpeed;
    public Vector3 destination;

    public List<ArcController> arcss;
    public Queue<ArcController> pool;

    bool moving;

    private void Start()
    {
        destination = player.position;
        pool = new Queue<ArcController>();
        SpawnFirstArcs();

    }


    void SpawnFirstArcs()
    {
        for (int i = -2; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                ArcController arc = SpawnArc(new Vector3(i, j, 0), Random.Range(90, 190), (Direction)Random.Range(0, 2));

                arcss.Add(arc);
            }
        }
    }

    void Update()
    {
#if UNITY_EDITOR
        if (Input.anyKeyDown && !moving)
        {
            float XAxis = Input.GetAxisRaw("Horizontal");
            float YAxis = Input.GetAxisRaw("Vertical");

            if (XAxis != 0)
            {
                destination.x += XAxis;
                StartCoroutine(MovePlayer(destination));
                moveGrid(Vector3.right * XAxis);
            }
            else if (YAxis != 0)
            {
                destination.y += YAxis;
                StartCoroutine(MovePlayer(destination));
                moveGrid(Vector3.up * YAxis);
            }

        }

#elif UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).deltaPosition.x > 0)
            {
                moveGrid(Vector3.right);
                destination.x += 1;
            }
            if (Input.GetTouch(0).deltaPosition.x < 0)
            {
                destination.x -= 1;
                moveGrid(-Vector3.right);
            }
            if (Input.GetTouch(0).deltaPosition.y > 0)
            {
                destination.y += 1;
                moveGrid(Vector3.up);
            }
            if (Input.GetTouch(0).deltaPosition.x < 0)
            {
                destination.y -= 1;
                moveGrid(-Vector3.up);
            }
            StartCoroutine(MovePlayer(destination));
        }
#endif
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

        ClearArray();

    }
    public int removeDistX;
    public int removeDistY;
    void ClearArray()
    {
        List<ArcController> rem = arcss.FindAll(x => Mathf.Abs(x.transform.position.x - player.position.x) > removeDistX || Mathf.Abs(x.transform.position.y - player.position.y) > removeDistY);//new List<ArcController>();

        for (int i = 0; i < rem.Count; i++)
        {
            var r = rem[i];
            r.gameObject.SetActive(false);
            r.transform.position = Vector2.one * 100;
            pool.Enqueue(r);
            arcss.Remove(r);
        }

    }

    public ArcController SpawnArc(Vector3 pos, float spinSpeed, Direction spinDirection)
    {
        ArcController arc;
        if (pool.Count > 0)
        {
            arc = pool.Dequeue();
            arc.transform.position = pos;
            arc.gameObject.SetActive(true);
        }
        else
        {
            arc = Instantiate(arcPrefab, pos, Quaternion.identity, grid.transform);
        }

        arc.rotationSpeed = spinSpeed;
        arc.rotationDirection = spinDirection;
        arc.col = colors[Random.Range(0, colors.Length)];
        if (Random.value < CoinChance)
            Instantiate(coinPrefab, pos, Quaternion.identity, grid.transform);
        return arc;
    }





    public void moveGrid(Vector3 direction)
    {
        if (direction.y == 0)
        {
            for (int i = -2; i < 3; i++)
            {
                ArcController arc = SpawnArc(new Vector3(destination.x + direction.x * removeDistX, destination.y + i, 0), Random.Range(90, 190), (Direction)Random.Range(0, 2));
                arcss.Add(arc);
            }
        }
        else

        {
            for (int i = -2; i < 3; i++)
            {
                ArcController arc = SpawnArc(new Vector3(destination.x + i, destination.y + direction.y * removeDistY, 0), Random.Range(90, 190), (Direction)Random.Range(0, 2));
                arcss.Add(arc);
            }
        }



    }



}
