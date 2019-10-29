using System;

namespace ExcelExport.Models
{
    public class ArticlePoint
    {
        public int ArticlePointId { get; set; }
        public DateTime CreateOn { get; set; }
        public string Id { get; set; }      // Id of User that review Article
        public ApplicationUser JuryUser { get; set; }
        public string UserId { get; set; } // Id of User that write Article
        public int ArticleId { get; set; }
        public string JuryReview { get; set; }
    }
}