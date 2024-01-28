﻿namespace ELearn.Domain.Entities
{
    public class UserSurvey
    {
       
        public int SurveyId { get; set; }
        public Survey Survey { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int OptionId { get; set; }
    }
}