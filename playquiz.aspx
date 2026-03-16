<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="playquiz.aspx.cs" Inherits="Quiz_new.playquiz" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Play Quiz</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/css/bootstrap.min.css"
        rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.3/dist/js/bootstrap.bundle.min.js"></script>

    <script type="text/javascript">
        let timeLeft = 300;
        let interval;

        function startTimer() {
            const timerDisplay = document.getElementById('timerLabel');
            interval = setInterval(() => {
                let minutes = Math.floor(timeLeft / 60);
                let seconds = timeLeft % 60;
                timerDisplay.innerText = `${minutes}:${seconds < 10 ? '0' : ''}${seconds}`;
                if (timeLeft <= 0) {
                    clearInterval(interval);
                    document.getElementById('<%= Button2.ClientID %>').click();
                }
                timeLeft--;
            }, 1000);
        }

        function startQuizUI() {
            debugger;
            const welcomeDiv = document.getElementById('<%= welcome.ClientID %>');
            const quizDiv = document.getElementById('<%= quiz.ClientID %>');
            if (welcomeDiv) welcomeDiv.style.display = 'none';
            if (quizDiv) quizDiv.style.display = 'block';
            startTimer();
        }

        window.onload = function () {
            const quizDiv = document.getElementById('<%= quiz.ClientID %>');
            if (quizDiv) quizDiv.style.display = 'none';
        };
    </script>
</head>
<body>
    <form id="form1" runat="server" class="container mt-4">
        <asp:HiddenField ID="hfQuizId" runat="server" />

        <div id="welcome" runat="server" class="text-center mt-5">
            <h2>Welcome to the Quiz!</h2>

            <p>
                <strong>Quiz ID:</strong>
                <asp:TextBox ID="txtQuizID" runat="server" CssClass="form-control d-inline-block w-auto" />
            </p>

            <asp:Button ID="btnLoadQuiz" runat="server" Text="Load Quiz Info" CssClass="btn btn-info mt-2"
                OnClick="btnLoadQuiz_Click" />



            <asp:Label ID="txtQuizName" runat="server" Text="Quiz Name" CssClass="form-control"
                readOnly="true" Visible="false"></asp:Label>
            <asp:HyperLink ID="lnkMethodology" runat="server" Text="Methodology" Target="_blank"
                CssClass="d-block" Visible="false"></asp:HyperLink>

            <asp:Button ID="btnStartQuiz" runat="server" CssClass="btn btn-success ms-2"
                Text="Start Quiz"
                OnClientClick="startQuizUI(); return false;" OnClick="btnStartQuiz_Click"
                Visible="false" />
        </div>

        <div id="quiz" runat="server">
            <div class="text-end mb-3">
                <strong>Time Remaining: <span id="timerLabel" class="text-danger fw-bold">5:00</span></strong>
            </div>

            <asp:Repeater ID="Repeater1" runat="server" OnItemDataBound="rptQuestions_ItemDataBound">
                <ItemTemplate>
                    <div class="mb-3">
                        <h5>Q<%# Container.ItemIndex + 1 %>: <%# Eval("Question") %></h5>
                        <asp:RadioButtonList ID="rblOptions" runat="server" CssClass="form-check" />
                        <asp:HiddenField ID="hfquestion_no" runat="server" Value='<%# Eval("id") %>' />
                        <asp:HiddenField ID="hfCorrectAnswer" runat="server" Value='<%# Eval("Answer") %>' />
                    </div>
                    <hr />
                </ItemTemplate>
            </asp:Repeater>

            <asp:Button ID="Button2" runat="server" Text="Submit" CssClass="btn btn-primary mb-4"
                OnClick="Btn_submit_Click" />
            <asp:Literal ID="Literal1" runat="server" />
        </div>
    </form>
</body>
</html>
