using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using PCODSurvey.Module;

namespace PCODSurvey
{
    public partial class Form1 : Form
    {
        List<QuestionViewModel> _questionlist = new List<QuestionViewModel>();
        List<OptionViewModel> _optionlist = new List<OptionViewModel>();
        int currentindex = 0;

        // Hardcoded controls
        private Label label2;
        private Button button2, button1, button3;
        private RadioButton radioButton1, radioButton2, radioButton3, radioButton4, radioButton5;

        public Form1()
        {
            InitializeComponent();

            // Initialize Controls
            InitializeFormControls();
        }

        private void InitializeFormControls()
        {
            // Initialize label2 (Question Display)
            label2 = new Label();
            label2.Size = new System.Drawing.Size(500, 50);
            label2.Location = new System.Drawing.Point(30, 30);
            label2.Text = "Question will appear here.";
            label2.Font = new System.Drawing.Font("Arial", 12);
            Controls.Add(label2);

            // Initialize RadioButtons for answers
            radioButton1 = new RadioButton { Text = "Strongly Agree", Location = new System.Drawing.Point(30, 90) };
            radioButton2 = new RadioButton { Text = "Agree", Location = new System.Drawing.Point(30, 120) };
            radioButton3 = new RadioButton { Text = "Neutral", Location = new System.Drawing.Point(30, 150) };
            radioButton4 = new RadioButton { Text = "Disagree", Location = new System.Drawing.Point(30, 180) };
            radioButton5 = new RadioButton { Text = "Strongly Disagree", Location = new System.Drawing.Point(30, 210) };

            Controls.Add(radioButton1);
            Controls.Add(radioButton2);
            Controls.Add(radioButton3);
            Controls.Add(radioButton4);
            Controls.Add(radioButton5);

            // Initialize Next Button (button2)
            button2 = new Button();
            button2.Text = "Next";
            button2.Location = new System.Drawing.Point(30, 250);
            button2.Click += new EventHandler(button2_Click);
            Controls.Add(button2);

            // Initialize Previous Button (button1)
            button1 = new Button();
            button1.Text = "Previous";
            button1.Location = new System.Drawing.Point(120, 250);
            button1.Click += new EventHandler(button1_Click);
            Controls.Add(button1);

            // Initialize Submit Button (button3)
            button3 = new Button();
            button3.Text = "Submit";
            button3.Location = new System.Drawing.Point(30, 280);
            button3.Visible = false;
            button3.Click += new EventHandler(button3_Click);
            Controls.Add(button3);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var connectionString = "server=localhost\\SQLEXPRESS;database=PCODSurvey;integrated Security=SSPI;";

            // Load questions
            using (SqlConnection _con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM Questiontable";
                using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                {
                    DataTable Questiontable = new DataTable();
                    SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                    _con.Open();
                    _dap.Fill(Questiontable);
                    _con.Close();

                    foreach (DataRow row in Questiontable.Rows)
                    {
                        QuestionViewModel _temp = new QuestionViewModel();
                        _temp.questionname = row["questionname"].ToString();
                        _questionlist.Add(_temp);
                    }
                }
            }

            // Display the first question
            label2.Text = _questionlist[0].questionname;

            // Load options
            using (SqlConnection _con = new SqlConnection(connectionString))
            {
                string queryStatement = "SELECT * FROM Optiontable";
                using (SqlCommand _cmd = new SqlCommand(queryStatement, _con))
                {
                    DataTable OptionTable = new DataTable();
                    SqlDataAdapter _dap = new SqlDataAdapter(_cmd);
                    _con.Open();
                    _dap.Fill(OptionTable);
                    _con.Close();

                    foreach (DataRow row in OptionTable.Rows)
                    {
                        OptionViewModel _temp = new OptionViewModel();
                        _temp.optionname = row["optionname"].ToString();
                        _temp.optionid = Convert.ToInt32(row["optionid"]);
                        _temp.optionvalue = Convert.ToInt32(row["optionvalue"]);
                        _optionlist.Add(_temp);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            bool isanswered = true;

            // Get the selected answer
            if (radioButton1.Checked) _questionlist[currentindex].optionvalue = 5;
            else if (radioButton2.Checked) _questionlist[currentindex].optionvalue = 4;
            else if (radioButton3.Checked) _questionlist[currentindex].optionvalue = 3;
            else if (radioButton4.Checked) _questionlist[currentindex].optionvalue = 2;
            else if (radioButton5.Checked) _questionlist[currentindex].optionvalue = 1;
            else
            {
                isanswered = false;
                MessageBox.Show("Please answer");
            }

            if (isanswered)
            {
                currentindex++;
                if (currentindex < _questionlist.Count)
                {
                    label2.Text = _questionlist[currentindex].questionname;
                    radioButton1.Checked = false;
                    radioButton2.Checked = false;
                    radioButton3.Checked = false;
                    radioButton4.Checked = false;
                    radioButton5.Checked = false;
                }
                else
                {
                    button3.Visible = true;
                    button2.Visible = false;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            currentindex--;
            label2.Text = _questionlist[currentindex].questionname;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int totalcount = 0;
            foreach (QuestionViewModel que in _questionlist)
            {
                totalcount += que.optionvalue;
            }

            // Logic for determining PCOD possibility based on the total score
            string message = "Your total score is: " + totalcount.ToString() + "\n";

            if (totalcount > 25)
            {
                message += "You have PCOD.";
            }
            else
            {
                message += "You do not have PCOD.";
            }

            // Show result in a new form
            ResultForm resultForm = new ResultForm(message);
            resultForm.Show();
        }
    }
}
