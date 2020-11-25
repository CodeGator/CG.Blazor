using CG.Validations;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace CG.Blazor
{
    public static partial class ClaimsPrincipalExtensions
    {
        // *******************************************************************
        // Public methods.
        // *******************************************************************

        #region Public methods

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.Name"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserName(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.Name
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.NameIdentifier"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserId(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.NameIdentifier
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.Email"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserEmail(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.Email
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.Country"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserCountry(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.Country
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.PostalCode"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserPostalCode(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.PostalCode
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.Sid"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserSid(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.Sid
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.Surname"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserSurname(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.Surname
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.StateOrProvince"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserStateOrProvince(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.StateOrProvince
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.DateOfBirth"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserDateOfBirth(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.DateOfBirth
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.Gender"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserGender(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.Gender
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.GivenName"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserGivenName(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.GivenName
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.HomePhone"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserHomePhone(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.HomePhone
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.MobilePhone"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserMobilePhone(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.MobilePhone
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.OtherPhone"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserOtherPhone(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.OtherPhone
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.StreetAddress"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserStreetAddress(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.StreetAddress
                );

            // Return the value.
            return value;
        }

        // *******************************************************************

        /// <summary>
        /// This method returns the value of the <see cref="ClaimTypes.Thumbprint"/>
        /// claim, on the specified <see cref="ClaimsPrincipal"/> object
        /// </summary>
        /// <param name="principal"></param>
        /// <returns></returns>
        public static string UserThumbprint(
            this ClaimsPrincipal principal
            )
        {
            // Validate the parameters before attempting to use them.
            Guard.Instance().ThrowIfNull(principal, nameof(principal));

            // Get the value of the claim.
            var value = principal.FindFirstValue(
                ClaimTypes.Thumbprint
                );

            // Return the value.
            return value;
        }

        #endregion
    }
}
