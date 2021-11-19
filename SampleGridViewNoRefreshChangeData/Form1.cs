using System;
using System.Windows.Forms;

namespace SampleGridViewNoRefreshChangeData
{
    public partial class Form1 : Form
    {
        private readonly int limitLines = 5;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //sample
            {
                STAGE[] bays = new STAGE[3];
                STAGE bay1 = new();
                bay1.Stage1 = "A1";
                bay1.Stage2 = "A2";
                bay1.Stage3 = "A3";
                bays[0] = bay1;
                STAGE bay2 = new();
                bay2.Stage1 = "B1";
                bay2.Stage2 = "B2";
                bay2.Stage3 = "B3";
                bays[1] = bay2;
                STAGE bay3 = new();
                bay3.Stage1 = "C1";
                bay3.Stage2 = "C2";
                bay3.Stage3 = "C3";
                bays[2] = bay3;

                SetData(bays);
            }
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            STAGE[] bays = new STAGE[2];
            STAGE bay1 = new();
            bay1.Stage1 = "11";
            bay1.Stage2 = "22";
            bay1.Stage3 = "33";
            bays[0] = bay1;
            STAGE bay2 = new();
            bay2.Stage1 = "44";
            bay2.Stage2 = "55";
            bay2.Stage3 = "66";
            bays[1] = bay2;

            SetData(bays);
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            STAGE[] bays = new STAGE[7];
            STAGE bay1 = new();
            bay1.Stage1 = "AA1";
            bay1.Stage2 = "AA2";
            bay1.Stage3 = "AA3";
            bays[0] = bay1;
            STAGE bay2 = new();
            bay2.Stage1 = "BB1";
            bay2.Stage2 = "BB2";
            bay2.Stage3 = "BB3";
            bays[1] = bay2;
            STAGE bay3 = new();
            bay3.Stage1 = "CC1";
            bay3.Stage2 = "CC2";
            bay3.Stage3 = "CC3";
            bays[2] = bay3;
            STAGE bay4 = new();
            bay4.Stage1 = "DD1";
            bay4.Stage2 = "DD2";
            bay4.Stage3 = "DD3";
            bays[3] = bay4;
            STAGE bay5 = new();
            bay5.Stage1 = "EE1";
            bay5.Stage2 = "EE2";
            bay5.Stage3 = "EE3";
            bays[4] = bay5;
            STAGE bay6 = new();
            bay6.Stage1 = "FF1";
            bay6.Stage2 = "FF2";
            bay6.Stage3 = "FF3";
            bays[5] = bay6;
            STAGE bay7 = new();
            bay7.Stage1 = "GG1";
            bay7.Stage2 = "GG2";
            bay7.Stage3 = "GG3";
            bays[6] = bay7;

            SetData(bays);
        }

        private void Button3_Click(object sender, EventArgs e)
        {
            DataGridViewLog.Rows.Clear();
        }

        private void SetData(STAGE[] bays)
        {
            int number = 0;
            int totalCount = bays.Length;
            foreach (STAGE bay in bays)
            {
                if (bay != null)
                {
                    number++;
                    SetLogText(number, totalCount, bay);
                }
            }
        }

        delegate void CrossThreadSafetyText(int number, int totalCount, STAGE bay);
        public void SetLogText(int number, int totalCount, STAGE bay)
        {
            if (DataGridViewLog != null)
            {
                if (DataGridViewLog.InvokeRequired)
                {
                    try
                    {
                        if (DataGridViewLog != null)
                        {
                            DataGridViewLog.Invoke(new CrossThreadSafetyText(SetLogText), number, totalCount, bay);
                        }
                    }
                    finally { }
                }
                else
                {
                    try
                    {
                        int gridRowCount = DataGridViewLog.Rows.Count;

                        //수정
                        if (gridRowCount >= number)
                        {
                            DataGridViewRow currentRow = DataGridViewLog.Rows[number - 1];
                            currentRow.Cells[0].Value = number;
                            currentRow.Cells[1].Value = bay.Stage1;
                            currentRow.Cells[2].Value = bay.Stage2;
                            currentRow.Cells[3].Value = bay.Stage3;

                            bool IsRowView = true;
                            if (radioButton1.Checked)
                            {
                                //제한 수 까지만 보여짐
                                if (number > limitLines)
                                {
                                    IsRowView = false;
                                }
                            }
                            else if (radioButton2.Checked)
                            {
                                if (number <= limitLines)
                                {
                                    IsRowView = false;
                                }
                            }
                            else //if (radioButton3.Checked)
                            {
                                //none
                            }

                            currentRow.Visible = IsRowView;
                        }
                        //추가
                        else
                        {
                            //중복이 있다면 확인
                            DataGridViewRow AddRow = new();
                            AddRow.CreateCells(DataGridViewLog);
                            AddRow.Cells[0].Value = number;
                            AddRow.Cells[1].Value = bay.Stage1;
                            AddRow.Cells[2].Value = bay.Stage2;
                            AddRow.Cells[3].Value = bay.Stage3;

                            bool IsRowView = true;
                            if(radioButton1.Checked)
                            {
                                //제한 수 까지만 보여짐
                                if (number > limitLines)
                                {
                                    IsRowView = false;
                                }
                            }
                            else if (radioButton2.Checked)
                            {
                                if (number <= limitLines)
                                {
                                    IsRowView = false;
                                }
                            }
                            else //if (radioButton3.Checked)
                            {
                                //none
                            }

                            AddRow.Visible = IsRowView;

                            //생성해서 넣을 때 Row 높이를 맞춰줘야 함
                            AddRow.Height = DataGridViewLog.RowTemplate.Height;
                            DataGridViewLog.Rows.Add(AddRow);
                        }

                        //마지막 개수가 현재 번호라면
                        if (totalCount == number)
                        {
                            if (gridRowCount > totalCount)
                            {
                                for (int i = gridRowCount; i > totalCount; i--)
                                {
                                    DataGridViewLog.Rows.RemoveAt(i-1);
                                }
                            }
                        }

                        //데이터가 많을 경우 디스플레이 인덱스 제어가 필요할 수 있음
                        //DataGridViewLog.FirstDisplayedScrollingRowIndex = DataGridViewLog.RowCount - 1;
                    }
                    finally { }
                }
            }
        }

        private void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (sender == radioButton1)
            {
                int cnt = 0;
                foreach(DataGridViewRow row in DataGridViewLog.Rows)
                {
                    cnt++;
                    if (cnt <= limitLines)
                    {
                        if (!row.Visible)
                        {
                            row.Visible = true;
                        }
                    }
                    else
                    {
                        if (row.Visible)
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            else if (sender == radioButton2)
            {
                int cnt = 0;
                foreach (DataGridViewRow row in DataGridViewLog.Rows)
                {
                    cnt++;
                    if (cnt > limitLines)
                    {
                        if (!row.Visible)
                        {
                            row.Visible = true;
                        }
                    }
                    else
                    {
                        if (row.Visible)
                        {
                            row.Visible = false;
                        }
                    }
                }
            }
            else if (sender == radioButton3)
            {
                foreach (DataGridViewRow row in DataGridViewLog.Rows)
                {
                    if (!row.Visible)
                    {
                        row.Visible = true;
                    }
                }
            }
        }
    }
}
