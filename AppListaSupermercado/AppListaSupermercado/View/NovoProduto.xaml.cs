using AppListaSupermercado.Model;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppListaSupermercado.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NovoProduto : ContentPage
    {
        public NovoProduto()
        {
            InitializeComponent();
        }
        //Tratará o evento do clicked do ToolbarItem
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                //Preencherá a model do Produto com os dados digitados pelo usuário
                Produto p = new Produto
                {
                    NomeProduto= txt_descricao.Text,
                    Quantidade = Convert.ToDouble(txt_quantidade.Text),
                    PrecoUnit = Convert.ToDouble(txt_preco_unitario.Text),
                };

                //Fará a inserção dos dados no banco de dados
                await App.Database.Insert(p);


                //Avisará do sucesso da operação
                await DisplayAlert("Sucesso!", "Produto Cadastrado", "OK");

                //Navegará para a pagina ListaProdutos
                await Navigation.PushAsync(new ListaProdutos());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}