using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Lakiaro
{
    public enum Type
    {
        Root,
        Pebble
    }
    public Type type;
}
public class Pebble : Lakiaro
{
    public Pebble()
    {
        this.type = Type.Pebble;
    }
}

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

    public Tilemap lakiaroRootTileMap, lakiaroDirtTileMap_Lower, lakiaroDirtTilemap_Upper;
    public Lakiaro[,] lakiaroRoot = new Lakiaro[12,12];

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

    Vector3 mousePosition;
    // Update is called once per frame
    void Update()
    {
        if (gamePause) return;
        if (Input.GetMouseButtonUp(1))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Debug.Log(mousePosition);
            if (CheckRoot(mousePosition)) ;
            inGame_UI.UpdateRemainText(currRemainDirt, currRemainRoot, currRemainPebble);
        }
        else if(Input.GetMouseButtonUp(0))
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CheckRoot2(mousePosition);
            inGame_UI.UpdateRemainText(currRemainDirt, currRemainRoot, currRemainPebble);
        }
    }

    public void GenerateRoot(int root = 1, int pebble = 1)
    {
        gamePause = true;
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

        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            for (int j = 0; j < lakiaroRoot.GetLength(1); j++)
            {
                if(Random.Range(0,4) == 1)
                {
                    if (i < 8 && i > 3 && j > 3 && j < 8) continue;
                    lakiaroRoot[i,j] = new Root();
                    currRemainDirt--;
                    currRemainRoot++;
                    vector3Int.x = i;
                    vector3Int.y = j;
                    lakiaroRootTileMap.SetTile(vector3Int, temp);
                }
            }
        }
        GenerateDirt();
        GeneratePebble(Random.Range(0, 20));
        inGame_UI.UpdateRemainText(currRemainDirt, currRemainRoot, currRemainPebble);


        //gamePause = false;
    }

    Vector3Int vector3Int = new Vector3Int();
    public bool CheckRoot(Vector2 point)
    {
        bool isRoot = false;

        Debug.Log(point);
        if (lakiaroRoot[(int)point.x, (int)point.y] != null)
        {
            if (lakiaroRoot[(int)point.x, (int)point.y].type == Lakiaro.Type.Root)
            {
                isRoot = true;
                currRemainRoot--;

                vector3Int.x = (int)point.x;
                vector3Int.y = (int)point.y;
                lakiaroDirtTilemap_Upper.SetTile(vector3Int, null);
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
        bool isDirt = false;

        int manosHoeLevel = 7, currCheck = 0;

        for (int y = 1; y > -2; y--)
        {
            for (int x = -1; x < 2; x++)
            {
                vector3Int.x = (int)point.x + x;
                vector3Int.y = (int)point.y + y;
                if (vector3Int.x < 8 && vector3Int.x > 3 && vector3Int.y > 3 && vector3Int.y < 8) continue;

                if (0 <= vector3Int.x && vector3Int.x < 12 && 0 <= vector3Int.y && vector3Int.y < 12)
                {
                    if (lakiaroRoot[vector3Int.x, vector3Int.y] == null)
                    {
                        currCheck++;
                        currRemainDirt--;
                        lakiaroDirtTilemap_Upper.SetTile(vector3Int, null);
                    }
                    else
                    {
                        if(x == 0 && y == 0)
                        {
                            currCheck++;
                            if (lakiaroRoot[vector3Int.x, vector3Int.y].type == Lakiaro.Type.Pebble)
                            {
                                currRemainPebble--;
                            }
                            else if(lakiaroRoot[vector3Int.x, vector3Int.y].type == Lakiaro.Type.Root)
                            {
                                currRemainRoot--;
                            }

                            lakiaroDirtTilemap_Upper.SetTile(vector3Int, null);
                        }
                    }
                    
                }

                if (manosHoeLevel == currCheck) break;
            }
        }

        if (currRemainDirt == 0)
        {
            UIManager.Instance.CallMainUI();
            InitRoot();
            gamePause = true;
        }

        return isDirt;
    }

        public void GenerateDirt(int level = 0)
    {
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

    public void GeneratePebble(int pebble = 0)
    {
        int currPebble = 0, x, y;
        while (currPebble < pebble)
        {
            x = Random.Range(0, lakiaroRoot.GetLength(0));
            y = Random.Range(0, lakiaroRoot.GetLength(1));
            if (lakiaroRoot[x, y] == null)
            {
                currPebble++;
                lakiaroRoot[x, y] = new Pebble();

                vector3Int.x = x;
                vector3Int.y = y;
                lakiaroRootTileMap.SetTile(vector3Int, pebbleTile[Random.Range(0, pebbleTile.Count)]);
                currRemainPebble++;
                currRemainDirt--;
            }
        }
    }

    public void InitRoot()
    {
        currRemainDirt = 128;
        currRemainRoot = 0;
        currRemainPebble = 0;
        for (int i = 0; i < lakiaroRoot.GetLength(0); i++)
        {
            for (int j = 0; i < lakiaroRoot.GetLength(1); j++)
            {
                lakiaroRoot[i, j] = null;
            }
        }
    }
}
