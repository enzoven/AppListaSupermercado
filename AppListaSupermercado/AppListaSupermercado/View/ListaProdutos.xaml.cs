using AppListaSupermercado.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppListaSupermercado.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaProdutos : ContentPage
    {
        // A ObservableCollection é uma classe que armazena um array de objetos do tipo de Produto.Utilizamos essa classe quando estamos apresentando um array de objetos ao usuário. Diferencial dessa classe é que toda vez que um item é add, removido ou modificado no array de objetos a interface gráfica também é atualizada. Assim as modificações feitas no array sempre estão na vista do usuário.
   
        ObservableCollection<Produto> lista_produtos = new ObservableCollection<Produto>();


        public ListaProdutos()
        {
            InitializeComponent();
            IconImageSource = ("AppListaSupermercado.Imagens.novo.png");
            // Referenciando que a a fonte itens (a serem mostrados ao usuário) a ListView é a ObservableCollection definida acima. Fazendo essa definição no construtor estamos amarrando a fonte de dados da ListView assim que ela é criada.
          

            lst_produtos.ItemsSource = lista_produtos;
        }

        //Fará a navegação para a tela de um novo produto

        private void ToolbarItem_Clicked_Novo(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new NovoProduto());

            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }


        //Fará a soma de todos os produtos
        private void ToolbarItem_Clicked_Somar(object sender, EventArgs e)
        {
            double soma = lista_produtos.Sum(i => i.PrecoUnit * i.Quantidade);

            string msg = "O total da compra é: R$ " + soma;

            DisplayAlert("SOMA", msg, "OK");
        }


        protected override void OnAppearing()
        {
          
            if (lista_produtos.Count == 0)
            {
                //Inicializando a Thread que irá buscar o array de objetos no arquivo db3 via classe SQLiteDatabaseHelper encapsulada na propriedade Database da classe App.
                 
                System.Threading.Tasks.Task.Run(async () =>
                {
                    //Retornando o array de objetos vindos do db3, foi usada uma variável tem do tipo List para que abaixo no foreach possamos percorrer a lista temporária e add os itens à ObservableCollection
                  
                    List<Produto> temp = await App.Database.GetAll();

                    foreach (Produto item in temp)
                    {
                        lista_produtos.Add(item);
                    }
                    // Após carregar os registros para a ObservableCollection removemos o loading da tela.
                     
                    ref_carregando.IsRefreshing = false;
                });
            }
        }

        
        //Trata o evento Clicked do MenuItem da ViewCell.ContextActions perguntando ao usuário se ele realmente deseja remover o item do arquivo db3
       

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            
            MenuItem disparador = (MenuItem)sender;


            Produto produto_selecionado = (Produto)disparador.BindingContext;

           
            bool confirmacao = await DisplayAlert("Tem Certeza?", "Remover Item?", "Sim", "Não");

            if (confirmacao)
            {
                
                await App.Database.Delete(produto_selecionado.Id);

               
                lista_produtos.Remove(produto_selecionado);
            }
        }


       //Receberá os novos valores digitados
        private void txt_busca_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Obterá o valor do search
            string buscou = e.NewTextValue;

            System.Threading.Tasks.Task.Run(async () =>
            {
                List<Produto> temp = await App.Database.Search(buscou);

                //Limpará a ObservableCollection antes de add os itens vindos da busca.
               

                lista_produtos.Clear();

                foreach (Produto item in temp)
                {
                    lista_produtos.Add(item);
                }

                ref_carregando.IsRefreshing = false;
            });
        }
        // Tratará o evento ItemSelected da ListView navegando para a página de detalhes.
        private void lst_produtos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Forma contraída de definir o BindingContext da página EditarProduto como sendo o Produto que foi selecionado na ListView (item da ListView) e em seguida já redicionando na navegação.
          
            Navigation.PushAsync(new EditarProduto
            {
                BindingContext = (Produto)e.SelectedItem
            });
        }
    }
}