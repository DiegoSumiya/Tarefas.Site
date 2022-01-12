namespace Tarefas.Dominio.Models
{
    

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
