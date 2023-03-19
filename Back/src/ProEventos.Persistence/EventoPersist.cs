using Microsoft.EntityFrameworkCore;
using ProEventos.Domain;
using ProEventos.Persistence.Contexts;
using ProEventos.Persistence.Contracts;

namespace ProEventos.Persistence
{
    public class EventoPersist : IEventoPersist
    {
        private readonly ProEventosContext _context;
        public EventoPersist(ProEventosContext context)
        {
            _context = context;
            //_context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais);

            if(includePalestrantes){
                query = query.Include(evento => evento.PalestrantesEventos)
                                .ThenInclude(palestranteEvento => palestranteEvento.Palestrante);
            }

            query = query.AsNoTracking().OrderBy(evento => evento.Id);

            return await query.ToArrayAsync();
        }

        public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais);

            if(includePalestrantes){
                query = query.Include(evento => evento.PalestrantesEventos)
                                .ThenInclude(palestranteEvento => palestranteEvento.Palestrante);
            }

            query = query.OrderBy(evento => evento.Id)
                    .AsNoTracking()
                    .Where(evento => evento.Tema.ToLower()
                        .Contains(tema.ToLower()));

            return await query.ToArrayAsync();
        }
        public async Task<Evento> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
        {
            IQueryable<Evento> query = _context.Eventos
                .Include(evento => evento.Lotes)
                .Include(evento => evento.RedesSociais);

            if(includePalestrantes){
                query = query.Include(evento => evento.PalestrantesEventos).ThenInclude(palestranteEvento => palestranteEvento.Palestrante);
            }

            query = query.OrderBy(evento => evento.Id)
                            .AsNoTracking()
                            .Where(evento => evento.Id == eventoId);

            return await query.FirstOrDefaultAsync();
        }
    }
}