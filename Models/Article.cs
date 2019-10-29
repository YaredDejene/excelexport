using System.Collections.Generic;

namespace ExcelExport.Models
{
    public class Article
    {
      public int ArticleId { get; set; }  
      public string ArticleTitle { get; set; }

      public  ICollection<ArticleReview> Reviews  { get; set; }
    }
}