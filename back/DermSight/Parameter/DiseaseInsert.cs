using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DermSight.Parameter
{
    public class DiseaseInsert
    {
        public int DiseaseId { get; set; }
        [DisplayName("名稱")]
        [Required(ErrorMessage = "請輸入名稱")]
        public required string Name { get; set; }

        [DisplayName("描述")]
        [Required(ErrorMessage = "請輸入描述")]
        public required string Description { get; set; }
        [DisplayName("症狀")]
        [Required(ErrorMessage = "請輸入症狀")]
        public required List<string> Symptom { get; set;}
    }
}
