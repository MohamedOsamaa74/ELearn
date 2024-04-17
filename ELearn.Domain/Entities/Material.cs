using ELearn.Domain.Enum;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ELearn.Domain.Entities
{
    public class Material
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int Week { get; set; }
        public required MaterialType Type { get; set; }
        public required DateTime CreationDate { get; set; } = DateTime.UtcNow.ToLocalTime();
        
        #region ForeignKeys
        public required int GroupId { get; set; }
        public required string UserId { get; set; }
        #endregion

        #region NavigationProperties
        public virtual ApplicationUser User { get; set; }
        public virtual Group Group { get; set; }
        public virtual ICollection<FileEntity> Files { get; } = new List<FileEntity>();
        #endregion
    }
}