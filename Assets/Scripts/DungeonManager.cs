using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Linq;
using MyUtility;

public class DungeonManager : SingletonMonoBehaviour<DungeonManager>
{
    //id
    public int id;

    //部屋の数
    public static int sequenceSizeX;
    public static int sequenceSizeY;

    //部屋の最大サイズ
    static int minRoomSize;
    static int maxRoomSize;

    //データをロードしてパラメータを作成
    public void getDungeonData(int number)
    {
        XmlNodeList nodes = XMLReader.Instance.dungeonNodes;
        XmlNode tempNode = nodes [number];

        int count = 0;
        Floor.Instance.enemyDictionary.Clear();

        id = int.Parse(tempNode.Attributes.GetNamedItem("id").Value);

        foreach (XmlNode node in tempNode.ChildNodes)
        {
            if (node.Name == "floorSizeX")
            {
                sequenceSizeX = int.Parse(node.InnerText);
            }
            if (node.Name == "floorSizeY")
            {
                sequenceSizeY = int.Parse(node.InnerText);
            }
            if (node.Name == "minRoomSize")
            {
                minRoomSize = int.Parse(node.InnerText);
            }
            if (node.Name == "maxRoomSize")
            {
                maxRoomSize = int.Parse(node.InnerText);
            }
            //enemyDictionaryの作成
            if (node.Name == "enemy")
            {
                int EnemyID = int.Parse(node.Attributes.GetNamedItem("EnemyID").Value);
                int frequency = int.Parse(node.Attributes.GetNamedItem("frequency").Value);

                Floor.Instance.enemyDictionary.Add(EnemyID, frequency);
                Debug.Log("idは" + Floor.Instance.enemyDictionary.ElementAt(count).Key);
                Debug.Log("出現率は" + Floor.Instance.enemyDictionary.ElementAt(count).Value);
                count++;
            }
        }
    }

    public Room[,] room;

    public override void Awake()
    {
        base.Awake();
    }

    /// <summary>
    /// ダンジョンの階層ひとつを管理するシングルトンクラス
    /// </summary>
    public class Floor : Singleton<Floor>
    {
        //部屋クラスを管理
        public Pillar[,] pillar;
        public Room[,] room;

        //部屋の並びのサイズ
        public MyVector2 sequenceSize;

        //ダンジョンに出現する敵の管理（ID、確率）
        public Dictionary<int, int> enemyDictionary = new Dictionary<int, int>();
        //ダンジョンに出現するアイテムの管理（ID、確率）
        //      public Dictionary<int, int> itemDictionary = new Dictionary<int, int>();

        public void setFloor(int x, int y)
        {
            sequenceSize = new MyVector2(x, y);
            Debug.Log(sequenceSize.x + "と" + sequenceSize.y);
            pillar = new Pillar[sequenceSize.x, sequenceSize.y];
            room = new Room[sequenceSize.x, sequenceSize.y];
        }

        //一番最初の処理
        public void createTest()
        {
            //予め部屋のデータを登録
            for (int n = 0; n < sequenceSize.y; n++)
            {
                for (int m = 0; m < sequenceSize.x; m++)
                {
                    room [m, n] = Room.createRoomTest(m, n);
                    pillar [m, n] = new Pillar();
                }
            }

            Floor.Instance.breakPathFlag();

            //部屋をランダムに選ぶ
            int tmpX = Random.Range(0, sequenceSize.x);
            int tmpY = Random.Range(0, sequenceSize.y);


            //テスト
            for (int n = 0; n < sequenceSize.y; n ++)
            {
                for (int m = 0; m < sequenceSize.x; m ++)
                {
                    room [m, n].createStage();
                }
            }

            //選ばれた部屋を実体化
            //           room[tmpX, tmpY].createStage();

            room [tmpX, tmpY].randomizeToSquare2(ObjectManager.Instance.character [0]);
            for (int i = 1; i < ObjectManager.Instance.character.Count(); i++)
            {
                room [tmpX, tmpY].randomizeToSquare2(ObjectManager.Instance.character [i]);
            }
        }

        public void createNext(MyVector2 sequence)
        {
            if (room [sequence.x, sequence.y].isBuild == false)
            {
                room [sequence.x, sequence.y].createStage();
            }
        }

        public void destroyPrevious(MyVector2 sequence)
        {
            room [sequence.x, sequence.y].destroyStage();
        }

        public void randomizeToSquare(MyVector2 sequence, GameObject obj)
        {
            room [sequence.x, sequence.y].randomizeToSquare2(obj);
        }

        public int getRandomEnemy()
        {
            int accumulation = 0;
            int randomNumber = Random.Range(1, enemyDictionary.Values.Sum());
            for (int i = 0; i < enemyDictionary.Count; i++)
            {
                if ((enemyDictionary.ElementAt(i).Value + accumulation) >= randomNumber)
                {
                    return enemyDictionary.ElementAt(i).Key;
                }
                accumulation += enemyDictionary.ElementAt(i).Value;
            }
            return 0;
        }

        //用確認
        public void prepareEnemy()
        {
            //ランダムに部屋を選ぶ
            int tmpX = Random.Range(0, sequenceSize.x);
            int tmpY = Random.Range(0, sequenceSize.y);

            int i = getRandomEnemy();
            room [tmpX, tmpY].enemyList.Add(new EnemyContainer(i));
        }





        //小路のフラグを折る
        public void breakPathFlag()
        {
            for (int n = 0; n < sequenceSize.y-1; n++)
            {
                for (int m = 0; m < sequenceSize.x-1; m++)
                {

                    if(pillar[m, n].direction == Pillar.Direction.up)
                    {
                        room[m,n].path.right = -1;
                        room[m + 1,n].path.left = -1;
                    }

                    if(pillar[m, n].direction == Pillar.Direction.down)
                    {
                        room[m,n+1].path.right = -1;
                        room[m+1,n+1].path.left = -1;
                    }

                    if(pillar[m, n].direction == Pillar.Direction.left)
                    {
                        room[m,n].path.up = -1;
                        room[m,n+1].path.down = -1;
                    }
                    if(pillar[m, n].direction == Pillar.Direction.right)
                    {
                        room[m+1,n].path.up = -1;
                        room[m+1,n+1].path.down = -1;
                    }
                }
            }
        }





    }

    /// <summary>
    /// ダンジョン作成用の柱
    /// </summary>
    public class Pillar
    {
        public Pillar()
        {
            direction = (Direction)Random.Range(0, 4);
            Debug.Log(direction);
        }
        public enum Direction
        {
            up = 0,
            down,
            left,
            right };
        public Direction direction;
    }

    /// <summary>
    /// 階層内の部屋を管理する各クラス
    /// </summary>
    public class Room
    {
        public List<EnemyContainer> enemyList = new List<EnemyContainer>();
        public List<ItemContainer> itemList = new List<ItemContainer>();

        //大きさ
        public MyVector2 size;

        public static MyVector2 randomCreateSize()
        {
            return new MyVector2(Random.Range(minRoomSize, maxRoomSize + 1), Random.Range(minRoomSize, maxRoomSize + 1));
        }

        //シークエンス中の位置
        public MyVector2 sequence;

        public static MyVector2 createPosition(int x, int y)
        {
            return new MyVector2(x, y);
        }


        //小路クラス
        public Path path;
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

            public static Path randomCreatePath(MyVector2 tempSize)
            {
                return new Path(Random.Range(0, tempSize.y), Random.Range(0, tempSize.y),
                                Random.Range(0, tempSize.x), Random.Range(0, tempSize.x));
            }
        }



        //実体化しているかどうか
        public bool isBuild = false;

        //プレイヤーがいるかどうか
        public bool isPlayerIn;

        //コンストラクタ（サイズ、ポジション）
        protected Room(MyVector2 size, MyVector2 position)
        {
            this.size = size;
            this.sequence = position;
        }

        public static Room createRoomTest(int x, int y)
        {
            var room = new Room(randomCreateSize(), createPosition(x, y));
            room.path = Path.randomCreatePath(room.size);


            room.itemList.Add(new ItemContainer((int)ItemContainer.type.Sword));
            room.itemList.Add(new ItemContainer((int)ItemContainer.type.Flower));
            Debug.Log(room.itemList [1].item);

            return room;
        }

        //＊＊＊＊＊テスト、ステージの生成＊＊＊＊＊
        public void createStage()
        {
            GameObject square = PrefabManager.Instance.square;
            for (int n = 0; n < size.x; n++)
            {
                for (int m = 0; m < size.y; m++)
                {
                    int x = (m + sequence.x * (maxRoomSize + 3)) * 10;
                    int z = (n + sequence.y * (maxRoomSize + 3)) * 10;
                    GameObject tmp = Instantiate(square, new Vector3(x, 50, z), Quaternion.identity) as GameObject;
                    tmp.GetComponent<AbstractSquare>().sequence = new MyVector2(sequence.x, sequence.y);
                }
            }
            createStagePath();

            //敵の生成
            if (enemyList.Count() != 0)
            {
                foreach (var n in enemyList)
                {
                    var obj = Instantiate(n.prefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
                    obj.GetComponent<AbstractCharacterObject>().parameter = n.parameter;
                    Debug.Log(n.parameter.cName + "ですにゃあ");
                    Debug.Log(obj.GetComponent<AbstractCharacterObject>().parameter.cName + "ですにゃん");
                    randomizeToSquare2(obj);
                }
            }
            enemyList.Clear();

            //アイテムの生成
            if (itemList.Count() != 0)
            {
                foreach (var n in itemList)
                {
                    if (n.vector3 != new Vector3(-1, -1, -1))
                    {
                        var obj = Instantiate(PrefabManager.Instance.item, n.vector3, Quaternion.identity) as GameObject;
                        obj.GetComponent<FieldItem>().item = n.item;
                    }
                    if (n.vector3 == new Vector3(-1, -1, -1))
                    {
                        var obj = Instantiate(PrefabManager.Instance.item) as GameObject;
                        obj.GetComponent<FieldItem>().item = n.item;
                        randomizeToSquare2(obj);
                    }
                }
            }
            itemList.Clear();

            isBuild = true;
        }

        //小路を設置
        void createStagePath()
        {
            GameObject pathSquare = PrefabManager.Instance.pathSquare;
            GameObject tmp;

            int tmpX;
            int tmpZ;

            //up
            if (sequence.y != Floor.Instance.sequenceSize.y - 1 && path.up >= 0)
            {
                tmpX = (path.up + sequence.x * (maxRoomSize + 3)) * 10;
                tmpZ = (size.x + sequence.y * (maxRoomSize + 3)) * 10;
                tmp = Instantiate(pathSquare, new Vector3(tmpX, 50, tmpZ), Quaternion.identity) as GameObject;
                tmp.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 1.0f, 1.0f);
                tmp.GetComponent<PathSquare>().setPathSquare(sequence.x, sequence.y, 0, 1, PathSquare.Direction.up);
                tmp.GetComponent<PathSquare>().type = AbstractSquare.Type.Path;
            }

            //down
            if (sequence.y != 0 && path.down >= 0)
            {
                tmpX = (path.down + sequence.x * (maxRoomSize + 3)) * 10;
                tmpZ = -10 + (sequence.y * (maxRoomSize + 3)) * 10;
                tmp = Instantiate(pathSquare, new Vector3(tmpX, 50, tmpZ), Quaternion.identity) as GameObject;
                tmp.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 1.0f, 1.0f);
                tmp.GetComponent<PathSquare>().setPathSquare(sequence.x, sequence.y, 0, -1, PathSquare.Direction.down);
                tmp.GetComponent<PathSquare>().type = AbstractSquare.Type.Path;
            }

            //left
            if (sequence.x != 0 && path.left >= 0)
            {
                tmpX = (sequence.x * (maxRoomSize + 3) - 1) * 10;
                tmpZ = (path.left + sequence.y * (maxRoomSize + 3)) * 10;
                tmp = Instantiate(pathSquare, new Vector3(tmpX, 50, tmpZ), Quaternion.identity) as GameObject;
                tmp.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 1.0f, 1.0f);
                tmp.GetComponent<PathSquare>().setPathSquare(sequence.x, sequence.y, -1, 0, PathSquare.Direction.left);
                tmp.GetComponent<PathSquare>().type = AbstractSquare.Type.Path;
            }

            //right
            if (sequence.x != Floor.Instance.sequenceSize.x - 1 && path.right >= 0)
            {
                tmpX = (size.y + sequence.x * (maxRoomSize + 3)) * 10;
                tmpZ = (path.right + sequence.y * (maxRoomSize + 3)) * 10;
                tmp = Instantiate(pathSquare, new Vector3(tmpX, 50, tmpZ), Quaternion.identity) as GameObject;
                tmp.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 1.0f, 1.0f);
                tmp.GetComponent<PathSquare>().setPathSquare(sequence.x, sequence.y, 1, 0, PathSquare.Direction.right);
                tmp.GetComponent<PathSquare>().type = AbstractSquare.Type.Path;
            }
        }

        //ステージを廃棄
        public void destroyStage()
        {
            //床を消す
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Square"))
            {
                Destroy(obj);
            }

            //敵を消す
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Character"))
            {
                if (obj.GetComponent<AbstractCharacterObject>().type == AbstractCharacterObject.Type.Enemy)
                {
                    var i = obj.GetComponent<AbstractCharacterObject>().parameter;
                    enemyList.Add(new EnemyContainer(i));
                    Destroy(obj);
                }
            }

            //アイテムを消す
            foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Item"))
            {
                Debug.Log(GameObject.FindGameObjectsWithTag("Item").Count());
                var i = obj.GetComponent<FieldItem>().item;
                var v = obj.transform.position;
                itemList.Add(new ItemContainer(i, v));
                Destroy(obj);
            }

            //罠を消す



            //実体化フラグをオフ
            isBuild = false;
        }

        //オブジェクトをランダムなマスに配置
        public void randomizeToSquare2(GameObject obj)
        {
            //すべての床を取得
            ObjectManager.Instance.setSquare();

            //床のリスト
            var tmpSquare = from n in ObjectManager.Instance.square
                where n.GetComponent<AbstractSquare>().type == AbstractSquare.Type.Normal
                && n.GetComponent<AbstractSquare>().sequence.isEqual(this.sequence)
                && n.GetComponent<AbstractSquare>().isCharacterOn() == false
                && n.GetComponent<AbstractSquare>().isItemOn() == false
                    select n;

            //床のリストからランダムに取得
            var tmpObj = tmpSquare.ElementAt(Random.Range(0, tmpSquare.Count()));

            //取得した位置へ移動
            obj.transform.position = new Vector3(tmpObj.transform.position.x, 55f, tmpObj.transform.position.z);
        }

        //＊＊＊＊＊＊＊＊＊＊　　Roomクラスここまで　　＊＊＊＊＊＊＊＊＊＊
    }
}
