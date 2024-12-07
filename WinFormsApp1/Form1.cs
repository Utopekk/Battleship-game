using System.Diagnostics;

namespace WinFormsApp1
{
    public partial class Form1 : Form
    {
        private int[,] plansza1 = new int[12, 12];
        private int[,] plansza2 = new int[12, 12];
        private int lastHitX = -1;
        private int lastHitY = -1;
        private int currentDirection = -1;
        private bool huntingMode = false;
        private List<(int, int)> successfulHits = new List<(int, int)>();
        public Form1()
        {
            InitializeComponent();
        }
        private void ZaznaczKomorkiWokolStatku(int startX, int startY, int[,] plansza, DataGridView dataGridView)
        {
            List<(int, int)> punktyStatku = new List<(int, int)>();
            Stack<(int, int)> stos = new Stack<(int, int)>();
            
            stos.Push((startX, startY));

            while (stos.Count > 0)
            {
                var (x, y) = stos.Pop();
                if (x >= 0 && x < 12 && y >= 0 && y < 12 && plansza[x, y] == 2 && !punktyStatku.Contains((x, y)))
                {
                    punktyStatku.Add((x, y));
                    stos.Push((x - 1, y));
                    stos.Push((x + 1, y));
                    stos.Push((x, y - 1));
                    stos.Push((x, y + 1));
                }
            }
            foreach (var (x, y) in punktyStatku)
            {
                for (int i = x - 1; i <= x + 1; i++)
                {
                    for (int j = y - 1; j <= y + 1; j++)
                    {
                        if (i >= 0 && i < 12 && j >= 0 && j < 12 && plansza[i, j] != 2)
                        {
                           dataGridView.Rows[i].Cells[j].Style.BackColor = Color.Red;
                        }
                    }
                }
            }
        }
        private bool CzyZniszczonyStatek(int startX, int startY, int[,] plansza)
        {
            List<(int, int)> punktyStatku = new List<(int, int)>();
            Stack<(int, int)> stos = new Stack<(int, int)>();

            stos.Push((startX, startY));

            while (stos.Count > 0)
            {
                var (x, y) = stos.Pop();
                if (x >= 0 && x < 12 && y >= 0 && y < 12 && (plansza[x, y] == 1 || plansza[x, y] == 2) && !punktyStatku.Contains((x, y)))
                {
                    punktyStatku.Add((x, y));
                    stos.Push((x - 1, y));
                    stos.Push((x + 1, y));
                    stos.Push((x, y - 1));
                    stos.Push((x, y + 1));
                }
            }
            return punktyStatku.All(p => plansza[p.Item1, p.Item2] == 2);
        
        }
        private void button1_Click(object sender, EventArgs e)
        {
            plansza1 = new int[12, 12];
            plansza2 = new int[12, 12];
            bool[,] boolki = new bool[12, 12];
            bool[,] boolki1 = new bool[12, 12];
            Random r = new Random();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            dataGridView2.Rows.Clear();
            dataGridView2.Columns.Clear();

            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.ColumnHeadersVisible = false;
            dataGridView1.ColumnCount = 12;
            dataGridView1.AllowUserToAddRows = false;

            dataGridView2.AllowUserToResizeRows = false;
            dataGridView2.AllowUserToResizeColumns = false;
            dataGridView2.RowHeadersVisible = false;
            dataGridView2.ColumnHeadersVisible = false;
            dataGridView2.ColumnCount = 12;
            dataGridView2.ReadOnly = true;
            dataGridView2.AllowUserToAddRows = false;

            sprawdz(plansza1, boolki, 4, r);
            sprawdz(plansza1, boolki, 3, r);
            sprawdz(plansza1, boolki, 3, r);
            for (int i = 0; i < 3; i++)
            {
                sprawdz(plansza1, boolki, 2, r);
            }
            for (int i = 0; i < 4; i++)
            {
                sprawdz(plansza1, boolki, 1, r);
            }

            sprawdz(plansza2, boolki1, 4, r);
            sprawdz(plansza2, boolki1, 3, r);
            sprawdz(plansza2, boolki1, 3, r);
            for (int i = 0; i < 3; i++)
            {
                sprawdz(plansza2, boolki1, 2, r);
            }
            for (int i = 0; i < 4; i++)
            {
                sprawdz(plansza2, boolki1, 1, r);
            }


            for (int i = 0; i < 12; i++)
            {
                var wiersz = new string[12];
                var wiersz1 = new string[12];
                for (int j = 0; j < 12; j++)
                {
                    wiersz[j] = "0";
                    wiersz1[j] = plansza2[i, j].ToString();
                }
                dataGridView1.Rows.Add(wiersz);
                dataGridView2.Rows.Add(wiersz1);
                for (int j = 0; j < 12; j++)
                {
                    dataGridView1.Columns[j].Width = 20;
                    dataGridView2.Columns[j].Width = 20;
                }
            }
            ResetGridBorders(dataGridView1);
            ResetGridBorders(dataGridView2);
        }   
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            int x = e.RowIndex;
            int y = e.ColumnIndex;

            if (x >= 0 && x < 12 && y >= 0 && y < 12)
            {
                var cellColor = dataGridView1.Rows[x].Cells[y].Style.BackColor;
                if (cellColor == Color.Red || cellColor == Color.Green)
                {
                    return; 
                }
                if (plansza1[x, y] == 1) //trafil
                {
                    dataGridView1.Rows[x].Cells[y].Style.BackColor = Color.Green;
                    dataGridView1.Rows[x+1].Cells[y-1].Style.BackColor = Color.Red;
                    dataGridView1.Rows[x-1].Cells[y-1].Style.BackColor = Color.Red;
                    dataGridView1.Rows[x+1].Cells[y+1].Style.BackColor = Color.Red;
                    dataGridView1.Rows[x-1].Cells[y+1].Style.BackColor = Color.Red;
                    plansza1[x, y] = 2; 

                    if (CzyZniszczonyStatek(x, y,plansza1))
                    {
                        ZaznaczKomorkiWokolStatku(x, y,plansza1,dataGridView1);
                    }
                }
                else // Nie trafi³
                {
                    dataGridView1.Rows[x].Cells[y].Style.BackColor = Color.Red;
                    dataGridView1.Rows[x].Cells[y].Value = "0";
                }
            }
            CheckWinCondition(plansza1);
            komputer(plansza2);
            dataGridView1.ClearSelection();
            ResetGridBorders(dataGridView1);
        }
        private void komputer(int[,] plansza2)
        {
            Random r = new Random();
            int x = -1, y = -1;
            bool validShot = false;

            while (!validShot)
            {
                if (!huntingMode)
                {
                    do
                    {
                        x = r.Next(1, 11);
                        y = r.Next(1, 11);
                    } while (!IsValidShot(x, y, plansza2));
                    validShot = true;
                }
                else
                {
                    (x, y) = GetNextHuntTarget();
                    if (x != -1 && y != -1)
                    {
                        validShot = true;
                    }
                    else
                    {
                        huntingMode = false;
                        currentDirection = -1;
                    }
                }
            }

            if (plansza2[x, y] == 1)
            {
                dataGridView2.Rows[x].Cells[y].Style.BackColor = Color.Green;
                plansza2[x, y] = 2;
                lastHitX = x;
                lastHitY = y;
                huntingMode = true;
                successfulHits.Add((x, y));

                if (CzyZniszczonyStatek(x, y, plansza2))
                {
                    ZaznaczKomorkiWokolStatku(x, y, plansza2, dataGridView2);
                    huntingMode = false;
                    currentDirection = -1;
                    successfulHits.Clear();
                }
            }
            else
            {
                dataGridView2.Rows[x].Cells[y].Style.BackColor = Color.Red;
                plansza2[x, y] = 3;
                if (huntingMode)
                {
                    SwitchToOppositeDirection();
                }
            }

            CheckWinCondition(plansza2);

            ResetGridBorders(dataGridView2);
        }

        private bool IsValidShot(int x, int y, int[,] plansza)
        {
            if (x >= 1 && x <= 10 && y >= 1 && y <= 10)
            {
                var color = dataGridView2.Rows[x].Cells[y].Style.BackColor;
                return plansza[x, y] != 2 && plansza[x, y] != 3 &&
                       color != Color.Green && color != Color.Red;
            }
            return false;
        }

        private (int, int) GetNextHuntTarget()
        {
            int newX = -1, newY = -1;

            if (currentDirection == -1)
            {
                currentDirection = 0;
            }

            while (currentDirection < 4)
            {
                switch (currentDirection)
                {
                    case 0: newX = lastHitX - 1; newY = lastHitY; break; // Up
                    case 1: newX = lastHitX + 1; newY = lastHitY; break; // Down
                    case 2: newX = lastHitX; newY = lastHitY - 1; break; // Left
                    case 3: newX = lastHitX; newY = lastHitY + 1; break; // Right
                }

                if (IsValidCell(newX, newY) && plansza2[newX, newY] != 2 && plansza2[newX, newY] != 3)
                {
                    return (newX, newY);
                }
                else
                {
                    currentDirection++;
                }
            }

            SwitchToOppositeDirection();
            return GetNextHuntTarget();
        }

        private void SwitchToOppositeDirection()
        {
            if (successfulHits.Count > 0)
            {
                var (initialX, initialY) = successfulHits[0];
                currentDirection = (currentDirection + 2) % 4;
                lastHitX = initialX;
                lastHitY = initialY;
            }
        }

        private bool IsValidCell(int x, int y)
        {
            return x >= 1 && x <= 10 && y >= 1 && y <= 10;
        }
        private void ResetGridBorders(DataGridView dataGridView)
        {
            for (int i = 0; i < 12; i++)
            {
                dataGridView.Rows[0].Cells[i].Style.BackColor = Color.Blue;
                dataGridView.Rows[11].Cells[i].Style.BackColor = Color.Blue;
                dataGridView.Rows[i].Cells[0].Style.BackColor = Color.Blue;
                dataGridView.Rows[i].Cells[11].Style.BackColor = Color.Blue;
            }
        }

        private void CheckWinCondition(int[,] plansza)
        {
            bool allShipsDestroyed = true;
            string message = "Wygrales";
            if (plansza == plansza2)
            {
                message = "Przegrales";
            }
            for (int i = 0; i < 12; i++)
            {
                for (int j = 0; j < 12; j++)
                {
                    if (plansza[i, j] == 1)
                    {
                        allShipsDestroyed = false;
                        break;
                    }
                }
                if (!allShipsDestroyed)
                {
                    break;
                }
            }

            if (allShipsDestroyed)
            {
                MessageBox.Show(message);
            }
        }

        static void Stateczek(int[,] plansza1, bool[,] boolki, int x, int y, int len, int direction)
        {
            for (int i = 0; i < len; i++)
            {
                if (direction == 1) // Poziomo
                {
                    plansza1[x, y + i] = 1;
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (x + dx >= 0 && x + dx < 12 && y + i + dy >= 0 && y + i + dy < 12)
                            {
                                boolki[x + dx, y + i + dy] = true;
                            }
                        }
                    }
                }
                else // Pionowo
                {
                    plansza1[x + i, y] = 1;
                    for (int dx = -1; dx <= 1; dx++)
                    {
                        for (int dy = -1; dy <= 1; dy++)
                        {
                            if (x + i + dx >= 0 && x + i + dx < 12 && y + dy >= 0 && y + dy < 12)
                            {
                                boolki[x + i + dx, y + dy] = true;
                            }
                        }
                    }
                }
            }
        }

        static void sprawdz(int[,] plansza1, bool[,] boolki, int len, Random r)
        {
            int x, y, direction;
            bool bol = false;

            while (!bol)
            {
                x = r.Next(1, 10);
                y = r.Next(1, 10);
                direction = r.Next(1, 3); // 1 - poziomo, 2 - pionowo

                if ((direction == 1 && y + len > 10) || (direction == 2 && x + len > 10))
                    continue;

                bool czy = true;
                for (int i = 0; i < len; i++)
                {
                    if (direction == 1 && boolki[x, y + i])
                    {
                        czy = false;
                        break;
                    }
                    else if (direction == 2 && boolki[x + i, y])
                    {
                        czy = false;
                        break;
                    }
                }
                if (czy)
                {
                    Stateczek(plansza1, boolki, x, y, len, direction);
                    bol = true;
                }
            }
        }        
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }
}
