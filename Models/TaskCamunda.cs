namespace TramiteGov.Models
{
    public class TaskCamunda
    {
        public string id { get; set; }
        public string name { get; set; }
        public string assignee { get; set; }
        public string created { get; set; }
        public string due { get; set; }
        public string followUp { get; set; }
        public string lastUpdate { get; set; }
        public string delegationState { get; set; }
        public string description { get; set; }
        public string executionId { get; set; }
        public string owner { get; set; }
        public string parentTaskId { get; set; }
        public string priority { get; set; }
        public string processDefinitionId { get; set; }
        public string processInstanceId { get; set; }
        public string taskDefinitionKey { get; set; }
        public string caseDefinitionId { get; set; }
        public string caseExecutionId { get; set; }
        public string caseInstanceId { get; set; }
        public string tenantId { get; set; }
        public string formKey { get; set; }
        public string camundaFormRef { get; set; }
        public string suspended { get; set; }
        


    }
}
