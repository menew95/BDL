using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
    [System.Serializable]
    public class LakiaroInfoData
    {
        [SerializeField]
        Vector3 pos; // 라키아로 버튼 UI 위치(앵커포지션 기준)
        [SerializeField]
        int lakiaroLevel; // 라키아로 레벨
        [SerializeField]
        string generateTime; // 라키아로 생성 시간
        [SerializeField]
        int coolTime = 0; // 라키아로 캔후 재생성까지 걸릴 시간
        [SerializeField]
        bool isDig = false; // 수확이 끝난 라키아로 확인
        [SerializeField]
        bool currDigging = false; // 진행중이던 게임 확인

        public LakiaroInfoData()
        {
            coolTime = 0;
            IsDig = true;
            CurrDigging = false;
        }

        public LakiaroInfoData(Vector3 _pos, int _lakiaroLevel)
        {
            pos = _pos;
            lakiaroLevel = _lakiaroLevel;
            coolTime = 0;
            IsDig = false;
        }

        public Vector3 Pos { get => pos; set => pos = value; }
        public int LakiaroLevel { get => lakiaroLevel; set => lakiaroLevel = value; }
        public int CoolTime { get => coolTime; set => coolTime = value; }
        public string GenerateTime { get => generateTime; set => generateTime = value; }
        public bool IsDig { get => isDig; set => isDig = value; }
        public bool CurrDigging { get => currDigging; set => currDigging = value; }
    }


    [System.Serializable]
    public class Lakiaro
    {
        public enum Type
        {
            Dirt,
            Root,
            Pebble,
            Flower
        }
        public Type type;

        public bool isChecked = false;

        public void Init()
        {
            isChecked = false;
        }

        public Lakiaro()
        {

        }

        public Lakiaro(Lakiaro _lakiaro)
        {
            this.type = _lakiaro.type;
            this.isChecked = _lakiaro.isChecked;
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
    public class Flower : Lakiaro
    {
        public Flower()
        {
            this.type = Type.Flower;
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

        public Root(Direction _direction, int _currRootx, int _currRooty)
        {
            this.type = Type.Root;

            direction = _direction;
            currRootx = _currRootx;
            currRooty = _currRooty;
        }

        public Root(int _rootState = 0, Direction _direction = Direction.아래쪽_오른쪽, int _startPoint = 0, int _finishPoint = 0, int _preRootx = 0, int _preRooty = 0, int _currRootx = 0, int _currRooty = 0, int _nextRootx = 0, int _nextRooty = 0)
        {
            this.type = Type.Root;

            rootState = _rootState;
            direction = _direction;
            startPoint = _startPoint;
            finishPoint = _finishPoint;
            preRootx = _preRootx;
            preRooty = _preRooty;
            currRootx = _currRootx;
            currRooty = _currRooty;
            nextRootx = _nextRootx;
            nextRooty = _nextRooty;
        }

        public int rootState = 0;//0 > start 1 > middle 2 > finish
        public enum Direction // 시작_끝
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
        [SerializeField]
        private Direction direction;
        public Direction GetDirection { get => direction; set => direction = value; }

        public int startPoint;
        public int finishPoint;

        public Vector3Int GetPreRoot()
        {
            return new Vector3Int(preRootx, preRooty, 0);
        }
        public void SetPreRoot(Vector3Int pos)
        {
            preRootx = pos.x;
            preRooty = pos.y;
        }
        public Vector3Int GetCurrRoot()
        {
            return new Vector3Int(currRootx, currRooty, 0);
        }
        public void SetCurrRoot(Vector3Int pos)
        {
            currRootx = pos.x;
            currRooty = pos.y;
        }
        public Vector3Int GetNextRoot()
        {
            return new Vector3Int(nextRootx, nextRooty, 0);
        }
        public void SetNextRoot(Vector3Int pos)
        {
            nextRootx = pos.x;
            nextRooty = pos.y;
        }

        [SerializeField]
        private int preRootx, preRooty;
        [SerializeField]
        private int currRootx, currRooty;
        [SerializeField]
        private int nextRootx, nextRooty;

        public Root(Root _root)
        {
            this.rootState = _root.rootState;
            this.direction = _root.direction;
            GetDirection = _root.GetDirection;
            this.startPoint = _root.startPoint;
            this.finishPoint = _root.finishPoint;
            this.preRootx = _root.preRootx;
            this.preRooty = _root.preRooty;
            this.currRootx = _root.currRootx;
            this.currRooty = _root.currRooty;
            this.nextRootx = _root.nextRootx;
            this.nextRooty = _root.nextRooty;
        }
    }
}
