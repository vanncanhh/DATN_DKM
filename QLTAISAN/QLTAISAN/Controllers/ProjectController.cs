namespace QLTAISAN.Controllers
{
    public class ProjectController : Controller
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }

        public async Task<IActionResult> Project()
        {
            var projects = await _projectService.GetAllProjectsAsync();
            ViewBag.ProjectNb = projects.Count();
            ViewData["User"] = await _projectService.GetAllUsersAsync();
            return View(projects);
        }

        public async Task<IActionResult> AddProject()
        {
            ViewData["User"] = await _projectService.GetAllUsersAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddProject(ProjectDkc project)
        {
            await _projectService.AddProjectAsync(project);
            return RedirectToAction("Project");
        }

        public async Task<IActionResult> EditProject(int id)
        {
            var project = await _projectService.GetProjectByIdAsync(id);
            ViewData["User"] = await _projectService.GetAllUsersAsync();
            return View(project);
        }

        [HttpPost]
        public async Task<IActionResult> EditProject(ProjectDkc project)
        {
            await _projectService.UpdateProjectAsync(project);
            return RedirectToAction("Project");
        }

        [HttpPost]
        public async Task<JsonResult> DeleteProject(int id)
        {
            var result = await _projectService.DeleteProjectAsync(id);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> AddDeviceToProject(int projectId, int deviceId, string notes)
        {
            var result = await _projectService.AddDeviceToProjectAsync(projectId, deviceId, notes);
            return Json(result);
        }

        [HttpPost]
        public async Task<JsonResult> ReturnDeviceFromProject(int projectId, int deviceId, string notes)
        {
            var result = await _projectService.ReturnDeviceFromProjectAsync(projectId, deviceId, notes);
            return Json(result);
        }
    }
}
