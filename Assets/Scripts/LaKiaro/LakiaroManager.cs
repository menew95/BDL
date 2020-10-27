using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
public class Lakiaro
{
    public enum Type
    {
        Dirt,
        Root,
        Pebble
    }
    public Type type;

    public bool isChecked = false;

    public void Init()
    {
        isChecked = false;
    }
}

[System.Serializable]
public class Pebble : Lakiaro
{
    public Pebble()
    {
        this.type = Type.Pebble;
    }
}

[System.Serializable]
public class Dirt : Lakiaro
{
    public Dirt()
    {
        this.type = Type.Dirt;
    }
}

[System.Serializable]
public class Root : Lakiaro
{
    public Root()
    {
        this.type = Type.Root;
    }

    public bool isFirstRoot = false;
    public bool isFinishRoot = false;
    enum Direction // 시작_끝
    {
        오른쪽_위쪽,
        오른쪽_왼쪽,
        오른쪽_아래쪽,
        위쪽_왼쪽,
        위쪽_아래쪽,
        위쪽_오른쪽,
        왼쪽_아래쪽,
        왼쪽_오른쪽,
        왼쪽_위쪽,
        아래쪽_오른쪽,
        아래쪽_위쪽,
        아래쪽_왼쪽
    }
    private Direction direction;

    private int startPoint;
    private int finishPoint;

    private int preRootx, preRooty;
    private int nextRootx, nextRooty;
}

public class LakiaroManager : MonoBehaviour
{

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
    // Start is called before the first frame update
    void Start()
    {
        
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
                if (i < 8 && i > 3 && j > 3 && j < 8) continue;
                
                lakiaroRoot[i, j] = dirtPool.Dequeue();
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
                if (lakiaroRoot[i,j].type == Lakiaro.Type.Root)
                {
                    currRemainRoot--;
                }
                else if (lakiaroRoot[i,j].type == Lakiaro.Type.Pebble)
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
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckRoot2(mousePosition);
            inGame_UI.UpdateRemainText(currRemainDirt, currRemainRoot, currRemainPebble);
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            Debug.Log("a");
            test();
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
        GenerateRoot(level); // 뿌리 생성
        GeneratePebble(Random.Range(0, 16)); // 뿌리가 안지나가는 공간에 랜덤으로 자갈 생성
        
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

    public void GenerateRoot(int root = 1)
    {
        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
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
        }
        
        //gamePause = false;
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
