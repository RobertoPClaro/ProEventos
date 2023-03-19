using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class PalestrantePersist : IPalestrantePersist
    {
        private readonly ProEventosContext _context;
        public PalestrantePersist(ProEventosContext context)
        {
            _context = context;
            
        }

        public async Task<Palestrante[]> GetAllPalestrantesAsync(bool includeEventos = false)
        {
             IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante => palestrante.RedesSociais);

            if(includeEventos){
                query = query.Include(evento => evento.PalestrantesEventos)
                                .ThenInclude(palestranteEvento => palestranteEvento.Evento);
            }

            query = query.AsNoTracking().OrderBy(palestrante => palestrante.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante[]> GetAllPalestrantesByNomeAsync(string nome, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante=> palestrante.RedesSociais);

            if(includeEventos){
                query = query.Include(evento => evento.PalestrantesEventos)
                                .ThenInclude(palestranteEvento => palestranteEvento.Evento);
            }

            query = query.AsNoTracking()
                            .OrderBy(palestrante => palestrante.Id)
                            .Where(palestrante => palestrante.Nome.ToLower()
                                .Contains(nome.ToLower()));

            return await query.ToArrayAsync();
        }

        public async Task<Palestrante> GetPalestranteByIdAsync(int paletranteId, bool includeEventos)
        {
            IQueryable<Palestrante> query = _context.Palestrantes
                .Include(palestrante=> palestrante.RedesSociais);

            if(includeEventos){
                query = query.Include(evento => evento.PalestrantesEventos)
                                .ThenInclude(palestranteEvento => palestranteEvento.Evento);
            }

            query = query.AsNoTracking().OrderBy(palestrante => palestrante.Id)
                            .Where(palestrante => palestrante.Id == paletranteId);

            return await query.FirstOrDefaultAsync();
        }
    }
}