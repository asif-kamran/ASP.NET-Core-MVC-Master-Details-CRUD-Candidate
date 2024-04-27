namespace Practice_03.Models.ViewModel
{
    public class CandidateVM
    {
        public int CandidateId { get; set; }

        public string CandidateName { get; set; } = null!;

        public DateTime DateOfBirth { get; set; }

        public string Phone { get; set; } = null!;

        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }

        public bool Fresher { get; set; }
        public List<int> candidateskill {  get; set; }=new List<int>();
    }
}
