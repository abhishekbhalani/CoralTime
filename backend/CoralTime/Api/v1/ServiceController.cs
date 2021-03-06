using CoralTime.BL.Interfaces;
using CoralTime.Common.Attributes;
using CoralTime.Common.Middlewares;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace CoralTime.Api.v1
{
    [Route("api/v1/[controller]")]
    [ServiceFilter(typeof(CheckSecureHeaderServiceFilter))]
    public class ServiceController : BaseController<ServiceController, IMemberService>
    {
        private readonly IMemberProjectRoleService _roleService;
        private readonly IRefreshDataBaseService _refreshDataBaseService;
        private readonly IAvatarService _avatarService;

        public ServiceController(
            IMemberService service, 
            IMemberProjectRoleService roleService, 
            IRefreshDataBaseService refreshDataBaseService, 
            IAvatarService avatarService,
            ILogger<ServiceController> logger) : base(logger, service)
        {
            _roleService = roleService;
            _refreshDataBaseService = refreshDataBaseService;
            _avatarService = avatarService;
        }

        // GET api/v1/Service/UpdateManagerRoles
        [HttpGet]
        [Route("UpdateManagerRoles")]
        public ActionResult UpdateManagerRoles()
        {
            try
            {
                var result = _roleService.FixAllManagerRoles();
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"UpdateManagerRoles method {e}");
                var errors = ExceptionsChecker.CheckProjectRolesException(e);
                return BadRequest(errors);
            }
        }

        // GET api/v1/Service/UpdateClaims
        [HttpGet]
        [Route("UpdateClaims")]
        public ActionResult UpdateClaims()
        {
            try
            {
                _service.UpdateUsersClaims();
                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"UpdateClaims method {e}");
                var errors = ExceptionsChecker.CheckMembersException(e);
                return BadRequest(errors);
            }
        }

        [HttpGet]
        [Route("RefreshDataBase")]
        public async Task<ActionResult >RefreshDataBase()
        {
            try
            {
                await _refreshDataBaseService.RefreshDataBase();
                return Ok(true);
            }
            catch (Exception e)
            {
                _logger.LogWarning($"RefreshDataBase method {e}");
                var errors = ExceptionsChecker.CheckMembersException(e);
                return BadRequest(errors);
            }
        }

        // GET api/v1/Service/UpdateIconFilesCache
        [HttpGet]
        [Route("UpdateIconFilesCache")]
        public ActionResult UpdateIconFilesCache()
        {
            try
            {
                _avatarService.SaveAllIconsAndAvatarsInStaticFiles();
                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"SaveAllIconsAndAvatarsInStaticFiles method {e}");
                return BadRequest(e.Message);
            }
        }
    }
}