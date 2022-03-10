using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProEventos.Application.Contratos;
using ProEventos.Application.Dtos;
using ProEventos.Domain;
using ProEventos.Persistence.Contratos;

namespace ProEventos.Application
{
  public class EventoService : IEventoService
  {
    private readonly IGeralPersist geralPersist;
    private readonly IEventoPersist eventoPersist;
    public EventoService(IGeralPersist geralPersist, IEventoPersist eventoPersist)
    {
      this.eventoPersist = eventoPersist;
      this.geralPersist = geralPersist;
    }
    public async Task<EventoDto> AddEventos(EventoDto model)
    {
      try
      {
          this.geralPersist.Add<Evento>(model);
          if (await this.geralPersist.SaveChangesAsync())
          {
              return await this.eventoPersist.GetEventoByIdAsync(model.Id, false);
          }
          return null;
      }
      catch (Exception ex)
      {          
          throw new Exception(ex.Message);
      }
    }

    public async Task<EventoDto> UpdateEvento(int eventoId, EventoDto model)
    {
      try
      {
          var evento = await this.eventoPersist.GetEventoByIdAsync(eventoId, false);
          if (evento == null) return null;

          model.Id = evento.Id;

          this.geralPersist.Update(model);
          if (await this.geralPersist.SaveChangesAsync())
          {
              return await this.eventoPersist.GetEventoByIdAsync(model.Id, false);
          }
          return null;
      }
      catch (Exception ex)
      {          
          throw new Exception(ex.Message);
      }
    }

    public Task<EventoDto> UpdateEvento(int eventoId)
    {
      throw new NotImplementedException();
    }

    public async Task<bool> DeleteEvento(int eventoId)
    {
      try
      {
          var evento = await this.eventoPersist.GetEventoByIdAsync(eventoId, false);
          if (evento == null) throw new Exception("Evento para delete não foi encontrado");

          this.geralPersist.Delete<Evento>(evento);
          return await this.geralPersist.SaveChangesAsync();         
      }
      catch (Exception ex)
      {          
          throw new Exception(ex.Message);
      }
    }

    public async Task<Evento[]> GetAllEventosAsync(bool includePalestrantes = false)
    {
      try
      {
          var eventos = await this.eventoPersist.GetAllEventosAsync(includePalestrantes);
          if (eventos == null) return null;

          var eventosRetorno = new List<EventoDto>();

                foreach (var evento in eventos)
                {
                    eventosRetorno.Add(new EventoDto() {
                        Id = evento.Id,
                        Local = evento.Local,
                        DataEvento = evento.DataEvento.ToString(),
                        Tema = evento.Tema,
                        QtdPessoas = evento.QtdPessoas,
                        ImagemURL = evento.ImagemURL,
                        Telefone = evento.Telefone,
                        Email = evento.Email,
                    });
                }

          return Ok(eventosRetorno);
      }
      catch (Exception ex)
      {          
          throw new Exception(ex.Message);
      }
    }

    public async Task<Evento[]> GetAllEventosByTemaAsync(string tema, bool includePalestrantes = false)
    {
      try
      {
          var eventos = await this.eventoPersist.GetAllEventosByTemaAsync(tema, includePalestrantes);
          if (eventos == null) return null;

          return eventos;
      }
      catch (Exception ex)
      {          
          throw new Exception(ex.Message);
      }
    }

    public async Task<EventoDto> GetEventoByIdAsync(int eventoId, bool includePalestrantes = false)
    {
      try
      {
          var eventos = await this.eventoPersist.GetEventoByIdAsync(eventoId, includePalestrantes);
          if (eventos == null) return null;

          return eventos;
      }
      catch (Exception ex)
      {          
          throw new Exception(ex.Message);
      }
    }    
  }
}