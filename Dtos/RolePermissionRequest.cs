public class RolePermissionRequest
{
    public int role_id { get; set; }
    public List<int> permission_ids { get; set; } = new();
}
