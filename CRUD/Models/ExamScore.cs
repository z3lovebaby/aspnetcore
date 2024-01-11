using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD.Models
{
    public class ExamScore
    {
        public int Id { get; set; }
        public int StId { get; set; }
        [ForeignKey("StId")]
        public Student? Student { get; set; }
        public int CId { get; set; }
        [ForeignKey("CId")]
        public Course? Course { get; set; }
        public float Score { get; set; }
    }
}
