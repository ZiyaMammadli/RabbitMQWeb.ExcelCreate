﻿using System.ComponentModel.DataAnnotations.Schema;

namespace RabbitMQWeb.ExcelCreate.Models
{
    public enum FileStatus
    {
        Creating,
        Completed
    }
    public class UserFIle
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public DateTime? CreatedDate { get; set; }
        public FileStatus FileStatus { get; set; }
        public AppUser User { get; set; }

        [NotMapped]
        public string GetCreatedDate => CreatedDate.HasValue ? CreatedDate.Value.ToShortDateString() : "-";

    }
}
