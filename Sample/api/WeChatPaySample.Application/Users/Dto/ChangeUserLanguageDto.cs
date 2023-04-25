using System.ComponentModel.DataAnnotations;

namespace WeChatPaySample.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}