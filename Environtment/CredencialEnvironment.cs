namespace TramiteGov.Environtment
{
    public static class CredencialEnvironment
    {
        //Local
        public static string CamundaBaseUrl = "http://localhost:8080/engine-rest";

        //Produccion
        //public static string CamundaBaseUrl = "http://179.32.53.161:8080/engine-rest";
        public static string NameProcess = "TramiteGov";

        public static string GetCamundaUrl()
        {
            return CamundaBaseUrl;
        }

        public static string GetNameProcess()
        {
            return NameProcess;
        }

    }
}
