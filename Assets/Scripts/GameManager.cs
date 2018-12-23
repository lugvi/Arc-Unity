using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton

    public static GameManager instance;
    private void Awake() { instance = this; }
    #endregion

    [Header("References")]
    public Grid grid;
    public FloatVariable swipeSensitivity;
    public FloatVariable arcShrinkTime;

    public IntVariable currentScore;
    public IntVariable highScore;

    public ArcController arcPrefab;
    public GameObject coinPrefab;

    public Transform player;
    public BoolVariable playing;
    UIManager ui;

    [Header("Modifiable Values")]
    [Range(0, 1)]
    public float CoinChance;
    public float playerSpeed;
    public Color[] colors;

    public float RotationSpeedMin;
    public float RotationSpeedMax;

    List<ArcController> arcs;
    Queue<ArcController> pool;

    



    public int removeDistX;
    public int removeDistY;

    [Header("Read Only Values")]

    public Vector2Int playerpos;
    public Vector3 destination;
    bool moving;

    void Update()
    {

        // #if UNITY_EDITOR
        //        t = Input.GetTouch(0);
        // #endif
        // PlayerMove(destination);
        if(playing.value)
        {
            
        TouchControls2();
        KeyboardControls();
        Rotator();
        }
    }



    public void InitValues()
    {
        playing.value = true;
        destination = player.position;
        playerpos.x = (int)player.position.x;
        playerpos.y = (int)player.position.y;
        ui = UIManager.instance;
        pool = new Queue<ArcController>();
        arcs = new List<ArcController>();
        SpawnFirstArcs();
        currentScore.value = 0;
        arcShrinkTime.value = 10;
        Time.timeScale = 1;
    }



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

    Touch t;

    public void KeyboardControls()
    {
        if (Input.anyKeyDown && !moving)
        {
            float XAxis = Input.GetAxisRaw("Horizontal");
            float YAxis = Input.GetAxisRaw("Vertical");

            if (XAxis != 0)
            {
                destination.x += XAxis;
                StartCoroutine(MovePlayer(destination));
                moveGrid(Vector2Int.right * (int)XAxis);
            }
            else if (YAxis != 0)
            {
                destination.y += YAxis;
                StartCoroutine(MovePlayer(destination));
                moveGrid(Vector2Int.up * (int)YAxis);
            }

        }

    }

    public void TouchControls2()
    {
        Vector2 mousepos;



        // Debug.Log((Input.mousePosition.x - Screen.width / 2) + " " + (Input.mousePosition.y - Screen.height / 2));
        if (Input.touchCount > 0 && !moving)
        {
            mousepos.x = Input.GetTouch(0).position.x - Screen.width / 2;
            mousepos.y = Input.GetTouch(0).position.y - Screen.height / 2;
            if (Mathf.Abs(mousepos.x) > Mathf.Abs(mousepos.y))
            {
                if (mousepos.x > 0)
                {
                    destination.x = playerpos.x + 1;
                    StartCoroutine(MovePlayer(destination));
                    moveGrid(Vector2Int.right);
                }

                if (mousepos.x < 0)
                {
                    destination.x = playerpos.x - 1;
                    StartCoroutine(MovePlayer(destination));
                    moveGrid(Vector2Int.left);
                }

            }
            else
            {
                if (mousepos.y > 0)
                {
                    destination.y = playerpos.y + 1;
                    StartCoroutine(MovePlayer(destination));
                    moveGrid(Vector2Int.up);
                }

                if (mousepos.y < 0)
                {
                    destination.y = playerpos.y - 1;
                    StartCoroutine(MovePlayer(destination));
                    moveGrid(Vector2Int.down);
                }

            }
        }
    }


    public void TouchControls()
    {
        Debug.Log(t.deltaPosition);

        if (Input.GetMouseButton(0) && !moving)
        {
            TrackTouch();
            if (Input.mousePosition != Vector3.zero)
            {
                if (Mathf.Abs(t.deltaPosition.x) > Mathf.Abs(t.deltaPosition.y))
                {
                    if (t.deltaPosition.x > swipeSensitivity.value)
                    {
                        destination = new Vector3(playerpos.x + 1, playerpos.y);
                        moveGrid(Vector2Int.right);
                        StartCoroutine(MovePlayer(destination));
                    }
                    else if (t.deltaPosition.x < -swipeSensitivity.value)
                    {
                        destination = new Vector3(playerpos.x - 1, playerpos.y);
                        moveGrid(Vector2Int.left);
                        StartCoroutine(MovePlayer(destination));
                    }
                }


                else
                {

                    if (t.deltaPosition.y > swipeSensitivity.value)
                    {
                        destination = new Vector3(playerpos.x, playerpos.y + 1);
                        moveGrid(Vector2Int.up);
                        StartCoroutine(MovePlayer(destination));
                    }
                    else if (t.deltaPosition.y < -swipeSensitivity.value)
                    {
                        destination = new Vector3(playerpos.x, playerpos.y - 1);
                        moveGrid(Vector2Int.down);
                        StartCoroutine(MovePlayer(destination));
                    }
                }
            }
        }
    }

    Vector3 lastMousePosition;
    private void TrackTouch()
    {

        t = new Touch();
        t.fingerId = 11;
        t.position = Input.mousePosition;
        // t.deltaTime = Time.deltaTime;
        if (lastMousePosition != Vector3.zero)
            t.deltaPosition = Input.mousePosition - lastMousePosition;
        else
        {
            t.deltaPosition = Vector2.zero;
        }

        if (Input.GetMouseButtonDown(0))
        {
            t.phase = TouchPhase.Began;
            lastMousePosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            t.phase = t.deltaPosition.sqrMagnitude > 1f ? TouchPhase.Moved : TouchPhase.Stationary;
            lastMousePosition = Input.mousePosition;
        }


        if (Input.GetMouseButtonUp(0))
        {
            t.phase = TouchPhase.Ended;
            lastMousePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            t.deltaPosition = Vector3.zero;
        }



    }


    void Rotator()
    {
        foreach (ArcController arc in arcs)
        {
            arc.Rotate();
        }


    }

    public float lastMovetime;

    public void PlayerMove(Vector3 dest)
    {
        player.position = Vector3.Lerp(player.position, dest, playerSpeed);
        // if(Vector3.Distance(player.position, dest)<0.01f)
        if (player.position == dest)
        {
            ClearArray();
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

            lastMovetime = Time.time;
            moving = false;
            playerpos = new Vector2Int((int)player.position.x, (int)player.position.y);
        }

        ClearArray();


    }

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
            arc.wing1.transform.localRotation = arcPrefab.wing1.transform.localRotation;
            arc.wing2.transform.localRotation = arcPrefab.wing2.transform.localRotation;
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





    public void moveGrid(Vector2Int direction)
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
        playing.value = false;
      //  Time.timeScale = 0;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        currentScore.value = 0;

    }

    public void AddScore()
    {
        currentScore.value++;

        if (currentScore.value % 5 == 0)
        {
            if (arcShrinkTime.value > 5)
            {

                arcShrinkTime.value--;
            }
            if (player.transform.localScale.x < 1.8)
            {
                player.transform.localScale *= 1.08f;
            }
        }
        UIManager.instance.UpdateScoreUI();
        if (currentScore.value > highScore.value)
            highScore.value = currentScore.value;
    }



}
