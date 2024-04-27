create table Skills
(
	SkillId int primary key identity,
	SkillName nvarchar(50) not null
)

create table Candidates
(
	CandidateId int primary key identity,
	CandidateName nvarchar(50) not null,
	DateOfBirth date not null,
	Phone nvarchar(50) not null,
	Image nvarchar(max) ,
	Fresher bit
)

create table CandidateSkills
(
	CandidateSkillId int primary key identity,
	CandidateId int references  Candidates(CandidateId),
	SkillId int references  Skills(SkillId)
)