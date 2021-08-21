using AppCarnesDF.Helpers.Common;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AppCarnesDF.Helpers
{
    /// <summary>
    /// Metodo encargado de validar strings
    /// </summary>
    public class ValidationString
    {
        #region Properties
        private LogMessageAttention Message { get; set; }

        private const string EmailRegex = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";
        #endregion

        public void SetMessage(LogMessageAttention message)
        {
            Message = message;
        }

        /// <summary>
        /// Permite validar la contraseña proporcionada
        /// </summary>
        /// <param name="Password">String que contiene contraseña a evaluar</param>
        /// <returns>Bandera que indica si se proceso correctamente una contraseña</returns>
        public async Task<bool> ValidatePassword(string Password)
        {
            //Valida si se ingresan caracteres especiales
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{6,50}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (string.IsNullOrEmpty(Password))
            {
                await Message.Failed("Estimado usuario, debe proporcionar una contraseña para proceder.");
                return false;
            }

            if (!hasSymbols.IsMatch(Password))
            {
                await Message.Failed("Estimado usuario, la contraseña debe contener como minimo un caracter especial.");
                return false;
            }

            //la contraseña debe contener entre 6 y 50 caracteres
            if (!hasMiniMaxChars.IsMatch(Password))
            {
                await Message.Failed("Estimado usuario, la contraseña debe contener entre 6 y 50 caracteres.");
                return false;
            }

            //Valida que alguno de los caracteres sea digito
            if (!hasNumber.IsMatch(Password))
            {
                await Message.Failed("Estimado usuario, la contraseña debe contener al menos un número.");
                return false;
            }

            //Valida que alguno de los caracteres sea letra
            if (!Password.ToCharArray().Any(char.IsLetter))
            {
                await Message.Failed("Estimado usuario, la contraseña debe contener letras.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Permite validar la contraseña proporcionada
        /// </summary>
        /// <param name="Password">String que contiene contraseña a evaluar</param>
        /// <returns>Bandera que indica si se proceso correctamente una contraseña</returns>
        public bool ValidatePasswordSimple(string Password)
        {
            //Valida si se ingresan caracteres especiales
            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMiniMaxChars = new Regex(@".{6,50}");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }

            if (!hasSymbols.IsMatch(Password))
            {
                return false;
            }

            //la contraseña debe contener entre 6 y 50 caracteres
            if (!hasMiniMaxChars.IsMatch(Password))
            {
                return false;
            }

            //Valida que alguno de los caracteres sea digito
            if (!hasNumber.IsMatch(Password))
            {
                return false;
            }

            //Valida que alguno de los caracteres sea letra
            if (!Password.ToCharArray().Any(char.IsLetter))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Permite validar el email proporcionado
        /// </summary>
        /// <param name="Email">cadena que contiene Email a validar</param>
        /// <returns>es una bandera que permite indicar si es valido el email proporcionado
        /// </returns>
        public async Task<bool> ValidateEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email))
            {
                await Message.Failed("Estimado usuario, el campo del email es requerido para poder continuar con la operación actual.");
                return false;
            }

            if (!Regex.IsMatch(Email, EmailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250)))
            {
                await Message.Failed("Estimado usuario, el campo del email no contiene el formato correcto.");
                return false;
            }

            return true;
        }


        /// <summary>
        /// Metodo para encriptar contraseña
        /// </summary>
        /// <param name="texto">La contraseña a encriptar</param>
        /// <returns>La contraseña encriptada</returns>
        public async Task<string> Encriptar(string texto)
        {
            try
            {

                string key = "qualityinfosolutions"; //llave para encriptar datos

                byte[] keyArray;

                byte[] Arreglo_a_Cifrar = UTF8Encoding.UTF8.GetBytes(texto);

                //Se utilizan las clases de encriptación MD5

                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                //Algoritmo TripleDES
                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateEncryptor();

                byte[] ArrayResultado = cTransform.TransformFinalBlock(Arreglo_a_Cifrar, 0, Arreglo_a_Cifrar.Length);

                tdes.Clear();

                //se regresa el resultado en forma de una cadena
                texto = Convert.ToBase64String(ArrayResultado, 0, ArrayResultado.Length);

            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió un error: " + ex.Message);
            }

            return texto;
        }

        /// <summary>
        /// Metodo para desencriptar contraseña
        /// </summary>
        /// <param name="texto">La contraseña a desencriptar</param>
        /// <returns>La contraseña desencriptada</returns>
        public async Task<string> Desencriptar(string textoEncriptado)
        {
            try
            {
                string key = "qualityinfosolutions";
                byte[] keyArray;
                byte[] Array_a_Descifrar = Convert.FromBase64String(textoEncriptado);

                //algoritmo MD5
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();

                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();

                TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();

                tdes.Key = keyArray;
                tdes.Mode = CipherMode.ECB;
                tdes.Padding = PaddingMode.PKCS7;

                ICryptoTransform cTransform = tdes.CreateDecryptor();

                byte[] resultArray = cTransform.TransformFinalBlock(Array_a_Descifrar, 0, Array_a_Descifrar.Length);

                tdes.Clear();
                textoEncriptado = UTF8Encoding.UTF8.GetString(resultArray);

            }
            catch (Exception ex)
            {
                await Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }

            return textoEncriptado;
        }

        public async Task<bool> ValidateCedula(string texto)
        {
            bool IsValid = true;

            if (!string.IsNullOrEmpty(texto))
            {
                if (texto.Length != 9)
                {
                    IsValid = false;
                }
                else
                {
                    IsValid = true;
                }

                if (!IsValid)
                {
                    await Message.Failed("Estimado usuario, la cédula no posee el formato correcto.");
                }
            }

            return IsValid;
        }

        public async Task<bool> VerificarTelefono(string number) 
        {
            if (string.IsNullOrEmpty(number) || number.Length != 8)
            {
                await Message.Failed("Estimado usuario, el número de teléfono no posee el formato correcto.");
                return false;
            }

            return true;
        }
    }
}
