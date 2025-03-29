﻿using System.ComponentModel.DataAnnotations;

namespace RentalsProject.Models
{
    public class FileModel
    {
        [Key]
        public int Id { get; set; }


        public string? FileName { get; set; }


        public string? ContentType { get; set; }


        public byte[]? Content { get; set; }
    }
}
