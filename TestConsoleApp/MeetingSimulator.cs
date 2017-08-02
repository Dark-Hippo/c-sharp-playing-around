using System;

namespace TestConsoleApp
{
    class MeetingSimulator
    {
        enum State
        {
            Start,
            Give_An_Estimation,
            Ask_A_Question,
            Escape_For_Coffee,
            Flip_The_Table,
            End
        }

        State gamestate = State.Start;

        private void ChangeState()
        {
            switch (gamestate)
            {
                case State.Give_An_Estimation: GiveAnEstimation();
                    break;
                case State.Ask_A_Question: AskAQuestion();
                    break;
                case State.Escape_For_Coffee: EscapeForCoffee();
                    break;
                case State.Flip_The_Table: FlipTheTable();
                    break;
                case State.Start: Start();
                    break;
                case State.End: End();
                    break;
            }
        }
        private void UpdateState(string choice)
        {
            switch (choice)
            {
                case "G":
                case "g":
                    gamestate = State.Give_An_Estimation;
                    ChangeState();
                    break;
                case "A":
                case "a":
                    gamestate = State.Ask_A_Question;
                    ChangeState();
                    break;
                case "E":
                case "e":
                    gamestate = State.Escape_For_Coffee;
                    ChangeState();
                    break;
                case "F":
                case "f":
                    gamestate = State.Flip_The_Table;
                    ChangeState();
                    break;
                case "S":
                case "s":
                    gamestate = State.Start;
                    ChangeState();
                    break;
                default: ChangeState();
                    break;
            }
        }
        public void Start()
        {
            Console.WriteLine("Enter a selection and press return key");
            Console.WriteLine();
            Console.WriteLine("You are sitting in a planning meeting, processes are being discussed.");
            Console.WriteLine("You are asked to estimate hours for a task that you have no idea about...");
            Console.WriteLine("(G)ive an estimation, (A)sk another question, (E)scape for coffee, (F)lip the table and walk out");
            Console.WriteLine();
            UpdateState(Console.ReadLine());
        }

        public void GiveAnEstimation()
        {
            Console.WriteLine($"You estimate {new Random().Next(2, 14)} hours");
            Console.WriteLine("Everyone is happy that the meeting is movin on");
            Console.WriteLine("(S)tart again.");
            Console.WriteLine();
            UpdateState(Console.ReadLine());

        }

        public void AskAQuestion()
        {
            Console.WriteLine("You ask a question, everybody groans and the discussion continues");
            Console.WriteLine("(G)ive an estimation, (A)sk another question, (E)scape for coffee, (F)lip the table and walk out");
            Console.WriteLine();
            UpdateState(Console.ReadLine());

        }

        public void EscapeForCoffee()
        {
            Console.WriteLine("You stand up, mutter something about needing coffee and leave the room");
            Console.WriteLine("Once outside, you realise that you've had no relevant input to the meeting, and head to a local pub for a more fun afternoon");
            Console.WriteLine("Game over.");
            Console.WriteLine("(S)tart again");
            Console.WriteLine();
            UpdateState(Console.ReadLine());

        }

        public void FlipTheTable()
        {
            Console.WriteLine("You brace yourself, and in one smooth move, stand from your chair and flip the table in front of you");
            Console.WriteLine("The room decends into silence and you turn, take a couple of steps and dive out of the nearest window, scattering glass everywhere");
            Console.WriteLine("Congratulations, you have escaped");
            Console.WriteLine("(S)tart again");
            Console.WriteLine();
            UpdateState(Console.ReadLine());
        }

        public void End()
        {

        }
    }
}
