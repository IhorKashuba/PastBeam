using System.ComponentModel.DataAnnotations.Schema;

namespace PastBeam.Core.Library.Entities;

[Table("user_courses")]
public class UserCourse
{
    [ForeignKey("User")]
    [Column("user_id")]
    public int UserId { get; set; }

    [ForeignKey("Course")]
    [Column("course_id")]
    public int CourseId { get; set; }

    [Column("progress")]
    public float Progress { get; set; }

    public User User { get; set; } = null!;
    public Course Course { get; set; } = null!;
}
