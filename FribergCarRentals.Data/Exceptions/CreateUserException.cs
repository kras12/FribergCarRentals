using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FribergCarRentals.Data.Exceptions
{
    /// <summary>
    /// An exception that is thrown when user creation failed.
    /// </summary>
    public class CreateUserException : Exception
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="message">An optional message.</param>
        public CreateUserException(string? message) : base(message)
        {
        }
    }
}
