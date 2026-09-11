using Api.Constantes;
using Api.Excepciones;
using System.Runtime.CompilerServices;

namespace Api.Servicios
{
    public class TransicionesSolicitud
    {
        public TransicionesSolicitud    (string estadoInicial = "Nueva") 
        {
            _EstadoActual = estadoInicial switch
            {
                "Asignada" => new Asignada(),
                "EnProceso" => new EnProceso(),
                "Resuelta" => new Resuelta(),
                "Cerrada" => new Cerrada(),
                "Cancelada" => new Cancelada(),
                _ => new Nueva()
            };
        }
        private ITransicionesSolicitud _EstadoActual  ;
        public string EstadoActual=> _EstadoActual.GetType().Name;
        public  string UsuarioAsignado { get;  set; } =string.Empty;
        public string Motivo { get; set; } = string.Empty; 

        public void CambiarEstado(ITransicionesSolicitud nuevoEstado)
        {
            _EstadoActual = nuevoEstado;
        }
        public void Asignar(string usuario)
        {
            
            _EstadoActual.Asignar(this, usuario);

        }

        public void Cancelar(string motivo)
        {
            if (string.IsNullOrEmpty(motivo) || motivo.Trim().Length < 11)
            {
                throw ExcepcionApi.MotivoRequerido();
            }
            _EstadoActual.Cancelar(this, motivo);
        }

        public void Cerrar()
        {
            _EstadoActual.Cerrar(this);
        }

        public void Iniciar()
        {
            _EstadoActual.Iniciar(this);
        }

        public void Reabrir()
        {
            _EstadoActual.Reabrir(this );
        }

        public void Resolver( string motivo )
        {

            if (string.IsNullOrEmpty(motivo) || motivo.Trim().Length < 21)
            {
                throw ExcepcionApi.MotivoRequerido();
            }
            _EstadoActual.Resolver(this , motivo);
        }
    }


    internal class Nueva : ITransicionesSolicitud
    {
        public void Asignar(TransicionesSolicitud transicionesSolicitud, string usuario)
        {
            transicionesSolicitud.UsuarioAsignado = usuario;
            transicionesSolicitud.CambiarEstado(new Asignada());
        }

        public void Cancelar(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
         
            transicionesSolicitud.Motivo = motivo;
            transicionesSolicitud.CambiarEstado(new Cancelada() );
            transicionesSolicitud.UsuarioAsignado = string.Empty;
        }

        public void Cerrar(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Iniciar(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Reabrir(TransicionesSolicitud transicionesSolicitud)
        {
            throw   ExcepcionApi.TransicionInvalida();
        }

        public void Resolver(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            throw ExcepcionApi.TransicionInvalida();
        }
    }
    internal class Asignada : ITransicionesSolicitud
    {
        public void Asignar(TransicionesSolicitud transicionesSolicitud, string usuario)
        {
           transicionesSolicitud.UsuarioAsignado = usuario;
            transicionesSolicitud.CambiarEstado(new Asignada());
        }

        public void Cancelar(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            transicionesSolicitud.UsuarioAsignado = string.Empty;
            transicionesSolicitud.CambiarEstado(new Cancelada());
        }

        public void Cerrar(TransicionesSolicitud transicionesSolicitud)
        {
           throw ExcepcionApi.TransicionInvalida();
        }

        public void Iniciar(TransicionesSolicitud transicionesSolicitud)
        {
            transicionesSolicitud.CambiarEstado(new EnProceso());
        }

        public void Reabrir(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Resolver(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            throw ExcepcionApi.TransicionInvalida();
        }
    }

    internal class EnProceso : ITransicionesSolicitud
    {
        public void Asignar(TransicionesSolicitud transicionesSolicitud, string usuario)
        {
            transicionesSolicitud.UsuarioAsignado = usuario;
            transicionesSolicitud.CambiarEstado(new Asignada());
        }

        public void Cancelar(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            transicionesSolicitud.UsuarioAsignado = string.Empty;
            transicionesSolicitud.CambiarEstado(new Cancelada());
        }

        public void Cerrar(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();  
        }

        public void Iniciar(TransicionesSolicitud transicionesSolicitud)
        {
            throw   ExcepcionApi.TransicionInvalida();
        }

        public void Reabrir(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Resolver(TransicionesSolicitud transicionesSolicitud, string motivo)
        {  transicionesSolicitud.Motivo = motivo;
            transicionesSolicitud.CambiarEstado(new Resuelta());
        }
    }
    internal class Resuelta : ITransicionesSolicitud
    {
        public void Asignar(TransicionesSolicitud transicionesSolicitud, string usuario)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Cancelar(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Cerrar(TransicionesSolicitud transicionesSolicitud)
        {
            transicionesSolicitud.CambiarEstado(new Cerrada());
            transicionesSolicitud.UsuarioAsignado = string.Empty; 

        }

        public void Iniciar(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Reabrir(TransicionesSolicitud transicionesSolicitud)
        {
            transicionesSolicitud.CambiarEstado(new EnProceso());
        }

        public void Resolver(TransicionesSolicitud   transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            throw ExcepcionApi.TransicionInvalida();
        }
    }
    internal class Cerrada : ITransicionesSolicitud
    {
        public void Asignar(TransicionesSolicitud transicionesSolicitud, string usuario)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Cancelar(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Cerrar(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Iniciar(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Reabrir(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Resolver(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            throw ExcepcionApi.TransicionInvalida();
        }
    }
    internal class Cancelada : ITransicionesSolicitud
    {
        public void Asignar(TransicionesSolicitud transicionesSolicitud, string usuario)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Cancelar(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Cerrar(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Iniciar(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Reabrir(TransicionesSolicitud transicionesSolicitud)
        {
            throw ExcepcionApi.TransicionInvalida();
        }

        public void Resolver(TransicionesSolicitud transicionesSolicitud, string motivo)
        {
            transicionesSolicitud.Motivo = motivo;
            throw ExcepcionApi.TransicionInvalida();
        }
    }
}
