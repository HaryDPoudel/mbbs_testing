namespace mbbs_testing.Models
{
    //public class QuestionItem
    //{
    //    public string? question { get; set; }
    //    public string? opa { get; set; }
    //    public string? opb { get; set; }
    //    public string? opc { get; set; }
    //    public string? opd { get; set; }
    //    public string? subject_name { get; set; }
    //    public string? topic_name { get; set; }
    //    public string? id { get; set; }
    //    public string? choice_type { get; set; }
    //    //public string? explanation { get; set; }
    //}


    public class QuestionItem
    {
        // The main question text
        public string? question { get; set; }

        // The full explanation/rationale
        public string? exp { get; set; }

        // The index of the correct option (e.g., 1, 2, 3, or 4)
        public int cop { get; set; }

        // Option A text
        public string? opa { get; set; }

        // Option B text
        public string? opb { get; set; }

        // Option C text
        public string? opc { get; set; }

        // Option D text
        public string? opd { get; set; }

        // Metadata fields
        public string? subject_name { get; set; }
        public string? topic_name { get; set; }
        public string? id { get; set; }
        public string? choice_type { get; set; }
    }
}
