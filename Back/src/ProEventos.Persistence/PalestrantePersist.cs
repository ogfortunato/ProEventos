using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contextos;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Persistence
{
  public class PalestrantePersist : IPalestrantePersist
  {
    private readonly ProEventosContext context;
    public PalestrantePersist(ProEventosContext context)
    {
      this.context = context;
    }  

    public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
    {
      IQueryable<Palestrante> query = this.context.Palestrantes
        .Include(p => p.RedesSociais);

      if (includeEventos) 
      {
          query = query
                .Include(p => p.PalestranteEventos)
                .ThenInclude(pe => pe.Evento);
      }

      query = query.AsNoTracking().OrderBy(p => p.Id);

      return await query.ToArrayAsync();
    }

    public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
    {
      IQueryable<Palestrante> query = this.context.Palestrantes
        .Include(p => p.RedesSociais);

      if (includeEventos) 
      {
          query = query
                .Include(p => p.PalestranteEventos)
                .ThenInclude(pe => pe.Evento);
      }

      query = query.AsNoTracking().OrderBy(p => p.Id)
                   .Where(p => p.Nome.ToLower().Contains(nome.ToLower()));

      return await query.ToArrayAsync();
    }

    public async Task<Palestrante> GetPalestranteByIdAsync(int palestranteId, bool includeEventos)
    {
      IQueryable<Palestrante> query = this.context.Palestrantes
        .Include(p => p.RedesSociais);

      if (includeEventos) 
      {
          query = query
                .Include(p => p.PalestranteEventos)
                .ThenInclude(pe => pe.Evento);
      }

      query = query.AsNoTracking().OrderBy(p => p.Id)
                   .Where(p => p.Id == palestranteId);

      return await query.FirstOrDefaultAsync();
    } 
  }
}