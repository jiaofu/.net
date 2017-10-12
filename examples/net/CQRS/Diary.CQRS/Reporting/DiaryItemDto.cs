using System;
using System.ComponentModel.DataAnnotations;


namespace Diary.CQRS.Reporting
{
    public class DiaryItemDto
    {

        public Guid Id { get; set; }


        public int Version { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime From { get; set; }
        
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime To { get; set; }

        [DataType(DataType.MultilineText)]
        public string Description { get; set; }
    }
}