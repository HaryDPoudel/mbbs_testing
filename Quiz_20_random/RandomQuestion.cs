using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Data.SqlClient;

public class GetRandomQuestions
{
    [Function("GetRandomQuestions")]
    public async Task<HttpResponseData> Run(
        [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
    {
        try
        {
            var connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("SqlConnectionString not set.");

            var questions = new List<QuestionItem>();

            using var conn = new SqlConnection(connectionString);
            await conn.OpenAsync();

            var cmd = new SqlCommand(@"
                SELECT TOP 20 *
                FROM MedicalTest
                ORDER BY NEWID()", conn);

            using var reader = await cmd.ExecuteReaderAsync();

            string GetStringSafe(SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);
            }

            int GetInt32Safe(SqlDataReader reader, string columnName)
            {
                int ordinal = reader.GetOrdinal(columnName);
                return reader.IsDBNull(ordinal) ? 0 : reader.GetInt32(ordinal); // default 0 if null
            }



            while (await reader.ReadAsync())
            {
                questions.Add(new QuestionItem
                {
                    Id = GetStringSafe(reader, "id"),
                    QuestionText = GetStringSafe(reader, "question"),
                    Explanation = GetStringSafe(reader, "exp"),
                    CorrectOption = GetInt32Safe(reader, "cop"),
                    OptionA = GetStringSafe(reader, "opa"),
                    OptionB = GetStringSafe(reader, "opb"),
                    OptionC = GetStringSafe(reader, "opc"),
                    OptionD = GetStringSafe(reader, "opd"),
                    SubjectName = GetStringSafe(reader, "subject_name"),
                    TopicName = GetStringSafe(reader, "topic_name"),
                    ChoiceType = GetStringSafe(reader, "choice_type")
                });
            }

            var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(questions);
            return response;
        }
        catch (Exception ex)
        {
            var response = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            await response.WriteStringAsync($"Error: {ex.Message}\n{ex.StackTrace}");
            return response;
        }
    }
}

// Model
public class QuestionItem
{
    public string Id { get; set; }
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
