using System;


namespace HackAtHome.Entities
{
    public class ResultInfo 
    {
        public Status Status { get; set; }
        // El Token expira después de 10 minutos del último acceso al servicio REST
        public string Token { get; set; }
        public string FullName { get; set; }
    }
}
