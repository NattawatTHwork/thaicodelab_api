using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class EquipmentTransactionDetail
{
    [Key]
    public int equipment_transaction_detail_id { get; set; }

    [Required]
    [ForeignKey("EquipmentTransaction")]
    public int equipment_transaction_id { get; set; }

    public string equipment_transaction_detail_code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Equipment ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Equipment ID must be a valid ID")]
    public int equipment_id { get; set; }

    public int? operator_return_user_id { get; set; }

    public int? return_user_id { get; set; }

    public DateTime return_timestamp { get; set; } = DateTime.UtcNow;

    [MaxLength(255, ErrorMessage = "Note must be less than 255 characters")]
    public string note { get; set; } = string.Empty;

    public DateTime updated_at { get; set; } = DateTime.UtcNow;

    public int updated_by { get; set; }

    public bool is_deleted { get; set; } = false;
}
