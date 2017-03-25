using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BuzzFeed2
{
    class Program
    {
        static void Main(string[] args)
        {

         /*   List<string> Question_ID_List = new List<string>();
            List<string> Question_Text_List = new List<string>();
            List<string> Question_Answers_List = new List<string>();
            List<string> Question_ID_List = new List<string>();*/

            List<List<String>> QandA_List = new List<List<String>>(); //Creates new nested List
            QandA_List.Add(new List<String>());
            QandA_List.Add(new List<String>());
            QandA_List.Add(new List<String>());
            QandA_List.Add(new List<String>()); //Creates two sub lists
            QandA_List.Add(new List<String>()); //Creates two sub lists


            char[] abc_choices = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(); // create character alphabet array for the answers later


            SqlConnection connection = new SqlConnection(@"Data Source=(LocalDb)\MSSQLLocalDB;AttachDbFilename=C:\Users\zackg\Source\Repos\QuizApp\QuizApp\Database1.mdf;Integrated Security=True"); connection.Open();

            bool keepPlaying = true;
            while (keepPlaying == true)
            {
                SqlCommand command;
                SqlDataReader reader;

                Console.WriteLine("Do you want to take a (Q)uiz or e(X)it?");
                string answer = Console.ReadLine().ToUpper();

                if (answer == "Q")
                {

                    //get a user name
                    Console.WriteLine("What is your name?");
                    string userName = Console.ReadLine();

                    command = new SqlCommand($"INSERT INTO Users (Name) VALUES ('{userName}')", connection);
                    command.ExecuteNonQuery();

                    //ask what quiz they want to take
                    Console.WriteLine("What quiz would you like to do?");

                    command = new SqlCommand("SELECT * FROM Quiz", connection);
                    reader = command.ExecuteReader();


                    //int quizCounter = 1;
                    //
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                           Console.WriteLine($"{reader["ID"]} - {reader["name"]}");
                            //quizCounter++;
                        }
                    }
                    Console.Write("Type in the Quiz number:  ");
                    int quizChoice = Convert.ToInt32(Console.ReadLine());

                    reader.Close();
                    //pass quiz_id to Results table
                    command = new SqlCommand($"INSERT INTO Results (quiz_id) VALUES ('{quizChoice}')", connection);
                    command.ExecuteNonQuery();






                    //ask a question and show choices
                    SqlCommand questionsCommand = new SqlCommand($"SELECT * FROM Questions JOIN Answers on Questions.Id = Answers.Question_id WHERE Quiz_id = {quizChoice}", connection);
                    reader = questionsCommand.ExecuteReader();

                    if (reader.HasRows)
                    {
                        string oldQuestion = "";

                        while (reader.Read())
                        {
                            if (!reader["Id"].ToString().Equals(oldQuestion))
                            {
                                //store question
                                oldQuestion = reader["Id"].ToString();
                                QandA_List[0].Add($"{reader["title"]}"); //ADD TO LIST question being asked
                            }
                            //Console.WriteLine($"The ID is = {reader["id"]} for {reader["text"]}");
                            // STORE ALL VARIABLES IN LIST FOR DISPLAY
                            QandA_List[1].Add($"{reader["text"]}"); //  ADD TO LIST  text for each answer
                            QandA_List[2].Add($"{reader["question_id"]}"); // ADD TO LIST question identifier
                            QandA_List[3].Add($"{reader["id"]}"); // ADD TO LIST  answer_id
                            QandA_List[4].Add($"{reader["SortOrder"]}");
                            //get an answer from user


                        }
                    }

                    int answer_id = 0;
                    int answer_counter = 0;
                    for (int Question_Index = 0; Question_Index < QandA_List[0].Count; Question_Index++) //for each question
                    {
                        Console.WriteLine(QandA_List[0][Question_Index]); // print question.
                        string CurrentQuestion = QandA_List[4][Question_Index]; // letter answer holder
                        Dictionary<string, string> dictionary = new Dictionary<string, string>(); // establishes dictionary





                        for (int Answer_Index = 0; Answer_Index < QandA_List[1].Count; Answer_Index++) // for each answer LOOP
                        {
                            if (CurrentQuestion == QandA_List[4][Answer_Index]) // if the answer option belongs to the current question
                            {
                                Console.WriteLine(abc_choices[answer_counter] + "\t" + QandA_List[1][answer_id]); //print answer text

                                dictionary.Add(abc_choices[answer_counter].ToString(), QandA_List[3][answer_id]);//makes dictionary key to match ID with (A, B, C, D)

                                answer_id++;
                                answer_counter++;
                            }

                        }
                        Console.Write(" Question " + Question_Index + 1 + " Response/Answer: ");
                        string user_answer = Console.ReadLine().ToUpper();

                        Console.WriteLine("dictionary of answer: " + dictionary[user_answer]);
                        Console.ReadLine();






                        answer_counter = 0;   // reset answer counter

                        //NEED to store answer
                    }



                    Console.WriteLine("End of for loop");
                        Console.ReadLine();




                    //tally result of answers

                    //give user the result

                    reader.Close();
                }//END OF Q
                else if (answer == "X")
                {
                    Console.WriteLine("Thank you for taking our quiz.  Good bye!");
                    keepPlaying = false;
                }
                else
                {
                    Console.WriteLine($"{answer} was not a valid response.  Please Try again.");
                }
            }//END OF MAIN LOOP
            connection.Close();
        }//END OF MAIN
    }//END OF CLASS


}