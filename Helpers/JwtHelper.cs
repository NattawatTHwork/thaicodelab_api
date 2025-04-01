using System.Security.Claims;

namespace thaicodelab_api.Helpers
{
    public static class JwtHelper
    {
        public static int GetUserIdFromToken(ClaimsPrincipal user)
        {
            var userIdClaim = user.FindFirst("user_id")?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            throw new UnauthorizedAccessException("User ID not found in token.");
        }

        public static int GetRoleIdFromToken(ClaimsPrincipal user)
        {
            var roleIdClaim = user.FindFirst("role_id")?.Value;
            if (int.TryParse(roleIdClaim, out int roleId))
            {
                return roleId;
            }
            throw new UnauthorizedAccessException("Role ID not found in token.");
        }


        public static int GetDepartmentIdFromToken(ClaimsPrincipal user)
        {
            var departmentIdClaim = user.FindFirst("department_id")?.Value;
            if (int.TryParse(departmentIdClaim, out int departmentId))
            {
                return departmentId;
            }
            throw new UnauthorizedAccessException("Department ID not found in token.");
        }
    }
}
