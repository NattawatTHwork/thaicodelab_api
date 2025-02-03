public class UserAllRequest
{
    public int user_id { get; set; }
    public string user_code { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public int role_id { get; set; }
    public string role_code { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
    public int department_id { get; set; }
    public string department_code { get; set; } = string.Empty;
    public string department { get; set; } = string.Empty;
    public int rank_id { get; set; }
    public string rank_code { get; set; } = string.Empty;
    public string full_rank { get; set; } = string.Empty;
    public string short_rank { get; set; } = string.Empty;
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public int gender_id { get; set; }
    public string gender_code { get; set; } = string.Empty;
    public string gender { get; set; } = string.Empty;
    public DateOnly birthdate { get; set; }
    public string phone_number { get; set; } = string.Empty;
    public int user_status_id { get; set; }
    public string user_status_code { get; set; } = string.Empty;
    public string user_status { get; set; } = string.Empty;
}

public class UserProfileRequest
{
    public int user_id { get; set; }
    public string user_code { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public int role_id { get; set; }
    public string role_code { get; set; } = string.Empty;
    public string role { get; set; } = string.Empty;
    public int department_id { get; set; }
    public string department_code { get; set; } = string.Empty;
    public string department { get; set; } = string.Empty;
    public int rank_id { get; set; }
    public string rank_code { get; set; } = string.Empty;
    public string full_rank { get; set; } = string.Empty;
    public string short_rank { get; set; } = string.Empty;
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public int gender_id { get; set; }
    public string gender_code { get; set; } = string.Empty;
    public string gender { get; set; } = string.Empty;
    public DateOnly birthdate { get; set; }
    public string phone_number { get; set; } = string.Empty;
    public int user_status_id { get; set; }
    public string user_status_code { get; set; } = string.Empty;
    public string user_status { get; set; } = string.Empty;
}

public class UserRequest
{
    public int user_id { get; set; }
    public string user_code { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string user_password { get; set; } = string.Empty;
    public string confirm_user_password { get; set; } = string.Empty;
    public int role_id { get; set; }
    public int department_id { get; set; }
    public int rank_id { get; set; }
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public int gender_id { get; set; }
    public DateOnly birthdate { get; set; }
    public string phone_number { get; set; } = string.Empty;
    public string recovery_code { get; set; } = string.Empty;
    public DateTime? recovery_expiration { get; set; }
    public DateTime created_at { get; set; } = DateTime.UtcNow;
    public DateTime updated_at { get; set; } = DateTime.UtcNow;
    public int created_by { get; set; }
    public int updated_by { get; set; }
    public int user_status_id { get; set; }
    public bool is_deleted { get; set; } = false;
}

