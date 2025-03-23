using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities;

public class UserCourse
{
    [ForeignKey("User")]
    public int UserId { get; set; }

    [ForeignKey("Course")]
    public int CourseId { get; set; }

    public float Progress { get; set; }

    public User User { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
