using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Web.Agenda.Models
{
    public class TarefaMod
    {
        [Display(Name ="Codigo das tarefas")]
        public int Id { get; set; }

        [Required(ErrorMessage ="O campo de ser preenchido")]
        [MaxLength(100,ErrorMessage ="O campo deve ter 100 caracteres")]
        public string Nome { get; set; }

        [Required]
        public int Prioridade { get; set; }

        [Display(Name ="Status Atual")]
        public bool Concluido { get; set; }

        [MaxLength(200,ErrorMessage ="O campo deve ter 200 caracteres")]
        public string Observacao { get; set; }
    }
}