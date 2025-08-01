﻿using System.ComponentModel.DataAnnotations;

namespace ParcelPro.Models
{
    public class AppTheme
    {
        [Key]
        public int Id { get; set; }
        public string? StyleFileName { get; set; }
        public string? CssClass { get; set; }
        public bool IsDark { get; set; }
    }
}
