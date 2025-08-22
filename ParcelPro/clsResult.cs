﻿namespace ParcelPro
{
    public class clsResult
    {
        public bool Success { get; set; } = false;
        public string? Message { get; set; }
        public string? returnUrl { get; set; }
        public int updateType { get; set; } = 1; // 1 = Refresh Page  2 =Update Ajax 
        public string? updateTargetId { get; set; }  //#My_Element 
        public bool ShowMessage { get; set; } = true;
        public Guid? Id { get; set; }
        public object? ReturnData { get; set; } = null;

    }
}
