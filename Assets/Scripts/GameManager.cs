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

    public ArcController arcPrefab;
    public GameObject coinPrefab;

    public Transform player;
    UIManager ui;

    [Header("Modifiable Values")]
    public Vector2 swipeSensitivity;
    public float arcCloseTime;
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

    public int currentScore;
    public int highScore;

    public int currentCoins;
    public bool playing;
    public bool moving;

    public bool canSwipe;


    void Update()
    {

        if (playing)
        {
            SwipeMovement();
            KeyboardControls();
            Rotator();
        }
    }



    public void InitValues()
    {
        ui = UIManager.instance;
        canSwipe = true;
        pool = new Queue<ArcController>();
        arcs = new List<ArcController>();
        currentCoins = 0;
        currentScore = 0;
        highScore = PlayerPrefs.GetInt("Highscore");
        arcCloseTime = 10;
        SpawnFirstArcs();
        playing = true;
        //StartCoroutine(DelayedInitializer());

        swipeSensitivity = new Vector2(Screen.width*0.1f,Screen.height*0.1f);
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



    public void KeyboardControls()
    {
        if (Input.anyKeyDown && !moving)
        {
            float XAxis = Input.GetAxisRaw("Horizontal");
            float YAxis = Input.GetAxisRaw("Vertical");

            if (XAxis != 0)
            {
                StartCoroutine(MovePlayer(Vector3.right*XAxis));
            }
            else if (YAxis != 0)
            {
                Vector3 dir = Vector3.up*YAxis;
                StartCoroutine(MovePlayer(Vector3.up*YAxis));
            }
        }

    }

   


    public void SwipeMovement()
    {
        if(Input.touchCount==1&&canSwipe)
        {
            Touch touch = Input.GetTouch(0);
            if(Mathf.Abs(touch.deltaPosition.x)>Mathf.Abs(touch.deltaPosition.y))//vertical swipe
            {
                if(touch.deltaPosition.x > swipeSensitivity.x)
                {
                    StartCoroutine(MovePlayer(Vector3.right));
                }
                else if(touch.deltaPosition.x < -swipeSensitivity.x)
                {
                    StartCoroutine(MovePlayer(Vector3.left));                    
                }
                else
                {
                    return;
                }
            }
            else//horizontal swipe
            {
                if(touch.deltaPosition.y > swipeSensitivity.y)
                {
                    StartCoroutine(MovePlayer(Vector3.up));
                }
                else if(touch.deltaPosition.y < -swipeSensitivity.y)
                {
                    StartCoroutine(MovePlayer(Vector3.down));                    
                }
                else
                {
                    return;
                }
                
            }
        }

    }

    public IEnumerator DelayedInitializer()
    {
        yield return new WaitForSeconds(0.05f);
        playing = true;
    }



    void Rotator()
    {
        foreach (ArcController arc in arcs)
        {
            arc.Rotate();
        }


    }



    public IEnumerator MovePlayer(Vector3 dir)
    {
        if (moving||!playing)
        {
            canSwipe = true;
            yield break;
        }
        else
        {
            AddScore();
            canSwipe = false;
            moving = true;
            Vector3 startpos = player.position;
            Vector3 endpos = player.position + dir;
            float lerpTime=0;
            while (player.position != endpos && playing)
            {
               // player.position = Vector3.MoveTowards(player.position, dest, Time.deltaTime * playerSpeed);
               
                lerpTime+= Time.deltaTime;
                player.position = Vector3.Lerp(startpos, endpos, lerpTime*playerSpeed);
                yield return null;
            }

            MoveGrid(dir,endpos);
            ClearArray();
            canSwipe = true;
            moving = false;
        }
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





    public void MoveGrid(Vector3 direction,Vector3 destination)
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
        playing = false;
    }

    public void RestartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        currentScore = 0;

    }

    public void AddScore()
    {
        currentScore++;

        if (currentScore % 5 == 0)
        {
            if (arcCloseTime > 5)
            {

                arcCloseTime--;
            }
            if (player.transform.localScale.x < 1.8)
            {
                player.transform.localScale *= 1.08f;
            }
        }
        UIManager.instance.UpdateScoreUI();
        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt("Highscore", currentScore);
            highScore = currentScore;
        }
    }

    public void AddCoins()
    {
        currentCoins++;
        UIManager.instance.UpdateScoreUI();

    }



}
