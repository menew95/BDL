using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Root
{
    public Root()
    {

    }

    private bool isFirstRoot = false;
    private bool isFinishRoot = false;
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

    public Tilemap LakiaroTileMap;

    public Root[,] LakiaroRoot = new Root[12,12];

    [SerializeField]
    private int currRemainDirt, currRemainRoot, currRemainPebble;

    public TileBase temp;

    public List<TileBase> basicTile = new List<TileBase>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (CheckRoot(Input.mousePosition))
            {
                Debug.Log((int)Input.mousePosition.x + " : " + (int)Input.mousePosition.y + " 는 뿌리이다.");
            }
            else
            {
                Debug.Log((int)Input.mousePosition.x + " : " + (int)Input.mousePosition.y + " 는 뿌리가 아니다.");
            }
        }
    }

    public void GenerateRoot(int root = 1, int pebble = 1)
    {
        for (int i = 0; i < LakiaroRoot.Length; i++)
        {
            for (int j = 0; i < LakiaroRoot.Length; j++)
            {
                if (i < 8 && i > 3 && j > 3 && j < 8) continue;
                if (Random.Range(0, 2) == 1)
                {
                    vector3Int.x = i;
                    vector3Int.y = j;
                    LakiaroTileMap.SetTile(vector3Int, basicTile[Random.Range(0, 7)]);
                }
            }
        }

        for (int i = 0; i < LakiaroRoot.GetLength(0); i++)
        {
            for (int j = 0; j < LakiaroRoot.GetLength(1); j++)
            {
                if(Random.Range(0,2) == 1)
                {
                    if (i < 8 && i > 3 && j > 3 && j < 8) continue;
                    Debug.Log(i + " : " + j + LakiaroRoot.GetLength(0)+" : " + LakiaroRoot.GetLength(1));
                    LakiaroRoot[i,j] = new Root();
                    currRemainDirt--;
                    currRemainRoot++;
                }
            }
        }
    }

    Vector3Int vector3Int = new Vector3Int();
    public bool CheckRoot(Vector2 point)
    {
        bool isRoot = false;

        if (LakiaroRoot[(int)point.x, (int)point.y] != null) isRoot = true;

        vector3Int.x = (int)point.x;
        vector3Int.y = (int)point.y;
        if (isRoot) LakiaroTileMap.SetTile(vector3Int, temp);
        else LakiaroTileMap.SetTile(vector3Int, null);

        return isRoot;
    }

    public void InitRoot()
    {
        currRemainDirt = 128;
        currRemainRoot = 0;
        currRemainPebble = 0;
        for (int i = 0; i < LakiaroRoot.Length; i++)
        {
            for (int j = 0; i < LakiaroRoot.Length; j++)
            {
                if (Random.Range(0, 2) == 1)
                {
                    LakiaroRoot[i, j] = null;
                }
            }
        }
    }
}
