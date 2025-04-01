public class EquipmentRequest
{
    public int equipment_id { get; set; }
    public string equipment_code { get; set; } = string.Empty;
    public string equipment_unique_code { get; set; } = string.Empty;
    public string equipment { get; set; } = string.Empty;
    public string description { get; set; } = string.Empty;
    public int equipment_group_id { get; set; }
    public string equipment_group_code { get; set; } = string.Empty;
    public string equipment_group { get; set; } = string.Empty;
    public int equipment_type_id { get; set; }
    public string equipment_type_code { get; set; } = string.Empty;
    public string equipment_type { get; set; } = string.Empty;
    public int equipment_status_id { get; set; }
    public string equipment_status_code { get; set; } = string.Empty;
    public string equipment_status { get; set; } = string.Empty;
    public int borrow_user_id { get; set; }
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string full_rank { get; set; } = string.Empty;
    public string short_rank { get; set; } = string.Empty;

}
