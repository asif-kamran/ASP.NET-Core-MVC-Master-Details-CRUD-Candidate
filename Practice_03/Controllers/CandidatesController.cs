using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Practice_03.Models;
using Practice_03.Models.ViewModel;

namespace Practice_03.Controllers
{
    public class CandidatesController : Controller
    {
        private readonly CandidateDbCoreContext db;
        private readonly IWebHostEnvironment he;
        public CandidatesController(CandidateDbCoreContext db, IWebHostEnvironment he)
        {
            this.db = db;
            this.he = he;
        }
        public IActionResult Index(string userText, string sortOrder)
        {
            ViewBag.sWord = userText;
            ViewBag.sort = string.IsNullOrEmpty(sortOrder) ? "desc_name" : "";
            IQueryable<Candidate> can = db.Candidates.Include(x => x.CandidateSkills).ThenInclude(y => y.Skill);
            if (!string.IsNullOrEmpty(userText))
            {
                userText = userText.ToLower();
                can = can.Where(x => x.CandidateName.ToLower().Contains(userText));
            }
            switch (sortOrder)
            {
                case "desc_name":
                    can = can.OrderByDescending(x => x.CandidateName);
                    break;
                default:
                    can = can.OrderBy(x => x.CandidateName);
                    break;
            }
            return View(can);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult skill(int id)
        {
            ViewBag.skill = new SelectList(db.Skills, "SkillId", "SkillName", id.ToString() ?? "");
            return PartialView("_skill");
        }
        [HttpPost]
        public async Task<IActionResult> Create(CandidateVM candidateVM, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Candidate candidate = new Candidate()
                {
                    CandidateName = candidateVM.CandidateName,
                    DateOfBirth = candidateVM.DateOfBirth,
                    Phone = candidateVM.Phone,
                    Fresher = candidateVM.Fresher
                };
                //image
                var file = candidateVM.ImageFile;
                string webrooit = he.WebRootPath;
                string folder = "Images";
                string imgfilename = Path.GetFileName(candidateVM.ImageFile.FileName);
                string fileToSave = Path.Combine(webrooit, folder, imgfilename);
                if (file != null)
                {
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        candidateVM.ImageFile.CopyTo(stream);
                        candidate.Image = "/" + folder + "/" + imgfilename;
                    }
                }
                //skill
                foreach (var item in skillId)
                {
                    CandidateSkill candidateSkill = new CandidateSkill()
                    {
                        Candidate = candidate,
                        CandidateId = candidate.CandidateId,
                        SkillId = item
                    };
                    db.CandidateSkills.Add(candidateSkill);
                }
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
        public IActionResult Delete(int id)
        {
            var candidate = db.Candidates.Find(id);
            if (id != null)
            {
                var exSkill = db.CandidateSkills.Where(x => x.CandidateId == id).ToList();
                db.CandidateSkills.RemoveRange(exSkill);
                db.Candidates.Remove(candidate);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Edit(int id)
        {
            var candidate = await db.Candidates.FirstOrDefaultAsync(x => x.CandidateId == id);
            CandidateVM candidateVM = new CandidateVM()
            {
                CandidateId=candidate.CandidateId,
                CandidateName = candidate.CandidateName,
                DateOfBirth = candidate.DateOfBirth,
                Phone = candidate.Phone,
                Image=candidate.Image,
                Fresher=candidate.Fresher
            };
            var exixtSkill =db.CandidateSkills.Where(x=>x.CandidateId == id).ToList();
            foreach (var item in exixtSkill)
            {
                candidateVM.candidateskill.Add((int)item.SkillId);
            }
            return View(candidateVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(CandidateVM candidateVM, int[] skillId)
        {
            if (ModelState.IsValid)
            {
                Candidate candidate = new Candidate()
                {
                    CandidateId= candidateVM.CandidateId,
                    CandidateName = candidateVM.CandidateName,
                    DateOfBirth = candidateVM.DateOfBirth,
                    Phone = candidateVM.Phone,
                    Image=candidateVM.Image,
                    Fresher = candidateVM.Fresher
                };
                //image
                var file = candidateVM.ImageFile;
                if (file != null)
                {
                    string webrooit = he.WebRootPath;
                    string folder = "Images";
                    string imgfilename = Path.GetFileName(candidateVM.ImageFile.FileName);
                    string fileToSave = Path.Combine(webrooit, folder, imgfilename);
                    using (var stream = new FileStream(fileToSave, FileMode.Create))
                    {
                        candidateVM.ImageFile.CopyTo(stream);
                        candidate.Image = "/" + folder + "/" + imgfilename;
                    }
                }
                else
                {
                    candidate.Image = candidateVM.Image;
                }
                var existSkill = db.CandidateSkills.Where(x => x.CandidateId == candidate.CandidateId).ToList();
                foreach (var item in existSkill)
                {
                    db.CandidateSkills.Remove(item);
                }
                foreach (var item in skillId)
                {
                    CandidateSkill candidateSkill = new CandidateSkill()
                    {
                        CandidateId = candidate.CandidateId,
                        SkillId = item
                    };
                    db.CandidateSkills.Add(candidateSkill);
                }
                db.Update(candidate);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
