using System.ComponentModel.DataAnnotations;

public class EquipmentStatus
{
    public int equipment_status_id { get; set; }

    public string equipment_status_code { get; set; } = string.Empty;

    [MaxLength(50, ErrorMessage = "Status name must be less than 50 characters")]
    public string equipment_status { get; set; } = string.Empty;

    public DateTime created_at { get; set; } = DateTime.UtcNow;

    public DateTime updated_at { get; set; } = DateTime.UtcNow;

    public int created_by { get; set; }

    public int updated_by { get; set; }
    
    public bool is_deleted { get; set; } = false;
}
