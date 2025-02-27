using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Data;

namespace test.Entity
{
    [Table("Product")]
    public class TourEntity :  StateObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecId { get; set; }
        public int MainRecId { get; set; }
        public int SubRecId { get; set; }
        public int IntermediateRecId { get; set; }
        public string ProductCode { get; set; }
        public int Status { get; set; }
        public string Code { get; set; }
        public string Remark { get; set; }

        public int? IsDeleted { get; set; }
        [NotMapped]
        public PostValue PostValue { get; set; }

    }
}
