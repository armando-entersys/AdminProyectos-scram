using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace PresentationLayer.Controllers
{
    /// <summary>
    /// Controlador base que proporciona métodos helper para acceder de forma segura a los Claims del usuario
    /// </summary>
    public abstract class BaseController : Controller
    {
        /// <summary>
        /// Obtiene el ID del usuario autenticado de forma segura
        /// </summary>
        /// <returns>ID del usuario o null si no está disponible</returns>
        protected int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim))
            {
                return null;
            }

            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }

            return null;
        }

        /// <summary>
        /// Obtiene el ID del usuario autenticado o lanza excepción si no está disponible
        /// </summary>
        /// <returns>ID del usuario</returns>
        /// <exception cref="UnauthorizedAccessException">Si el usuario no está autenticado</exception>
        protected int GetUserIdOrThrow()
        {
            var userId = GetUserId();
            if (!userId.HasValue)
            {
                throw new UnauthorizedAccessException("Usuario no autenticado o ID de usuario no válido");
            }
            return userId.Value;
        }

        /// <summary>
        /// Obtiene el ID del rol del usuario autenticado de forma segura
        /// </summary>
        /// <returns>ID del rol o null si no está disponible</returns>
        protected int? GetUserRoleId()
        {
            var roleIdClaim = User.FindFirst(ClaimTypes.Role)?.Value;
            if (string.IsNullOrWhiteSpace(roleIdClaim))
            {
                return null;
            }

            if (int.TryParse(roleIdClaim, out int roleId))
            {
                return roleId;
            }

            return null;
        }

        /// <summary>
        /// Obtiene el ID del rol del usuario autenticado o lanza excepción si no está disponible
        /// </summary>
        /// <returns>ID del rol</returns>
        /// <exception cref="UnauthorizedAccessException">Si el rol no está disponible</exception>
        protected int GetUserRoleIdOrThrow()
        {
            var roleId = GetUserRoleId();
            if (!roleId.HasValue)
            {
                throw new UnauthorizedAccessException("Rol de usuario no disponible");
            }
            return roleId.Value;
        }

        /// <summary>
        /// Obtiene el email del usuario autenticado
        /// </summary>
        /// <returns>Email del usuario o null si no está disponible</returns>
        protected string GetUserEmail()
        {
            return User.FindFirst(ClaimTypes.Email)?.Value;
        }

        /// <summary>
        /// Obtiene el nombre del usuario autenticado
        /// </summary>
        /// <returns>Nombre del usuario o null si no está disponible</returns>
        protected string GetUserName()
        {
            return User.FindFirst(ClaimTypes.Name)?.Value;
        }

        /// <summary>
        /// Verifica si el usuario tiene un rol específico
        /// </summary>
        /// <param name="roleId">ID del rol a verificar</param>
        /// <returns>True si el usuario tiene el rol especificado</returns>
        protected bool HasRole(int roleId)
        {
            var userRoleId = GetUserRoleId();
            return userRoleId.HasValue && userRoleId.Value == roleId;
        }

        /// <summary>
        /// Verifica si el usuario es administrador (RolId = 1)
        /// </summary>
        /// <returns>True si el usuario es administrador</returns>
        protected bool IsAdmin()
        {
            return HasRole(1);
        }

        /// <summary>
        /// Verifica si el usuario es usuario regular (RolId = 2)
        /// </summary>
        /// <returns>True si el usuario es usuario regular</returns>
        protected bool IsRegularUser()
        {
            return HasRole(2);
        }

        /// <summary>
        /// Verifica si el usuario es de producción (RolId = 3)
        /// </summary>
        /// <returns>True si el usuario es de producción</returns>
        protected bool IsProduction()
        {
            return HasRole(3);
        }
    }
}
