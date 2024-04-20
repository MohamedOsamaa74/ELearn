namespace ELearn.Application.DTOs.QuestionDTOs
{
    public class CreateQuestionDTO
    {
        public string Text { get; set; }
        //file
        public string Option1 { get; set; }
        public string Option2 { get; set; }
        public string Option3 { get; set; }
        public string Option4 { get; set; }
        public string Option5 { get; set; }

        public string CorrectOption { get; set; }
        public int Grade { get; set; }

    }
}
