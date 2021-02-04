using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DataInfo;

[System.Serializable]
public class RootList
{
    public List<Root> rootList = new List<Root>();
}
public class LakiaroManager : MonoBehaviour
{
    bool isDailyGame = false;

    [Header("Lakiaro Hard Root")]
    public RootDataInfo rootDataInfo;

    [Header("Lakiaro Tilemap")]
    public Tilemap lakiaroFlowerTileMap;
    public Tilemap lakiaroRootTileMap;
    public Tilemap lakiaroDirtTileMap_Lower;
    public Tilemap lakiaroDirtTilemap_Upper;

    [SerializeField] // 라키아로 게임 데이터
    private List<RootList> rootLists = new List<RootList>();
    public Lakiaro[,] lakiaroRoot = new Lakiaro[12, 12];
    [SerializeField]
    private int lakiaroLevel, currLakiaroLevel = 0, manosHoeLevel, currRemainTryTime;
    public float progress = 100f;

    private bool gameResult = true;

    public Queue<Dirt> dirtPool = new Queue<Dirt>();
    public Queue<Root> rootPool = new Queue<Root>();
    public Queue<Pebble> pebblePool = new Queue<Pebble>();

    [SerializeField]
    private int currRemainDirt = 128, currRemainRoot = 0, currRemainPebble = 0;
    public TileBase temp;

    public bool gamePause = true;

    [Header("Lakiaro Flower TileBase")]
    public List<TileBase> lakiaro_Flower_TileBase = new List<TileBase>();


    [Header("Lakiaro Dirt TileBase")]
    public List<List<TileBase>> basicTile = new List<List<TileBase>>();
    public List<TileBase> basicTile1 = new List<TileBase>();
    public List<TileBase> basicTile2 = new List<TileBase>();
    public List<TileBase> basicTile3 = new List<TileBase>();
    public List<TileBase> basicTile4 = new List<TileBase>();
    public List<TileBase> basicTile5 = new List<TileBase>();
    public List<TileBase> basicTile6 = new List<TileBase>();
    public List<TileBase> pebbleTile = new List<TileBase>();
    public InGame_UI inGame_UI;

    [Header("Lakiaro Root TileBase")]
    public TileBase[] right_Upper; //오른쪽_위쪽,
    public TileBase[] right_Left;//오른쪽_왼쪽,
    public TileBase[] right_Lower;//오른쪽_아래쪽,
    public TileBase[] upper_Left;//위쪽_왼쪽,
    public TileBase[] upper_Lower;//위쪽_아래쪽,
    public TileBase[] upper_Right;//위쪽_오른쪽,
    public TileBase[] left_Lower;//왼쪽_아래쪽,
    public TileBase[] left_Right;//왼쪽_오른쪽,
    public TileBase[] left_Upper;//왼쪽_위쪽,
    public TileBase[] lower_Right;//아래쪽_오른쪽,
    public TileBase[] lower_Left;//아래쪽_위쪽,
    public TileBase[] lower_Upper;//아래쪽_왼쪽
    public TileBase[] end;//아래쪽_왼쪽


    // Start is called before the first frame update
    void Start()
    {
        basicTile.Add(basicTile1);
        basicTile.Add(basicTile2);
        basicTile.Add(basicTile3);
        basicTile.Add(basicTile4);
        basicTile.Add(basicTile5);
        basicTile.Add(basicTile6);

        min = Camera.main.WorldToViewportPoint(Vector3.zero);
        max = Camera.main.WorldToViewportPoint(Vector3.one * 11);
    }

    void dataCheck()
    {
        for (int i = 0; i < rootDataInfo.up_RootDataList.Count; i++)
        {
            Debug.Log((i + 1) + "번째 뿌리");
            string s = "";
            for (int j = 0; j < rootDataInfo.up_RootDataList[i].third.Count; j++)
            {
                s += rootDataInfo.up_RootDataList[i].third[j].ToString() + ", ";
            }
            Debug.Log("Third(" + rootDataInfo.up_RootDataList[i].third.Count + ") : " + s);
            s = "";
            for (int j = 0; j < rootDataInfo.up_RootDataList[i].fourth.Count; j++)
            {
                s += rootDataInfo.up_RootDataList[i].fourth[j].ToString() + ", ";
            }
            Debug.Log("Fourth(" + rootDataInfo.up_RootDataList[i].fourth.Count + ") : " + s);
            s = "";
            for (int j = 0; j < rootDataInfo.up_RootDataList[i].fifth.Count; j++)
            {
                s += rootDataInfo.up_RootDataList[i].fifth[j].ToString() + ", ";
            }
            Debug.Log("Fifth(" + rootDataInfo.up_RootDataList[i].fifth.Count + ") : " + s);
        }
    }

    void Awake()
    {
        FirstLoading();
    }

    void FirstLoading() // 오브젝트 풀 생성 및, 배열에 Dirt 배정
    {
        for (int i = 0; i < 128; i++)
        {
            dirtPool.Enqueue(new Dirt());
            rootPool.Enqueue(new Root());
            pebblePool.Enqueue(new Pebble());
        }

        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                if (i < 8 && i > 3 && j > 3 && j < 8) lakiaroRoot[i, j] = new Flower();
                else lakiaroRoot[i, j] = dirtPool.Dequeue();
            }
        }
        SetCam();
    }

    float maxCamSzie;
    void SetCam()
    {
        Vector3 min, max;
        bool finish = false;

        while (!finish)
        {
            Camera.main.orthographicSize += 0.01f;
            min = Camera.main.WorldToViewportPoint(Vector3.zero);
            max = Camera.main.WorldToViewportPoint(Vector3.one * 12);

            if (min.x > 0f && max.x < 1f) finish = true;
        }
        maxCamSzie = Camera.main.orthographicSize;
    }

    /*public void test()
    {
        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                if (lakiaroRoot[i, j] == null) continue;
                vector3Int.x = i;
                vector3Int.y = j;
                lakiaroRoot[vector3Int.x, vector3Int.y].isChecked = true;
                lakiaroDirtTilemap_Upper.SetTile(vector3Int, null);
                if (lakiaroRoot[i, j].type == Lakiaro.Type.Root)
                {
                    currRemainRoot--;
                }
                else if (lakiaroRoot[i, j].type == Lakiaro.Type.Pebble)
                {
                    currRemainPebble--;
                }
                else
                {
                    currRemainDirt--;
                }
            }
        }
        UIManager.Instance.CallMainUI();
        InitGame();
        gamePause = true;
    }*/

    Vector3 mousePosition;

    float oldTouchDis = 0;
    Vector2 oldTouchPos;
    float cameraSize = 11f;

    bool touchs = false;
    bool moved = false;
    Vector3 min, max;
    Vector3 vtowMin, vtowMax, cen;
    float xSize, ySize;
    Vector3 initPos = new Vector3(6f, 7f, -10f);
    // Update is called once per frame
    void Update()
    {
        if (gamePause)
        {
            return;
        }
        else
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount == 1)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        SetCamMoveBox();
                        oldTouchPos = Input.GetTouch(0).position;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        Vector3 cPos = oldTouchPos - Input.GetTouch(0).position;
                        if (cPos.sqrMagnitude > 100)
                        {
                            moved = true;

                            MoveCam(cPos, Input.GetTouch(0).position);

                            oldTouchPos = Input.GetTouch(0).position;
                        }
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        if (touchs)
                        {
                            touchs = false;

                            oldTouchPos = Input.GetTouch(0).position;
                        }
                        else if (moved) moved = false;
                        else
                        {
                            if (inGame_UI.currDig == 0)
                            {
                                if (currRemainTryTime != 0)
                                {
                                    mousePosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                                    SwallowlyDig(mousePosition);
                                }
                            }
                            else
                            {
                                mousePosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                                DeeplyDig(mousePosition);
                            }
                        }
                    }
                }
                else if (Input.touchCount == 2)
                {
                    touchs = true;
                    if (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved)
                    {
                        float touchDis = (Input.touches[0].position - Input.touches[1].position).sqrMagnitude;
                        float fDis = (touchDis - oldTouchDis) * 0.00001f;
                        cameraSize -= fDis;
                        cameraSize = Mathf.Clamp(cameraSize, 4f, maxCamSzie);
                        Camera.main.orthographicSize = cameraSize;//Mathf.Lerp(Camera.main.orthographicSize, cameraSize, Time.deltaTime);
                        oldTouchDis = touchDis;
                        
                        SetCamMoveBox();

                        if (Camera.main.orthographicSize == maxCamSzie)
                        {
                            Camera.main.transform.position = initPos;
                        }
                        else
                        {
                            Vector3 pos = Camera.main.transform.position;

                            pos.x = Mathf.Clamp(pos.x, minCamBox.x, maxCamBox.x);
                            pos.y = Mathf.Clamp(pos.y, minCamBox.y, maxCamBox.y);

                            Camera.main.transform.position = pos;
                        }
                    }
                }
            }
            else
            {
                if (Input.GetMouseButtonUp(0))
                {
                    if (inGame_UI.currDig == 0)
                    {
                        if (currRemainTryTime != 0)
                        {
                            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                            SwallowlyDig(mousePosition);
                        }
                    }
                    else
                    {
                        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                        DeeplyDig(mousePosition);
                    }
                }
                if (Input.GetMouseButtonDown(1))
                {
                    SetCamMoveBox();
                    oldTouchPos = Input.mousePosition;
                }
                if (Input.GetMouseButton(1))
                {
                    Vector3 cPos = (Vector3)oldTouchPos - Input.mousePosition;

                    if (cPos.sqrMagnitude > 100)
                    {
                        MoveCam(cPos, Input.mousePosition);
                    }
                }
            }
        }

    }

    Vector2 minCamBox = Vector2.zero, maxCamBox = Vector2.one, centerBox = Vector2.zero;
    void SetCamMoveBox()
    {
        centerBox = Camera.main.ViewportToWorldPoint((inGame_UI.viewMax + inGame_UI.viewMin) / 2f);
        float width, height, offset;
        width = Camera.main.ViewportToWorldPoint(inGame_UI.viewMax).x - Camera.main.ViewportToWorldPoint(inGame_UI.viewMin).x;
        height = Camera.main.ViewportToWorldPoint(inGame_UI.viewMax).y - Camera.main.ViewportToWorldPoint(inGame_UI.viewMin).y;

        offset = Camera.main.orthographicSize / maxCamSzie;

        minCamBox.x = width / 2f; minCamBox.y = height / 2f + offset;
        maxCamBox.x = 12f - width / 2f; maxCamBox.y = 12f - height / 2f + offset;

        Debug.LogWarning("MinCamBox : " + minCamBox + " MaxCamBox : " + offset);
    }

    void MoveCam(Vector3 cPos, Vector2 mousePos)
    {
        Vector3 pos = Camera.main.transform.position;
        pos += cPos * 0.01f;
        pos.x = Mathf.Clamp(pos.x, minCamBox.x, maxCamBox.x);
        pos.y = Mathf.Clamp(pos.y, minCamBox.y, maxCamBox.y);
        
        Camera.main.transform.position = pos;

        oldTouchPos = mousePos;
    }

    Vector3Int vector3Int = new Vector3Int();
    public bool SwallowlyDig(Vector2 point)
    {
        Debug.Log(point);
        if (0 > point.x || point.x > 12 || 0 > point.y || point.y > 12) return false;
        if (lakiaroRoot[(int)point.x, (int)point.y].isChecked) return false; // 이미 확인한 곳이면 스킾

        bool isRoot = false;

        GameManager.Instance.audioManager.CallAudioClip(2);
        if (lakiaroRoot[(int)point.x, (int)point.y] != null)
        {
            if (lakiaroRoot[(int)point.x, (int)point.y].type == Lakiaro.Type.Root)
            {
                isRoot = true;
                currRemainRoot--;

                vector3Int.x = (int)point.x;
                vector3Int.y = (int)point.y;
                lakiaroDirtTilemap_Upper.SetTile(vector3Int, null);
                lakiaroRoot[vector3Int.x, vector3Int.y].isChecked = true;
                Debug.Log((int)mousePosition.x + ", " + (int)mousePosition.y + " 는 뿌리이다.");

                inGame_UI.UpdateRemainLakiaroText(1 ,currRemainRoot);
            }
            else if (lakiaroRoot[(int)point.x, (int)point.y].type == Lakiaro.Type.Pebble)
            {
                Debug.Log((int)mousePosition.x + ", " + (int)mousePosition.y + " 는 자갈이다.");
            }

            if (lakiaroRoot[(int)point.x, (int)point.y].type != Lakiaro.Type.Flower) currRemainTryTime--;
        }
        else
        {
            Debug.Log((int)mousePosition.x + ", " + (int)mousePosition.y + " 는 흙이다.");
        }
        
        inGame_UI.UpdateRemainTryTime(currRemainTryTime);

        return isRoot;
    }

    public bool DeeplyDig(Vector2 point)
    {
        if (0 > point.x || point.x > 12 || 0 > point.y || point.y > 12) return false;
        if (lakiaroRoot[(int)point.x, (int)point.y].isChecked) return false; // 이미 확인한 곳이면 스킾

        bool isDirt = false;

        int manos = 9, currCheck = 0;

        switch (manosHoeLevel)
        {
            case 1:
                manos = 6;
                break;
            case 2:
                manos = 7;
                break;
            case 3:
                manos = 8;
                break;
            case 4:
                manos = 9;
                break;
            default:
                manos = 5;
                break;
        }

        GameManager.Instance.audioManager.CallAudioClip(0);
        bool dirt = false, root = false, pebble = false;
        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                vector3Int.x = (int)point.x + x;
                vector3Int.y = (int)point.y + y;

                if (0 > vector3Int.x || vector3Int.x > 11 || 0 > vector3Int.y || vector3Int.y > 11) continue;
                if (lakiaroRoot[vector3Int.x, vector3Int.y] == null) continue;
                if (lakiaroRoot[vector3Int.x, vector3Int.y].isChecked) continue;

                if (lakiaroRoot[vector3Int.x, vector3Int.y].type == Lakiaro.Type.Dirt)
                {
                    currCheck++;
                    currRemainDirt--;
                    lakiaroDirtTilemap_Upper.SetTile(vector3Int, null);

                    lakiaroRoot[vector3Int.x, vector3Int.y].isChecked = true;

                    dirt = true;
                }
                else
                {
                    if (x == 0 && y == 0)
                    {
                        currCheck++;
                        if (lakiaroRoot[vector3Int.x, vector3Int.y].type == Lakiaro.Type.Pebble)
                        {
                            currRemainPebble--;
                            pebble = true;
                        }
                        else if (lakiaroRoot[vector3Int.x, vector3Int.y].type == Lakiaro.Type.Root)
                        {
                            DigRoot(vector3Int);
                            (lakiaroRoot[vector3Int.x, vector3Int.y] as Root).IsDameaed = true;
                            currRemainRoot--;
                            root = true;
                        }

                        lakiaroDirtTilemap_Upper.SetTile(vector3Int, null);
                        lakiaroRoot[vector3Int.x, vector3Int.y].isChecked = true;
                    }
                }

                if (manos == currCheck) break;
            }
        }

        if (dirt) inGame_UI.UpdateRemainLakiaroText(0, currRemainDirt);
        if (root) inGame_UI.UpdateRemainLakiaroText(1, currRemainRoot);
        if (pebble) inGame_UI.UpdateRemainLakiaroText(2, currRemainPebble);

        if (currRemainDirt == 0)
        {
            if(lakiaroLevel == currLakiaroLevel)
            {
                StartCoroutine(FinishDig(currLakiaroLevel, manosHoeLevel, true));
            }
            else
            {
                currLakiaroLevel++;
                StartCoroutine(FinishDig(currLakiaroLevel, manosHoeLevel, false));
            }
        }

        return isDirt;
    }

    public void StartGame(int _lakiaroLevel, int _manosHoeLevel, bool isLoad, bool _isDailyGame)
    {
        lakiaroLevel = _lakiaroLevel;
        manosHoeLevel = _manosHoeLevel;
        currLakiaroLevel = 0; progress = 100f;
        isDailyGame = _isDailyGame;
        if (isLoad)
        {
            if (_isDailyGame)
            {
                LoadGameDailyData();
            }
            else
            {
                LoadGameData();
            }
        }
        else
        {
            switch (manosHoeLevel)
            {
                case 0:
                    currRemainTryTime = 18;
                    break;
                case 1:
                    currRemainTryTime = 20;
                    break;
                case 2:
                    currRemainTryTime = 22;
                    break;
                case 3:
                    currRemainTryTime = 25;
                    break;
                case 4:
                    currRemainTryTime = 28;
                    break;
            }
            GenerateLakiaro(0);
        }

        inGame_UI.UpdateRemainTexts(currRemainDirt, currRemainRoot, currRemainPebble, currRemainTryTime, currLakiaroLevel, lakiaroLevel, progress);

        gamePause = false;
    }

    void NextGame(int nextLevel, int _manosHoeLevel)
    {
        switch (manosHoeLevel)
        {
            case 0:
                currRemainTryTime = 18;
                break;
            case 1:
                currRemainTryTime = 20;
                break;
            case 2:
                currRemainTryTime = 22;
                break;
            case 3:
                currRemainTryTime = 25;
                break;
            case 4:
                currRemainTryTime = 28;
                break;
        }

        GenerateLakiaro(nextLevel);

        inGame_UI.UpdateRemainTexts(currRemainDirt, currRemainRoot, currRemainPebble, currRemainTryTime, currLakiaroLevel, lakiaroLevel, progress);
    }

    IEnumerator FinishDig(int nextLevel, int _manosHoeLevel, bool lastRound)
    {
        switch (manosHoeLevel)
        {
            case 0:
                currRemainTryTime = 18;
                break;
            case 1:
                currRemainTryTime = 20;
                break;
            case 2:
                currRemainTryTime = 22;
                break;
            case 3:
                currRemainTryTime = 25;
                break;
            case 4:

           currRemainTryTime = 28;
                break;
        }

        float timer = 0f;

        /*if (!lastRound)
        {
            inGame_UI.EnableRound();

            lakiaroDirtTilemap_Upper.ClearAllTiles();

            while (timer < 3f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            inGame_UI.DisableRound();
        }*/

        if (lastRound)
        {
            gamePause = true;
            inGame_UI.OnResultUI(gameResult);
            /*GameManager.Instance.FinishDigLakiaro(lakiaroLevel, progress, true);
            UIManager.Instance.CallLobbyUI();
            UIManager.Instance.lobby_UI.GetComponent<Lobby_UI>().DigFinishLakiaro();
            gamePause = true;*/
        }
        else
        {
            inGame_UI.EnableRound();

            lakiaroDirtTilemap_Upper.ClearAllTiles();

            while (timer < 3f)
            {
                yield return null;
                timer += Time.deltaTime;
            }
            inGame_UI.DisableRound();
            
            InitGame();

            GenerateLakiaro(nextLevel);

            inGame_UI.UpdateRemainTexts(currRemainDirt, currRemainRoot, currRemainPebble, currRemainTryTime, currLakiaroLevel, lakiaroLevel, progress);
        }
    }

    public void ReturnToLobby()
    {
        UIManager.Instance.CallMainUI();
        UIManager.Instance.lobby_UI.GetComponent<NewGame_UI>().DigFinishLakiaro(isDailyGame);
        GameManager.Instance.FinishDigLakiaro(lakiaroLevel, manosHoeLevel, progress, gameResult, isDailyGame);
    }

    void SetDamagedRootTileBase(Vector3Int pos)
    {
        if (lakiaroRoot[pos.x, pos.y].type != Lakiaro.Type.Root) return;
        TileBase[] tileBaseList;

        if ((lakiaroRoot[pos.x, pos.y] as Root).rootState == 3)
        {
            switch ((lakiaroRoot[pos.x, pos.y] as Root).GetDirection)
            {
                case Root.Direction.아래쪽_위쪽:
                    lakiaroRootTileMap.SetTile(pos, end[6]);
                    break;
                case Root.Direction.왼쪽_오른쪽:
                    lakiaroRootTileMap.SetTile(pos, end[4]);
                    break;
                case Root.Direction.위쪽_아래쪽:
                    lakiaroRootTileMap.SetTile(pos, end[7]);
                    break;
                case Root.Direction.오른쪽_왼쪽:
                    lakiaroRootTileMap.SetTile(pos, end[5]);
                    break;
            }
        }
        else
        {
            switch ((lakiaroRoot[pos.x, pos.y] as Root).GetDirection)
            {
                case Root.Direction.아래쪽_오른쪽:
                    tileBaseList = lower_Right;
                    break;
                case Root.Direction.아래쪽_위쪽:
                    tileBaseList = lower_Upper;
                    break;
                case Root.Direction.아래쪽_왼쪽:
                    tileBaseList = lower_Left;
                    break;
                case Root.Direction.왼쪽_위쪽:
                    tileBaseList = left_Upper;
                    break;
                case Root.Direction.왼쪽_오른쪽:
                    tileBaseList = left_Right;
                    break;
                case Root.Direction.왼쪽_아래쪽:
                    tileBaseList = left_Lower;
                    break;
                case Root.Direction.위쪽_오른쪽:
                    tileBaseList = upper_Right;
                    break;
                case Root.Direction.위쪽_아래쪽:
                    tileBaseList = upper_Lower;
                    break;
                case Root.Direction.위쪽_왼쪽:
                    tileBaseList = upper_Left;
                    break;
                case Root.Direction.오른쪽_아래쪽:
                    tileBaseList = right_Lower;
                    break;
                case Root.Direction.오른쪽_왼쪽:
                    tileBaseList = right_Left;
                    break;
                case Root.Direction.오른쪽_위쪽:
                    tileBaseList = right_Upper;
                    break;
                default:
                    tileBaseList = right_Left;
                    break;
            }
            if ((lakiaroRoot[pos.x, pos.y] as Root).rootState == 0) lakiaroRootTileMap.SetTile(pos, tileBaseList[3]);
            else if ((lakiaroRoot[pos.x, pos.y] as Root).rootState == 1) lakiaroRootTileMap.SetTile(pos, tileBaseList[4]);
            else if ((lakiaroRoot[pos.x, pos.y] as Root).rootState == 2) lakiaroRootTileMap.SetTile(pos, tileBaseList[5]);
        }
    }

    void DigRoot(Vector3Int pos)
    {
        if (lakiaroRoot[pos.x, pos.y].type != Lakiaro.Type.Root) return;
        SetDamagedRootTileBase(pos);
        float rootCount = 0;
        for(int i = 0; i < rootLists.Count; i++)
        {
            rootCount += rootLists[i].rootList.Count;
        }
        
        float damp = 0;
        progress -= 1.14f;
        progress = Mathf.Clamp(progress, 0, 100);
        inGame_UI.UpdateProgress(progress, lakiaroLevel);

        if (progress == 0)
        {
            gameResult = false;
            gamePause = true;
            inGame_UI.OnResultUI(gameResult);
        }
    }

    public void GenerateLakiaro(int level = 0)
    {
        GenerateDirt(level); // 단계에 맞게 흙 설정
        GenerateRoot(); // 뿌리 생성
        GeneratePebble(level); // 뿌리가 안지나가는 공간에 랜덤으로 자갈 생성
        SetFlowerTile(level);
        for (int i = 0; i < rootLists.Count; i++)
        {
            currRemainRoot += rootLists[i].rootList.Count;
            currRemainDirt -= rootLists[i].rootList.Count;
        }
    }

    public void SetFlowerTile(int _currLevel)
    {
        int index = 0;

        switch (lakiaroLevel)
        {
            case 1:
                index = 1;
                break;
            case 2:
                index = 3;
                break;
            case 3:
                index = 6;
                break;
            case 4:
                index = 10;
                break;
            default:
                index = 0;
                break;
        }
        Debug.LogWarning(index.ToString() + " "  + currLakiaroLevel.ToString());

        lakiaroFlowerTileMap.SetTile(new Vector3Int(4,7,0), lakiaro_Flower_TileBase[index + currLakiaroLevel]);
    }

    public void GenerateDirt(int level = 0)
    {
        /* 처음 단계일 경우 잔디 밭 생성 이후로는 아래 흙을 위로 바꿔줌
         * 아래 흙은 매 단계 레벨에 맞는 흙 랜덤 생성
         */
        if (level == 0)
        {
            for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
            {
                for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
                {
                    if (i < 8 && i > 3 && j > 3 && j < 8) continue;
                    vector3Int.x = i;
                    vector3Int.y = j;
                    lakiaroDirtTilemap_Upper.SetTile(vector3Int, basicTile[0][Random.Range(0, basicTile.Count)]);
                }
            }
        }
        else
        {

            for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
            {
                for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
                {
                    if (i < 8 && i > 3 && j > 3 && j < 8) continue;
                    vector3Int.x = i;
                    vector3Int.y = j;
                    lakiaroDirtTilemap_Upper.SetTile(vector3Int, lakiaroDirtTileMap_Lower.GetTile(vector3Int));
                    Debug.Log(lakiaroDirtTileMap_Lower.GetTile(vector3Int).name + " " + lakiaroDirtTilemap_Upper.GetTile(vector3Int).name);
                }
            }
        }

        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                if (i < 8 && i > 3 && j > 3 && j < 8) continue;
                
                vector3Int.x = i;
                vector3Int.y = j;
                lakiaroDirtTileMap_Lower.SetTile(vector3Int, basicTile[level + 1][Random.Range(0, basicTile2.Count)]);
                Debug.Log(lakiaroDirtTileMap_Lower.GetTile(vector3Int).name + " " + lakiaroDirtTilemap_Upper.GetTile(vector3Int).name);
            }
        }
    }

    Dictionary<int, Vector3Int> startPos = new Dictionary<int, Vector3Int>
        {
            {0, new Vector3Int(4,8,0)},{1, new Vector3Int(5,8,0)},{2, new Vector3Int(6,8,0)},{3, new Vector3Int(7,8,0)},
            {4, new Vector3Int(8,7,0)},{5, new Vector3Int(8,6,0)},{6, new Vector3Int(8,5,0)},{7, new Vector3Int(8,4,0)},
            {8, new Vector3Int(7,3,0)},{9, new Vector3Int(6,3,0)},{10, new Vector3Int(5,3,0)},{11, new Vector3Int(4,3,0)},
            {12, new Vector3Int(3,4,0)},{13, new Vector3Int(3,5,0)},{14, new Vector3Int(3,6,0)},{15, new Vector3Int(3,7,0)},
        }; // 시작 지점 위치 시계방향순

    public void GenerateRoot()
    {

        int currRootCount = 0, rootCount = Random.Range(5, 9); // 생성될 뿌리 5~9개 랜덤

        Debug.Log("rootCount : " + rootCount);
        List<int> checkedStartPosList = new List<int>();
        
        while (currRootCount != rootCount)
        {
            int startPosIndex = Random.Range(0, 16); // 상단 좌측 모서리를 기준으로 시계방향

            if (checkedStartPosList.Count >= 16)
            {
                Debug.LogWarning("뿌리를 전부다 생성 실패");
                break;
            }

            if (checkedStartPosList.Contains(startPosIndex)) continue; // 이미 뿌리가 있을 경우 다시 뽑음
            else checkedStartPosList.Add(startPosIndex);
            

            Debug.Log(startPosIndex + " " + startPos[startPosIndex] + " " + startPosIndex / 4);
            if (!CheckCanMakeThickRoot(startPos[startPosIndex], startPosIndex / 4))
            {
                Debug.LogWarning(startPos[startPosIndex].ToString() + currRootCount + " 굵은 뿌리 생성에서 에러가 발생");
            }
            else  currRootCount++;
        }

        for(int i = 0; i < rootLists.Count; i++)
        {
            CheckcanMakeThinRoot(rootLists[i]);
        }
        for (int i = 0; i < rootLists.Count; i++)
        {
            SettingDir(rootLists[i]);
        }
        for (int i = 0; i < rootLists.Count; i++)
        {
            for(int j =  0; j < rootLists[i].rootList.Count; j++)
            {
                Debug.Log(i + "." + j + "번째 뿌리 :" + rootLists[i].rootList[j].GetCurrRoot() + " : " + rootLists[i].rootList[j].rootState + " : " + rootLists[i].rootList[j].GetDirection);
            }
        }

        for (int i = 0; i < rootLists.Count; i++)
        {
            SettingRootTileBase(rootLists[i]);
        }
    }

    List<int> tempIndexList = new List<int>();
    bool CheckCanMakeThickRoot(Vector3Int checkPoint, int dir) // 해당 포인트에 들어 갈 수 있는 뿌리 확인
    {
        // 1~24형태의 뿌리중 가능한 뿌리 인덱스를 출력
        // RootData의 각 뿌리 위치를 하나씩 대조하여 가능한 뿌리를 찾음
        // Third, Fourth, Fifth => 각 뿌리의 세번째 네번째 다섯번째 뿌리가 안될경우 안되는 다른 뿌리 형태의 인덱스 리스트 
        bool canMake = false;

        if (lakiaroRoot[checkPoint.x, checkPoint.y].type != Lakiaro.Type.Dirt) return canMake;

        int randomIndex = 0, x = 0, y = 0, ranSixth = 0;
        List<int> sixth = new List<int>();
        while (!canMake)
        {
            if (tempIndexList.Count >= 24) break;

            randomIndex = Random.Range(0, 24);

            if (tempIndexList.Contains(randomIndex + 1)) continue;

            bool cantMake = false;

            for (int i = 1; i < 5; i++)
            {
                switch (dir)
                {
                    case 0: // up
                        x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i].x;
                        y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i].y;
                        break;
                    case 1:// right
                        x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i].y;
                        y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i].x;
                        break;
                    case 2:// down
                        x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i].x;
                        y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i].y;
                        break;
                    case 3:// left
                        x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i].y;
                        y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i].x;
                        break;
                }

                if (lakiaroRoot[x, y].type != Lakiaro.Type.Dirt)
                {

                    Debug.LogWarning("["+x+","+ y + "] "+ + (randomIndex + 1) + "형태의 "+ (i + 1) + "번째 뿌리 위치");

                    cantMake = true;
                    if (i == 1)
                    {
                        return canMake;
                    }
                    else if (i == 2)
                    {
                        tempIndexList.Add(randomIndex + 1);
                        for (int j = 0; j < rootDataInfo.up_RootDataList[randomIndex].third.Count; j++)
                        {
                            if (!tempIndexList.Contains(rootDataInfo.up_RootDataList[randomIndex].third[j])) tempIndexList.Add(rootDataInfo.up_RootDataList[randomIndex].third[j]);
                        }
                        break;
                    }
                    else if (i == 3)
                    {
                        tempIndexList.Add(randomIndex + 1);
                        for (int j = 0; j < rootDataInfo.up_RootDataList[randomIndex].fourth.Count; j++)
                        {
                            if (!tempIndexList.Contains(rootDataInfo.up_RootDataList[randomIndex].fourth[j])) tempIndexList.Add(rootDataInfo.up_RootDataList[randomIndex].fourth[j]);
                        }
                        break;
                    }
                    else if (i == 4)
                    {
                        tempIndexList.Add(randomIndex + 1);
                        for (int j = 0; j < rootDataInfo.up_RootDataList[randomIndex].fifth.Count; j++)
                        {
                            if (!tempIndexList.Contains(rootDataInfo.up_RootDataList[randomIndex].fifth[j])) tempIndexList.Add(rootDataInfo.up_RootDataList[randomIndex].fifth[j]);
                        }
                        break;
                    }
                }

                cantMake = false;
            }
            if (cantMake)
            {
                Debug.LogWarning("3~5 번째 뿌리에서 뿌리 발견 생성 불가");
                continue;
            }

            sixth.Clear();
            // 6번째 뿌리 확인
            for (int i = 0; i < rootDataInfo.up_RootDataList[randomIndex].lastRootList.Count; i++)
            {
                switch (dir)
                {
                    case 0: // up
                        x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].lastRootList[i].x;
                        y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].lastRootList[i].y;
                        break;
                    case 1:// right
                        x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].lastRootList[i].y;
                        y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].lastRootList[i].x;
                        break;
                    case 2:// down
                        x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].lastRootList[i].x;
                        y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].lastRootList[i].y;
                        break;
                    case 3:// left
                        x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].lastRootList[i].y;
                        y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].lastRootList[i].x;
                        break;
                }
                if (lakiaroRoot[x, y].type == Lakiaro.Type.Dirt)
                {
                    Debug.Log(x + " " + y + "가능");
                    sixth.Add(i);
                }
                else
                {
                    Debug.LogWarning(x + " " + y + "불가능");
                }
            }

            if (sixth.Count == 0)
            {
                Debug.Log("가능한 6번째 뿌리가 없음");
                continue;
            }
            else
            {
                ranSixth = sixth[Random.Range(0, sixth.Count)];
                canMake = true;
            }

            if(canMake) Debug.Log(randomIndex + "가능");
        }


        if (canMake)
        {
            Root root;
            RootList rootList = new RootList();

            for (int i = 0; i < 5; i++)
            {
                root = rootPool.Dequeue();
                rootList.rootList.Add(root);

                ConvertDir(dir, checkPoint, rootDataInfo.up_RootDataList[randomIndex].rootList[i]);
                root.SetCurrRoot(vector3Int);

                dirtPool.Enqueue(lakiaroRoot[vector3Int.x, vector3Int.y] as Dirt);
                lakiaroRoot[vector3Int.x, vector3Int.y] = root;

                if (i == 0)
                {
                    root.rootState = 0;
                    ConvertDir(dir, checkPoint, rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1]);
                    root.SetNextRoot(vector3Int);
                }
                else if (i == 4)
                {
                    root.rootState = 1;
                    
                    ConvertDir(dir, checkPoint, rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1]);
                    root.SetPreRoot(vector3Int);

                    ConvertDir(dir, checkPoint, rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth]);
                    root.SetNextRoot(vector3Int);
                }
                else
                {
                    root.rootState = 0;

                    ConvertDir(dir, checkPoint, rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1]);
                    root.SetPreRoot(vector3Int);

                    ConvertDir(dir, checkPoint, rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1]);
                    root.SetNextRoot(vector3Int);
                }
            }

            ConvertDir(dir, checkPoint, rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth]);
            root = rootPool.Dequeue();
            rootList.rootList.Add(root);
            root.SetCurrRoot(vector3Int);
            dirtPool.Enqueue(lakiaroRoot[vector3Int.x, vector3Int.y] as Dirt);
            lakiaroRoot[vector3Int.x, vector3Int.y] = root;

            root.rootState = 2;
            ConvertDir(dir, checkPoint, rootDataInfo.up_RootDataList[randomIndex].rootList[4]);
            root.SetPreRoot(vector3Int);

            rootLists.Add(rootList);
        }
        else
        {
            tempIndexList.Sort();
            string s = "";
            for(int i = 0; i < tempIndexList.Count; i++)
            {
                s += tempIndexList[i] + " ";
            }
            Debug.LogWarning(s + "확인이 끝남");
        }
        tempIndexList.Clear();

        return canMake;
    }

    bool CheckcanMakeThinRoot(RootList rootList) // 6번째 뿌리 이후 얇은 뿌리 생성
    {
        bool canMake = true, makeFinish = false;
        int index = 5;
        List<Root.Direction> dirList = new List<Root.Direction>();

        while (!makeFinish)
        {
            int dir = -1;
            switch (rootList.rootList[index].GetDirection)
            {
                case Root.Direction.아래쪽_위쪽:
                    dir = 0;
                    break;
                case Root.Direction.왼쪽_오른쪽:
                    dir = 1;
                    break;
                case Root.Direction.위쪽_아래쪽:
                    dir = 2;
                    break;
                case Root.Direction.오른쪽_왼쪽:
                    dir = 3;
                    break;
            }
            switch (rootList.rootList[index].GetDirection)
            {
                case Root.Direction.아래쪽_위쪽:
                case Root.Direction.왼쪽_위쪽:
                case Root.Direction.오른쪽_위쪽:
                    dir = 0;
                    break;
                case Root.Direction.위쪽_오른쪽:
                case Root.Direction.아래쪽_오른쪽:
                case Root.Direction.왼쪽_오른쪽:
                    dir = 1;
                    break;
                case Root.Direction.위쪽_아래쪽:
                case Root.Direction.왼쪽_아래쪽:
                case Root.Direction.오른쪽_아래쪽:
                    dir = 2;
                    break;
                case Root.Direction.위쪽_왼쪽:
                case Root.Direction.아래쪽_왼쪽:
                case Root.Direction.오른쪽_왼쪽:
                    dir = 3;
                    break;
            }

            if (dir == 0)
            {
                if (rootList.rootList[index].GetCurrRoot().x != 11)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x + 1, rootList.rootList[index].GetCurrRoot().y].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.아래쪽_오른쪽);
                }
                if (rootList.rootList[index].GetCurrRoot().x != 0)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x - 1, rootList.rootList[index].GetCurrRoot().y].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.아래쪽_왼쪽);
                }
                if (rootList.rootList[index].GetCurrRoot().y != 11)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x, rootList.rootList[index].GetCurrRoot().y + 1].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.아래쪽_위쪽);
                }
            }
            else if (dir == 1)
            {
                if (rootList.rootList[index].GetCurrRoot().x != 11)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x + 1, rootList.rootList[index].GetCurrRoot().y].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.왼쪽_오른쪽);
                }
                if (rootList.rootList[index].GetCurrRoot().y != 11)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x, rootList.rootList[index].GetCurrRoot().y + 1].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.왼쪽_위쪽);
                }
                if (rootList.rootList[index].GetCurrRoot().y != 0)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x, rootList.rootList[index].GetCurrRoot().y - 1].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.왼쪽_아래쪽);
                }
            }
            else if (dir == 2)
            {
                if (rootList.rootList[index].GetCurrRoot().x != 11)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x + 1, rootList.rootList[index].GetCurrRoot().y].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.위쪽_오른쪽);
                }
                if (rootList.rootList[index].GetCurrRoot().x != 0)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x - 1, rootList.rootList[index].GetCurrRoot().y].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.위쪽_왼쪽);
                }
                if (rootList.rootList[index].GetCurrRoot().y != 0)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x, rootList.rootList[index].GetCurrRoot().y - 1].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.위쪽_아래쪽);
                }
            }
            else if (dir == 3)
            {
                if (rootList.rootList[index].GetCurrRoot().x != 0)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x - 1, rootList.rootList[index].GetCurrRoot().y].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.오른쪽_왼쪽);
                }
                if (rootList.rootList[index].GetCurrRoot().y != 11)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x, rootList.rootList[index].GetCurrRoot().y + 1].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.오른쪽_위쪽);
                }
                if (rootList.rootList[index].GetCurrRoot().y != 0)
                {
                    if (lakiaroRoot[rootList.rootList[index].GetCurrRoot().x, rootList.rootList[index].GetCurrRoot().y - 1].type == Lakiaro.Type.Dirt) dirList.Add(Root.Direction.오른쪽_아래쪽);
                }
            }

            if(dirList.Count == 0)
            {
                rootList.rootList[index].rootState = 3;
                Debug.LogWarning("더이상 진행 불가능 마지막 뿌리 설정");
                break;
            }
            else
            {
                rootList.rootList[index].GetDirection = dirList[Random.Range(0, dirList.Count)];

                Root root = rootPool.Dequeue();

                switch (rootList.rootList[index].GetDirection)
                {
                    case Root.Direction.아래쪽_위쪽:
                    case Root.Direction.왼쪽_위쪽:
                    case Root.Direction.오른쪽_위쪽:
                        dir = 0;
                        break;
                    case Root.Direction.위쪽_오른쪽:
                    case Root.Direction.아래쪽_오른쪽:
                    case Root.Direction.왼쪽_오른쪽:
                        dir = 1;
                        break;
                    case Root.Direction.위쪽_아래쪽:
                    case Root.Direction.왼쪽_아래쪽:
                    case Root.Direction.오른쪽_아래쪽:
                        dir = 2;
                        break;
                    case Root.Direction.위쪽_왼쪽:
                    case Root.Direction.아래쪽_왼쪽:
                    case Root.Direction.오른쪽_왼쪽:
                        dir = 3;
                        break;
                }

                if (dir == 0)
                {
                    root.GetDirection = Root.Direction.아래쪽_위쪽;
                    root.SetCurrRoot(rootList.rootList[index].GetCurrRoot() + Vector3Int.up);
                }
                else if (dir == 1)
                {
                    root.GetDirection = Root.Direction.왼쪽_오른쪽;
                    root.SetCurrRoot(rootList.rootList[index].GetCurrRoot() + Vector3Int.right);
                }
                else if (dir == 2)
                {
                    root.GetDirection = Root.Direction.위쪽_아래쪽;
                    root.SetCurrRoot(rootList.rootList[index].GetCurrRoot() + Vector3Int.down);
                }
                else if (dir == 3)
                {
                    root.GetDirection = Root.Direction.오른쪽_왼쪽;
                    root.SetCurrRoot(rootList.rootList[index].GetCurrRoot() + Vector3Int.left);
                }
                else
                {
                    Debug.LogWarning(rootList.rootList[index].GetDirection + "마지막 뿌리 설정 에러");
                }

                root.SetPreRoot(rootList.rootList[index].GetCurrRoot());
                root.rootState = 2;
                rootList.rootList[index].SetNextRoot(root.GetCurrRoot());
                
                rootList.rootList.Add(root);
                dirtPool.Enqueue(lakiaroRoot[root.GetCurrRoot().x, root.GetCurrRoot().y] as Dirt);
                lakiaroRoot[root.GetCurrRoot().x, root.GetCurrRoot().y] = root;
                Debug.Log(index + " 번째 얇은뿌리 : " + rootList.rootList[index].GetCurrRoot() + " : " + rootList.rootList.Count);
            }

            if (Random.Range(0f, 1f) < 0.1f || index == 7)
            {
                Debug.Log("마지막 뿌리 설정 " + index);
                rootList.rootList[rootList.rootList.Count - 1].rootState = 3;
                break;
            }
            else
            {
                index++;
                dirList.Clear();
            }
        }

        for (int i = 0; i < rootList.rootList.Count; i++)
        {
            Debug.Log(i + " 번째 뿌리 : " + rootList.rootList[i].GetCurrRoot());
        }

        return canMake;
    }
    
    bool SettingDir(RootList rootList)
    {
        bool setting = true;

        Root currRoot, nextRoot;
        int tempX, tempY; 


        currRoot = lakiaroRoot[rootList.rootList[0].GetCurrRoot().x, rootList.rootList[0].GetCurrRoot().y] as Root;

        if (rootList.rootList[0].GetCurrRoot().y == 8) currRoot.GetDirection = Root.Direction.아래쪽_위쪽;
        else if (rootList.rootList[0].GetCurrRoot().x == 8) currRoot.GetDirection = Root.Direction.왼쪽_오른쪽;
        else if (rootList.rootList[0].GetCurrRoot().y == 3) currRoot.GetDirection = Root.Direction.위쪽_아래쪽;
        else if (rootList.rootList[0].GetCurrRoot().x == 3) currRoot.GetDirection = Root.Direction.오른쪽_왼쪽;
        else setting = false;


        for (int i = 1; i < rootList.rootList.Count - 1; i++)
        {
            currRoot = rootList.rootList[i];

            tempX = currRoot.GetCurrRoot().x - currRoot.GetPreRoot().x;
            tempY = currRoot.GetCurrRoot().y - currRoot.GetPreRoot().y;

            if (tempX == 1) // 왼쪽_**
            {
                tempX = currRoot.GetNextRoot().x - currRoot.GetCurrRoot().x;
                tempY = currRoot.GetNextRoot().y - currRoot.GetCurrRoot().y;
                if (tempX == 1) currRoot.GetDirection = Root.Direction.왼쪽_오른쪽;
                else if (tempY == 1) currRoot.GetDirection = Root.Direction.왼쪽_위쪽;
                else if (tempY == -1) currRoot.GetDirection = Root.Direction.왼쪽_아래쪽;
                else setting = false;
            }
            else if (tempX == -1) // 오른쪽_**
            {
                tempX = currRoot.GetNextRoot().x - currRoot.GetCurrRoot().x;
                tempY = currRoot.GetNextRoot().y - currRoot.GetCurrRoot().y;
                if (tempX == -1) currRoot.GetDirection = Root.Direction.오른쪽_왼쪽;
                else if (tempY == 1) currRoot.GetDirection = Root.Direction.오른쪽_위쪽;
                else if (tempY == -1) currRoot.GetDirection = Root.Direction.오른쪽_아래쪽;
                else setting = false;
            }
            else if (tempY == 1) // 아래쪽_**
            {
                tempX = currRoot.GetNextRoot().x - currRoot.GetCurrRoot().x;
                tempY = currRoot.GetNextRoot().y - currRoot.GetCurrRoot().y;
                if (tempX == 1) currRoot.GetDirection = Root.Direction.아래쪽_오른쪽;
                else if (tempX == -1) currRoot.GetDirection = Root.Direction.아래쪽_왼쪽;
                else if (tempY == 1) currRoot.GetDirection = Root.Direction.아래쪽_위쪽;
                else setting = false;
            }
            else if (tempY == -1) // 위쪽_**
            {
                tempX = currRoot.GetNextRoot().x - currRoot.GetCurrRoot().x;
                tempY = currRoot.GetNextRoot().y - currRoot.GetCurrRoot().y;
                if (tempX == 1) currRoot.GetDirection = Root.Direction.위쪽_오른쪽;
                else if (tempX == -1) currRoot.GetDirection = Root.Direction.위쪽_왼쪽;
                else if (tempY == -1) currRoot.GetDirection = Root.Direction.위쪽_아래쪽;
                else setting = false;
            }
            else
            {
                Debug.LogWarning("방향 설정 에러");
            }
        }

        currRoot = rootList.rootList[rootList.rootList.Count - 1];
        tempX = currRoot.GetCurrRoot().x - currRoot.GetPreRoot().x;
        tempY = currRoot.GetCurrRoot().y - currRoot.GetPreRoot().y;
        if (tempX == 1) currRoot.GetDirection = Root.Direction.왼쪽_오른쪽;
        else if (tempX == -1) currRoot.GetDirection = Root.Direction.오른쪽_왼쪽;
        else if (tempY == 1) currRoot.GetDirection = Root.Direction.아래쪽_위쪽;
        else if (tempY == -1) currRoot.GetDirection = Root.Direction.위쪽_아래쪽;
        else setting = false;

        return setting;

    }

    bool SettingRootTileBase(RootList rootList)
    {
        Root root;
        TileBase[] tileBaseList;
        for(int i = 0; i < rootList.rootList.Count; i++)
        {
            root = rootList.rootList[i];

            if (root.IsDameaed)
            {
                SetDamagedRootTileBase(root.GetCurrRoot());
                continue;
            }

            switch (root.GetDirection)
            {
                case Root.Direction.아래쪽_오른쪽:
                    tileBaseList = lower_Right;
                    break;
                case Root.Direction.아래쪽_위쪽:
                    tileBaseList = lower_Upper;
                    break;
                case Root.Direction.아래쪽_왼쪽:
                    tileBaseList = lower_Left;
                    break;
                case Root.Direction.왼쪽_위쪽:
                    tileBaseList = left_Upper;
                    break;
                case Root.Direction.왼쪽_오른쪽:
                    tileBaseList = left_Right;
                    break;
                case Root.Direction.왼쪽_아래쪽:
                    tileBaseList = left_Lower;
                    break;
                case Root.Direction.위쪽_오른쪽:
                    tileBaseList = upper_Right;
                    break;
                case Root.Direction.위쪽_아래쪽:
                    tileBaseList = upper_Lower;
                    break;
                case Root.Direction.위쪽_왼쪽:
                    tileBaseList = upper_Left;
                    break;
                case Root.Direction.오른쪽_아래쪽:
                    tileBaseList = right_Lower;
                    break;
                case Root.Direction.오른쪽_왼쪽:
                    tileBaseList = right_Left;
                    break;
                case Root.Direction.오른쪽_위쪽:
                    tileBaseList = right_Upper;
                    break;
                default:
                    Debug.LogWarning(root.GetDirection);
                    return false;
            }
            
            if (root.rootState == 0) lakiaroRootTileMap.SetTile(root.GetCurrRoot(), tileBaseList[0]);
            else if (root.rootState == 1) lakiaroRootTileMap.SetTile(root.GetCurrRoot(), tileBaseList[1]);
            else if (root.rootState == 2) lakiaroRootTileMap.SetTile(root.GetCurrRoot(), tileBaseList[2]);
        }

        root = rootList.rootList[rootList.rootList.Count - 1];
        switch (root.GetDirection)
        {
            case Root.Direction.아래쪽_위쪽:
                lakiaroRootTileMap.SetTile(root.GetCurrRoot(), end[2]);
                break;
            case Root.Direction.왼쪽_오른쪽:
                lakiaroRootTileMap.SetTile(root.GetCurrRoot(), end[0]);
                break;
            case Root.Direction.위쪽_아래쪽:
                lakiaroRootTileMap.SetTile(root.GetCurrRoot(), end[3]);
                break;
            case Root.Direction.오른쪽_왼쪽:
                lakiaroRootTileMap.SetTile(root.GetCurrRoot(), end[1]);
                break;
            default:
                Debug.Log((rootList.rootList.Count - 1) + " : " + root.GetDirection);
                break;
        }

        return true;
    }
    
    private void ConvertDir(int dir, Vector3Int checkPoint, Vector3Int convertPos)
    {
        switch (dir)
        {
            case 0: // up
                vector3Int.x = checkPoint.x + convertPos.x;
                vector3Int.y = checkPoint.y + convertPos.y;
                break;
            case 1:// right
                vector3Int.x = checkPoint.x + convertPos.y;
                vector3Int.y = checkPoint.y - convertPos.x;
                break;
            case 2:// down
                vector3Int.x = checkPoint.x - convertPos.x;
                vector3Int.y = checkPoint.y - convertPos.y;
                break;
            case 3:// left
                vector3Int.x = checkPoint.x - convertPos.y;
                vector3Int.y = checkPoint.y + convertPos.x;
                break;
        }
    }

    bool CheckDir(ref Root.Direction dir, Root root)
    {
        bool canMake = true;

        Vector3Int nextPos = new Vector3Int();
        switch (root.GetDirection)
        {
            case Root.Direction.왼쪽_위쪽:
            case Root.Direction.오른쪽_위쪽:
            case Root.Direction.아래쪽_위쪽:
                nextPos = Vector3Int.up;
                break;
            case Root.Direction.왼쪽_오른쪽:
            case Root.Direction.아래쪽_오른쪽:
            case Root.Direction.위쪽_오른쪽:
                nextPos = Vector3Int.right;
                break;
            case Root.Direction.위쪽_아래쪽:
            case Root.Direction.오른쪽_아래쪽:
            case Root.Direction.왼쪽_아래쪽:
                nextPos = Vector3Int.down;
                break;
            case Root.Direction.아래쪽_왼쪽:
            case Root.Direction.오른쪽_왼쪽:
            case Root.Direction.위쪽_왼쪽:
                nextPos = Vector3Int.left;
                break;
        }
        
        return canMake;
    }

    public bool CanMakeRoot(Vector3Int _startPoint)
    {
        bool canMake = false;

        Queue<Vector3Int> remainCheck = new Queue<Vector3Int>();
        Vector3Int checkPos;
        remainCheck.Enqueue(_startPoint);
        int count = 1; // 현재 빈공간 개수
        bool isFirst = true;
        while (!canMake) // 더이상 추가 가능한 뿌리가 없거나 만들수 있을경우
        {
            if (remainCheck.Count == 0) break;

            checkPos = remainCheck.Dequeue();
            Debug.Log("현재 위치" + checkPos);
            if(checkPos.z == 0 || checkPos.z != 2)
                if (checkPos.x + 1 >= 0 && checkPos.x + 1 <= 11 && checkPos.y >= 0 && checkPos.y <= 11)
                    if (lakiaroRoot[checkPos.x + 1, checkPos.y] != null)
                        if (lakiaroRoot[checkPos.x + 1, checkPos.y].type == Lakiaro.Type.Dirt)
                        {
                            Debug.Log(remainCheck.Count + "개");
                            remainCheck.Enqueue(new Vector3Int(checkPos.x + 1, checkPos.y, 1)); count++;
                        }
            if (checkPos.z == 0 || checkPos.z != 1)
                if (checkPos.x - 1 >= 0 && checkPos.x - 1 <= 11 && checkPos.y >= 0 && checkPos.y <= 11)
                    if (lakiaroRoot[checkPos.x - 1, checkPos.y] != null)
                        if (lakiaroRoot[checkPos.x - 1, checkPos.y].type == Lakiaro.Type.Dirt)
                        {
                            Debug.Log(remainCheck.Count + "개");
                            remainCheck.Enqueue(new Vector3Int(checkPos.x - 1, checkPos.y, 2)); count++;
                        }
            if (checkPos.z == 0 || checkPos.z != 4)
                if (checkPos.x >= 0 && checkPos.x <= 11 && checkPos.y + 1 >= 0 && checkPos.y + 1 <= 11)
                    if (lakiaroRoot[checkPos.x, checkPos.y + 1] != null)
                        if (lakiaroRoot[checkPos.x, checkPos.y + 1].type == Lakiaro.Type.Dirt)
                        {
                            Debug.Log(remainCheck.Count + "개");
                            remainCheck.Enqueue(new Vector3Int(checkPos.x, checkPos.y + 1, 3)); count++;
                        }
            if (checkPos.z == 0 || checkPos.z != 3)
                if (checkPos.x >= 0 && checkPos.x <= 11 && checkPos.y - 1 >= 0 && checkPos.y - 1 <= 11)
                    if (lakiaroRoot[checkPos.x, checkPos.y - 1] != null)
                        if (lakiaroRoot[checkPos.x, checkPos.y - 1].type == Lakiaro.Type.Dirt)
                        {
                            Debug.Log(remainCheck.Count + "개");
                            remainCheck.Enqueue(new Vector3Int(checkPos.x, checkPos.y - 1, 4)); count++;
                        }

            Debug.Log("현재" + remainCheck.Count + "개");
            if (count >= 8) canMake = true; // 빈공간이 8개 일 경우 어떤 형태라도 1개는 생성 가능
        }

        if (canMake) Debug.Log("8개이상 발견으로 생성 가능");
        else Debug.LogWarning(count + "개 발견으로 생성 불가능");

        return canMake;
    }

    public void GeneratePebble(int level = 0)
    {
        // 자갈이 없는 공간을 찾고 받은 인수 만큼의 자갈 생성
        // 1단계 0~10
        // 2단계 0~15
        // 3단계 0~20
        // 4단계 5~20
        // 5단계 5~25
        int currPebble = 0, pebbleCount, x, y;

        switch (level)
        {
            case 1:
                pebbleCount = Random.Range(0, 16);
                break;
            case 2:
                pebbleCount = Random.Range(0, 21);
                break;
            case 3:
                pebbleCount = Random.Range(5, 21);
                break;
            case 4:
                pebbleCount = Random.Range(5, 26);
                break;
            default:
                pebbleCount = Random.Range(0, 11); 
                break;
        }

        while (currPebble < pebbleCount)
        {
            x = Random.Range(0, lakiaroRoot.GetLength(0));
            y = Random.Range(0, lakiaroRoot.GetLength(1));
            if (x < 8 && x > 3 && y > 3 && y < 8) continue;

            if (lakiaroRoot[x, y].type == Lakiaro.Type.Dirt)
            {
                currPebble++;
                dirtPool.Enqueue((Dirt)lakiaroRoot[x, y]);
                lakiaroRoot[x, y] = pebblePool.Dequeue();

                vector3Int.x = x;
                vector3Int.y = y;
                lakiaroRootTileMap.SetTile(vector3Int, pebbleTile[Random.Range(0, pebbleTile.Count)]);
                currRemainPebble++;
                currRemainDirt--;
            }
            else
            {
                Debug.LogWarning(x + ", " + y + "에 " + lakiaroRoot[x, y].type + "가 존재");
            }
        }
    }

    public void InitGame()
    {
        rootLists.Clear();
        currRemainDirt = 128;
        currRemainRoot = 0;
        currRemainPebble = 0;
        
        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                if (i < 8 && i > 3 && j > 3 && j < 8) continue;

                lakiaroRoot[i, j].Init();

                if (lakiaroRoot[i, j].type == Lakiaro.Type.Root)
                {
                    rootPool.Enqueue(lakiaroRoot[i, j] as Root);
                    lakiaroRoot[i, j] = dirtPool.Dequeue();
                }
                else if (lakiaroRoot[i, j].type == Lakiaro.Type.Pebble)
                {
                    pebblePool.Enqueue(lakiaroRoot[i, j] as Pebble);
                    lakiaroRoot[i, j] = dirtPool.Dequeue();
                }
            }
        }
        lakiaroRootTileMap.ClearAllTiles();

        rootLists.Clear();
        try
        {
            rootLists.Clear();
            currRemainDirt = 128;
            currRemainRoot = 0;
            currRemainPebble = 0;
            for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
            {
                for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
                {
                    if (i < 8 && i > 3 && j > 3 && j < 8) continue;

                    lakiaroRoot[i, j].Init();

                    if (lakiaroRoot[i, j].type == Lakiaro.Type.Root)
                    {
                        rootPool.Enqueue(lakiaroRoot[i, j] as Root);
                        lakiaroRoot[i, j] = dirtPool.Dequeue();
                    }
                    else if (lakiaroRoot[i, j].type == Lakiaro.Type.Pebble)
                    {
                        pebblePool.Enqueue(lakiaroRoot[i, j] as Pebble);
                        lakiaroRoot[i, j] = dirtPool.Dequeue();
                    }
                }
            }
            lakiaroRootTileMap.ClearAllTiles();

            rootLists.Clear();
        }
        catch (System.Exception e)
        {
            Debug.LogWarning(dirtPool.Count);
            Debug.LogWarning(rootPool.Count);
            Debug.LogWarning(pebblePool.Count);
            Debug.LogWarning(e);
        }

        Debug.Log(dirtPool.Count);
        Debug.Log(rootPool.Count);
        Debug.Log(pebblePool.Count);
    }

    public void LoadGameData()
    {
        for (int i = 0; i < GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroRoot.Count; i++)
        {
            for (int j = 0; j < GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroRoot[i].LakiaroList.Count; j++)
            {
                switch (GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroRoot[i].LakiaroList[j].type)
                {
                    case Lakiaro.Type.Pebble:
                        dirtPool.Enqueue(lakiaroRoot[i, j] as Dirt);
                        lakiaroRoot[i, j] = pebblePool.Dequeue();
                        break;
                    case Lakiaro.Type.Root:
                        dirtPool.Enqueue(lakiaroRoot[i, j] as Dirt);
                        lakiaroRoot[i, j] = rootPool.Dequeue();
                        break;
                }

                lakiaroRoot[i, j].isChecked = GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroRoot[i].LakiaroList[j].isChecked;
            }
        }
        
        for (int i = 0; i < GameManager.Instance.dataManager.gameData.lakiaroGameData.RootLists.Count; i++)
        {
            rootLists.Add(new RootList());
            for (int j = 0; j < GameManager.Instance.dataManager.gameData.lakiaroGameData.RootLists[i].rootList.Count; j++)
            {
                vector3Int = GameManager.Instance.dataManager.gameData.lakiaroGameData.RootLists[i].rootList[j].GetCurrRoot();
                Root root = lakiaroRoot[vector3Int.x, vector3Int.y] as Root;
                root.CopyData(GameManager.Instance.dataManager.gameData.lakiaroGameData.RootLists[i].rootList[j]);
                rootLists[i].rootList.Add(root);
            }
        }
        //rootLists = GameManager.Instance.dataManager.gameData.lakiaroGameData.RootLists;

        lakiaroLevel = GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroLevel;
        currLakiaroLevel = GameManager.Instance.dataManager.gameData.lakiaroGameData.CurrLevel;
        manosHoeLevel = GameManager.Instance.dataManager.gameData.lakiaroGameData.ManosHoeLevel;
        currRemainTryTime = GameManager.Instance.dataManager.gameData.lakiaroGameData.CurrRemainTryTime;
        progress = GameManager.Instance.dataManager.gameData.lakiaroGameData.Progress;
        LoadLakiaro();
    }

    void LoadLakiaro()
    {
        SetFlowerTile(currLakiaroLevel);
        LoadDirt();

        currRemainDirt = 0;
        currRemainPebble = 0;
        currRemainRoot = 0;
        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            for(int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                switch (lakiaroRoot[i, j].type)
                {
                    case Lakiaro.Type.Pebble:
                        vector3Int.x = i;
                        vector3Int.y = j;
                        lakiaroRootTileMap.SetTile(vector3Int,pebbleTile[Random.Range(0, pebbleTile.Count)]);
                        if (!lakiaroRoot[i, j].isChecked)
                        {
                            currRemainPebble++;
                        }
                        break;
                    case Lakiaro.Type.Root:
                        if (!lakiaroRoot[i, j].isChecked)
                        {
                            currRemainRoot++;
                        }
                        break;
                    case Lakiaro.Type.Dirt:
                        if (!lakiaroRoot[i, j].isChecked)
                        {
                            currRemainDirt++;
                        }
                        break;
                }
            }
        }

        for(int i = 0; i < rootLists.Count; i++)
        {
            if(!SettingRootTileBase(rootLists[i])) Debug.Log("불러온 뿌리 데이터 타일맵 설정중 에러 " + i);
        }
        
    }

    void LoadDirt()
    {
        lakiaroDirtTilemap_Upper.ClearAllTiles();
        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                if (i < 8 && i > 3 && j > 3 && j < 8) continue;

                vector3Int.x = i;
                vector3Int.y = j;
                
                if (!lakiaroRoot[i, j].isChecked)
                {
                    lakiaroDirtTilemap_Upper.SetTile(vector3Int, basicTile[currLakiaroLevel][Random.Range(0, basicTile[currLakiaroLevel].Count)]);
                }
                lakiaroDirtTileMap_Lower.SetTile(vector3Int, basicTile[currLakiaroLevel + 1][Random.Range(0, basicTile[currLakiaroLevel + 1].Count)]);

            }
        }
    }

    public void SaveGameData()
    {
        if (isDailyGame)
        {
            SaveGameDailyData();
            return;
        }

        if (GameManager.Instance.dataManager.gameData.lakiaroGameData == null) GameManager.Instance.dataManager.gameData.lakiaroGameData = new LakiaroGameData();
        GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroRoot.Clear();
        //GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroRoot = lakiaroRoot;
        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroRoot.Add(new LakiaroListData());
            for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroRoot[i].LakiaroList.Add(new Lakiaro(lakiaroRoot[i, j]));
            }
        }
        GameManager.Instance.dataManager.gameData.lakiaroGameData.RootLists.Clear();
        for (int i = 0; i < rootLists.Count; i++)
        {
            GameManager.Instance.dataManager.gameData.lakiaroGameData.RootLists.Add(new RootList());
            for (int j = 0; j < rootLists[i].rootList.Count; j++)
            {
                GameManager.Instance.dataManager.gameData.lakiaroGameData.RootLists[i].rootList.Add(new Root(rootLists[i].rootList[j]));
            }
        }
        //GameManager.Instance.dataManager.gameData.lakiaroGameData.RootLists = rootLists;

        GameManager.Instance.dataManager.gameData.lakiaroGameData.LakiaroLevel = lakiaroLevel;
        GameManager.Instance.dataManager.gameData.lakiaroGameData.CurrLevel = currLakiaroLevel;
        GameManager.Instance.dataManager.gameData.lakiaroGameData.ManosHoeLevel = manosHoeLevel;
        GameManager.Instance.dataManager.gameData.lakiaroGameData.CurrRemainTryTime = currRemainTryTime;
        GameManager.Instance.dataManager.gameData.lakiaroGameData.Progress = progress;
        InitGame();
    }

    public void LoadGameDailyData()
    {
        for (int i = 0; i < GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroRoot.Count; i++)
        {
            for (int j = 0; j < GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroRoot[i].LakiaroList.Count; j++)
            {
                switch (GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroRoot[i].LakiaroList[j].type)
                {
                    case Lakiaro.Type.Pebble:
                        dirtPool.Enqueue(lakiaroRoot[i, j] as Dirt);
                        lakiaroRoot[i, j] = pebblePool.Dequeue();
                        break;
                    case Lakiaro.Type.Root:
                        dirtPool.Enqueue(lakiaroRoot[i, j] as Dirt);
                        lakiaroRoot[i, j] = rootPool.Dequeue();
                        break;
                }

                lakiaroRoot[i, j].isChecked = GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroRoot[i].LakiaroList[j].isChecked;
            }
        }

        for (int i = 0; i < GameManager.Instance.dataManager.gameData.dailyChallengeData.RootLists.Count; i++)
        {
            rootLists.Add(new RootList());
            for (int j = 0; j < GameManager.Instance.dataManager.gameData.dailyChallengeData.RootLists[i].rootList.Count; j++)
            {
                vector3Int = GameManager.Instance.dataManager.gameData.dailyChallengeData.RootLists[i].rootList[j].GetCurrRoot();
                Root root = lakiaroRoot[vector3Int.x, vector3Int.y] as Root;
                root.CopyData(GameManager.Instance.dataManager.gameData.dailyChallengeData.RootLists[i].rootList[j]);
                rootLists[i].rootList.Add(root);
            }
        }
        //rootLists = GameManager.Instance.dataManager.gameData.dailyChallengeData.RootLists;

        lakiaroLevel = GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroLevel;
        currLakiaroLevel = GameManager.Instance.dataManager.gameData.dailyChallengeData.CurrLevel;
        manosHoeLevel = GameManager.Instance.dataManager.gameData.dailyChallengeData.ManosHoeLevel;
        currRemainTryTime = GameManager.Instance.dataManager.gameData.dailyChallengeData.CurrRemainTryTime;
        progress = GameManager.Instance.dataManager.gameData.dailyChallengeData.Progress;
        LoadLakiaro();
    }

    public void SaveGameDailyData()
    {
        GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroRoot.Clear();
        //GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroRoot = lakiaroRoot;
        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroRoot.Add(new LakiaroListData());
            for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroRoot[i].LakiaroList.Add(new Lakiaro(lakiaroRoot[i, j]));
            }
        }
        GameManager.Instance.dataManager.gameData.dailyChallengeData.RootLists.Clear();
        for (int i = 0; i < rootLists.Count; i++)
        {
            GameManager.Instance.dataManager.gameData.dailyChallengeData.RootLists.Add(new RootList());
            for (int j = 0; j < rootLists[i].rootList.Count; j++)
            {
                GameManager.Instance.dataManager.gameData.dailyChallengeData.RootLists[i].rootList.Add(new Root(rootLists[i].rootList[j]));
            }
        }
        //GameManager.Instance.dataManager.gameData.dailyChallengeData.RootLists = rootLists;

        GameManager.Instance.dataManager.gameData.dailyChallengeData.LakiaroLevel = lakiaroLevel;
        GameManager.Instance.dataManager.gameData.dailyChallengeData.CurrLevel = currLakiaroLevel;
        GameManager.Instance.dataManager.gameData.dailyChallengeData.ManosHoeLevel = manosHoeLevel;
        GameManager.Instance.dataManager.gameData.dailyChallengeData.CurrRemainTryTime = currRemainTryTime;
        GameManager.Instance.dataManager.gameData.dailyChallengeData.Progress = progress;
        InitGame();
    }
}

