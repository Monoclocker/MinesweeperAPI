using MinesweeperAPI.DataAccessLayer.Helpers;
using MongoDB.Bson.Serialization.Attributes;

namespace MinesweeperAPI.DataAccessLayer.Entities
{
    public class Game
    {
        public Guid Id { get; private set;}
        public bool IsCompleted { get; private set; } = false;
        public (int, int)[] MinesCoordinates { get; private set;} = default!;
        public string[][] Field { get; private set;} = default!;

        public Game(int width, int height, int minesCount)
        {
            GenerateField(width, height);
            GenerateMines(width, height, minesCount);
        }

        [BsonConstructor]
        public Game(Guid Id, bool IsCompleted, (int, int)[] MinesCoordinates, string[][] Field)
        {
            this.Id = Id;
            this.IsCompleted = IsCompleted;
            this.MinesCoordinates = MinesCoordinates;
            this.Field = Field;
        }

        //метод, который завершает игру
        public void CompleteGame()
        {
            IsCompleted = true;
        }

        //проверка на мину под указанной клеткой
        private bool IsMine(int row, int col)
        {
            return MinesCoordinates.Where(x => x == (row, col)).Any();
        }

        //проверка на то, нажал ли игрок на мину
        public bool CheckMine(int row, int col)
        {
            if (IsMine(row, col))
            {
                AllMineReveal(MineRevealOptions.Failure);
                return true;
            }
            else
            {
                return false;
            }
        }

        //раскрытие всех мин
        private void AllMineReveal(MineRevealOptions options)
        {
            foreach ((int, int) mine in MinesCoordinates)
            {
                switch (options)
                {
                    case MineRevealOptions.Win:
                        Field[mine.Item1][mine.Item2] = "M";
                        break;
                    case MineRevealOptions.Failure:
                        Field[mine.Item1][mine.Item2] = "X";
                        break;
                }
            }
        }

        //проверка на то, открыта ли клетка
        public bool IsAlreadyOpened(int row, int col)
        {
            return Field[row][col] != " ";
        }

        //открывает всё оставшееся игровое поле
        public void RevealAllField()
        {
            for (int row = 0; row < Field.Length; row++)
            {
                for (int col = 0; col < Field[0].Length; col++)
                {
                    if (!IsAlreadyOpened(row, col))
                    {
                        RevealOneCell(row, col);
                    }
                }
            }
        }

        //проверка на то, окончена ли игра из-за победы игрока
        public bool CheckWin()
        {
            int closedMines = Field.Sum(x => x.Where(y => y == " ").Count());

            if (closedMines == MinesCoordinates.Length)
            {
                foreach((int, int) mine in MinesCoordinates)
                {
                    AllMineReveal(MineRevealOptions.Win);
                }

                return true;
            }

            return false;

        }

        //проверка на то, существует ли клетка с заданными координатами на поле
        public bool IsOnField(int row, int col)
        {
            return row >= 0 &&
                row < Field.Length &&
                col >= 0 &&
                col < Field[0].Length;
        }

        //раскрытие одной клетки
        //в случае, если рядом с клеткой нет мин, то происходит рекурсивное раскрытие смежных ей клеток
        public void RevealOneCell(int row, int col)
        {
            if (IsAlreadyOpened(row, col)) return;
            if (IsMine(row, col)) return;

            int minesCount = 0;

            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    int newRow = row + i;
                    int newCol = col + j;
                    if (IsOnField(newRow, newCol) && 
                        IsMine(newRow, newCol))
                    {
                        minesCount++;
                    }
                }
            }

            Field[row][col] = minesCount.ToString();

            if(minesCount == 0)
            {
                for (int i = -1; i <= 1; i++)
                {
                    for (int j = -1; j <= 1; j++)
                    {
                        int newRow = row + i;
                        int newCol = col + j;
                        if ((i != 0 || 
                            j != 0) &&
                            IsOnField(newRow, newCol))
                        {
                            RevealOneCell(newRow, newCol);
                        }
                    }
                }
            }

        }


        //метод для генерации координат мин
        void GenerateMines(int width, int height, int minesCount)
        {
            Random rnd = new Random();
            MinesCoordinates = new (int, int)[minesCount];
            int currentMineIndex = 0;

            while (currentMineIndex < minesCount)
            {
                (int, int) mine = (rnd.Next(height), rnd.Next(width));

                if (!MinesCoordinates.Where(x => x == mine).Any())
                {
                    MinesCoordinates[currentMineIndex] = mine;
                    currentMineIndex++;
                }
            }
        }

        //метод для генерации игрового поля
        void GenerateField(int width, int height)
        {
            Field = new string[height][];

            for (int i = 0; i < height; i++)
            {
                Field[i] = new string[width];
                for (int j = 0; j < width; j++)
                {
                    Field[i][j] = " ";
                }
            }
        }


    }
}
