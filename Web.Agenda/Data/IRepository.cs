using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Agenda.Models;

namespace Web.Agenda.Data
{
    interface IRepository
    {
        bool Create(TarefaMod tarefa);
        TarefaMod Read(int id);
        bool Update(TarefaMod tarefa);
        bool Delete(int id);
        List<TarefaMod> ReadAll();
    }
}
