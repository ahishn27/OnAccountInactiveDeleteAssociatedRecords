﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xrm.Sdk;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;



namespace DeleteARecord
{
    public class deleteArecord : IPlugin
    {
        public void Execute(IServiceProvider serviceProvider)
        {
            ITracingService tracingService = (ITracingService)serviceProvider.GetService(typeof(ITracingService));
            tracingService.Trace("Tracing service invoked");

            IPluginExecutionContext context = (IPluginExecutionContext)serviceProvider.GetService(typeof(IPluginExecutionContext));
            tracingService.Trace("Context Obtained Invoked");


            if(context.InputParameters.Contains("Target") & context.InputParameters["Target"] is Entity)
            {

                Entity account = (Entity)context.InputParameters["Target"];

                if (account.LogicalName != "account")
                    return;

            try
            {
            IOrganizationServiceFactory organizationServiceFactory = (IOrganizationServiceFactory)serviceProvider.GetService
                        (typeof(IOrganizationServiceFactory));
            IOrganizationService service = organizationServiceFactory.CreateOrganizationService(context.UserId);
            tracingService.Trace("Org Service Invoked");

            String AccountStatus =account.FormattedValues["new_account_status"].ToString();
            tracingService.Trace("Account Status Updated to "+ AccountStatus);

            if (AccountStatus == "Inactive")
            {

            QueryExpression query = new QueryExpression("contact");
            query.ColumnSet = new ColumnSet(new string[] { "contactid", "fullname" });
                        query.Criteria.AddCondition(new ConditionExpression("parentcustomerid", ConditionOperator.Equal, account.Id));
            EntityCollection results = service.RetrieveMultiple(query);
                        tracingService.Trace("inside if block");
                        tracingService.Trace("Results :" + results);
            
            }                

            }
            catch (Exception e)
            {
                    tracingService.Trace("{0}", e.ToString());
                    throw;
            }
          }
       }
    }
}
