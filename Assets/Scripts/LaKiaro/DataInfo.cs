using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DataInfo
{
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

        private int preRootx, preRooty;
        private int currRootx, currRooty;
        private int nextRootx, nextRooty;
    }
}
