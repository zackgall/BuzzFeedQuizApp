using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
// TO DO
//      display questions in sort order
//      
// taly up result IDs at end to determine outcome

namespace BuzzFeed2
{
   /* 
    * POSSIBLE CLASS TO USE
    * class Questions
    {
        public int Questions_SortOrder { get; set; }
        public string Questions_Title { get; set; }
        public string Answers_Text { get; set; }
        public string Questions_Question_ID { get; set; }
        public string Answers_Answer_ID { get; set; }
        public string Answers_Result_ID { get; set; }
    } */


    class Program
    {
        static void Main(string[] args)
        {








            string user_id = "";
            string users_name = "";


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
                    //LISTS to store info for selected quiz.
                    List<string> Questions_Title_List = new List<string>();
                    List<string> Answers_Text_List = new List<string>();
                    List<string> Questions_Question_ID_List = new List<string>();
                    List<string> Answers_Answer_ID_List = new List<string>();
                    List<int> Questions_SortOrder_List = new List<int>();
                    List<string> Answers_Result_ID_List = new List<string>();
                    List<string> Results_Tally = new List<string>();
                    char[] abc_choices = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToCharArray(); // create character alphabet array for the answers later



                    //get a user name
                    Console.WriteLine("What is your name?");
                    users_name = Console.ReadLine();
                    command = new SqlCommand($"INSERT INTO Users (Name) VALUES ('{users_name}'); SELECT @@Identity AS ID", connection);
                    command.ExecuteNonQuery();

                    reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        user_id = (reader["id"]).ToString(); // save user_id as variable to be passed to UserAnswers later.
                    }
                    reader.Close();
                          Console.Clear();
                    //ask what quiz they want to take
                    Console.WriteLine($"Thanks, {users_name}!\nPlease enter the number of the quiz would you like to try.");

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
                                Questions_Title_List.Add($"{reader["title"]}"); //ADD TO LIST question being asked
                                Questions_SortOrder_List.Add(Convert.ToInt16($"{reader["SortOrder"]}")); // ADD TO LIST

                            }
                            // STORE ALL VARIABLES IN LIST FOR DISPLAY LATER
                            Answers_Text_List.Add($"{reader["text"]}"); //  ADD TO LIST  text for each answer
                            Questions_Question_ID_List.Add($"{reader["question_id"]}"); // ADD TO LIST question identifier
                            Answers_Answer_ID_List.Add($"{reader["id"]}"); // ADD TO LIST  answer_id
                            Answers_Result_ID_List.Add($"{reader["result_id"]}");
                        }
                    }
                    reader.Close(); // close reader

                    //begin question display
                    int answer_id = 0;
                    int answer_counter = 0;
                    //0 = title (writtenn question)
                    //1 = text (answer)
                    //2 = question_id (answer)
                    //3 = id (answer_id)
                    //4 = SortOrder (answers)
                    //5 = result_id (answers)


                    for (int Listed_Question = 0; Listed_Question < Questions_Title_List.Count; Listed_Question++) //for each question
                    {


                        // this is where I can order questions by sort order

                        Console.WriteLine(Questions_Title_List[Listed_Question]); // print question starting at index 0.
                        string CurrentQuestion = Questions_Question_ID_List[Listed_Question]; // abc letter answer holder
                        Dictionary<string, string> answer_key = new Dictionary<string, string>(); // establishes dictionary to be used as hash

                        for (int Answer_Index = 0; Answer_Index < Answers_Text_List.Count; Answer_Index++) // for each answer in ....
                        {
                            if (CurrentQuestion == Questions_Question_ID_List[Answer_Index]) // if the answer option belongs to the current question
                            {
                                Console.WriteLine(abc_choices[answer_counter] + "\t" + Answers_Text_List[answer_id]); //print answer text

                                answer_key.Add(abc_choices[answer_counter].ToString(), Answers_Answer_ID_List[answer_id]);// Sends answer_ID and associated letter to dictionary

                                answer_id++;
                                answer_counter++;
                            }

                        }
                        Console.Write(" Question " + Listed_Question + 1 + " Response/Answer: ");
                        string user_answer_id = answer_key[Console.ReadLine().ToUpper()];   // convert typed letter answer to associated answer_ID.


                        int index = Answers_Answer_ID_List.IndexOf(user_answer_id); // get index position of answer
                        Results_Tally.Add($"{Answers_Result_ID_List[index]}"); // Save result_ID from same index position

                        //pass user_id and answer_id to UserAnswers table
                        command = new SqlCommand($"INSERT INTO UserAnswers (user_id, answer_id) VALUES ('{user_id}','{user_answer_id}')", connection);
                        command.ExecuteNonQuery();
                        answer_counter = 0;   // reset answer counter
                    }
                    Console.WriteLine("End of for loop");
                        Console.ReadLine();




                    //tally result of answers
                    // NEED TO FIGURE OUT WAY TO SORT BY result_id COUNT, Match that result_id to Results table and display title and text.
                    var results = Results_Tally.GroupBy(i => i);
                    foreach (var result_id in results)
                    {
                        Console.WriteLine("Result ID: {0} Tally: {1}", result_id.Key, result_id.Count());
                    }




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