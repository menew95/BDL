using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using DataInfo;
public class LakiaroManager : MonoBehaviour
{
    [Header("Lakiaro Hard Root")]
    public RootDataInfo rootDataInfo;

    [Header("Lakiaro Tilemap")]
    public Tilemap lakiaroRootTileMap;
    public Tilemap lakiaroDirtTileMap_Lower;
    public Tilemap lakiaroDirtTilemap_Upper;

    public Lakiaro[,] lakiaroRoot = new Lakiaro[12, 12];

    public Queue<Dirt> dirtPool = new Queue<Dirt>();
    public Queue<Root> rootPool = new Queue<Root>();
    public Queue<Pebble> pebblePool = new Queue<Pebble>();

    [SerializeField]
    private int currRemainDirt = 128, currRemainRoot = 0, currRemainPebble = 0;

    public TileBase temp;

    public bool gamePause = true;

    public List<TileBase> basicTile = new List<TileBase>();
    public List<TileBase> basicTile2 = new List<TileBase>();
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
    }

    public void test()
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
    }

    Vector3 mousePosition;
    // Update is called once per frame
    void Update()
    {
        if (gamePause) return;
        if (Input.GetMouseButtonUp(1))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (CheckRoot(mousePosition)) ;
            inGame_UI.UpdateRemainText(currRemainDirt, currRemainRoot, currRemainPebble);
        }
        if (Input.GetMouseButtonUp(0))
        {
            /*mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckRoot2(mousePosition);
            inGame_UI.UpdateRemainText(currRemainDirt, currRemainRoot, currRemainPebble);*/
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vector3Int.x = (int)mousePosition.x;
            vector3Int.y = (int)mousePosition.y;
            CanMakeRoot(vector3Int);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("a");
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            vector3Int.x = (int)mousePosition.x;
            vector3Int.y = (int)mousePosition.y;

            int dir = 0;
            if (vector3Int.x == 3 && vector3Int.y > 3 && vector3Int.y < 8) dir = 3;
            else if (vector3Int.x == 8 && vector3Int.y > 3 && vector3Int.y < 8) dir = 1;
            else if (vector3Int.y == 3 && vector3Int.x > 3 && vector3Int.x < 8) dir = 2;
            else if (vector3Int.y == 8 && vector3Int.x > 3 && vector3Int.x < 8) dir = 0;
            else return;
            CheckCanMakeHardRoot(vector3Int, dir);
            inGame_UI.UpdateRemainText(currRemainDirt, currRemainRoot, currRemainPebble);
        }
    }

    Vector3Int vector3Int = new Vector3Int();
    public bool CheckRoot(Vector2 point)
    {
        if (0 > (int)point.x || (int)point.x > 11 || 0 > (int)point.y || (int)point.y > 11) return false;
        if (lakiaroRoot[(int)point.x, (int)point.y].isChecked) return false; // 이미 확인한 곳이면 스킾

        bool isRoot = false;

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
            }
            else if (lakiaroRoot[(int)point.x, (int)point.y].type == Lakiaro.Type.Pebble)
            {
                Debug.Log((int)mousePosition.x + ", " + (int)mousePosition.y + " 는 자갈이다.");
            }
        }
        else
        {
            Debug.Log((int)mousePosition.x + ", " + (int)mousePosition.y + " 는 흙이다.");
        }

        return isRoot;
    }

    public bool CheckRoot2(Vector2 point)
    {
        if (0 > (int)point.x || (int)point.x > 11 || 0 > (int)point.y || (int)point.y > 11) return false;
        if (lakiaroRoot[(int)point.x, (int)point.y].isChecked) return false; // 이미 확인한 곳이면 스킾

        bool isDirt = false;

        int manosHoeLevel = 9, currCheck = 0;

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
                }
                else
                {
                    if (x == 0 && y == 0)
                    {
                        currCheck++;
                        if (lakiaroRoot[vector3Int.x, vector3Int.y].type == Lakiaro.Type.Pebble)
                        {
                            currRemainPebble--;
                        }
                        else if (lakiaroRoot[vector3Int.x, vector3Int.y].type == Lakiaro.Type.Root)
                        {
                            currRemainRoot--;
                        }

                        lakiaroDirtTilemap_Upper.SetTile(vector3Int, null);
                        lakiaroRoot[vector3Int.x, vector3Int.y].isChecked = true;
                    }
                }

                if (manosHoeLevel == currCheck) break;
            }
        }

        if (currRemainDirt == 0)
        {
            UIManager.Instance.CallMainUI();
            InitGame();
            gamePause = true;
        }

        return isDirt;
    }

    public void GenerateLakiaro(int level = 0)
    {
        GenerateDirt(level); // 단계에 맞게 흙 설정
        //GenerateRoot(level); // 뿌리 생성
        //GeneratePebble(Random.Range(0, 16)); // 뿌리가 안지나가는 공간에 랜덤으로 자갈 생성

        inGame_UI.UpdateRemainText(currRemainDirt, currRemainRoot, currRemainPebble);
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
                    lakiaroDirtTilemap_Upper.SetTile(vector3Int, basicTile[Random.Range(0, basicTile.Count)]);
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
                lakiaroDirtTileMap_Lower.SetTile(vector3Int, basicTile2[Random.Range(0, basicTile2.Count)]);
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

    public void GenerateRoot(int root = 1)
    {
        /*for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                if (i < 8 && i > 3 && j > 3 && j < 8) continue;
                if (Random.Range(0, 4) == 1)
                {
                    dirtPool.Enqueue((Dirt)lakiaroRoot[i, j]);
                    lakiaroRoot[i, j] = rootPool.Dequeue();
                    currRemainDirt--;
                    currRemainRoot++;
                    vector3Int.x = i;
                    vector3Int.y = j;
                    lakiaroRootTileMap.SetTile(vector3Int, temp);
                }
            }
        }*/

        int currRootCount = 0, rootCount = Random.Range(5, 9); // 생성될 뿌리 5~9개 랜덤
        List<int> startList = new List<int>();

        List<Root> roots = new List<Root>();

        MakeHardRoot(rootCount);

        while (currRootCount < rootCount)
        {
            bool makeFinish = false, canMake = true;

            int start = Random.Range(0, 16); // 상단 좌측 모서리를 기준으로 시계방향
            if (startList.Contains(start)) continue; // 이미 뿌리가 있을 경우 다시 뽑음
            else startList.Add(start);

            int currLength = 0; // 현재까지 생성된 뿌리 길이
            Root.Direction dir = Root.Direction.오른쪽_왼쪽;
            switch (start)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    dir = Root.Direction.아래쪽_위쪽;
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    dir = Root.Direction.왼쪽_오른쪽;
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                    dir = Root.Direction.위쪽_아래쪽;
                    break;
                case 12:
                case 13:
                case 14:
                case 15:
                    dir = Root.Direction.오른쪽_왼쪽;
                    break;
            }

            roots.Add(new Root(0, dir, 0, 0, 0, 0, startPos[start].x, startPos[start].y));

            while (!makeFinish && canMake)
            {
                if (CheckDir(ref dir, roots[currLength]))

                    if (currLength == 8) makeFinish = true;
            }

            currRootCount++;
        }
    }

    List<int> tempIndexList = new List<int>();
    bool CheckCanMakeHardRoot(Vector3Int checkPoint, int dir) // 해당 포인트에 들어 갈 수 있는 뿌리 확인
    {
        // 1~24형태의 뿌리중 가능한 뿌리 인덱스를 출력
        // RootData의 각 뿌리 위치를 하나씩 대조하여 가능한 뿌리를 찾음
        // Third, Fourth, Fifth => 각 뿌리의 세번째 네번째 다섯번째 뿌리가 안될경우 안되는 다른 뿌리 형태의 인덱스 리스트 
        bool canMake = false;

        int randomIndex = 0, x = 0, y = 0, ranSixth = 0;
        List<int> sixth = new List<int>();
        while (!canMake)
        {
            if (tempIndexList.Count == 24) break;

            randomIndex = Random.Range(0, 24);

            if (tempIndexList.Contains(randomIndex + 1)) continue;

            bool cantMake = false;

            for (int i = 2; i < 5; i++)
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
                    if (i == 2)
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
                Debug.Log("continue");
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
                Debug.Log(x + " " + y);
                if (lakiaroRoot[x, y].type == Lakiaro.Type.Dirt)
                {
                    sixth.Add(i);
                }
            }

            if (sixth.Count == 0) continue;
            else
            {
                ranSixth = Random.Range(0, sixth.Count);
                canMake = true;
            }

            if(canMake) Debug.Log(randomIndex + "가능");
        }

        Debug.Log(canMake);
        if (canMake)
        {
            Root root;
            

            for (int i = 0; i < 5; i++)
            {
                root = rootPool.Dequeue();

                switch (dir)
                {
                    case 0: // up
                        vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i].x;
                        vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i].y;
                        break;
                    case 1:// right
                        vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i].y;
                        vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i].x;
                        break;
                    case 2:// down
                        vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i].x;
                        vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i].y;
                        break;
                    case 3:// left
                        vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i].y;
                        vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i].x;
                        break;
                }

                root.SetCurrRoot(vector3Int);
                lakiaroRoot[vector3Int.x, vector3Int.y] = root;

                if (i == 0)
                {
                    root.rootState = 0;
                    switch (dir)
                    {
                        case 0: // up
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i +1].x;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].y;
                            break;
                        case 1:// right
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].y;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].x;
                            break;
                        case 2:// down
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].x;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].y;
                            break;
                        case 3:// left
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].y;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].x;
                            break;
                    }
                    root.SetNextRoot(vector3Int);
                }
                else if (i == 4)
                {
                    root.rootState = 1;

                    switch (dir)
                    {
                        case 0: // up
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].x;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].y;
                            break;
                        case 1:// right
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].y;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].x;
                            break;
                        case 2:// down
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].x;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].y;
                            break;
                        case 3:// left
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].y;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].x;
                            break;
                    }
                    root.SetPreRoot(vector3Int);

                    switch (dir)
                    {
                        case 0: // up
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].x;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].y;
                            break;
                        case 1:// right
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].y;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].x;
                            break;
                        case 2:// down
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].x;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].y;
                            break;
                        case 3:// left
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].y;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].x;
                            break;
                    }
                    root.SetNextRoot(vector3Int);
                }
                else
                {
                    root.rootState = 0;
                    switch (dir)
                    {
                        case 0: // up
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].x;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].y;
                            break;
                        case 1:// right
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].y;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].x;
                            break;
                        case 2:// down
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].x;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].y;
                            break;
                        case 3:// left
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].y;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i - 1].x;
                            break;
                    }
                    root.SetPreRoot(vector3Int);

                    switch (dir)
                    {
                        case 0: // up
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].x;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].y;
                            break;
                        case 1:// right
                            vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].y;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].x;
                            break;
                        case 2:// down
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].x;
                            vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].y;
                            break;
                        case 3:// left
                            vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].y;
                            vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[i + 1].x;
                            break;
                    }
                    root.SetNextRoot(vector3Int);
                }
            }

            switch (dir)
            {
                case 0: // up
                    vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].x;
                    vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].y;
                    break;
                case 1:// right
                    vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].y;
                    vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].x;
                    break;
                case 2:// down
                    vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].x;
                    vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].y;
                    break;
                case 3:// left
                    vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].y;
                    vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].lastRootList[ranSixth].x;
                    break;
            }
            Debug.Log(vector3Int);
            root = rootPool.Dequeue();
            root.SetCurrRoot(vector3Int);
            lakiaroRoot[vector3Int.x, vector3Int.y] = root;

            root.rootState = 3;
            switch (dir)
            {
                case 0: // up
                    vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[4].x;
                    vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[4].y;
                    break;
                case 1:// right
                    vector3Int.x = checkPoint.x + rootDataInfo.up_RootDataList[randomIndex].rootList[4].y;
                    vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[4].x;
                    Debug.Log(rootDataInfo.up_RootDataList[randomIndex].rootList[4]);
                    break;
                case 2:// down
                    vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[4].x;
                    vector3Int.y = checkPoint.y - rootDataInfo.up_RootDataList[randomIndex].rootList[4].y;
                    break;
                case 3:// left
                    vector3Int.x = checkPoint.x - rootDataInfo.up_RootDataList[randomIndex].rootList[4].y;
                    vector3Int.y = checkPoint.y + rootDataInfo.up_RootDataList[randomIndex].rootList[4].x;
                    break;
            }
            root.SetPreRoot(vector3Int);

            Root temp = lakiaroRoot[checkPoint.x, checkPoint.y] as Root;
            Debug.Log(lakiaroRoot[checkPoint.x, checkPoint.y].type);
            for(int i = 0; i < 6; i++)
            {
                Debug.Log(i + " " + temp.GetCurrRoot());
                temp = lakiaroRoot[temp.GetNextRoot().x, temp.GetNextRoot().y] as Root;
            }
        }
        else
        {
            tempIndexList.Sort();
            string s = "";
            for(int i = 0; i < tempIndexList.Count; i++)
            {
                s += tempIndexList[i] + " ";
            }
        }
        tempIndexList.Clear();

        /*
         * 
         * 임시
         * 
         */


        Root currRoot, nextRoot;
        int tempX, tempY;
        if (canMake)
        {
            currRoot = lakiaroRoot[checkPoint.x, checkPoint.y] as Root;

            switch (dir)
            {
                case 0:
                    currRoot.GetDirection = Root.Direction.아래쪽_위쪽;
                    break;
                case 1:
                    currRoot.GetDirection = Root.Direction.왼쪽_오른쪽;
                    break;
                case 2:
                    currRoot.GetDirection = Root.Direction.위쪽_아래쪽;
                    break;
                case 3:
                    currRoot.GetDirection = Root.Direction.오른쪽_왼쪽;
                    break;
            }
            RootTileSet(currRoot.GetCurrRoot());
            currRoot = lakiaroRoot[currRoot.GetNextRoot().x, currRoot.GetNextRoot().y] as Root;

            for (int i = 0; i < 4; i++)
            {

                tempX = currRoot.GetCurrRoot().x - currRoot.GetPreRoot().x;
                tempY = currRoot.GetCurrRoot().y - currRoot.GetPreRoot().y;

                if (tempX == 1) // 왼쪽_**
                {
                    tempX = currRoot.GetNextRoot().x - currRoot.GetCurrRoot().x;
                    tempY = currRoot.GetNextRoot().y - currRoot.GetCurrRoot().y;
                    if (tempX == 1) currRoot.GetDirection = Root.Direction.왼쪽_오른쪽;
                    else if (tempY == 1) currRoot.GetDirection = Root.Direction.왼쪽_위쪽;
                    else if (tempY == -1) currRoot.GetDirection = Root.Direction.왼쪽_아래쪽;
                }
                else if (tempX == -1) // 오른쪽_**
                {
                    tempX = currRoot.GetNextRoot().x - currRoot.GetCurrRoot().x;
                    tempY = currRoot.GetNextRoot().y - currRoot.GetCurrRoot().y;
                    if (tempX == -1) currRoot.GetDirection = Root.Direction.오른쪽_왼쪽;
                    else if (tempY == 1) currRoot.GetDirection = Root.Direction.오른쪽_위쪽;
                    else if (tempY == -1) currRoot.GetDirection = Root.Direction.오른쪽_아래쪽;
                }
                else if (tempY == 1) // 아래쪽_**
                {
                    tempX = currRoot.GetNextRoot().x - currRoot.GetCurrRoot().x;
                    tempY = currRoot.GetNextRoot().y - currRoot.GetCurrRoot().y;
                    if (tempX == 1) currRoot.GetDirection = Root.Direction.아래쪽_오른쪽;
                    else if (tempX == -1) currRoot.GetDirection = Root.Direction.아래쪽_왼쪽;
                    else if (tempY == 1) currRoot.GetDirection = Root.Direction.아래쪽_위쪽;
                }
                else if (tempY == -1) // 위쪽_**
                {
                    tempX = currRoot.GetNextRoot().x - currRoot.GetCurrRoot().x;
                    tempY = currRoot.GetNextRoot().y - currRoot.GetCurrRoot().y;
                    if (tempX == 1) currRoot.GetDirection = Root.Direction.위쪽_오른쪽;
                    else if (tempX == -1) currRoot.GetDirection = Root.Direction.위쪽_왼쪽;
                    else if (tempY == -1) currRoot.GetDirection = Root.Direction.위쪽_아래쪽;
                }
                RootTileSet(currRoot.GetCurrRoot());

                currRoot = lakiaroRoot[currRoot.GetNextRoot().x, currRoot.GetNextRoot().y] as Root;
            }

            // 마지막 뿌리
            tempX = currRoot.GetCurrRoot().x - currRoot.GetPreRoot().x;
            tempY = currRoot.GetCurrRoot().y - currRoot.GetPreRoot().y;
            if (tempX == 1) currRoot.GetDirection = Root.Direction.왼쪽_오른쪽;
            else if (tempX == -1) currRoot.GetDirection = Root.Direction.오른쪽_왼쪽;
            else if (tempY == 1) currRoot.GetDirection = Root.Direction.아래쪽_위쪽;
            else if (tempY == -1) currRoot.GetDirection = Root.Direction.위쪽_아래쪽;
            Debug.Log("kk");
            RootTileSet(currRoot.GetCurrRoot());
        }

        return canMake;
    }

    void RootTileSet(Vector3Int pos)
    {

        if(lakiaroRoot[pos.x, pos.y].type != Lakiaro.Type.Root)
        {
            Debug.Log(pos + " : 에러 "+ lakiaroRoot[pos.x, pos.y].type);
            return;
        }
        Root root = lakiaroRoot[pos.x, pos.y] as Root;
        TileBase[] tileBaseList;
        

        if(root.rootState == 3)
        {
            switch (root.GetDirection)
            {
                case Root.Direction.아래쪽_위쪽:
                    Debug.Log(pos);
                    lakiaroRootTileMap.SetTile(pos, end[2]);
                    break;
                case Root.Direction.왼쪽_오른쪽:
                    Debug.Log(pos);
                    lakiaroRootTileMap.SetTile(pos, end[0]);
                    break;
                case Root.Direction.위쪽_아래쪽:
                    Debug.Log(pos);
                    lakiaroRootTileMap.SetTile(pos, end[3]);
                    break;
                case Root.Direction.오른쪽_왼쪽:
                    Debug.Log(pos);
                    lakiaroRootTileMap.SetTile(pos, end[1]);
                    break;
            }
        }
        else
        {
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
                    tileBaseList = right_Upper;
                    break;
            }

            TileBase tileBase = null; ;
            if (root.rootState == 0) tileBase = tileBaseList[0];
            if (root.rootState == 1) tileBase = tileBaseList[1];
            if (root.rootState == 2) tileBase = tileBaseList[2];

            lakiaroRootTileMap.SetTile(pos, tileBase);
            Debug.Log(pos);
        }
    }

    bool MakeHardRoot(int _rootCount)
    {
        bool canMake = true;
        int currRootCount = 0;

        List<int> startList = new List<int>();
        List<Root> listRoot = new List<Root>();
        Queue<Root> queueRoot = new Queue<Root>();
        while (canMake || _rootCount == currRootCount)
        {
            int start = Random.Range(0, 16); // 상단 좌측 모서리를 기준으로 시계방향
            if (startList.Contains(start) || lakiaroRoot[startPos[start].x, startPos[start].y].type == Lakiaro.Type.Root) continue; // 그위치에 뿌리가있거나 이미 제외된 위치일경우
            else startList.Add(start);
            
            int currLength = 1; // 처음 뿌리
            Root.Direction dir = Root.Direction.오른쪽_왼쪽;
            switch (start)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    vector3Int = Vector3Int.up;
                    dir = Root.Direction.아래쪽_위쪽;
                    break;
                case 4:
                case 5:
                case 6:
                case 7:
                    vector3Int = Vector3Int.right;
                    dir = Root.Direction.왼쪽_오른쪽;
                    break;
                case 8:
                case 9:
                case 10:
                case 11:
                    vector3Int = Vector3Int.down;
                    dir = Root.Direction.위쪽_아래쪽;
                    break;
                case 12:
                case 13:
                case 14:
                case 15:
                    vector3Int = Vector3Int.left;
                    dir = Root.Direction.오른쪽_왼쪽;
                    break;
            }
            // 처음 시작 뿌리 찾기
            
            if (!CanMakeRoot(startPos[start] + vector3Int)) continue; // 두번째 위치가 막혀있을경우 다시

            Root root = rootPool.Dequeue();
            root.rootState = 0;
            root.GetDirection = dir;
            root.SetCurrRoot(startPos[start]);
            root.SetNextRoot(startPos[start] + vector3Int);

            listRoot.Add(root);
            queueRoot.Enqueue(root);

        }

        return canMake;
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

    public void GeneratePebble(int pebble = 0)
    {
        // 자갈이 없는 공간을 찾고 받은 인수 만큼의 자갈 생성

        int currPebble = 0, x, y;
        
        while (currPebble < pebble)
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
        }
    }

    public void InitGame()
    {
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
    }
}

