namespace Api.Servicios
{
    public  interface ITransicionesSolicitud
    {
        public void Asignar(TransicionesSolicitud transicionesSolicitud, string usuario);
        public void Cancelar(TransicionesSolicitud transicionesSolicitud,string motivo);
        public void Iniciar(TransicionesSolicitud transicionesSolicitud);
        public void Resolver(TransicionesSolicitud transicionesSolicitud, string motivo);
        public void Cerrar(TransicionesSolicitud transicionesSolicitud);
        public void Reabrir(TransicionesSolicitud transicionesSolicitud);

    }
}
