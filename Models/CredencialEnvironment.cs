namespace TramiteGov.Models
{
    public static class CredencialEnvironment
    {
        public static string CamundaBaseUrl = "http://localhost:8080/engine-rest";
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
