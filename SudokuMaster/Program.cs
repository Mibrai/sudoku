using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Timers;

namespace SudokuMaster
{
    internal class Program
    {
        int currentPosCol = Console.CursorLeft;
        int currentPosRow = Console.CursorTop;
        static Position currentPosition =  new Position(0,0);
        static string[,] gamePlan = new string[9,9];    
        static Position[,] gamePlanIndex = new Position[9,9];

        static int score = 0;
        static string currentPlayingBackup;
        static string currentPlayer;

        static string PATH = "C:\\Users\\paric\\source\\repos\\sudoku\\Backup\\";
        static string SAVE_DIRECTORY = "C:\\Users\\paric\\source\\repos\\sudoku\\Backup";
        static List<string> saveListFiles = new List<string>();
        static void Main(string[] args)
        {
            printPlayerNameBox();
            Console.Clear();
            initGameMatrix("_");
            drawGamePlane();
            fillGamePlaneWithMatrixValue();

            drawInfoBox("* ", 35, 0);
            moveCursor(gamePlanIndex[0, 0]);
            moveCursorByPress();

            Console.ReadLine();
        }

        /**
         * fill the Matric with de given Value
         * @initVal
         * void
         */
        static void initGameMatrix(string initVal)
        {
            for(int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    gamePlan[i,j] = initVal;
                }
            }
        }

        /**
         * Write the given Value at the given position in Console
         * @val string
         * @col int
         * @row int
         * return new current position of cursor
         */
        static int[] writeAt(string val, int col, int row)
        {
            Console.SetCursorPosition(col, row);
            Console.WriteLine(val);
            int[] result = { col, row };
            return result;
        }

        /**
         * check if the given value already exist in X and Y axis
         * @val string
         * @col int
         * @row int
         * return bool
         */
        static bool ifExist(string val,int col,int row) {
            bool result = false;

            //check col
            for (int i = 0; i < 9; i++)
            {
                if (gamePlan[i, col] == val)
                {
                    result = true;
                    break;
                }
            }

            //check row
            for (int i = 0;i < 9;i++)
            {
                if (gamePlan[row, i] == val)
                {
                    result = true;
                    break;
                }
            }

            return result;
        }

        /**
         * print all gamePlane Value 
         * void
         */
        static void printAllMatrixValue()
        {
            for(int i = 0; i < 9; i++)
            {
                for(int  j = 0; j < 9; j++)
                {
                    Console.Write(gamePlan[i, j]);
                }

                Console.WriteLine();
            }
        }

        static void drawGamePlane()
        {
            int counterPosition = 0;
            int indexY = 0;
            int indexX = 0;
            //i row
            for(int i = 0; i < 25; i++)
            {
                //j col
               
                for (int j = 0;j < 25; j++)
                {

                    if((i == 0 || i == 8 || i == 16 || i == 24) || (j == 0 || j == 8 || j == 16 || j == 24))
                    {
                        if(i % 2 == 0)
                        {
                            Console.SetCursorPosition(i, j);
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.Write("#");
                            Console.ForegroundColor = ConsoleColor.Gray;
                        }
                    } else
                    {
                        if(j % 2 == 0)
                        {
                            if((i % 2) == 0)
                            {
                                //Thread.Sleep(2000);
                                Console.SetCursorPosition(j, i);

                                Console.Write("_");
                                
                                indexX = counterPosition % 9;
                                indexY = (int)(counterPosition / 9);

                                Position pos = new Position(Console.GetCursorPosition().Left-1, Console.GetCursorPosition().Top);
                                gamePlanIndex[indexX, indexY] = pos;
                                counterPosition++;
                            }

                        }

                    }

                }
            }

        }

        static void fillGamePlaneWithMatrixValue()
        {
            for (int i = 0; i < 9; i++)
            {
                for(int j = 0; j < 9; j++)
                {
                    Console.SetCursorPosition(gamePlanIndex[i, j].getX(), gamePlanIndex[i, j].getY());
                    writeColor(new Position(-1,-1), gamePlan[i, j], ConsoleColor.Blue);
                }
            }
        }

        static void writeColor(Position pos, string value, ConsoleColor color)
        {
            if(pos.getX() == -1 && pos.getY() == -1)
            {
                Console.ForegroundColor = color;
                Console.Write(value);
                Console.ForegroundColor = ConsoleColor.Gray;
            } else
            {
                moveCursor(pos);
                Console.ForegroundColor = color;
                Console.Write(value);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        static void moveCursorByPress()
        {
            ConsoleKeyInfo keyInfo;
            int x = gamePlanIndex[0, 0].getX();
            int y = gamePlanIndex[0, 0].getY();
            do
            {
                keyInfo = Console.ReadKey(true);
                Position pos = new Position(Console.GetCursorPosition().Left, Console.GetCursorPosition().Top);
                currentPosition = pos;
                switch (keyInfo.Key)
                {
                    case ConsoleKey.RightArrow:
                        moveCursor(goNextRight(pos));
                        break;
                    case ConsoleKey.LeftArrow: 
                        moveCursor(goBackLeft(pos)); 
                        break;
                    case ConsoleKey.DownArrow: 
                        moveCursor(goDown(pos)); 
                        break;
                    case ConsoleKey.UpArrow: 
                        moveCursor(goUp(pos)); 
                        break;
                    case ConsoleKey.S:
                        exportGame(PATH);
                        Console.Clear();
                        drawGamePlane();
                        fillGamePlaneWithMatrixValue();
                        drawInfoBox("* ", 35, 0);
                        moveCursor(pos);
                        checkIfSubMatrixFill();
                        break;
                    case ConsoleKey.I:
                        importGame(PATH);
                        Console.Clear();
                        drawGamePlane();
                        fillGamePlaneWithMatrixValue();
                        drawInfoBox("* ", 35, 0);
                        moveCursor(pos);
                        checkIfSubMatrixFill();
                        break;
                    case ConsoleKey.L:
                        printListBackupFiles();
                        moveCursor(pos);
                        break;
                    case ConsoleKey.U:
                        printPlayerNameBox();
                        Console.Clear();
                        currentPlayingBackup = "";
                        initGameMatrix("_");
                        drawGamePlane();
                        fillGamePlaneWithMatrixValue();
                        drawInfoBox("* ", 35, 0);
                        moveCursor(pos);
                        break;
                    case ConsoleKey.C:
                        Console.Clear();
                        currentPlayingBackup = "";
                        initGameMatrix("_");
                        drawGamePlane();
                        fillGamePlaneWithMatrixValue();
                        drawInfoBox("* ", 35, 0);
                        moveCursor(pos);
                        break;
                    case ConsoleKey.R:
                        //initGameMatrix("_");
                        fillTwenthyPercentOfMatrixWithRandomValue(20);
                        drawGamePlane();
                        fillGamePlaneWithMatrixValue();
                        drawInfoBox("* ", 35, 0);
                        moveCursor(pos);
                        checkIfSubMatrixFill();
                        break;
                    case ConsoleKey.D0:
                        addValueAtPosition(pos,"_");
                        currentPosition = pos;
                        Console.Clear();
                        drawGamePlane();
                        fillGamePlaneWithMatrixValue();
                        drawInfoBox("* ", 35, 0);
                        moveCursor(pos);
                        checkIfSubMatrixFill();
                        break;
                    default: 
                        if(keyInfo.Key == ConsoleKey.D1 || keyInfo.Key == ConsoleKey.D2 
                            || keyInfo.Key == ConsoleKey.D3 || keyInfo.Key == ConsoleKey.D4 
                            || keyInfo.Key == ConsoleKey.D5 || keyInfo.Key == ConsoleKey.D6 
                            || keyInfo.Key == ConsoleKey.D7 || keyInfo.Key == ConsoleKey.D8 || keyInfo.Key == ConsoleKey.D9)
                        {
                            addValueAtPosition(pos, keyInfo.KeyChar.ToString());
                            currentPosition = pos;
                            Console.Clear();
                            drawGamePlane();
                            fillGamePlaneWithMatrixValue();
                            drawInfoBox("* ", 35, 0);
                            moveCursor(pos);
                            checkIfSubMatrixFill();
                        }

                        break;
                }
               
            } while (keyInfo.Key != ConsoleKey.Spacebar);
        }

        static void moveCursor(Position pos)
        {
            Console.SetCursorPosition(pos.getX(), pos.getY());
        }

        static Position goDown(Position pos)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (gamePlanIndex[i, j].getX() == pos.getX() && gamePlanIndex[i, j].getY() == pos.getY())
                    {
                        //check the key type
                        if((j+1) < 9)
                            currentPosition = gamePlanIndex[i, j + 1];
                        break;
                    }
                }
            }

            return currentPosition;
        }

        static Position goUp(Position pos)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (gamePlanIndex[i, j].getX() == pos.getX() && gamePlanIndex[i, j].getY() == pos.getY())
                    {
                        //check the key type
                        if((j - 1) >= 0)
                            currentPosition = gamePlanIndex[i, j - 1];
                        break;
                    }
                }
            }

            return currentPosition;
        }

        static Position goBackLeft(Position pos)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (gamePlanIndex[i, j].getX() == pos.getX() && gamePlanIndex[i, j].getY() == pos.getY())
                    {
                        //check the key type
                        if(i - 1 >= 0)
                            currentPosition = gamePlanIndex[i - 1, j];
                        break;
                    }
                }
            }

            return currentPosition;
        }

        static Position goNextRight(Position pos)
        {
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (gamePlanIndex[i, j].getX() == pos.getX() && gamePlanIndex[i, j].getY() == pos.getY())
                    {
                        //check the key type
                        if((i+ 1) < 9)
                           currentPosition = gamePlanIndex[i + 1, j];
                        break;
                    }
                }
            }

            return currentPosition;
        }

        static bool addValueAtPosition(Position pos,string value)
        {
            bool result = false;
            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (gamePlanIndex[i, j].getX() == pos.getX() && gamePlanIndex[i, j].getY() == pos.getY())
                    {
                        //check the key type
                        if(value != "_")
                        {
                            if (!existOnX(i, j, value) && !existOnY(j, i, value) && !checkIfElementExistInSubMatrix(i, j, value))
                            {
                                gamePlan[i, j] = value;
                                result = true;
                            }
                        } else
                        {
                            gamePlan[i, j] = value;
                            result = true;
                        }

                           
                        break;
                    }
                }
            }
            return result;
        }

        static void fillTwenthyPercentOfMatrixWithRandomValue(int percent)
        {
            int counter = 0;
            int number = 0;
            if(percent <= 100)
                counter = (int)((percent * 81) / 100);
            else
                counter = (int)((1 * 81) / 100);

            Random random = new Random();
            do
            {
                //int randomNumber = random.Next(1,10);

                for (int i = 0; i < 9; i++)
                {
                    for(int j = 0;j < 9; j++)
                    {
                        if (addValueAtPosition(gamePlanIndex[i, j], random.Next(1, 10).ToString()))
                        {
                            number++;
                            break;
                        }
                        
                    }
                }

            } while (number < counter);
        }

        static bool existOnX(int x,int y,string value)
        {
            bool result = false;
            for(int k = 0;k < 9; k++)
            {
                if(k != y)
                {
                    if (gamePlan[x, k] == value)
                        result = true;
                }

            }
            return result;  
        }

        static bool existOnY(int y,int x, string value)
        {
            bool result = false;
            for (int k = 0; k < 9; k++)
            {
                if(k != x)
                {
                    if (gamePlan[k, y] == value)
                        result = true;
                }

            }
            return result;
        }

        static bool matrixContains(Position pos)
        {
            bool result = false;

            for (int i = 0; i < 9; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (gamePlanIndex[i, j].getX() == pos.getX() && gamePlanIndex[i, j].getY() == pos.getY())
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        static bool fileExist(string path)
        {
            if (path != null && path.Length > 0)
            {
                if (!File.Exists(path))
                    using (StreamWriter sw = File.CreateText(path)) ;
                return true;
            }
            else
                return false;

        }

        static void importGame(string path)
        {
            string fileName = "";
            string str_path = "";
            do {
                fileName = printInfoBoxSave();
                str_path = path + fileName;
                if (!File.Exists(str_path))
                {
                    printErrorMessage("Die gegebene Datei existiert nicht ! ", new Position(37, 16), new Position(52, 15));
                }
               
            } while (!File.Exists(str_path));

            currentPlayingBackup = fileName[0].ToString().ToUpper() + fileName.Substring(1);
            StreamReader Textfile = new StreamReader(str_path);
            string line;
            int x  = 0;
                while ((line = Textfile.ReadLine()) != null)
                {
                    line = line.Trim();
                    for(int i = 0;i < 9;i++) {
                        gamePlan[i, x] = line[i].ToString();
                    }
                    x++;
                }
            Textfile.Close();
        }

        static void exportGame(string path)
        {
            string fileName = printInfoBoxSave();
            string str_path = path + fileName;

            string[] arrayString = matrixToArrayString();

            if(File.Exists(str_path))
                File.Delete(str_path);

            if (fileExist(str_path))
            {
                for(int i = 0; i < 9 ; i++)
                {
                    File.AppendAllText(str_path, arrayString[i]+"\n");
                }
            }
        }

        static int  listFileInDirectory(string path)
        {
            int numberOfFile = 0;
            if (Directory.Exists(path))
            {
                string[] listFiles = Directory.GetFiles(path);
                foreach(string file in listFiles)
                {
                    bool test = file.Contains("save");
                    if (test)
                    {
                        int firstPosition = file.IndexOf("save");
                        string str = file.Substring(firstPosition);
                        if (!saveListFiles.Contains(str))
                        {
                            saveListFiles.Add(str);
                            numberOfFile++;
                        }
                    }
                   
                }
            } else
            {
                //path is not a valid Directory
            }

            return numberOfFile;
        }

        static string[] matrixToArrayString()
        {
            string[] arrayString = new string[9];
            
            for(int i = 0; i < 9; i++)
            {
                string str = "";
                for(int j = 0;j < 9; j++)
                {
                    str += gamePlan[j, i];
                }
                arrayString[i] = str;
            }

            return arrayString;
        }

        static bool checkIfElementExistInSubMatrix(int i, int j, string value)
        {
            bool result = false;

            int counter = i + 1;

            int counterY = j + 1;

            //1er case
            if((counter + 2) % 3 == 0)
            {

                if((counterY + 2) % 3 == 0){
                    for (int x = i; x <= (i + 2); x++)
                    {
                        for (int y = j; y <= (j + 2); y++)
                        {
                            if (gamePlan[x, y] == value)
                            {
                                result = true;

                            }

                        }
                    }
                }

                if ((counterY + 2) % 3 == 1)
                {
                    for (int x = i; x <= (i + 2); x++)
                    {
                        for (int y = (j - 1); y <= (j + 1); y++)
                        {
                            if (gamePlan[x, y] == value)
                            {
                                result = true;

                            }

                        }
                    }
                }

                if ((counterY + 2) % 3 == 2)
                {
                    for (int x = i; x <= (i + 2); x++)
                    {
                        for (int y = j; y >= (j - 2); y--)
                        {
                            if (gamePlan[x, y] == value)
                            {
                                result = true;

                            }

                        }
                    }
                }

            }

            // 2nd case
            if ((counter + 2) % 3 == 1)
            {

                if ((counterY + 2) % 3 == 0)
                {
                    for (int x = (i - 1); x <= (i + 1); x++)
                    {
                        for (int y = j; y <= (j + 2); y++)
                        {
                            if (gamePlan[x, y] == value)
                            {
                                result = true;

                            }

                        }
                    }
                }

                if ((counterY + 2) % 3 == 1)
                {
                    for (int x = (i - 1); x <= (i + 1); x++)
                    {
                        for (int y = (j - 1); y <= (j + 1); y++)
                        {
                            if (gamePlan[x, y] == value)
                            {
                                result = true;

                            }

                        }
                    }
                }

                if ((counterY + 2) % 3 == 2)
                {
                    for (int x = (i - 1); x <= (i + 1); x++)
                    {
                        for (int y = j; y >= (j - 2); y--)
                        {
                            if (gamePlan[x, y] == value)
                            {
                                result = true;

                            }

                        }
                    }
                }

            }



            //3th case
            if ((counter + 2) % 3 == 2)
            {

                if ((counterY + 2) % 3 == 0)
                {
                    for (int x = i; x >= (i - 2); x--)
                    {
                        for (int y = j; y <= (j + 2); y++)
                        {
                            if (gamePlan[x, y] == value)
                            {
                                result = true;

                            }

                        }
                    }
                }

                if ((counterY + 2) % 3 == 1)
                {
                    for (int x = i; x >= (i - 2); x--)
                    {
                        for (int y = (j - 1); y <= (j + 1); y++)
                        {
                            if (gamePlan[x, y] == value)
                            {
                                result = true;

                            }

                        }
                    }
                }

                if ((counterY + 2) % 3 == 2)
                {
                    for (int x = i; x >= (i - 2); x--)
                    {
                        for (int y = j; y >= (j - 2); y--)
                        {
                            if (gamePlan[x, y] == value)
                            {
                                result = true;

                            }

                        }
                    }
                }

            }

                return result;
        }

        static void drawInfoBox(string msg, int x, int y)
        {
            for(int i = 35; i < 85; i++) { 
                for(int j = 0; j < 10; j++)
                {
                    if((i  == 35 || i == 84) || (i != 0 && j == 0 || j == 9) )
                        writeColor(new Position(i, j), msg,ConsoleColor.DarkYellow);

                    //underline Title sudoku Master
                    if(i > 35 && i < 84 && j == 2)
                        if(i % 2 == 0)
                            writeColor(new Position(i, j), "_", ConsoleColor.DarkRed);

                    //underline Player Name
                    if (i > 35 && i < 60 && j == 5)
                        if (i % 2 == 0)
                            writeColor(new Position(i, j), "_", ConsoleColor.DarkRed);

                }
            }

            //print title SUDOKU MASTER
            writeColor(new Position(55, 1), "SUDOKU MASTER", ConsoleColor.Cyan);

            //print player name
            writeColor(new Position(36, 4), currentPlayer , ConsoleColor.White);

            //print chrono
            //showChrono();

            //print score
            printScore(0);

            //print current playing loaded Backup
            printCurrentPlayingBackup();

            //print memory
            listFileInDirectory(SAVE_DIRECTORY);
            writeColor(new Position(36, 7), "Save  : "+ saveListFiles.Count(), ConsoleColor.Blue);

            //move cursor to first element
            moveCursor(currentPosition);
        }

        static string printInfoBoxSave() {
            string fileName = "save";
            for(int i = 35; i < 85; i++)
            {
                for(int j = 13; j < 18; j++)
                {
                    if ((i >= 35 && j == 13 || j == 17) || ((i == 35 || i == 84) && j >= 13 && j <= 17))
                    {
                        writeColor(new Position(i, j), "*", ConsoleColor.Magenta);                      
                    }
                }

            }

            //write name
            writeColor(new Position(36, 15), "Filename : ", ConsoleColor.Magenta);
            moveCursor(new Position(52,15));
            string name = Console.ReadLine();
            if(name.Length > 0)
                fileName += "" + name[0].ToString().ToUpper() + name.Substring(1);

            return fileName+".txt";
        }

        static void printPlayerNameBox()
        {
            Console.Clear();
            for (int i = 35; i < 85; i++)
            {
                for (int j = 13; j < 18; j++)
                {
                    if ((i >= 35 && j == 13 || j == 17) || ((i == 35 || i == 84) && j >= 13 && j <= 17))
                    {
                        writeColor(new Position(i, j), "*", ConsoleColor.White);
                    }
                }

            }
            string name = "";
            //write name
            do {
                writeColor(new Position(36, 15), "Player Name : ", ConsoleColor.Blue);
                moveCursor(new Position(52, 15));
                name = Console.ReadLine();
            } while (name.Length == 0);
            currentPlayer = name;
            
        }

        static void printListBackupFiles() {
            int maxj = 18 + saveListFiles.Count();
            int k = saveListFiles.Count() - 1;
            for (int i = 35; i < 85; i++)
            {
                for (int j = 18; j <= (maxj + 3); j++)
                {
                    if ((i >= 35 && j == 18 || j == (maxj + 3)) || ((i == 35 || i == 84) && j >= 18 && j <= (maxj + 3)))
                    {
                        writeColor(new Position(i, j), "*", ConsoleColor.Yellow);
                    } else
                    {
                        if(k >= 0 && j >= 20 && j <= (maxj + 2) )
                        {
                            writeColor(new Position(i, j), saveListFiles.ElementAt(k), ConsoleColor.Cyan);
                            k--;
                        }

                    }
                }

            }
            writeColor(new Position(36, 19), "List Of all Backup : ", ConsoleColor.DarkBlue);
            //moveCursor(new Position(52, 15));

        }

        static void printErrorMessage(string msg, Position msgPos,Position initPos)
        {
            msg = "WARNING : " + msg;
            writeColor(msgPos,msg, ConsoleColor.Red);
            moveCursor(initPos);
        }

        static void printScore(int currentScore) {
            writeColor(new Position(36, 6), "Score : "+currentScore, ConsoleColor.Blue);
            moveCursor(currentPosition);
        }

        static void printCurrentPlayingBackup()
        {
           if(currentPlayingBackup != null && currentPlayingBackup.Length > 0 && currentPlayingBackup.Contains("Save")) 
            {
                currentPlayingBackup = currentPlayingBackup.Substring(currentPlayingBackup.IndexOf("Save") + 4);
                writeColor(new Position(36, 8), "Aktuelles gespielte Backup : " + currentPlayingBackup, ConsoleColor.Blue);
                moveCursor(currentPosition);
            }
        }

        static void checkIfSubMatrixFill()
        {
            bool state = true;
            Position[] arrayPosition = new Position[9];
            int element = 0;
            score = 0;

            //1st Line

            //1st subMatrixe
            for(int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gamePlan[i, j] == "_")
                    {
                        state = false;
                        break;
                    } else
                    {
                        arrayPosition[element] = gamePlanIndex[i,j];
                        element++;
                    }
                        
                }
            }

            if(element == 9)
            {
                printArray(arrayPosition);
                score++;
                printScore(score);
            }

            /**
             * 
             * Treatment on array of Position here 
             * set array of Position to null
             * */

            arrayPosition = new Position[9];
            element = 0;

            //2nd subMatrixe
            for (int i = 0; i < 3; i++)
            {
                for (int j = 3; j < 6; j++)
                {
                    if (gamePlan[i, j] == "_")
                    {
                        state = false;
                        break;
                    }
                    else
                    {
                        arrayPosition[element] = gamePlanIndex[i, j];
                        element++;
                    }
                }
            }

            if (element == 9)
            {
                printArray(arrayPosition);
                score++;
                printScore(score);
            }

            /**
             * 
             * Treatment on array of Position here 
             * set array of Position to null
             * */

            arrayPosition = new Position[9];
            element = 0;

            //3th subMatrixe
            for (int i = 0; i < 3; i++)
            {
                for (int j = 6; j < 9; j++)
                {
                    if (gamePlan[i, j] == "_")
                    {
                        state = false;
                        break;
                    }
                    else
                    {
                        arrayPosition[element] = gamePlanIndex[i, j];
                        element++;
                    }
                }
            }

            if (element == 9)
            {
                printArray(arrayPosition);
                score++;
                printScore(score);
            }

            /**
             * 
             * Treatment on array of Position here 
             * set array of Position to null
             * */

            arrayPosition = new Position[9];
            element = 0;

            //2nd Line

            //1st subMatrixe
            for (int i = 3; i < 6; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gamePlan[i, j] == "_")
                    {
                        state = false;
                        break;
                    }
                    else
                    {
                        arrayPosition[element] = gamePlanIndex[i, j];
                        element++;
                    }
                }
            }

            if (element == 9)
            {
                printArray(arrayPosition);
                score++;
                printScore(score);
            }

            /**
             * 
             * Treatment on array of Position here 
             * set array of Position to null
             * */

            arrayPosition = new Position[9];
            element = 0;
            //2nd subMatrixe
            for (int i = 3; i < 6; i++)
            {
                for (int j = 3; j < 6; j++)
                {
                    if (gamePlan[i, j] == "_")
                    {
                        state = false;
                        break;
                    }
                    else
                    {
                        arrayPosition[element] = gamePlanIndex[i, j];
                        element++;
                    }
                }
            }

            if (element == 9)
            {
                printArray(arrayPosition);
                score++;
                printScore(score);
            }

            /**
             * 
             * Treatment on array of Position here 
             * set array of Position to null
             * */

            arrayPosition = new Position[9];
            element = 0;

            //3th subMatrixe
            for (int i = 3; i < 6; i++)
            {
                for (int j = 6; j < 9; j++)
                {
                    if (gamePlan[i, j] == "_")
                    {
                        state = false;
                        break;
                    }
                    else
                    {
                        arrayPosition[element] = gamePlanIndex[i, j];
                        element++;
                    }
                }
            }

            if (element == 9)
            {
                printArray(arrayPosition);
                score++;
                printScore(score);
            }

            //3th Line

            /**
             * 
             * Treatment on array of Position here 
             * set array of Position to null
             * */

            arrayPosition = new Position[9];
            element = 0;

            //1st subMatrixe
            for (int i = 6; i < 9; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (gamePlan[i, j] == "_")
                    {
                        state = false;
                        break;
                    }
                    else
                    {
                        arrayPosition[element] = gamePlanIndex[i, j];
                        element++;
                    }
                }
            }

            if (element == 9)
            {
                printArray(arrayPosition);
                score++;
                printScore(score);
            }

            /**
             * 
             * Treatment on array of Position here 
             * set array of Position to null
             * */

            arrayPosition = new Position[9];
            element = 0;

            //2nd subMatrixe
            for (int i = 6; i < 9; i++)
            {
                for (int j = 3; j < 6; j++)
                {
                    if (gamePlan[i, j] == "_")
                    {
                        state = false;
                        break;
                    }
                    else
                    {
                        arrayPosition[element] = gamePlanIndex[i, j];
                        element++;
                    }
                }
            }

            if (element == 9)
            {
                printArray(arrayPosition);
                score++;
                printScore(score);
            }

            /**
             * 
             * Treatment on array of Position here 
             * set array of Position to null
             * */

            arrayPosition = new Position[9];
            element = 0;

            //3th subMatrixe
            for (int i = 6; i < 9; i++)
            {
                for (int j = 6; j < 9; j++)
                {
                    if (gamePlan[i, j] == "_")
                    {
                        state = false;
                        break;
                    }
                    else
                    {
                        arrayPosition[element] = gamePlanIndex[i, j];
                        element++;
                    }
                }
            }

            if (element == 9)
            {
                printArray(arrayPosition);
                score++;
                printScore(score);
            }


        }

        static void printArray(Position[] array)
        {
            for(int t = 0; t < 9; t++)
            {
                for (int i = 0; i < 9; i++)
                {
                    for (int j = 0; j < 9; j++)
                    {
                        if (gamePlanIndex[i, j].getX() == array[t].getX() && gamePlanIndex[i, j].getY() == array[t].getY())
                        {
                            writeColor(array[t], gamePlan[i, j], ConsoleColor.Yellow);
                            moveCursor(currentPosition);
                        }
                    }
                }
            }

        }
    }


}