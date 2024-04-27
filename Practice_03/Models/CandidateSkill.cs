using System;
using System.Collections.Generic;

namespace Practice_03.Models;

public partial class CandidateSkill
{
    public int CandidateSkillId { get; set; }

    public int? CandidateId { get; set; }

    public int? SkillId { get; set; }

    public virtual Candidate? Candidate { get; set; }

    public virtual Skill? Skill { get; set; }
}
