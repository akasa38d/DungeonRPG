using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FloorBuilder : MonoBehaviour
{
    GameObject Cube;

    GameObject Enemy;
    GameObject Enemy2;

    GameObject Panel;

    //区画の数（横、縦）
    int wide;
    //    int height;

    //一区画の大きさ（小部屋の最大サイズ）
    int maxRoomWide;
    int maxRoomHeight;

    //小部屋の最小サイズ
    const int minRoomWide = 6;
    const int minRoomHeight = 6;

    //区画の数（合計）
    int number;

    //小部屋のリスト
    List<TempFloor> room = new List<TempFloor>();

    //**********ここからクラスとかメソッド**********//

    //コンストラクタ
    public FloorBuilder() { }

    //小部屋クラス
    class TempFloor
    {
        //コンストラクタ
        public TempFloor() { }

        //小部屋の起点
        public int rX;
        public int rY;

        //小部屋の大きさ
        public int rWide;
        public int rHeight;
    }

    //処理
    public void operation(GameObject cube, GameObject enemy, GameObject enemy2, GameObject panel)
    {
        setObjects(cube, enemy, enemy2, panel);

        setFloor(4, 3);

        createFloor(15, 12);

        RandomizeFirstPosition(this.Cube);
        RandomizeFirstPosition(this.Enemy);
        RandomizeFirstPosition(this.Enemy2);

        createRoots();
    }

    //オブジェクトの取得
    void setObjects(GameObject cube, GameObject enemy, GameObject enemy2, GameObject panel)
    {
        this.Cube = cube;
        this.Enemy = enemy;
        this.Enemy2 = enemy2;
        this.Panel = panel;
    }

    //区画の設定
    void setFloor(int wide, int height)
    {
        //小部屋の数を決定
        number = wide * height;

        //部屋の数だけリストに加える
        for (int i = 0; i < number; i++)
        {
            room.Add(new TempFloor());
        }

        //部屋の数を設定
        this.wide = wide;
        //        this.height = height;
    }

    //**********createFloor**********//

    //小部屋を並べる
    void createFloor(int maxX, int maxY)
    {
        setMaxRoomSize(maxX, maxY);

        randomizeRoomSize();

        randomizeRoomPosition();

        //区画ループ
        for (int m = 0; m < number; m++)
        {
            createRoom(m);
        }
    }

    //一区画の大きさをセット
    void setMaxRoomSize(int x, int y)
    {
        maxRoomWide = x;
        maxRoomHeight = y;
    }

    //一区画内の小部屋の大きさをセット
    void randomizeRoomSize()
    {
        for (int i = 0; i < number; i++)
        {
            room[i].rWide = Random.Range(minRoomWide, maxRoomWide);
            room[i].rHeight = Random.Range(minRoomHeight, maxRoomHeight);
        }
    }

    //小部屋の起点をセット
    void randomizeRoomPosition()
    {
        for (int i = 0; i < number; i++)
        {
            //x座標
            int adjustX = (i % wide) * (maxRoomWide + 2);
            room[i].rX = Random.Range(0, maxRoomWide - room[i].rWide) + adjustX;
            //y座標
            int adjustY = (i / wide) * (maxRoomHeight + 2);
            room[i].rY = Random.Range(0, maxRoomHeight - room[i].rHeight) + adjustY;
        }
    }

    //小部屋の製作
    void createRoom(int i)
    {
        //小部屋、縦ループ
        for (int y = 0; y < room[i].rHeight; y++)
        {
            //小部屋、横ループ
            for (int x = 0; x < room[i].rWide; x++)
            {
                Instantiate(Panel, new Vector3(((x + room[i].rX)) * 10, 1, (y + room[i].rY) * 10), Quaternion.identity);
            }
        }
    }

    //重複の確認
    void test()
    {
        GameObject[] obj = GameObject.FindGameObjectsWithTag("floor");
        foreach (GameObject temp in obj)
        {
            int count = 0;

            foreach (GameObject Panel2 in GameObject.FindGameObjectsWithTag("floor"))
            {
                if (temp.transform.position == Panel2.transform.position)
                {
                    count++;
                    temp.GetComponent<Renderer>().material.color = Color.blue;
                }
                if (count >= 2)
                {
                    temp.GetComponent<Renderer>().material.color = Color.green;
                }
            }
        }
    }

    void RandomizeFirstPosition(GameObject obj)
    {
        //部屋番号をランダムで取得
        int i = Random.Range(0, number);
        int count = 0;

        //部屋の中の座標をランダムで取得
        int firstX = (room[i].rX + Random.Range(0, room[i].rWide)) * 10;
        int firstY = (room[i].rY + Random.Range(0, room[i].rHeight)) * 10;

        obj.transform.position = new Vector3((float)firstX, 5, (float)firstY);
        Debug.Log(obj.transform.position);

        //重複したらもう一回
        foreach (GameObject temp in GameObject.FindGameObjectsWithTag("Character"))
        {
            if (obj.transform.position == temp.transform.position)
            {
                count++;
            }
            if (count > 1)
            {
                RandomizeFirstPosition(obj);
            }
        }
    }

    //**********createRoots**********//

    //小部屋を繋ぐ道を製作
    void createRoots()
    {
        createRootsRL();
        createRootsUD();
        test();
    }

    //左右
    private void createRootsRL()
    {
        for (int i = 0; i < number - 1; i++)
        {
            //起点
            int right = room[i].rY + Random.Range(0, room[i].rHeight);
            int left = room[i + 1].rY + Random.Range(0, room[i + 1].rHeight);

            //起点から伸ばしていく変数
            int a = room[i].rX + room[i].rWide;
            int b = room[i + 1].rX - 1;

            //端を除く
            if (i % wide != wide - 1)
            {
                //一段階（直線）
                createRootsRL1st(right, left, ref a, ref b);

                //二段階（カーブ）
                if (right != left)
                {
                    createRootsRL2nd(right, left, a);
                }
            }
        }
    }

    //左右一段階
    private void createRootsRL1st(int right, int left, ref int a, ref int b)
    {
        while (b - a >= 0)
        {
            Instantiate(Panel, new Vector3(b * 10, 1, left * 10), Quaternion.identity);
            b--;
            if (b > a)
            {
                Instantiate(Panel, new Vector3(a * 10, 1, right * 10), Quaternion.identity);
                a++;
            }
        }
    }

    //左右二段階
    private void createRootsRL2nd(int right, int left, int turningPoint)
    {
        int count = 0;

        while (right - count != left)
        {
            Instantiate(Panel, new Vector3(turningPoint * 10, 1, (right - count) * 10), Quaternion.identity);
            if (right > left) count++;
            if (right < left) count--;
        }
    }

    //上下
    private void createRootsUD()
    {
        for (int i = 0; i < number - wide; i++)
        {
            //起点
            int up = room[i].rX + Random.Range(0, room[i].rWide);
            int down = room[i + wide].rX + Random.Range(0, room[i + wide].rWide);

            //起点から伸ばしていく変数
            int c = room[i].rY + room[i].rHeight;
            int d = room[i + wide].rY - 1;

            //端を除く
            if (i < number - wide)
            {
                //一段階（直線）
                createRootsUD1st(up, down, ref c, ref d);

                //二段階（カーブ）
                if (up != down)
                {
                    createRootsUD2nd(up, down, c);
                }
            }
        }
    }

    //上下一段階
    private void createRootsUD1st(int up, int down, ref int c, ref int d)
    {
        while (d - c >= 0)
        {
            Instantiate(Panel, new Vector3(down * 10, 1, d * 10), Quaternion.identity);
            d--;
            if (d - c > 0)
            {
                Instantiate(Panel, new Vector3(up * 10, 1, c * 10), Quaternion.identity);
                c++;
            }
        }
    }

    //上下二段階
    private void createRootsUD2nd(int up, int down, int turningPoint)
    {
        int count = 0;

        while (up - count != down)
        {
            Instantiate(Panel, new Vector3((up - count) * 10, 1, turningPoint * 10), Quaternion.identity);
            //カウント調整
            if (up > down) count++;
            if (up < down) count--;
        }
    }
}
