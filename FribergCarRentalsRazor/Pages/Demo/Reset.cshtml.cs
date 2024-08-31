using FribergCarRentals.Data.DatabaseContexts;
using FribergCarRentals.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using FribergCarRentals.Shared.Mvc.Services;

namespace FribergCarRentals.Pages.Demo
{
    public class ResetModel : PageModelBase
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
        public ResetModel(ApplicationDbContext context, IMockDataSeeder mockDataSeeder, IAuthorizationService authorizationService,
            SignInManager<ApplicationUser> signInManager) : base(authorizationService, signInManager)
        {
            _context = context;
            _mockDataSeeder = mockDataSeeder;
        }

        #endregion

        #region HandlerMethods        

        public async Task<IActionResult> OnGet()
        {
            var migrator = _context.GetService<IMigrator>();
            await migrator.MigrateAsync("0");
            await _context.Database.MigrateAsync();

            await _mockDataSeeder.SeedAdmins(_mockDataSeeder.GetDefaultAdmins());
            await _mockDataSeeder.SeedCustomers(_mockDataSeeder.GetDefaultCustomers());
            await _mockDataSeeder.SeedCarsAndCategories(_mockDataSeeder.GetDefaultCarsAndCategories());

            return Page();
        }

        #endregion
    }
}
