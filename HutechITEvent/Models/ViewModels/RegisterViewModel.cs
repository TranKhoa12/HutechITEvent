using System.ComponentModel.DataAnnotations;

namespace HutechITEvent.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Họ tên là bắt buộc")]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email là bắt buộc")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Mật khẩu là bắt buộc")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Mật khẩu phải có ít nhất 6 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Xác nhận mật khẩu là bắt buộc")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu xác nhận không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Vui lòng chọn vai trò")]
        public string Role { get; set; } = RoleNames.Student;
        
        // For Student role
        public string? StudentId { get; set; }
        public string? Class { get; set; }
        public string? PhoneNumber { get; set; }
    }
}