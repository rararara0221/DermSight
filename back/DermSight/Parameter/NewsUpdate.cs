using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DermSight.Parameter
{
    public class NewsUpdate
    {
        [DisplayName("標題")]
        [Required(ErrorMessage ="請輸入標題")]
        public required string Title { get; set; }

        [DisplayName("內容")]
        [Required(ErrorMessage ="請輸入內容")]
        public required string Content { get; set; }
        [DisplayName("置頂")]
        public bool Pin { get; set; }
    }
}
