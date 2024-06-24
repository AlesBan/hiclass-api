using HiClass.Domain.Entities.Communication;
using HiClass.Domain.Entities.Job;
using HiClass.Domain.Entities.Location;
using HiClass.Domain.Entities.Notifications;
using HiClass.Domain.EntityConnections;

namespace HiClass.Domain.Entities.Main;

public class User
{
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string Email { get; set; } = string.Empty;
    public byte[] PasswordHash { get; set; } = new byte[64];
    public byte[] PasswordSalt { get; set; } = new byte[64];
    public string? AccessToken { get; set; } = string.Empty;
    public string? PasswordResetToken { get; set; } = string.Empty;
    public DateTime? ResetTokenExpires { get; set; }
    public string? PasswordResetCode { get; set; } = string.Empty;
    public string? VerificationCode { get; set; } = string.Empty;
    public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
    public bool IsCreatedAccount { get; set; }
    public bool IsVerified { get; set; }
    public bool IsInstitutionVerified { get; set; }
    public string? FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; } = string.Empty;
    public bool? IsATeacher { get; set; }
    public bool? IsAnExpert { get; set; }
    public ICollection<Class> Classes { get; set; } = new List<Class>();
    public Guid? InstitutionId { get; set; }
    public Institution? Institution { get; set; }
    public Guid? CityId { get; set; }
    public City? City { get; set; }
    public Guid? CountryId { get; set; }
    public Country? Country { get; set; }
    public string FullLocation => $"{City?.Title}, {(Country == null ? City?.Country?.Title : Country.Title)}";
    public ICollection<UserLanguage> UserLanguages { get; set; } = new List<UserLanguage>();
    public ICollection<UserDiscipline> UserDisciplines { get; set; } = new List<UserDiscipline>();
    public ICollection<UserGrade> UserGrades { get; set; } = new List<UserGrade>();
    public ICollection<Invitation> ReceivedInvitations { get; set; } = new List<Invitation>();
    public ICollection<Invitation> SentInvitations { get; set; } = new List<Invitation>();
    public ICollection<Feedback> ReceivedFeedbacks { get; set; } = new List<Feedback>();
    public ICollection<Feedback> SentFeedbacks { get; set; } = new List<Feedback>();
    public double Rating { get; set; }
    public string? Description { get; set; } = string.Empty;
    public string? ImageUrl { get; set; } = string.Empty;
    public string? BannerImageUrl { get; set; } = string.Empty;
    public ICollection<Device> Devices { get; set; } = new List<Device>();
    public ICollection<Notification> Notifications { get; set; } = new List<Notification>();
    public DateTime RegisteredAt { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }
    public DateTime LastOnlineAt { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}