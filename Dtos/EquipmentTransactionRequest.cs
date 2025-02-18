public class EquipmentTransactionRequest
{
    public int equipment_transaction_id { get; set; }
    public string equipment_group_code { get; set; } = string.Empty;
    public int approve_user_id { get; set; }
    public int operator_borrow_user_id { get; set; }
    public int borrow_user_id { get; set; }
    public int[] equipment_ids { get; set; } = Array.Empty<int>();
    public DateTime borrow_timestamp { get; set; } = DateTime.UtcNow;
    public string note { get; set; } = string.Empty;
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
    public int updated_by { get; set; }
    public bool is_deleted { get; set; } = false;
}

public class EquipmentReturnRequest
{
    public required List<EquipmentReturnDetail> equipment_return_details { get; set; } = new List<EquipmentReturnDetail>();
    public required int return_user_id { get; set; }
    public required int operator_return_user_id { get; set; }
}

public class EquipmentReturnDetail
{
    public required int equipment_transaction_detail_id { get; set; }
    public string note { get; set; } = string.Empty;
}

public class EquipmentTransactionWithDetails
{
    public int equipment_transaction_id { get; set; }
    public string equipment_transaction_code { get; set; } = string.Empty;
    public int approve_user_id { get; set; }
    public int borrow_user_id { get; set; }
    public DateTime borrow_timestamp { get; set; }
    public string? note { get; set; }
    public DateTime updated_at { get; set; }
    public int updated_by { get; set; }
    public List<EquipmentTransactionDetail>? details { get; set; }
}

public class UnreturnedEquipmentWithGroup
{
    public int equipment_id { get; set; }
    public string equipment { get; set; } = string.Empty;
    public string equipment_code { get; set; } = string.Empty;
    public int equipment_transaction_detail_id { get; set; }
    public int equipment_transaction_id { get; set; }
    public int equipment_group_id { get; set; }
    public string equipment_group { get; set; } = string.Empty;
    public int department_id { get; set; }
}

