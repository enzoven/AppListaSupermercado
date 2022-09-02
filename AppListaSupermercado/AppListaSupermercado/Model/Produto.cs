﻿// A biblioteca SQLite é chamada aqui para que as anotations de chave primária e de autoIncremente possam ser usadas na propriedade Id
using SQLite;

namespace AppListaSupermercado.Model
{
    // A classe model Produto define como os dados serão armazenados no SQLite (arquivo db3)e como serão transportados entre a View e o SQLite.
    public class Produto
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string NomeProduto { get; set; }
        public double Quantidade { get; set; }
        public double PrecoTotal { get; set; }
        public double PrecoUnit { get; set; }
    }
}
