using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
  public class EventoPersist : IEventPersist
  {
    private readonly ProEventosContext context;
    public EventoPersist(ProEventosContext context)
    {
      this.context = context;
    }   

    public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
    {
      IQueryable<Evento> query = this.context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

      if (includePalestrantes) 
      {
          query = query
                .Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Palestrante);
      }

      query = query.OrderBy(e => e.Id);

      return await query.ToArrayAsync();  
    }

    public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
      IQueryable<Evento> query = this.context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

      if (includePalestrantes) 
      {
          query = query
                .Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Palestrante);
      }

      query = query.OrderBy(e => e.Id)
                   .Where(e => e.Tema.ToLower().Contains(tema.ToLower()));

      return await query.ToArrayAsync();
    }

    public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
      IQueryable<Evento> query = this.context.Eventos
            .Include(e => e.Lotes)
            .Include(e => e.RedesSociais);

      if (includePalestrantes) 
      {
          query = query
                .Include(e => e.PalestranteEventos)
                .ThenInclude(pe => pe.Palestrante);
      }

      query = query.OrderBy(e => e.Id)
                   .Where(e => e.Id == eventoId);

      return await query.FirstOrDefaultAsync();
    }
  }
}