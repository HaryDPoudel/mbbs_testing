using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Data.SqlClient;



public static class GetQuestionsFunction
{
    [Function("GetQuestions")]
    public static async Task<IActionResult> Run(
        [HttpTrigger(AuthorizationLevel.Function, "get", Route = "questions")] HttpRequest req)
    {
        var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
        var questions = new List<Question>();

        using (var conn = new SqlConnection(connectionString))
        {
            await conn.OpenAsync();
            var query = @"SELECT Id, QuestionText, Explanation, CorrectOption, OptionA, OptionB, OptionC, OptionD, SubjectName, TopicName, ChoiceType 
                          FROM Questions";

            using (var cmd = new SqlCommand(query, conn))
            using (var reader = await cmd.ExecuteReaderAsync())
            {
                while (await reader.ReadAsync())
                {
                    questions.Add(new Question
                    {
                        Id = reader.GetGuid(0),
                        QuestionText = reader.GetString(1),
                        Explanation = reader.IsDBNull(2) ? null : reader.GetString(2),
                        CorrectOption = reader.GetInt32(3),
                        OptionA = reader.GetString(4),
                        OptionB = reader.GetString(5),
                        OptionC = reader.GetString(6),
                        OptionD = reader.GetString(7),
                        SubjectName = reader.GetString(8),
                        TopicName = reader.GetString(9),
                        ChoiceType = reader.GetString(10)
                    });
                }
            }
        }

        return new OkObjectResult(questions);
    }
}

public class Question
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; }
    public string Explanation { get; set; }
    public int CorrectOption { get; set; }
    public string OptionA { get; set; }
    public string OptionB { get; set; }
    public string OptionC { get; set; }
    public string OptionD { get; set; }
    public string SubjectName { get; set; }
    public string TopicName { get; set; }
    public string ChoiceType { get; set; }
}
