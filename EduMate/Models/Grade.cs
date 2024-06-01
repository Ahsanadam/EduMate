public class Grade
{
    public int Id { get; set; }
    public int SubmissionId { get; set; }
    public string GradedBy { get; set; }
    public string Feedback { get; set; }
    public int Score { get; set; }
    public DateTime GradedAt { get; set; }
}