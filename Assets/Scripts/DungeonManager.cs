using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DungeonManager : SingletonMonoBehaviour<DungeonManager>
{
    const int minRoomSize = 6;
    const int maxRoomSize = 10;

    Room[,] room;

	//＊＊＊＊＊＊＊＊＊＊　　オブジェクト　　＊＊＊＊＊＊＊＊＊＊
    public GameObject player;
    public GameObject[] enemy;

    //＊＊＊＊＊＊＊＊＊＊　　prefab　　＊＊＊＊＊＊＊＊＊＊
    public GameObject square;			//床のprefab
    public GameObject pathSquare;		//小路のprefab




    public override void Awake()
    {
        base.Awake();

        DontDestroyOnLoad(this.gameObject);

        Floor.Instance.setFloor(3, 4);
		Floor.Instance.createTest();
    }

    //＊＊＊＊＊＊＊＊＊＊　　Floorクラス　　＊＊＊＊＊＊＊＊＊＊
    public class Floor
    {
		public bool inPreparation = false;

        //シングルトン
        Floor() { }
        public static Floor instance = new Floor();
        public static Floor Instance
        {
            get { return instance; }
        }

        public Room[,] room;

        Size size;
        public class Size
        {
            public int wide;
            public int height;

            Size(int x, int y)
            {
                wide = x;
                height = y;
            }
            public static Size createSize(int x, int y) { return new Size(x, y); }
        }

        public void setFloor(int x, int y)
        {
            size = Size.createSize(x, y);
            room = new Room[size.wide + 1, size.height + 1];
        }

        public void createTest()
        {
            for (int n = 0; n < size.height; n++)
            {
                for (int m = 0; m < size.wide; m++)
                {
                    room[m, n] = Room.createRoomTest(size.wide, size.height, m, n);
                }
            }

            int tmpX = Random.Range(0, size.wide);
            int tmpY = Random.Range(0, size.height);
            room[tmpX, tmpY].createStage();
			room[tmpX, tmpY].randomizeToSquare2(DungeonManager.Instance.player);
			for (int i=0; i<DungeonManager.Instance.enemy.Length; i++) {
				room[tmpX, tmpY].randomizeToSquare2 (DungeonManager.Instance.enemy[i]);
			}
        }

        public void createNext(int x, int y)
        {
			if (room[x, y].isBuild == false)
			{
				room[x, y].createStage();
			}          
        }

        public void destroyPrevious(int x, int y)
        {
            room[x, y].destroyStage();
        }

		public void randomizeToSquare(int x, int y, GameObject obj)
		{
			room [x, y].randomizeToSquare2 (obj);
		}

	}

    //＊＊＊＊＊＊＊＊＊＊　　Roomクラス　　＊＊＊＊＊＊＊＊＊＊
    public class Room
    {
        public Size size;
        public Position position;
        public Path path;

        public bool isBuild = false;

        //プレイヤーがいるかどうか
        public bool isPlayerIn;

        //大きさクラス
        public class Size
        {
            public int wide;
            public int height;

            protected Size(int x, int y)
            {
                wide = x;
                height = y;
            }
            public static Size randomCreateSize()
            {
                return new Size(Random.Range(minRoomSize, maxRoomSize + 1),
                                Random.Range(minRoomSize, maxRoomSize + 1));
            }
        }

        //位置クラス
        public class Position
        {
            public int maxRow;
            public int maxColumn;
            public int row;				//横（列）
            public int column;			//縦（行）

            //跡でずれを導入（shiftRow）

            //コンストラクタ
            protected Position(int maxX, int maxY, int x, int y)
            {
                maxRow = maxX;
                maxColumn = maxY;
                row = x;
                column = y;
            }
            //インスタンスの作成
            public static Position createPosition(int maxX, int maxY, int x, int y)
            {
                return new Position(maxX, maxY, x, y);
            }
        }

        //小路クラス
        public class Path
        {
            public int up;
            public int down;
            public int left;
            public int right;

            protected Path(int u, int d, int l, int r)
            {
                up = u;
                down = d;
                left = l;
                right = r;
            }
            public static Path randomCreatePath(Size tempSize)
            {
                return new Path(Random.Range(0, tempSize.height), Random.Range(0, tempSize.height),
                                Random.Range(0, tempSize.wide), Random.Range(0, tempSize.wide));
            }
        }

        //コンストラクタ（サイズ、ポジション）
        protected Room(Size tempSize, Position tempPosition)
        {
            size = tempSize;
            position = tempPosition;
        }
        public static Room createRoomTest(int maxX, int maxY, int x, int y)
        {
            var room = new Room(Size.randomCreateSize(), Position.createPosition(maxX, maxY, x, y));
            room.path = Path.randomCreatePath(room.size);
            return room;
        }

        //＊＊＊＊＊テスト、ステージの生成＊＊＊＊＊
        public void createStage()
        {
            for (int n = 0; n < size.wide; n++)
            {
                for (int m = 0; m < size.height; m++)
                {
                    int x = (m + position.row * (maxRoomSize + 3)) * 10;
                    int z = (n + position.column * (maxRoomSize + 3)) * 10;

					GameObject tmp = Instantiate(DungeonManager.Instance.square, new Vector3(x, 50, z), Quaternion.identity) as GameObject;
					tmp.GetComponent<AbstractSquare>().position = AbstractSquare.Position.createPosition(position.row,position.column);
                }
            }
            createStagePath();
            isBuild = true;
        }


        void createStagePath()
        {
            GameObject tmp;
            int tmpX;
            int tmpZ;

            //up
            if (position.column + 1 != position.maxColumn)
            {
                tmpX = (path.up + position.row * (maxRoomSize + 3)) * 10;
                tmpZ = (size.wide + position.column * (maxRoomSize + 3)) * 10;
                tmp = Instantiate(DungeonManager.Instance.pathSquare, new Vector3(tmpX, 50, tmpZ), Quaternion.identity) as GameObject;
                tmp.GetComponent<Renderer>().material.color = Color.blue;
                tmp.GetComponent<PathSquare>().setPathSquare(position.row, position.column, 0, 1, PathSquare.Direction.up);
				tmp.GetComponent<PathSquare>().type = AbstractSquare.Type.Path;
            }

            //down
            if (position.column != 0)
            {
                tmpX = (path.down + position.row * (maxRoomSize + 3)) * 10;
                tmpZ = -10 + (position.column * (maxRoomSize + 3)) * 10;
                tmp = Instantiate(DungeonManager.Instance.pathSquare, new Vector3(tmpX, 50, tmpZ), Quaternion.identity) as GameObject;
                tmp.GetComponent<Renderer>().material.color = Color.blue;
                tmp.GetComponent<PathSquare>().setPathSquare(position.row, position.column, 0, -1, PathSquare.Direction.down);
            	tmp.GetComponent<PathSquare>().type = AbstractSquare.Type.Path;
			}

            //left
            if (position.row != 0)
            {
                tmpX = (position.row * (maxRoomSize + 3) - 1) * 10;
                tmpZ = (path.left + position.column * (maxRoomSize + 3)) * 10;
                tmp = Instantiate(DungeonManager.Instance.pathSquare, new Vector3(tmpX, 50, tmpZ), Quaternion.identity) as GameObject;
                tmp.GetComponent<Renderer>().material.color = Color.blue;
                tmp.GetComponent<PathSquare>().setPathSquare(position.row, position.column, -1, 0, PathSquare.Direction.left);
				tmp.GetComponent<PathSquare>().type = AbstractSquare.Type.Path;
            }

            //right
            if (position.row + 1 != position.maxRow)
            {
                tmpX = (size.height + position.row * (maxRoomSize + 3)) * 10;
                tmpZ = (path.left + position.column * (maxRoomSize + 3)) * 10;
                tmp = Instantiate(DungeonManager.Instance.pathSquare, new Vector3(tmpX, 50, tmpZ), Quaternion.identity) as GameObject;
                tmp.GetComponent<Renderer>().material.color = Color.blue;
                tmp.GetComponent<PathSquare>().setPathSquare(position.row, position.column, 1, 0, PathSquare.Direction.right);
				tmp.GetComponent<PathSquare>().type = AbstractSquare.Type.Path;
            }
        }

        public void destroyStage()
        {
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Square"))
            {
                Destroy(obj);
            }
			//実体化フラグをオフ
            isBuild = false;
        }

		//オブジェクトをランダムなマスに配置
		public void randomizeToSquare2(GameObject obj)
		{
			ObjectManager.Instance.setSquare ();

			var tmpSquare = new List<GameObject> ();
			foreach (var check in ObjectManager.Instance.square)
			{
				if(checkRoomAndSquare(check.GetComponent<AbstractSquare>()))
				{
					tmpSquare.Add(check);
				}
			}
			var tmpObj = tmpSquare [Random.Range (0, tmpSquare.Count)];

			if (tmpObj.GetComponent<AbstractSquare> ().type == AbstractSquare.Type.Path)
			{
				Debug.Log("もう一回！");
				randomizeToSquare2(obj);
			}
			
			var tmpX = tmpObj.transform.position.x;
			var tmpZ = tmpObj.transform.position.z;
			int count = 0;
			
			foreach (GameObject check in GameObject.FindGameObjectsWithTag("Character"))
			{
				if(obj != check){
					if(check.transform.position.x == tmpX && check.transform.position.z == tmpZ)
					{
						count++;
					}
				}
			}
			if(count>0)
			{
				Debug.Log("重複nyaa!");
				randomizeToSquare2(obj);
			}
			else obj.transform.position = new Vector3 (tmpX, 55f, tmpZ);
			
		}

		bool checkRoomAndSquare(AbstractSquare square)
		{
			if (square.position.row == this.position.row)
			{
				if(square.position.column == this.position.column)
				{
					return true;
				}
			}
			return false;
		}
    }

    //＊＊＊＊＊＊＊＊＊＊　　Roomクラスここまで　　＊＊＊＊＊＊＊＊＊＊

}
