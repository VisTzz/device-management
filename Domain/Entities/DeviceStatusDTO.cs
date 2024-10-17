﻿namespace DevicesManagement.Domain.Entities
{
    public class DeviceStatusDTO

    {

        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string DeviceStatus { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
}