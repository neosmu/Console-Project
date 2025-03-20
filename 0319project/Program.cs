using System.Dynamic;
using System.Runtime.InteropServices;

namespace _0319project
{
    internal class Program
    {
        struct Position
        {
            public int x;
            public int y;
        }
        static void Main(string[] args)
        {
            bool gameover = false;
            Position playerpos;
            Position goalpos;
            char[,] map;


            Start(out playerpos, out goalpos, out map);
            while (gameover == false)
            {
                Render(playerpos, goalpos, map);
                ConsoleKey key = Input();
                Update(key, ref playerpos, goalpos, map, ref gameover);
            }
             End();
        }

        // 게임시작
        static void Start(out Position playerpos, out Position goalpos, out char[,] map)
        {
            // 게임 설정
            Console.CursorVisible = false;

            // 플레이어 위치 설정
            playerpos.x = 1;
            playerpos.y = 1;

            // 골인 지점 설정
            goalpos.x = 6;
            goalpos.y = 7;

            map = new char[9, 9]
            { 
                { '▒', '▒', '▒', '▒', '▒', '▒', '▒', '▒', '▒'},
                { '▒', ' ', '▒', '▒', ' ', ' ', ' ', 'c', '▒'},
                { '▒', ' ', '▒', '▒', ' ', '▒', ' ', '▒', '▒'},
                { '▒', ' ', '▒', 'c', ' ', '▒', ' ', '▒', '▒'},
                { '▒', ' ', '▒', '▒', ' ', '▒', ' ', '▒', '▒'},
                { '▒', ' ', '▒', '▒', ' ', '▒', ' ', '▒', '▒'},
                { '▒', ' ', '▒', ' ', ' ', '▒', ' ', '▒', '▒'},
                { '▒', ' ', ' ', ' ', '▒', '▒', ' ', '▒', '▒'},
                { '▒', '▒', '▒', '▒', '▒', '▒', '▒', '▒', '▒'}
            };
        }
        // 출력 작업
        static void Render(Position playerpos, Position goalpos, char[,] map)
        {
            Console.SetCursorPosition(0,0);
            Printmap(map);
            Printplayer(playerpos);
            Printcoin(map);
            Printgoal(goalpos);
        }
        // 플레이어 설정
        static void Printplayer(Position playerpos)
        {
            // 플레이어 위치로 커서 이동
            Console.SetCursorPosition(playerpos.x, playerpos.y);
            Console.Write('p');
        }
        // 코인 설정
        static void Printcoin(char[,]map)
        {
           for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    if (map[y, x] == 'c')
                    {
                        Console.SetCursorPosition(x, y);
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write('c');
                        Console.ResetColor();
                    }
                }
            }
        }
        // 맵 설정
        static void Printmap(char[,] map)
        {
            for (int y = 0; y < map.GetLength(0); y++)
            {
                for (int x = 0; x < map.GetLength(1); x++)
                {
                    Console.Write(map[y, x]);
                }
                Console.WriteLine();
            }
        }
        // 골인 지점
        static void Printgoal(Position goalpos)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.SetCursorPosition(goalpos.x, goalpos.y);
            Console.Write('G');
            Console.ResetColor();
        }

        // 입력 작업
        static ConsoleKey Input()
        {
            return Console.ReadKey(true).Key;
        }

        // 처리 작업
        static void Update(ConsoleKey key, ref Position playerpos, Position goalpos, char[,] map, ref bool gameover)
        {
            Move(key, ref playerpos, map);
            bool isClear = IsClear(map, playerpos, goalpos);
            if (isClear)
            {
                gameover = true;
            }
            else if(playerpos.x ==goalpos.x && playerpos.y == goalpos.y)
            {
                Console.SetCursorPosition(0, map.GetLength(0));
                Console.WriteLine("코인을 먹어야합니다.");
            }
        }
        // 움직이는 작업
        static void Move(ConsoleKey key, ref Position playerpos, char[,] map)
        {
            Position targetPos;
            switch (key)
            {
                case ConsoleKey.A:
                    targetPos.x = playerpos.x - 1;
                    targetPos.y = playerpos.y;
                    break;

                case ConsoleKey.D:
                    targetPos.x = playerpos.x + 1;
                    targetPos.y = playerpos.y;
                    break;

                case ConsoleKey.W:
                    targetPos.x = playerpos.x;
                    targetPos.y = playerpos.y - 1;
                    break;

                case ConsoleKey.S:
                    targetPos.x = playerpos.x;
                    targetPos.y = playerpos.y + 1;
                    break;
                default:
                    return;
            }
            if (map[targetPos.y, targetPos.x] != '▒')
            {
                // 코인을 있어서 먹을때
                if (map[targetPos.y, targetPos.x] == 'c')
                {
                    map[targetPos.y, targetPos.x] = ' '; // 코인 제거
                    Console.SetCursorPosition(0, map.GetLength(0));
                    Console.WriteLine("코인을 획득하였습니다.");
                }
                playerpos.x = targetPos.x;
                playerpos.y = targetPos.y;
            }
        }
        static bool IsClear(char[,] map, Position playerpos, Position goalpos)
        {
            //클리어 조건 코인(c)가 없을때 클리어 가능
            foreach(char tile in map)
            {
                if (tile == 'c') // 코인이 남아있다면
                {
                    return false;
                }
            }
            // 골인지점 들어가는 조건
            bool success = (playerpos.x == goalpos.x) && (playerpos.y == goalpos.y); 
            return success;
        }
        // 게임 종료
        static void End()
        {
            Console.Clear();
            Console.WriteLine("모든 코인을 다 모아서 탈출하였습니다.");
        }
    }
}
