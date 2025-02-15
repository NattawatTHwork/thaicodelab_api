using System.ComponentModel.DataAnnotations;

public class EquipmentTransaction
{
    [Key]
    public int equipment_transaction_id { get; set; }

    public string equipment_transaction_code { get; set; } = string.Empty;

    [Required(ErrorMessage = "Approve User ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Approve User ID must be a valid ID")]
    public int approve_user_id { get; set; }

    [Required(ErrorMessage = "Operator Borrow User ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Operator Borrow User ID must be a valid ID")]
    public int operator_borrow_user_id { get; set; }

    [Required(ErrorMessage = "Borrow User ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Borrow User ID must be a valid ID")]
    public int borrow_user_id { get; set; }

    public DateTime borrow_timestamp { get; set; } = DateTime.UtcNow;

    [MaxLength(255, ErrorMessage = "Note must be less than 255 characters")]
    public string note { get; set; } = string.Empty;

    public DateTime updated_at { get; set; } = DateTime.UtcNow;

    public int updated_by { get; set; }

    public bool is_deleted { get; set; } = false;
}
