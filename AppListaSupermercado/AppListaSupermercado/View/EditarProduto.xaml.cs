using AppListaSupermercado.Model;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppListaSupermercado.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarProduto : ContentPage
    {
        public EditarProduto()
        {
            InitializeComponent();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            // Obtém qual foi o Produto anexado no BindingContext da página no momento que ela foi criada e enviada para navegação.
                 
            try
            {
                Produto produto_anexado = BindingContext as Produto;

                //Preencherá a Model com os valores dos entrys
                Produto p = new Produto
                {
                    Id = produto_anexado.Id,
                    NomeProduto = txt_descricao.Text,
                    Quantidade = Convert.ToDouble(txt_quantidade.Text),
                    
                    PrecoUnit = Convert.ToDouble(txt_preco_unitario.Text),
                };

               //Aqui atualizará o banco de dados com as novas informações da model
                await App.Database.Update(p);

                await DisplayAlert("Sucesso!", "Produto Editado", "OK");

                await Navigation.PushAsync(new ListaProdutos());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}