using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Web.UI.WebControls;

namespace Quiz_new
{
    public partial class playquiz : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                //quiz.Visible = false;
                txtQuizName.Visible = false;
                lnkMethodology.Visible = false;
                btnStartQuiz.Visible = false;
            }
        }
///
        protected void btnLoadQuiz_Click(object sender, EventArgs e)
        {
            string quizID = txtQuizID.Text.Trim();
            hfQuizId.Value = quizID;

            string connStr = "server=localhost;user id=root;password=pass2025;database=quiz;";
            using (MySqlConnection conn = new MySqlConnection(connStr))
            {
                conn.Open();
                string query = "SELECT Tittle, methodology FROM Details WHERE sl = @quizId";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@quizId", quizID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtQuizName.Text = "Quiz Name: " + reader["Tittle"].ToString();
                            string methodologyFile = reader["methodology"].ToString();

                            if (!string.IsNullOrEmpty(methodologyFile))
                            {
                                lnkMethodology.Text = "View Methodology PDF";
                                lnkMethodology.NavigateUrl = "files/" + methodologyFile;
                                lnkMethodology.Target = "_blank";                        
                                lnkMethodology.Visible = true;
                            }
                            else
                            {
                                lnkMethodology.Text = "No Methodology Available";
                                lnkMethodology.NavigateUrl = "";
                            }

                            
                            txtQuizName.Visible = true;
                            lnkMethodology.Visible = true;
                            btnStartQuiz.Visible = true;
                            LoadQuestions(txtQuizID.Text);
                        }
                        else
                        {
                            txtQuizName.Text = "";
                            lnkMethodology.Text = "Quiz not found!";
                            lnkMethodology.NavigateUrl = "";
                            txtQuizName.Visible = false;
                            lnkMethodology.Visible = false;
                        }
                    }
                }
            }
        }

        private void LoadQuestions(string quizId)
        {
            string connStr = "server=localhost;uid=root;pwd=pass2025;database=quiz;";
            using (MySqlConnection con = new MySqlConnection(connStr))
            {
                con.Open();
                string query = "SELECT id,Question, Answer, Option1, Option2, Option3, Option4 FROM questiontbl WHERE quiz_id = @quizId";
                using (MySqlCommand cmd = new MySqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@quizId", quizId);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        Repeater1.DataSource = dt;
                        Repeater1.DataBind();

                        quiz.Visible = dt.Rows.Count > 0;
                    }
                }
            }
        }
        protected void rptQuestions_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                DataRowView drv = (DataRowView)e.Item.DataItem;
                RadioButtonList rbl = (RadioButtonList)e.Item.FindControl("rblOptions");

                rbl.Items.Clear();
                rbl.Items.Add(new ListItem(drv["Option1"].ToString(), "Option1"));
                rbl.Items.Add(new ListItem(drv["Option2"].ToString(), "Option2"));
                rbl.Items.Add(new ListItem(drv["Option3"].ToString(), "Option3"));
                rbl.Items.Add(new ListItem(drv["Option4"].ToString(), "Option4"));

                HiddenField hfCorrectAnswer = (HiddenField)e.Item.FindControl("hfCorrectAnswer");
                hfCorrectAnswer.Value = drv["Answer"].ToString();
            }
        }

        protected void Btn_submit_Click(object sender, EventArgs e)
        {
            int correct = 0;
            int total = Repeater1.Items.Count;
            string quizId = hfQuizId.Value;

            for (int i = 0; i < total; i++)
            {
                RepeaterItem item = Repeater1.Items[i];
                RadioButtonList rbl = (RadioButtonList)item.FindControl("rblOptions");
                HiddenField hfCorrectAnswer = (HiddenField)item.FindControl("hfCorrectAnswer");

                HiddenField hfquestion_no = (HiddenField)item.FindControl("hfquestion_no");
                string selected = rbl.SelectedValue;
                //string correctAnswer = hfCorrectAnswer.Value;

                //if (!string.IsNullOrEmpty(selected))
                //{
                //    if (selected == correctAnswer)
                //        correct++;
                //    else
                //        wrong++;
                //}
                //else
                //{
                //    wrong++;
                //}

               // string quizId = hfQuizId.Value;
                int score = correct;

                string connStr = "server=localhost;uid=root;pwd=pass2025;database=quiz;";
                using (MySqlConnection con = new MySqlConnection(connStr))
                {
                    con.Open();
                    string query = @"INSERT INTO resulttbl 
                                (quiz_id, user_id, correct_answers,Question_no)
                                VALUES (@quizId, @userId,@correct,@Question_no)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@quizId", quizId);
                        cmd.Parameters.AddWithValue("@userId", "Guest");
                        cmd.Parameters.AddWithValue("@Question_no", hfquestion_no.Value);
                        cmd.Parameters.AddWithValue("@correct", correct);
                        
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            quiz.Visible = false;
            Literal1.Text = $"<div class='alert alert-success'>Quiz Completed!</div>";

            Response.Redirect($"admin_master.aspx?quiz_id={quizId}&correct={correct}");
        }

        protected void btnStartQuiz_Click(object sender, EventArgs e)
        {
            
        }
    }
}
