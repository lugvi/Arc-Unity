using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton

    public float swipeSensitivity;
    public static GameManager instance;
    private void Awake() { instance = this; }
    #endregion

    [Header("References")]
    public Grid grid;

    public IntVariable currentScore;
    public IntVariable highScore;

    public ArcController arcPrefab;
    public GameObject coinPrefab;

    public Transform player;

    Vector3 destination;
    [Header("Modifiable Values")]
    [Range(0, 1)]
    public float CoinChance;
    public float playerSpeed;
    public Color[] colors;

    public float RotationSpeedMin;
    public float RotationSpeedMax;

    List<ArcController> arcs;
    Queue<ArcController> pool;

    UIManager ui;

    bool moving;

    private void Start()
    {
        destination = player.position;
        ui = UIManager.instance;
        pool = new Queue<ArcController>();
        arcs = new List<ArcController>();
        SpawnFirstArcs();
        currentScore.value =0;
        Time.timeScale = 1;

    }

    public touchsimulator touchsimulator;


    void SpawnFirstArcs()
    {
        for (int i = -2; i < 3; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                ArcController arc = SpawnArc(new Vector3(i, j, 0), Random.Range(90, 190), (Direction)Random.Range(0, 2));

                arcs.Add(arc);
            }
        }
    }

     public  Vector2Int playerpos;
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

        if(Input.GetMouseButton(0)&& !moving)
        {
            if(touchsimulator.cardinalDirection.magnitude != 0)
            {
            destination.x = playerpos.x + touchsimulator.cardinalDirection.x;
            destination.y = playerpos.y + touchsimulator.cardinalDirection.y;
            moveGrid(touchsimulator.cardinalDirection);
            StartCoroutine(MovePlayer(destination));
            }

        }

 #elif UNITY_ANDROID
        if (Input.touchCount > 0 && !moving)
        {
            if (Input.GetTouch(0).deltaPosition.x > swipeSensitivity)
            {
                destination.x = playerpos.x + 1;
                moveGrid(Vector3.right);
                StartCoroutine(MovePlayer(destination));
            }
            else if (Input.GetTouch(0).deltaPosition.x < -swipeSensitivity)
            {
                destination.x = playerpos.x - 1;
                moveGrid(-Vector3.right);
                StartCoroutine(MovePlayer(destination));
            }
            else if (Input.GetTouch(0).deltaPosition.y > swipeSensitivity)
            {
                destination.y =playerpos.y + 1;
                moveGrid(Vector3.up);
                StartCoroutine(MovePlayer(destination));
            }
            else if (Input.GetTouch(0).deltaPosition.y < -swipeSensitivity)
            {
                destination.y = playerpos.y -1;
                moveGrid(-Vector3.up);
                StartCoroutine(MovePlayer(destination));
            }
        }
#endif
        Rotator();


    }

    void Rotator()
    {
        foreach (ArcController arc in arcs)
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
            playerpos = new Vector2Int((int)player.position.x,(int)player.position.y);
        }

        ClearArray();

    }
    public int removeDistX;
    public int removeDistY;
    void ClearArray()
    {
        List<ArcController> rem = arcs.FindAll(x => Mathf.Abs(x.transform.position.x - player.position.x) > removeDistX || Mathf.Abs(x.transform.position.y - player.position.y) > removeDistY);//new List<ArcController>();

        for (int i = 0; i < rem.Count; i++)
        {
            var r = rem[i];
            r.gameObject.SetActive(false);
            r.transform.position = Vector2.one * 100;
            pool.Enqueue(r);
            arcs.Remove(r);
            Destroy(r.coin);
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
        arc.coin = Instantiate(coinPrefab, arc.transform);
        return arc;
    }





    public void moveGrid(Vector3 direction)
    {
        if (direction.y == 0)
        {
            for (int i = -2; i < 3; i++)
            {
                ArcController arc = SpawnArc(new Vector3(destination.x + direction.x * removeDistX, destination.y + i, 0), Random.Range(RotationSpeedMin, RotationSpeedMax), (Direction)Random.Range(0, 2));
                arcs.Add(arc);
            }
        }
        else

        {
            for (int i = -2; i < 3; i++)
            {
                ArcController arc = SpawnArc(new Vector3(destination.x + i, destination.y + direction.y * removeDistY, 0), Random.Range(RotationSpeedMin, RotationSpeedMax), (Direction)Random.Range(0, 2));
                arcs.Add(arc);
            }
        }
    }


    public void OnGameOver()
    {
        Debug.Log("GameOver");
        ui.DisplayGameOverMenu();
        Time.timeScale = 0;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        currentScore.value = 0;

    }

    public void AddScore()
    {
        currentScore.value++;
        UIManager.instance.UpdateScoreUI();
        if(currentScore.value>highScore.value)
            highScore.value = currentScore.value;
    }



}
