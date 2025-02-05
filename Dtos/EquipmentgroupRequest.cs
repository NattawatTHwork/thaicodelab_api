public class EquipmentGroupRequest
{
    public int equipment_group_id { get; set; }
    public string equipment_group_code { get; set; } = string.Empty;
    public string equipment_group { get; set; } = string.Empty;
    public int department_id { get; set; }
    public string department_code { get; set; } = string.Empty; // เพิ่มฟิลด์จาก tb_departments
    public string department { get; set; } = string.Empty; // เพิ่มฟิลด์จาก tb_departments
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
    public int created_by { get; set; }
    public int updated_by { get; set; }
    public bool is_deleted { get; set; } = false;
}
