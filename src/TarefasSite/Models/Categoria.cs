
namespace TarefasSite.Models
{
    //public enum Categoria : byte
    //{
    //    //1	Pessoal
    //    //2	Trabalho
    //    //3	Outros

    //    Pessoal = 1,
    //    Trabalho,
    //    Outros
    //}

    public class Categoria
    {
        public Categoria(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }

        public int Id  { get; set; }
        public string Descricao { get; set; }
    }
}
