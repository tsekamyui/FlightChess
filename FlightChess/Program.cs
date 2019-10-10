using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightChess
{
    struct Vector2
    {
        public int x;
        public int y;
    }

    enum GameObjectType
    {
        Player1 = 0,
        Player2 = 1,
        Normal = 2,
        Bom = 3,
        Tube = 4,
        Roulette = 5,
        Pause = 6,

    }
    struct GameObject
    {
        public Vector2 pos;
        public string icon;
        public GameObjectType type;

        public void SetIconData()
        {
            switch (type)
            {
                case GameObjectType.Player1:
                    icon = "★";
                    break;
                case GameObjectType.Player2:
                    icon = "☆";
                    break;
                case GameObjectType.Normal:
                    icon = "□";
                    break;
                case GameObjectType.Bom:
                    icon = "▲";
                    break;
                case GameObjectType.Tube:
                    icon = "卐";
                    break;
                case GameObjectType.Roulette:
                    icon = "◎";
                    break;
                case GameObjectType.Pause:
                    icon = "×";
                    break;
                default:
                    break;
            }
        }

        public void Draw()
        {
            Console.SetCursorPosition(pos.x, pos.y);
            SetIconData();
            Console.Write(icon);
        }

    }
    struct Map
    {
        public int length;
        public GameObject[] mapData;
        public int index;
        public int[] dataMap;

        public void InitMap()
        {
            dataMap = new int[100];
            int[] luckyturn = { 6, 23, 40, 55, 69, 83 };
            int[] landMine = { 5, 13, 17, 33, 38, 50, 64, 80, 94 };
            int[] pause = { 9, 27, 60, 93 };
            int[] timeTunnel = { 20, 25, 45, 63, 72, 88, 90 };

            for (int i = 0; i < luckyturn.Length; i++)
            {
                dataMap[luckyturn[i]] = 1;
            }
            for (int i = 0; i < landMine.Length; i++)
            {
                dataMap[landMine[i]] = 2;
            }
            for (int i = 0; i < pause.Length; i++)
            {
                dataMap[pause[i]] = 3;
            }
            for (int i = 0; i < timeTunnel.Length; i++)
            {
                dataMap[timeTunnel[i]] = 4;
            }
        }
        
        public void GetMapLength(int count)
        {

            int sum = 0;
            for (int i = 1; i <= count; i++)
            {
                if (i % 4 == 1)
                {
                    sum += 20;
                }
                else if (i % 4 == 2)
                {
                    sum += 10;
                }
                else if (i % 4 == 3)
                {
                    sum += 20;
                }
                else if (i % 4 == 0)
                {
                    sum += 10;
                }
            }
            mapData = new GameObject[sum];
            length = sum;
        }

        public void CreateMapGameObject()
        {
            for (int i = 0; i < length; i++)
            {
                GameObject go = new GameObject();
                mapData[i] = go;
            }
        }

        public void SetMapGameObjectType()
        {
            Random r = new Random();
            for (int i = 0; i < length; i++)
            {
                int num = r.Next(1, 101);
                if (num <= 60)
                {
                    mapData[i].type = GameObjectType.Normal;
                } else if (num <= 70)
                {
                    mapData[i].type = GameObjectType.Bom;
                } else if (num <= 80)
                {
                    mapData[i].type = GameObjectType.Tube;
                } else if (num <= 90)
                {
                    mapData[i].type = GameObjectType.Roulette;
                } else if (num <= 100)
                {
                    mapData[i].type = GameObjectType.Pause;
                }
            }
        }

        public string DrawStringMap(int pos, ref Player p1, ref Player p2)
        {
            string temp = "";
            if (p1.index == p2.index && p1.index == pos)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                temp = "<>";
            }
            else if (p1.index == pos)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                temp = "★";
            }
            else if (p2.index == pos)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                temp = "☆";
            }
            else
            {
                switch (dataMap[pos])
                {
                    case 0:
                        Console.ForegroundColor = ConsoleColor.White;
                        temp = "□"; break;
                    case 1:
                        Console.ForegroundColor = ConsoleColor.Red;
                        temp = "◎"; break;
                    case 2:
                        Console.ForegroundColor = ConsoleColor.Blue;
                        temp = "▲"; break;
                    case 3:
                        Console.ForegroundColor = ConsoleColor.Green;
                        temp = "×"; break;
                    case 4:
                        Console.ForegroundColor = ConsoleColor.Magenta;
                        temp = "卐"; break;

                }
            }
            return temp;
        }

        public void DrawMapleftToRight(int left, int right, ref Player p1, ref Player p2)
        {
            for (int i = left; i <= right; i++)
            {
                Console.Write(DrawStringMap(i, ref p1, ref p2));
            }
        }

        public void DrawMap(ref Player p1, ref Player p2)
        {
            Console.WriteLine("图例：{0}：★   {1}：☆   幸运轮盘：◎   地雷：▲   暂停：×   时空隧道：卐", p1.name, p2.name);
            DrawMapleftToRight(0, 29, ref p1, ref p2);
            Console.WriteLine();
            for (int i = 30; i <= 34; i++)
            {
                for (int j = 0; j <= 28; j++)
                {
                    Console.Write("  ");
                }
                Console.Write(DrawStringMap(i, ref p1, ref p2));
                Console.WriteLine();
            }
            for (int i = 64; i >= 35; i--)
            {
                Console.Write(DrawStringMap(i, ref p1, ref p2));
            }
            Console.WriteLine();
            for (int i = 65; i <= 69; i++)
            {
                Console.Write(DrawStringMap(i, ref p1, ref p2));
                Console.WriteLine();
            }
            DrawMapleftToRight(70, 99, ref p1, ref p2);
            Console.WriteLine("");
        }
    }

    struct Player
    {
        public string name;
        public int index;
        public GameObject piece;
        public bool isPause;
        public Map map;

        public void CheckPos()
        {
            if (index > 99)
            {
                index = 99;
            }
            if (index < 0)
            {
                index = 0;
            }
        }

        public void Move(ref Player player, ref Map map)
        {
            Random r = new Random();
            int num = r.Next(1, 7);
            string msg = "";
            Console.WriteLine("{0}按任意键掷筛子", name);
            Console.ReadKey(true);
            Console.WriteLine("{0}掷出了{1}", name, num);
            Console.WriteLine("{0}按任意键开始行动......", name);
            Console.ReadKey(true);
            index += num;
            CheckPos();
            if (this.index == player.index)
            {
                msg = string.Format("玩家{0}踩到玩家{1}，玩家{2}退6格", this.name, player.name, player.name);
                player.index -= 6;
                CheckPos();
            }
            else
            {
                MapEvent(ref player);
            }
            Console.Clear();
            if (piece.type == GameObjectType.Player1)
            {
                map.DrawMap(ref this, ref player);
            } else
            {
                map.DrawMap(ref player, ref this);
            }
            Console.WriteLine(msg);
        }

        public int ReadInt(string msg, int min, int max)
        {
            while (true)
            {
                try
                {
                    Console.WriteLine(msg);
                    int number = Convert.ToInt32(Console.ReadLine());
                    if (number >= min && number <= max)
                    {
                        return number;
                    }
                    else
                    {
                        Console.WriteLine("你的输入不合法！只能输入{0}到{1}之间的数字！", min, max);
                        continue;
                    }
                }
                catch
                {
                    Console.WriteLine("输入有误，请重新输入！");
                }
            }
        }

        public void MapEvent(ref Player player)
        {
            string msg;
            switch (map.dataMap[index])
            {
                case 0:
                    msg = "行动完了";
                    break;
                case 1:
                    msg = string.Format("{0}走到幸运轮盘,请选择 1--交换位置，2--轰炸对方", this.name);
                    int number = ReadInt(msg, 1, 2);
                    if (number == 1)
                    {
                        int temp = 0;
                        temp = index;
                        index = player.index;
                        player.index = temp;
                        msg = string.Format("玩家{0}选择了与玩家{1}交换位置", this.name, player.name);
                    }
                    else
                    {
                        player.index = 0;
                        msg = string.Format("玩家{0}选择轰炸玩家{1}", this.name, player.name);
                    }
                    break;
                case 2:
                    msg = "恭喜你，能踩到地雷，百年不遇，退6格";
                    this.index -= 6;
                    CheckPos();
                    break;
                case 3:
                    msg = "踩到暂停了";
                    isPause = true;
                    break;
                case 4:
                    msg = "恭喜你，元芳让我告诉你将会穿越10步";
                    this.index += 10;
                    CheckPos();
                    break;
            }
        }
    }

    class Program
    {
        static void ShowUI()
        {
            Console.WriteLine("********************************");
            Console.WriteLine("*                              *");
            Console.WriteLine("*             飞行棋           *");
            Console.WriteLine("*                              *");
            Console.WriteLine("********************************");
        }

        static void Win()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("                                          ◆                      ");
            Console.WriteLine("                    ■                  ◆               ■        ■");
            Console.WriteLine("      ■■■■  ■  ■                ◆■         ■    ■        ■");
            Console.WriteLine("      ■    ■  ■  ■              ◆  ■         ■    ■        ■");
            Console.WriteLine("      ■    ■ ■■■■■■       ■■■■■■■   ■    ■        ■");
            Console.WriteLine("      ■■■■ ■   ■                ●■●       ■    ■        ■");
            Console.WriteLine("      ■    ■      ■               ● ■ ●      ■    ■        ■");
            Console.WriteLine("      ■    ■ ■■■■■■         ●  ■  ●     ■    ■        ■");
            Console.WriteLine("      ■■■■      ■             ●   ■   ■    ■    ■        ■");
            Console.WriteLine("      ■    ■      ■            ■    ■         ■    ■        ■");
            Console.WriteLine("      ■    ■      ■                  ■               ■        ■ ");
            Console.WriteLine("     ■     ■      ■                  ■           ●  ■          ");
            Console.WriteLine("    ■    ■■ ■■■■■■             ■              ●         ●");
            Console.ResetColor();
        }

        static void Main(string[] args)
        {
            ShowUI();
            Map map = new Map();
            Player a = new Player();
            a.piece.type = GameObjectType.Player1;
            Player b = new Player();
            b.piece.type = GameObjectType.Player2;
            map.InitMap();
            a.map = map;
            b.map = map;
            Console.WriteLine("请输入玩家A的姓名");
            a.name = Console.ReadLine();
            while (a.name == "")
            {
                Console.WriteLine("玩家A的姓名不能为空,请重新输入");
                a.name = Console.ReadLine();
            }
            Console.WriteLine("请输入玩家B的姓名");
            b.name = Console.ReadLine();
            while (b.name == a.name || b.name == "")
            {
                if (b.name == a.name)
                {
                    Console.WriteLine("玩家B和玩家A的姓名{0}不能相同，请重新输入玩家B的姓名", a.name);
                }
                else
                {
                    Console.WriteLine("玩家B的姓名为空,请重新输入");
                }
                b.name = Console.ReadLine();
            }
            Console.Clear();
            Console.WriteLine("对战开始......");
            Console.WriteLine("{0}的士兵用A表示", a.name);
            Console.WriteLine("{0}的士兵用B表示", b.name);

            Console.Clear();
            map.DrawMap(ref a, ref b);
            while (a.index <= 99 && b.index <= 99)
            {
                if (a.isPause == false)
                {
                    a.Move(ref b, ref map);
                }
                else
                {
                    a.isPause = false;
                }
                if (a.index == 99)
                {
                    Console.WriteLine("恭喜{0}胜利了！", a.name);
                    break;
                }
                if (b.isPause == false)
                {
                    b.Move(ref a, ref map);
                }
                else
                {
                    b.isPause = false;
                }
                if (b.index == 99)
                {
                    Console.WriteLine("恭喜{0}胜利了！", b.name);
                    break;
                }
            }
            Win();
            Console.ReadKey();
        }
    }
}
