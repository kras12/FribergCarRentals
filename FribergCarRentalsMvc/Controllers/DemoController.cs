using FribergCarRentals.Data.DatabaseContexts;
using FribergCarRentals.Shared.Services;
using FribergFastigheter.Server.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FribergCarRentals.Controllers
{
    /// <summary>
    /// A controller that handles endpoints that are specific to the demo parts of the application. 
    /// </summary>
    [Route("Demo")]
    public class DemoController : ViewControllerBase
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
        /// <param name="context">The injected database context.</param>
        /// <param name="mockDataSeeder">The injected mock data seeder. </param>
        /// <param name="authorizationService">The injected authorization service.</param>
        /// <param name="signInManager">The injected signin manager.</param>
        public DemoController(ApplicationDbContext context, IMockDataSeeder mockDataSeeder, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            _context = context;
            _mockDataSeeder = mockDataSeeder;
        }

        #endregion

        #region Actions

        /// <summary>
        /// Resets the application demo, which includes resetting the data in the database. 
        /// </summary>
        /// <returns></returns>
        [Route("Reset")]
        public async Task<IActionResult> Reset()
        {
            var migrator = _context.GetService<IMigrator>();
            await migrator.MigrateAsync("0");
            await _context.Database.MigrateAsync();

            await _mockDataSeeder.SeedAdmins(_mockDataSeeder.GetDefaultAdminUsers());
            await _mockDataSeeder.SeedCustomers(_mockDataSeeder.GetDefaultCustomerUsers());
            await _mockDataSeeder.SeedCarsAndCategories(_mockDataSeeder.GetDefaultCarsAndCategories());

            return View();
        }

        #endregion
    }
}
