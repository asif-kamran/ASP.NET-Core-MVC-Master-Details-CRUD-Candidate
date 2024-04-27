using System;
using System.Collections.Generic;

namespace Practice_03.Models;

public partial class Candidate
{
    public int CandidateId { get; set; }

    public string CandidateName { get; set; } = null!;

    public DateTime DateOfBirth { get; set; }

    public string Phone { get; set; } = null!;

    public string? Image { get; set; }

    public bool Fresher { get; set; }

    public virtual ICollection<CandidateSkill> CandidateSkills { get; set; } = new List<CandidateSkill>();
}
