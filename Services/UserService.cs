using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace thaicodelab_api.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext _context;

        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }

        // public async Task<List<UserAllRequest>> GetAllUsers()
        // {
        //     var query = from u in _context.tb_users
        //                 join r in _context.tb_roles on u.role_id equals r.role_id
        //                 join d in _context.tb_departments on u.department_id equals d.department_id
        //                 join rk in _context.tb_ranks on u.rank_id equals rk.rank_id
        //                 join g in _context.tb_genders on u.gender_id equals g.gender_id
        //                 join us in _context.tb_user_status on u.user_status_id equals us.user_status_id
        //                 where !u.is_deleted
        //                 orderby u.user_id
        //                 select new UserAllRequest
        //                 {
        //                     user_id = u.user_id,
        //                     user_code = u.user_code,
        //                     email = u.email,
        //                     role_id = r.role_id,
        //                     role_code = r.role_code,
        //                     role = r.role,
        //                     department_id = d.department_id,
        //                     department_code = d.department_code,
        //                     department = d.department,
        //                     rank_id = rk.rank_id,
        //                     rank_code = rk.rank_code,
        //                     full_rank = rk.full_rank,
        //                     short_rank = rk.short_rank,
        //                     firstname = u.firstname,
        //                     lastname = u.lastname,
        //                     gender_id = g.gender_id,
        //                     gender_code = g.gender_code,
        //                     gender = g.gender,
        //                     birthdate = u.birthdate,
        //                     phone_number = u.phone_number,
        //                     user_status_id = us.user_status_id,
        //                     user_status_code = us.user_status_code,
        //                     user_status = us.user_status
        //                 };

        //     return await query.ToListAsync();
        // }

        public async Task<List<UserAllRequest>> GetAllUsers()
        {
            return await _context.tb_users
                .Where(u => !u.is_deleted)
                .Join(_context.tb_roles, u => u.role_id, r => r.role_id, (u, r) => new { u, r })
                .Join(_context.tb_departments, ur => ur.u.department_id, d => d.department_id, (ur, d) => new { ur.u, ur.r, d })
                .Join(_context.tb_ranks, urd => urd.u.rank_id, rk => rk.rank_id, (urd, rk) => new { urd.u, urd.r, urd.d, rk })
                .Join(_context.tb_genders, urdk => urdk.u.gender_id, g => g.gender_id, (urdk, g) => new { urdk.u, urdk.r, urdk.d, urdk.rk, g })
                .Join(_context.tb_user_status, urdkg => urdkg.u.user_status_id, us => us.user_status_id, (urdkg, us) => new { urdkg.u, urdkg.r, urdkg.d, urdkg.rk, urdkg.g, us })
                .OrderBy(urdkgu => urdkgu.u.user_id)
                .Select(urdkgu => new UserAllRequest
                {
                    user_id = urdkgu.u.user_id,
                    user_code = urdkgu.u.user_code,
                    email = urdkgu.u.email,
                    role_id = urdkgu.r.role_id,
                    role_code = urdkgu.r.role_code,
                    role = urdkgu.r.role,
                    department_id = urdkgu.d.department_id,
                    department_code = urdkgu.d.department_code,
                    department = urdkgu.d.department,
                    rank_id = urdkgu.rk.rank_id,
                    rank_code = urdkgu.rk.rank_code,
                    full_rank = urdkgu.rk.full_rank,
                    short_rank = urdkgu.rk.short_rank,
                    firstname = urdkgu.u.firstname,
                    lastname = urdkgu.u.lastname,
                    gender_id = urdkgu.g.gender_id,
                    gender_code = urdkgu.g.gender_code,
                    gender = urdkgu.g.gender,
                    birthdate = urdkgu.u.birthdate,
                    phone_number = urdkgu.u.phone_number,
                    user_status_id = urdkgu.us.user_status_id,
                    user_status_code = urdkgu.us.user_status_code,
                    user_status = urdkgu.us.user_status
                })
                .ToListAsync();
        }

        public async Task<User?> GetUserById(int id)
        {
            return await _context.tb_users
                .FirstOrDefaultAsync(u => u.user_id == id && !u.is_deleted);
        }

        public async Task<int?> GetRoleIdByUserId(int userId)
        {
            return await _context.tb_users
                .Where(u => u.user_id == userId && !u.is_deleted)
                .Select(u => u.role_id)
                .FirstOrDefaultAsync();
        }

        public async Task<object?> GetUserNameById(int id)
        {
            return await _context.tb_users
                .Where(u => u.user_id == id && !u.is_deleted)
                .Select(u => new
                {
                    u.firstname,
                    u.lastname,
                    u.email
                })
                .FirstOrDefaultAsync();
        }

        // public async Task<UserProfileRequest?> GetUserProfileById(int id)
        // {
        //     var query = from u in _context.tb_users
        //                 join r in _context.tb_roles on u.role_id equals r.role_id
        //                 join d in _context.tb_departments on u.department_id equals d.department_id
        //                 join rk in _context.tb_ranks on u.rank_id equals rk.rank_id
        //                 join g in _context.tb_genders on u.gender_id equals g.gender_id
        //                 join us in _context.tb_user_status on u.user_status_id equals us.user_status_id
        //                 where u.user_id == id && !u.is_deleted
        //                 select new UserProfileRequest
        //                 {
        //                     user_id = u.user_id,
        //                     user_code = u.user_code,
        //                     email = u.email,
        //                     role_id = r.role_id,
        //                     role_code = r.role_code,
        //                     role = r.role,
        //                     department_id = d.department_id,
        //                     department_code = d.department_code,
        //                     department = d.department,
        //                     rank_id = rk.rank_id,
        //                     rank_code = rk.rank_code,
        //                     full_rank = rk.full_rank,
        //                     short_rank = rk.short_rank,
        //                     firstname = u.firstname,
        //                     lastname = u.lastname,
        //                     gender_id = g.gender_id,
        //                     gender_code = g.gender_code,
        //                     gender = g.gender,
        //                     birthdate = u.birthdate,
        //                     phone_number = u.phone_number,
        //                     user_status_id = us.user_status_id,
        //                     user_status_code = us.user_status_code,
        //                     user_status = us.user_status
        //                 };

        //     return await query.FirstOrDefaultAsync();
        // }

        public async Task<UserProfileRequest?> GetUserProfileById(int id)
        {
            return await _context.tb_users
                .Where(u => u.user_id == id && !u.is_deleted)
                .Join(_context.tb_roles, u => u.role_id, r => r.role_id, (u, r) => new { u, r })
                .Join(_context.tb_departments, ur => ur.u.department_id, d => d.department_id, (ur, d) => new { ur.u, ur.r, d })
                .Join(_context.tb_ranks, urd => urd.u.rank_id, rk => rk.rank_id, (urd, rk) => new { urd.u, urd.r, urd.d, rk })
                .Join(_context.tb_genders, urdk => urdk.u.gender_id, g => g.gender_id, (urdk, g) => new { urdk.u, urdk.r, urdk.d, urdk.rk, g })
                .Join(_context.tb_user_status, urdkg => urdkg.u.user_status_id, us => us.user_status_id, (urdkg, us) => new { urdkg.u, urdkg.r, urdkg.d, urdkg.rk, urdkg.g, us })
                .Select(urdkgus => new UserProfileRequest
                {
                    user_id = urdkgus.u.user_id,
                    user_code = urdkgus.u.user_code,
                    email = urdkgus.u.email,
                    role_id = urdkgus.r.role_id,
                    role_code = urdkgus.r.role_code,
                    role = urdkgus.r.role,
                    department_id = urdkgus.d.department_id,
                    department_code = urdkgus.d.department_code,
                    department = urdkgus.d.department,
                    rank_id = urdkgus.rk.rank_id,
                    rank_code = urdkgus.rk.rank_code,
                    full_rank = urdkgus.rk.full_rank,
                    short_rank = urdkgus.rk.short_rank,
                    firstname = urdkgus.u.firstname,
                    lastname = urdkgus.u.lastname,
                    gender_id = urdkgus.g.gender_id,
                    gender_code = urdkgus.g.gender_code,
                    gender = urdkgus.g.gender,
                    birthdate = urdkgus.u.birthdate,
                    phone_number = urdkgus.u.phone_number,
                    user_status_id = urdkgus.us.user_status_id,
                    user_status_code = urdkgus.us.user_status_code,
                    user_status = urdkgus.us.user_status
                })
                .FirstOrDefaultAsync();
        }

        public async Task<string> GenerateUserCode()
        {
            var latestUser = await _context.tb_users
                .OrderByDescending(u => u.user_code)
                .FirstOrDefaultAsync();

            string newCode = "USR0000001";
            if (latestUser != null && !string.IsNullOrEmpty(latestUser.user_code))
            {
                var match = Regex.Match(latestUser.user_code, @"\d+");
                if (match.Success)
                {
                    int latestNumber = int.Parse(match.Value);
                    newCode = $"USR{(latestNumber + 1).ToString("D7")}";
                }
            }
            return newCode;
        }

        public async Task<bool> CheckIfEmailExists(string email)
        {
            return await _context.tb_users
                .AnyAsync(u => u.email == email && !u.is_deleted);
        }

        public async Task<bool> CheckIfEmailExistsExceptCurrent(string email, int userId)
        {
            return await _context.tb_users
                .AnyAsync(u => u.email == email && u.user_id != userId && !u.is_deleted);
        }

        public async Task AddUser(UserRequest UserRequest, int userId)
        {
            if (string.IsNullOrEmpty(UserRequest.user_password))
            {
                throw new ArgumentException("Password cannot be null or empty.");
            }
            var user = new User
            {
                user_code = UserRequest.user_code,
                email = UserRequest.email,
                user_password = BCrypt.Net.BCrypt.HashPassword(UserRequest.user_password),
                role_id = UserRequest.role_id,
                department_id = UserRequest.department_id,
                rank_id = UserRequest.rank_id,
                firstname = UserRequest.firstname,
                lastname = UserRequest.lastname,
                gender_id = UserRequest.gender_id,
                birthdate = UserRequest.birthdate,
                phone_number = UserRequest.phone_number,
                created_at = DateTime.UtcNow,
                updated_at = DateTime.UtcNow,
                created_by = userId,
                updated_by = userId,
                user_status_id = UserRequest.user_status_id
            };
            _context.tb_users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteUser(User user, int userId)
        {
            user.is_deleted = true;
            user.updated_by = userId;
            user.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserEmail(User user, string newEmail, int userId)
        {
            user.email = newEmail;
            user.updated_by = userId;
            user.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserPassword(User user, string newPassword, int userId)
        {
            if (string.IsNullOrEmpty(newPassword))
            {
                throw new ArgumentException("Password cannot be null or empty.");
            }
            user.user_password = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.updated_by = userId;
            user.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserProfile(User existingUser, UserRequest UserRequest, int userId)
        {
            existingUser.firstname = UserRequest.firstname;
            existingUser.lastname = UserRequest.lastname;
            existingUser.role_id = UserRequest.role_id;
            existingUser.department_id = UserRequest.department_id;
            existingUser.rank_id = UserRequest.rank_id;
            existingUser.gender_id = UserRequest.gender_id;
            existingUser.birthdate = UserRequest.birthdate;
            existingUser.phone_number = UserRequest.phone_number;
            existingUser.user_status_id = UserRequest.user_status_id;
            existingUser.updated_by = userId;
            existingUser.updated_at = DateTime.UtcNow;

            await _context.SaveChangesAsync();
        }
    }
}
