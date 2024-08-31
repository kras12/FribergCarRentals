using FribergCarRentals.Data.DatabaseContexts;
using FribergCarRentals.Shared.Models.Dto.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore;
using FribergCarRentals.Shared.Mvc.Services;
using FribergCarRentals.Shared.Enums;

namespace FribergCarRentalsApi.Controllers.DemoApi
{
    /// <summary>
    /// API controller that handles demo related functionality. 
    /// </summary>
    [Route("demo-api")]
    [ApiController]
    public class DemoController : ApiControllerBase
    {
        #region Fields

        /// <summary>
        /// The injected database context.
        /// </summary>
        private readonly ApplicationDbContext _context;

        /// <summary>
        /// The injected mock data seeder. 
        /// </summary>
        private readonly IMockDataSeeder _mockDataSeeder;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="context">The injected database context.</param>
        /// <param name="mockDataSeeder">The injected mock data seeder. </param>
        public DemoController(IAuthorizationService authorizationService, ApplicationDbContext context, IMockDataSeeder mockDataSeeder)
            : base(authorizationService)
        {
            _context = context;
            _mockDataSeeder = mockDataSeeder;
        }

        #endregion

        #region Endpoints

        /// <summary>
        /// Resets the demo data.
        /// </summary>
        /// <returns>An <see cref="ApiResponseDto{T}"/> containing the result of the operation.</returns>
        [ProducesResponseType<ApiResponseDto>(StatusCodes.Status200OK)]
        [HttpGet("reset")]
        public async Task<IActionResult> Reset()
        {
            try
            {
                var migrator = _context.GetService<IMigrator>();
                await migrator.MigrateAsync("0");
                await _context.Database.MigrateAsync();

                await _mockDataSeeder.SeedAdmins(_mockDataSeeder.GetDefaultAdmins());
                await _mockDataSeeder.SeedCustomers(_mockDataSeeder.GetDefaultCustomers());
                await _mockDataSeeder.SeedCarsAndCategories(_mockDataSeeder.GetDefaultCarsAndCategories());

                return Ok(ApiResponseDto.CreateSuccessfulResponse());
            }
            catch (Exception ex)
            {
                return BadRequest(ApiResponseDto.CreateErrorResponse(ApiErrorMessageTypes.GeneralError.ToString(), ex.Message));
            }
        }

        #endregion
    }
}
