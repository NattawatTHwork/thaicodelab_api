using Microsoft.EntityFrameworkCore;

namespace thaicodelab_api.Services
{
    public class RolePermissionService
    {
        private readonly ApplicationDbContext _context;

        public RolePermissionService(ApplicationDbContext context)
        {
            _context = context;
        }

        // public async Task CreateMultipleRolePermissions(int roleId, List<int> permissionIds, int userId)
        // {
        //     var existingPermissions = await _context.tb_role_permissions
        //         .Where(rp => rp.role_id == roleId && !rp.is_deleted)
        //         .ToListAsync();

        //     var existingPermissionIds = existingPermissions.Select(rp => rp.permission_id).ToList();

        //     var permissionsToAdd = permissionIds.Except(existingPermissionIds).ToList();

        //     var permissionsToDelete = existingPermissionIds.Except(permissionIds).ToList();

        //     var newRolePermissions = permissionsToAdd.Select(permissionId => new RolePermission
        //     {
        //         role_id = roleId,
        //         permission_id = permissionId,
        //         is_deleted = false,
        //         created_at = DateTime.UtcNow,
        //         updated_at = DateTime.UtcNow,
        //         created_by = userId,
        //         updated_by = userId
        //     }).ToList();

        //     if (newRolePermissions.Any())
        //     {
        //         _context.tb_role_permissions.AddRange(newRolePermissions);
        //     }

        //     var rolePermissionsToUpdate = existingPermissions.Where(rp => permissionsToDelete.Contains(rp.permission_id)).ToList();
        //     foreach (var rp in rolePermissionsToUpdate)
        //     {
        //         rp.is_deleted = true;
        //         rp.updated_at = DateTime.UtcNow;
        //         rp.updated_by = userId;
        //     }

        //     await _context.SaveChangesAsync();
        // }
        public async Task CreateMultipleRolePermissions(int roleId, List<int> permissionIds, int userId)
        {
            foreach (var permissionId in permissionIds)
            {
                var existing = await _context.tb_role_permissions
                    .FirstOrDefaultAsync(rp => rp.role_id == roleId && rp.permission_id == permissionId);

                if (existing == null)
                {
                    // Create new
                    var newRolePermission = new RolePermission
                    {
                        role_id = roleId,
                        permission_id = permissionId,
                        created_by = userId,
                        updated_by = userId,
                        is_deleted = false,
                        created_at = DateTime.UtcNow,
                        updated_at = DateTime.UtcNow
                    };
                    _context.tb_role_permissions.Add(newRolePermission);
                }
                else
                {
                    // Update existing
                    existing.updated_at = DateTime.UtcNow;
                    existing.updated_by = userId;
                    if (existing.is_deleted)
                    {
                        existing.is_deleted = false;
                    }

                    _context.tb_role_permissions.Update(existing);
                }
            }

            // Soft delete permissions not in the list
            var existingRolePermissions = await _context.tb_role_permissions
                .Where(rp => rp.role_id == roleId && !permissionIds.Contains(rp.permission_id))
                .ToListAsync();

            foreach (var item in existingRolePermissions)
            {
                item.is_deleted = true;
                item.updated_at = DateTime.UtcNow;
                item.updated_by = userId;
            }

            await _context.SaveChangesAsync();
        }

        public async Task<List<int>> GetPermissionsByRoleId(int roleId)
        {
            return await _context.tb_role_permissions
                .Where(rp => rp.role_id == roleId && !rp.is_deleted)
                .Select(rp => rp.permission_id)
                .ToListAsync();
        }
    }
}
