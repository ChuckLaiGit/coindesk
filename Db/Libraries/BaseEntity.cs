using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Db.Libraries
{
    /// <summary>
    /// 基本 Entity 欄位
    /// </summary>
    public class BaseEntity
    {
        [Key]
        [Column(Order = 1)] // 指定 PK 為 Code
        public required string Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime CreateTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime UpdateTime { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public bool IsDeleted { get; set; }
    }
}
