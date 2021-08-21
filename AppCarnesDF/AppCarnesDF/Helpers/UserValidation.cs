using AppCarnesDF.Models.User;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AppCarnesDF.Helpers
{
    public class UserValidation
    {
        private ValidationString validationString = new ValidationString();

        /// <summary>
        /// Metodo que permite evaluar un modelo para determinar si su información esta completa
        /// </summary>
        /// <param name="User">Modelo a ser evaluado</param>
        /// <returns></returns>
        public async Task<bool> ValidateUserModel(UserModel User) {

            if (string.IsNullOrEmpty(User.Cedula))
            {
                return false;
            }

            if (string.IsNullOrEmpty(User.Tipo_Cedula))
            {
                return false;
            }

            if (string.IsNullOrEmpty(User.Nombre))
            {
                return false;
            }

            if (string.IsNullOrEmpty(User.Apellido))
            {
                return false;
            }

            if (!await validationString.ValidateEmail(User.Email))
            {
                return false;
            }

            if (string.IsNullOrEmpty(User.Fecha_Nacimiento))
            {
               return false;
            }

            if (string.IsNullOrEmpty(User.Genero))
            {
                return false;
            }

            if (string.IsNullOrEmpty(User.Sucursal1))
            {
                return false;
            }

            return true;
        }

    }
}
