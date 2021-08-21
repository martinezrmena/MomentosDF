using AppCarnesDF.Helpers.Common;
using AppCarnesDF.Models.User;
using AppCarnesDF.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppCarnesDF.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class General : ContentPage
    {
        public UserModel UserAutenticated { get; set; }
        private UserViewModel context = new UserViewModel();

        public General()
        {
            InitializeComponent();
            BindingContext = context;
            Content = MainStackLayout;
            context.UserAutenticated = UserAutenticated;
        }

        /// <summary>
        /// Permite consultar los tamaños de fuentes para el texto de la aplicación
        /// </summary>
        protected async override void OnAppearing()
        {
            base.OnAppearing();

            try
            {
                await context.UpdateFontSize();
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }

        }

        public void ModifyUserInformation(UserModel user)
        {
            UserAutenticated = user;
            context.UserAutenticated = UserAutenticated;
        }

        protected async override void OnDisappearing()
        {
            base.OnDisappearing();
            try
            {
                context.Actividades = null;
            }
            catch (Exception ex)
            {
                await context.Message.Failed("Estimado usuario ocurrió el siguiente error: " + ex.Message);
            }
        }

    }
}